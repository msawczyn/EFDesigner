#pragma warning disable IDE0017 // Simplify object initialization
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using log4net;

using Newtonsoft.Json;

using ParsingModels;

namespace EF6Parser
{
   public class Parser
   {
      private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      private readonly Assembly assembly;
      private readonly DbContext dbContext;
      private readonly MetadataWorkspace metadata;

      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
         Debugger.Break();
         
         this.assembly = assembly;
         Type contextType;

         if (dbContextTypeName != null)
         {
            log.Debug($"dbContextTypeName parameter is {dbContextTypeName}");
            contextType = assembly.GetExportedTypes().FirstOrDefault(t => t.FullName == dbContextTypeName || t.Name == dbContextTypeName);
            log.Info($"Using contextType = {contextType.FullName}");
         }
         else
         {
            log.Debug("dbContextTypeName parameter is null");
            List<Type> types = assembly.GetExportedTypes().Where(t => typeof(DbContext).IsAssignableFrom(t) && !t.FullName.Contains("`")).ToList();

            if (types.Count == 0)
            {
               log.Error($"No usable DBContext found in {assembly.Location}");
               throw new ArgumentException("Couldn't find DbContext-derived class in assembly. Is it public?");
            }

            if (types.Count > 1)
            {
               log.Error($"Found more than one class derived from DbContext: {string.Join(", ", types.Select(t => t.FullName))}");
               throw new AmbiguousMatchException("Found more than one class derived from DbContext");
            }

            contextType = types[0];

            log.Info($"Using contextType = {contextType.FullName}");
         }

         DbConfiguration.LoadConfiguration(contextType);
         ConstructorInfo constructor = contextType.GetConstructor(new[] {typeof(string)});

         // ReSharper disable once UnthrowableException
         if (constructor == null)
         {
            log.Error("Can't find constructor with one string parameter (connection string or connection name)");

            throw new MissingMethodException("Can't find constructor with one string parameter (connection string or connection name)");
         }

         try
         {
            DbContextInfo dbContextInfo = new DbContextInfo(contextType, new DbProviderInfo("System.Data.SqlClient", "2008"));
            dbContext = dbContextInfo.CreateInstance(); 
         }
         catch (InvalidOperationException e)
         {
            dbContext = assembly.CreateInstance(contextType.FullName, false, BindingFlags.Default, null, new object[] { "App=EntityFramework" }, null, null) as DbContext
                     ?? throw new Exception($"Failed to create an instance of {contextType.FullName}. "
                                          + "Please ensure it has either a default constructor or a constructor with one string parameter (connection string or connection name)"
                                          , e);
         }

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

