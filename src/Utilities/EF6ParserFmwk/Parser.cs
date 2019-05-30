#pragma warning disable IDE0017 // Simplify object initialization
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;

using Effort.Provider;

using Newtonsoft.Json;

using ParsingModels;
// ReSharper disable UseObjectOrCollectionInitializer

namespace EF6Parser
{
   public class Parser
   {
      private readonly Assembly assembly;
      private readonly DbContext dbContext;
      private readonly MetadataWorkspace metadata;

      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
         this.assembly = assembly;
         Type contextType;

         if (dbContextTypeName != null)
            contextType = assembly.GetExportedTypes().FirstOrDefault(t => t.FullName == dbContextTypeName);
         else
         {
            List<Type> types = assembly.GetExportedTypes().Where(t => typeof(DbContext).IsAssignableFrom(t)).ToList();

            // ReSharper disable once UnthrowableException
            if (types.Count != 1)
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");

            contextType = types[0];
         }

         ConstructorInfo constructor = contextType.GetConstructor(new[] {typeof(string)});

         // ReSharper disable once UnthrowableException
         if (constructor == null)
            throw new MissingMethodException("Can't find appropriate constructor");

         EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();
         dbContext = assembly.CreateInstance(contextType.FullName, false, BindingFlags.Default, null, new object[] {connection}, null, null) as DbContext;
         metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;
      }

      private static Multiplicity ConvertMultiplicity(RelationshipMultiplicity relationshipMultiplicity)
      {
         Multiplicity multiplicity = Multiplicity.ZeroOne;

         switch (relationshipMultiplicity)
         {
            case RelationshipMultiplicity.ZeroOrOne:
               multiplicity = Multiplicity.ZeroOne;

               break;

            case RelationshipMultiplicity.One:
               multiplicity = Multiplicity.One;

               break;

            case RelationshipMultiplicity.Many:
               multiplicity = Multiplicity.ZeroMany;

               break;
         }

         return multiplicity;
      }

      private NavigationProperty Inverse(NavigationProperty navProperty)
      {
         // ReSharper disable once UseNullPropagation
         if (navProperty == null)
            return null;

         EntityType toEntity = navProperty.ToEndMember.GetEntityType();
         return toEntity.NavigationProperties
                        .SingleOrDefault(n => ReferenceEquals(n.RelationshipType, navProperty.RelationshipType) && !ReferenceEquals(n, navProperty));
      }

