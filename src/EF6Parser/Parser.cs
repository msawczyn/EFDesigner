using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using ParsingModels;

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
            if (types.Count != 1)

               // ReSharper disable once UnthrowableException
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");

            contextType = types[0];
         }

         ConstructorInfo constructor = contextType.GetConstructor(new[] { typeof(string) });

         // ReSharper disable once UnthrowableException
         if (constructor == null)
            throw new MissingMethodException("Can't find appropriate constructor");

         dbContext = assembly.CreateInstance(contextType.FullName, false, BindingFlags.Default, null, new object[] { "App=EntityFramework" }, null, null) as DbContext;
         metadata = ((IObjectContextAdapter)dbContext).ObjectContext.MetadataWorkspace;
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

      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();

         result.Name = contextType.Name;
         result.Namespace = contextType.Namespace;
         return result;
      }

      private ModelClass ProcessEntity(EntityType entityType)
      {
         Type type = assembly.GetType(entityType.FullName);
         string customAttributes = $"[{string.Join("][", type.CustomAttributes.Select(a => a.ToString()))}]";
         ModelClass result = new ModelClass
                             {
                                Name = entityType.Name
                                , Namespace = entityType.NamespaceName
                                , IsAbstract = entityType.Abstract
                                , BaseClass = entityType.BaseType?.Name
                                , TableName = GetTableName(assembly.GetType(entityType.FullName), dbContext)
                                , IsDependentType = false
                                , CustomAttributes = customAttributes.Length > 2 ? customAttributes : null
                                , Properties = entityType.DeclaredProperties.Select(p => ProcessProperty(p, entityType)).Where(x => x != null).ToList()
                                , UnidirectionalAssociations = GetUnidirectionalAssociations(entityType)
                                , BidirectionalAssociations = GetBidirectionalAssociations(entityType)
                             };

         return result;
      }

      private List<ModelUnidirectionalAssociation> GetUnidirectionalAssociations(EntityType entityType)
      {
         return entityType.DeclaredNavigationProperties.Where(np => np.FromEndMember.GetEntityType() == entityType &&
                                                                    string.IsNullOrEmpty(np.FromEndMember.Name))
                          .Select(navigationProperty => new ModelUnidirectionalAssociation
                                                        {
                                                           SourceClassName = navigationProperty.FromEndMember.GetEntityType().Name
                                                           , TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name
                                                           , TargetPropertyName = navigationProperty.ToEndMember.Name
                                                           , TargetSummary = navigationProperty.ToEndMember.Documentation.Summary
                                                           , TargetDescription = navigationProperty.ToEndMember.Documentation.LongDescription
                                                           , SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity)
                                                           , TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity)
                                                        })
                          .ToList();
      }

      private List<ModelBidirectionalAssociation> GetBidirectionalAssociations(EntityType entityType)
      {
         return entityType.DeclaredNavigationProperties
                          .Where(np => np.FromEndMember.GetEntityType() == entityType &&
                                       !string.IsNullOrEmpty(np.FromEndMember.Name) &&
                                       !string.IsNullOrEmpty(np.ToEndMember.Name))
                          .Select(navigationProperty => new ModelBidirectionalAssociation
                                                        {
                                                           SourceClassName = navigationProperty.FromEndMember.GetEntityType().Name
                                                           , SourcePropertyName = navigationProperty.FromEndMember.Name
                                                           , SourceSummary = navigationProperty.FromEndMember.Documentation.Summary
                                                           , SourceDescription = navigationProperty.FromEndMember.Documentation.LongDescription
                                                           , TargetClassName = navigationProperty.ToEndMember.GetEntityType().Name
                                                           , TargetPropertyName = navigationProperty.ToEndMember.Name
                                                           , TargetSummary = navigationProperty.ToEndMember.Documentation.Summary
                                                           , TargetDescription = navigationProperty.ToEndMember.Documentation.LongDescription
                                                           , SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity)
                                                           , TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity)
                                                        })
                          .Distinct()
                          .ToList();
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

      private ModelProperty ProcessProperty(EdmProperty edmProperty, EntityType parent = null)
      {
         Type type = assembly.GetType(edmProperty.TypeName);
         List<CustomAttributeData> attributes = type.CustomAttributes.ToList();

         ModelProperty result = new ModelProperty
                                {
                                   TypeName = edmProperty.TypeName, 
                                   Name = edmProperty.Name, 
                                   IsIdentity = parent?.KeyProperties.Any(p => p.Name == edmProperty.Name) ?? false, 
                                   Required = !edmProperty.Nullable
                                };

         CustomAttributeData indexedAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "Indexed");
         result.Indexed = indexedAttribute != null;
         if (indexedAttribute != null) attributes.Remove(indexedAttribute);

         CustomAttributeData maxLengthAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "MaxLength" ||a.AttributeType.Name == "StringLength" );
         result.MaxStringLength = (int?)maxLengthAttribute?.ConstructorArguments.First().Value ?? 0;
         if (maxLengthAttribute != null) attributes.Remove(maxLengthAttribute);
         
         CustomAttributeData minLengthAttribute = attributes.FirstOrDefault(a => a.AttributeType.Name == "MinLength" );
         result.MinStringLength = (int?)minLengthAttribute?.ConstructorArguments.First().Value ?? 0;
         if (minLengthAttribute != null) attributes.Remove(minLengthAttribute);

         string customAttributes = $"[{string.Join("][", attributes.Select(a => a.ToString()))}]";
         result.CustomAttributes = customAttributes.Length > 2 ? customAttributes : null;

         return result;
      }

      private ModelClass ProcessEntity(ComplexType complexType)
      {
         Type type = assembly.GetType(complexType.FullName);
         string customAttributes = $"[{string.Join("][", type.CustomAttributes.Select(a => a.ToString()))}]";
         ModelClass result = new ModelClass
                             {
                                Name = complexType.Name
                                , Namespace = complexType.NamespaceName
                                , IsAbstract = complexType.Abstract
                                , BaseClass = complexType.BaseType?.Name
                                , TableName = GetTableName(assembly.GetType(complexType.FullName), dbContext)
                                , IsDependentType = true
                                , CustomAttributes = customAttributes.Length > 2 ? customAttributes : null
                                , Properties = complexType.Properties.Select(p => ProcessProperty(p)).Where(x => x != null).ToList()
                             };

         return result;
      }

      private static string GetTableName(Type type, DbContext context)
      {
         MetadataWorkspace metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

         // Get the part of the model that contains info about the actual CLR types
         ObjectItemCollection objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

         // Get the entity type from the model that maps to the CLR type
         EntityType entityType = metadata
                                 .GetItems<EntityType>(DataSpace.OSpace)
                                 .Single(e => objectItemCollection.GetClrType(e) == type);

         // Get the entity set that uses this entity type
         EntitySet entitySet = metadata
                               .GetItems<EntityContainer>(DataSpace.CSpace)
                               .Single()
                               .EntitySets
                               .Single(s => s.ElementType.Name == entityType.Name);

         // Find the mapping between conceptual and storage model for this entity set
         EntitySetMapping mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                                            .Single()
                                            .EntitySetMappings
                                            .Single(s => s.EntitySet == entitySet);

         // Find the storage entity set (table) that the entity is mapped
         EntitySet table = mapping
                           .EntityTypeMappings.Single()
                           .Fragments.Single()
                           .StoreEntitySet;

         // Return the table name from the storage entity set
         return (string)table.MetadataProperties["Table"].Value ?? table.Name;
      }

      private ModelEnum ProcessEnum(EnumType enumType)
      {
         Type type = assembly.GetType(enumType.FullName);
         string customAttributes = $"[{string.Join("][", type.CustomAttributes.Select(a => a.ToString()))}]";

         ModelEnum result = new ModelEnum
                            {
                               Name = enumType.Name
                               , Namespace = enumType.NamespaceName
                               , IsFlags = enumType.IsFlags
                               , ValueType = enumType.UnderlyingType.ClrEquivalentType.Name
                               , CustomAttributes = customAttributes.Length > 2 ? customAttributes : null
                               , Values = enumType.Members
                                                  .Select(enumMember => new ModelEnumValue
                                                                        {
                                                                           Name = enumMember.Name, 
                                                                           Value = enumMember.Value.ToString()
                                                                        })
                                                  .ToList()
                            };
         return result;
      }
   }
}