      private List<ModelBidirectionalAssociation> GetBidirectionalAssociations(EntityType entityType)
      {
         List<ModelBidirectionalAssociation> result = new List<ModelBidirectionalAssociation>();
         
         if (entityType == null)
            return result;

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) != null))
         {
            StructuralType sourceType = navigationProperty.DeclaringType;

            if (sourceType.Name != entityType.Name)
               continue;

            ModelBidirectionalAssociation association = new ModelBidirectionalAssociation();
            EntityType targetType = navigationProperty.ToEndMember.GetEntityType();

            AssociationType associationType = navigationProperty.RelationshipType as AssociationType;
            ReferentialConstraint constraint = associationType?.Constraint;
            AssociationEndMember principalEnd = constraint?.FromRole as AssociationEndMember;
            EntityType principalType = principalEnd?.GetEntityType();

            AssociationEndMember dependentEnd = constraint?.ToRole as AssociationEndMember;
            EntityType dependentType = dependentEnd?.GetEntityType();

            if (principalType?.Name == sourceType.Name)
            {
               association.SourceRole = AssociationRole.Principal;
               association.TargetRole = AssociationRole.Dependent;
            }
            else if (principalType?.Name == targetType.Name)
            {
               association.TargetRole = AssociationRole.Principal;
               association.SourceRole = AssociationRole.Dependent;
            }
            else if (principalType == null && dependentType == null)
            {
               association.SourceRole = AssociationRole.NotApplicable;
               association.TargetRole = AssociationRole.NotApplicable;
            }

            association.SourceClassName = sourceType.Name;
            association.SourceClassNamespace = sourceType.NamespaceName;
            association.TargetClassName = targetType.Name;
            association.TargetClassNamespace = targetType.NamespaceName;

            NavigationProperty inverse = Inverse(navigationProperty);

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = targetType.Name;
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

            // look for declared foreign keys
            List<EdmProperty> dependentProperties = navigationProperty.GetDependentProperties().ToList();

            if (dependentProperties.Any())
               association.ForeignKey = string.Join(",", dependentProperties.Select(p => p.Name));

            log.Debug($"Found bidirectional association {association.SourceClassName}.{association.TargetPropertyName} <-> {association.TargetClassName}.{association.SourcePropertyTypeName}");
            log.Debug("\n   " + JsonConvert.SerializeObject(association));
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

      private static EntitySet GetTable(Type type, DbContext context)
      {
         EntitySet table = null;
         MetadataWorkspace metadata = ((IObjectContextAdapter)context).ObjectContext.MetadataWorkspace;

         // Get the part of the model that contains info about the actual CLR types
         ObjectItemCollection objectItemCollection = (ObjectItemCollection)metadata.GetItemCollection(DataSpace.OSpace);

         // Get the entity type from the model that maps to the CLR type
         EntityType entityType = metadata.GetItems<EntityType>(DataSpace.OSpace)
                                         .SingleOrDefault(e => objectItemCollection.GetClrType(e) == type);

         if (entityType != null)
         {
            // Get the entity set that uses this entity type
            EntitySet entitySet = metadata.GetItems<EntityContainer>(DataSpace.CSpace)
                                          .SingleOrDefault()
                                         ?.EntitySets
                                         ?.SingleOrDefault(s => s.ElementType.Name == entityType.Name);

            if (entitySet != null)
            {
               // Find the mapping between conceptual and storage model for this entity set
               EntitySetMapping mapping = metadata.GetItems<EntityContainerMapping>(DataSpace.CSSpace)
                                                  .SingleOrDefault()
                                                 ?.EntitySetMappings
                                                 ?.SingleOrDefault(s => s.EntitySet == entitySet);

               // Find the storage entity set (table) that the entity is mapped
               table = mapping?.EntityTypeMappings?.SelectMany(m => m.Fragments.Select(f => f.StoreEntitySet))?.Distinct().FirstOrDefault();
               //table = mapping?.EntityTypeMappings.SingleOrDefault()?.Fragments?.SingleOrDefault()?.StoreEntitySet;
            }
         }

         return table;
      }

      private static string GetTableName(Type type, DbContext context)
      {
         EntitySet table = GetTable(type, context);

         return table == null
                   ? null
                   : (string)table.MetadataProperties["Table"].Value ?? table.Name;

         // Return the table name from the storage entity set
      }

      private List<ModelUnidirectionalAssociation> GetUnidirectionalAssociations(EntityType entityType)
      {
         List<ModelUnidirectionalAssociation> result = new List<ModelUnidirectionalAssociation>();

         if (entityType == null)
            return result;

         foreach (NavigationProperty navigationProperty in entityType.DeclaredNavigationProperties.Where(np => Inverse(np) == null))
         {
            ModelUnidirectionalAssociation association = new ModelUnidirectionalAssociation();

            //StructuralType sourceType = navigationProperty.DeclaringType;
            EntityType sourceType = navigationProperty.FromEndMember.GetEntityType();
            EntityType targetType = navigationProperty.ToEndMember.GetEntityType();

            association.SourceClassName = sourceType.Name;
            association.SourceClassNamespace = sourceType.NamespaceName;
            association.TargetClassName = targetType.Name;
            association.TargetClassNamespace = targetType.NamespaceName;

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = targetType.Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.ToEndMember.RelationshipMultiplicity);
            association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.FromEndMember.RelationshipMultiplicity);

            // look for declared foreign keys
            List<EdmProperty> dependentProperties = navigationProperty.GetDependentProperties().ToList();

            if (dependentProperties.Any())
               association.ForeignKey = string.Join(",", dependentProperties.Select(p => p.Name));

            string json = JsonConvert.SerializeObject(association);

            log.Debug($"Found unidirectional association {association.SourceClassName}.{association.TargetPropertyName} -> {association.TargetClassName}");
            log.Debug("\n   " + json);
            result.Add(association);
         }

         return result;
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

      public string Process()
      {
         if (dbContext == null)
         {
            log.Error("Process: dbContext is null");

            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");
         }

         ReadOnlyCollection<GlobalItem> oSpace = metadata.GetItems(DataSpace.OSpace);
         log.Debug($"Found {oSpace.Count} OSpace items");

         ReadOnlyCollection<GlobalItem> sSpace = metadata.GetItems(DataSpace.SSpace);
         log.Debug($"Found {sSpace.Count} SSpace items");

         ReadOnlyCollection<GlobalItem> cSpace = metadata.GetItems(DataSpace.CSpace);
         log.Debug($"Found {cSpace.Count} CSpace items");

         // Context
         ///////////////////////////////////////////
         ModelRoot modelRoot = ProcessRoot();

         // Entities
         ///////////////////////////////////////////
         List<ModelClass> modelClasses = oSpace.OfType<EntityType>()
                                               .Select(e => ProcessEntity(e.FullName
                                                                        , oSpace.OfType<EntityType>().SingleOrDefault(o => o.FullName == e.FullName)
                                                                        , sSpace?.OfType<EntityType>().SingleOrDefault(s => s.FullName == "CodeFirstDatabaseSchema." + e.FullName.Split('.').Last())
                                                                        , cSpace.OfType<EntityType>().SingleOrDefault(c => c.FullName == e.FullName)))
                                               .Where(x => x != null)
                                               .ToList();

         log.Debug($"Adding {modelClasses.Count} classes");
         modelRoot.Classes.AddRange(modelClasses);

         // Complex types
         ///////////////////////////////////////////
         modelClasses = oSpace.OfType<ComplexType>()
                              .Select(e => ProcessComplexType(e.FullName
                                                            , oSpace.OfType<EntityType>().SingleOrDefault(s => s.FullName == e.FullName)
                                                            , sSpace?.OfType<EntityType>().SingleOrDefault(s => s.FullName == "CodeFirstDatabaseSchema." + e.FullName.Split('.').Last())
                                                            , cSpace.OfType<EntityType>().SingleOrDefault(c => c.FullName == e.FullName)))
                              .Where(x => x != null)
                              .ToList();

         log.Debug($"Adding {modelClasses.Count} complex types");
         modelRoot.Classes.AddRange(modelClasses);

         // Enums
         ///////////////////////////////////////////
         List<ModelEnum> modelEnums = oSpace.OfType<EnumType>().Select(ProcessEnum).Where(x => x != null).ToList();
         log.Debug($"Adding {modelEnums.Count} enumerations");
         modelRoot.Enumerations.AddRange(modelEnums);

         // Put it all together
         ///////////////////////////////////////////
         log.Debug("Serializing to JSON string");

         return JsonConvert.SerializeObject(modelRoot);
      }

      private ModelClass ProcessComplexType(string entityFullName, EntityType oSpaceType, EntityType sSpaceType, EntityType cSpaceType)
      {
         Type type = assembly.GetType(entityFullName);

         if (type == null)
         {
            log.Warn($"Could not find type for complex type {entityFullName}");

            return null;
         }

         log.Debug($"Found complex type {entityFullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelClass result = new ModelClass
                             {
                                Name = oSpaceType.Name
                              , Namespace = oSpaceType.NamespaceName
                              , IsAbstract = oSpaceType.Abstract
                              , BaseClass = oSpaceType.BaseType?.Name
                              , IsDependentType = true
                              , CustomAttributes = customAttributes.Length > 2
                                                      ? customAttributes
                                                      : null
                              , CustomInterfaces = type.GetInterfaces().Any()
                                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                                      : null
                              , Properties = oSpaceType.DeclaredProperties
                                                       .Select(x => x.Name)
                                                       .Select(propertyName => ProcessProperty(oSpaceType
                                                                                             , oSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName)
                                                                                             , sSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName)
                                                                                             , cSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName)
                                                                                             , true))
                                                       .Where(x => x != null)
                                                       .ToList()
                              , TableName = null
                             };

         log.Debug("\n   " + JsonConvert.SerializeObject(result));

         return result;
      }

      private ModelClass ProcessEntity(string entityFullName, EntityType oSpaceType, EntityType sSpaceType, EntityType cSpaceType)
      {
         Type type = assembly.GetType(entityFullName);

         if (type == null)
         {
            log.Warn($"Could not find type for entity {entityFullName}");

            return null;
         }

         log.Debug($"Found entity {entityFullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelClass result = new ModelClass
                             {
                                Name = oSpaceType.Name
                              , Namespace = oSpaceType.NamespaceName
                              , IsAbstract = oSpaceType.Abstract
                              , BaseClass = oSpaceType.BaseType?.Name
                              , CustomInterfaces = type.GetInterfaces().Any()
                                                      ? string.Join(",", type.GetInterfaces().Select(t => t.FullName))
                                                      : null
                              , IsDependentType = false
                              , CustomAttributes = customAttributes.Length > 2
                                                      ? customAttributes
                                                      : null
                              , Properties = oSpaceType.DeclaredProperties
                                                       .Select(x => x.Name)
                                                       .Select(propertyName => ProcessProperty(oSpaceType
                                                                                             , oSpaceType.DeclaredProperties.FirstOrDefault(q => q.Name == propertyName)
                                                                                             , sSpaceType?.DeclaredProperties?.FirstOrDefault(q => q.Name == propertyName)
                                                                                             , cSpaceType?.DeclaredProperties?.FirstOrDefault(q => q.Name == propertyName)))
                                                       .Where(x => x != null)
                                                       .ToList()
                              , UnidirectionalAssociations = GetUnidirectionalAssociations(cSpaceType ?? oSpaceType)
                              , BidirectionalAssociations = GetBidirectionalAssociations(cSpaceType ?? oSpaceType)
                              , TableName = GetTableName(type, dbContext)
                             };

         log.Debug("\n   " + JsonConvert.SerializeObject(result));

         return result;
      }

      private ModelEnum ProcessEnum(EnumType enumType)
      {
         Type type = assembly.GetType(enumType.FullName);

         if (type == null)
         {
            log.Warn($"Could not find type for complex type {enumType.FullName}");

            return null;
         }

         log.Debug($"Found enum {enumType.FullName}");
         string customAttributes = GetCustomAttributes(type);

         ModelEnum result = new ModelEnum
                            {
                               Name = enumType.Name
                             , Namespace = enumType.NamespaceName
                             , IsFlags = enumType.IsFlags
                             , ValueType = enumType.UnderlyingType.ClrEquivalentType.Name
                             , CustomAttributes = customAttributes.Length > 2
                                                     ? customAttributes
                                                     : null
                             , Values = enumType.Members
                                                .Select(enumMember => new ModelEnumValue {Name = enumMember.Name, Value = enumMember.Value?.ToString()})
                                                .ToList()
                            };

         log.Debug("\n   " + JsonConvert.SerializeObject(result));

         return result;
      }

      // ReSharper disable once UnusedParameter.Local
      private ModelProperty ProcessProperty(EntityType parent, EdmProperty oSpaceProperty, EdmProperty sSpaceProperty, EdmProperty cSpaceProperty, bool isComplexType = false)
      {
         if (oSpaceProperty == null)
            return null;

         log.Debug($"Found property {parent.Name}.{oSpaceProperty.Name}");

         try
         {
            ModelProperty result = new ModelProperty
                                   {
                                      TypeName = oSpaceProperty.TypeUsage.EdmType.BuiltInTypeKind == BuiltInTypeKind.EnumType
                                                    ? oSpaceProperty.TypeUsage.EdmType.FullName
                                                    : oSpaceProperty.TypeUsage.EdmType.Name
                                    , Name = oSpaceProperty.Name
                                    , IsIdentity = !isComplexType && parent.KeyProperties.Any(p => p.Name == oSpaceProperty.Name)
                                    , IsIdentityGenerated = sSpaceProperty?.IsStoreGeneratedIdentity == true
                                    , Required = !(bool)oSpaceProperty.TypeUsage.Facets.First(facet => facet.Name == "Nullable").Value
                                    , Indexed = bool.TryParse(oSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "Indexed")?.Value?.ToString(), out bool indexed) && indexed
                                    , MaxStringLength = sSpaceProperty != null && int.TryParse(sSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "MaxLength")?.Value?.ToString(), out int maxLength) && maxLength < int.MaxValue / 2
                                                           ? maxLength
                                                           : 0
                                    , MinStringLength = sSpaceProperty != null && int.TryParse(sSpaceProperty.TypeUsage.Facets.FirstOrDefault(facet => facet.Name == "MinLength")?.Value?.ToString(), out int minLength)
                                                           ? minLength
                                                           : 0
                                   };

            log.Debug("\n   " + JsonConvert.SerializeObject(result));

            return result;
         }
         catch (InvalidOperationException) { }

         return null;
      }

      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();

         if (contextType == null)
            throw new InvalidDataException();

         log.Debug($"Found DbContext {contextType.Name}");

         result.EntityContainerName = contextType.Name;
         result.Namespace = contextType.Namespace;

         log.Debug("\n   " + JsonConvert.SerializeObject(result));

         return result;
      }
   }
}