      private List<ModelUnidirectionalAssociation> GetUnidirectionalAssociations(EntityType entityType)
      {
         List<ModelUnidirectionalAssociation> result = new List<ModelUnidirectionalAssociation>();

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) == null))
         {
            ModelUnidirectionalAssociation association = new ModelUnidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.NamespaceName;
            association.TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetClassNamespace = navigationProperty.ToEndMember.GetEntityType().NamespaceName;

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity);
            association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity);

            result.Add(association);
         }

         return result;
      }

      private List<ModelBidirectionalAssociation> GetBidirectionalAssociations(EntityType entityType)
      {
         List<ModelBidirectionalAssociation> result = new List<ModelBidirectionalAssociation>();

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) != null))
         {
            ModelBidirectionalAssociation association = new ModelBidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.NamespaceName;
            association.TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetClassNamespace = navigationProperty.ToEndMember.GetEntityType().NamespaceName;

            NavigationProperty inverse = Inverse(navigationProperty);

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.ToEndMember.GetEntityType().Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity);
            association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourcePropertyTypeName = navigationProperty.FromEndMember.GetEntityType().Name;
            association.SourcePropertyName = inverse?.Name;
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity);
            association.SourceSummary = navigationProperty.FromEndMember.Documentation?.Summary;
            association.SourceDescription = navigationProperty.FromEndMember.Documentation?.LongDescription;

            result.Add(association);
         }

         return result;

      }

      private static string GetCustomAttributes(Type type)
      {
         return type == null
                   ? string.Empty
                   : GetCustomAttributes(type.CustomAttributes);
      }

      private static string GetCustomAttributes(IEnumerable<CustomAttributeData> customAttributeData)
      {
         List<string> customAttributes = customAttributeData.Select(a => a.ToString()).ToList();
         customAttributes.Remove("[System.SerializableAttribute()]");
         customAttributes.Remove("[System.Runtime.InteropServices.ComVisibleAttribute((Boolean)True)]");
         customAttributes.Remove("[__DynamicallyInvokableAttribute()]");
         customAttributes.Remove("[System.Reflection.DefaultMemberAttribute(\"Chars\")]");
         customAttributes.Remove("[System.Runtime.Versioning.NonVersionableAttribute()]");
         customAttributes.Remove("[System.FlagsAttribute()]");
         return string.Join("", customAttributes);
      }

      private static string GetTableName(Type type, DbContext context)
      {
         MetadataWorkspace metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

         // Get the part of the model that contains info about the actual CLR types
         ObjectItemCollection objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

         // Get the entity type from the model that maps to the CLR type
         EntityType entityType = metadata.GetItems<EntityType>(DataSpace.OSpace)
                                         .SingleOrDefault(e => objectItemCollection.GetClrType(e) == type);

         if (entityType == null) 
            return null;

         // Get the entity set that uses this entity type
         EntitySet entitySet = metadata.GetItems<EntityContainer>(DataSpace.CSpace)
                                       .SingleOrDefault()
                                       ?.EntitySets
                                       ?.SingleOrDefault(s => s.ElementType.Name == entityType.Name);

         if (entitySet == null) 
            return null;

         // Find the mapping between conceptual and storage model for this entity set
         EntitySetMapping mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                                            .SingleOrDefault()
                                            ?.EntitySetMappings
                                            ?.SingleOrDefault(s => s.EntitySet == entitySet);

         // Find the storage entity set (table) that the entity is mapped
         EntitySet table = mapping?.EntityTypeMappings.SingleOrDefault()?.Fragments?.SingleOrDefault()?.StoreEntitySet;

         if (table == null) 
            return null;

         // Return the table name from the storage entity set
         return (string)table.MetadataProperties["Table"].Value ?? table.Name;
      }


      public string Process()
      {
         if (dbContext == null)

            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");

         ReadOnlyCollection<GlobalItem> cSpace = metadata.GetItems(DataSpace.CSpace);

         ModelRoot modelRoot = ProcessRoot();
         modelRoot.Classes.AddRange(cSpace.OfType<EntityType>().Select(ProcessEntity).Where(x => x != null));
         modelRoot.Classes.AddRange(cSpace.OfType<ComplexType>().Select(ProcessEntity).Where(x => x != null));
         modelRoot.Enumerations.AddRange(cSpace.OfType<EnumType>().Select(ProcessEnum).Where(x => x != null));

         return JsonConvert.SerializeObject(modelRoot);
      }

      private ModelClass ProcessEntity(EntityType entityType)
      {
         Type type = assembly.GetType(entityType.FullName);
         string customAttributes = GetCustomAttributes(type);
         ModelClass result = new ModelClass();

         result.Name = entityType.Name;
         result.Namespace = entityType.NamespaceName;
         result.IsAbstract = entityType.Abstract;
         result.BaseClass = entityType.BaseType?.Name;

         result.CustomInterfaces = type?.GetInterfaces().Any() == true
                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                      : null;

         try
         {
            result.TableName = type == null
                                  ? null
                                  : GetTableName(type, dbContext);
         }
         catch 
         {
            result.TableName = null;
         }

         result.IsDependentType = false;

         result.CustomAttributes = customAttributes.Length > 2
                                      ? customAttributes
                                      : null;

         result.Properties = entityType.DeclaredProperties.Select(p => ProcessProperty(p, entityType)).Where(x => x != null).ToList();
         result.UnidirectionalAssociations = GetUnidirectionalAssociations(entityType);
         result.BidirectionalAssociations = GetBidirectionalAssociations(entityType);

         return result;
      }

      private ModelClass ProcessEntity(ComplexType complexType)
      {
         Type type = assembly.GetType(complexType.FullName);
         string customAttributes = GetCustomAttributes(type);

         ModelClass result = new ModelClass();
         result.Name = complexType.Name;
         result.Namespace = complexType.NamespaceName;
         result.IsAbstract = complexType.Abstract;
         result.BaseClass = complexType.BaseType?.Name;
         try
         {
            result.TableName = GetTableName(assembly.GetType(complexType.FullName), dbContext);
         }
         catch 
         {
            result.TableName = null;
         }
         result.IsDependentType = true;

         result.CustomAttributes = customAttributes.Length > 2
                                      ? customAttributes
                                      : null;

         result.CustomInterfaces = type?.GetInterfaces().Any() == true
                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                      : null;

         result.Properties = complexType.Properties.Select(p => ProcessProperty(p)).Where(x => x != null).ToList();

         return result;
      }

      private ModelEnum ProcessEnum(EnumType enumType)
      {
         Type type = assembly.GetType(enumType.FullName);
         string customAttributes = GetCustomAttributes(type);

         ModelEnum result = new ModelEnum();
         result.Name = enumType.Name;
         result.Namespace = enumType.NamespaceName;
         result.IsFlags = enumType.IsFlags;
         result.ValueType = enumType.UnderlyingType.ClrEquivalentType.Name;

         result.CustomAttributes = customAttributes.Length > 2
                                      ? customAttributes
                                      : null;

         result.Values = enumType.Members
                                 .Select(enumMember => new ModelEnumValue {Name = enumMember.Name, Value = enumMember.Value.ToString()})
                                 .ToList();

         return result;
      }

      private ModelProperty ProcessProperty(EdmProperty edmProperty, EntityType parent = null)
      {
         string typename = edmProperty.TypeUsage.EdmType.FullName;

         if (typename.StartsWith("Edm."))
            typename = $"System.{typename.Substring(4)}";

         Type type = Type.GetType(typename) ?? assembly.GetType(typename);
         List<CustomAttributeData> attributes = type?.CustomAttributes.ToList() ?? new List<CustomAttributeData>();

         ModelProperty result = new ModelProperty {TypeName = edmProperty.TypeName, Name = edmProperty.Name, IsIdentity = parent?.KeyProperties.Any(p => p.Name == edmProperty.Name) ?? false, Required = !edmProperty.Nullable};

         CustomAttributeData indexedAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "Indexed");
         result.Indexed = indexedAttribute != null;

         if (indexedAttribute != null)
            attributes.Remove(indexedAttribute);

         CustomAttributeData maxLengthAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "MaxLength" || a.AttributeType.Name == "StringLength");
         result.MaxStringLength = (int?)maxLengthAttribute?.ConstructorArguments.First().Value ?? 0;

         if (maxLengthAttribute != null)
            attributes.Remove(maxLengthAttribute);

         CustomAttributeData minLengthAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "MinLength");
         result.MinStringLength = (int?)minLengthAttribute?.ConstructorArguments.First().Value ?? 0;

         if (minLengthAttribute != null)
            attributes.Remove(minLengthAttribute);

         string customAttributes = GetCustomAttributes(attributes);

         result.CustomAttributes = customAttributes.Length > 2
                                      ? customAttributes
                                      : null;

         return result;
      }

      // [System.SerializableAttribute()][System.Runtime.InteropServices.ComVisibleAttribute((Boolean)True)][__DynamicallyInvokableAttribute()]
      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();

         result.EntityContainerName = contextType.Name;
         result.Namespace = contextType.Namespace;

         return result;
      }
   }
}