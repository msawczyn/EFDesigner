using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Newtonsoft.Json;

using ParsingModels;

// ReSharper disable UseObjectOrCollectionInitializer

namespace EFCoreParser
{
   public class Parser
   {
      private readonly DbContext dbContext;

      private IModel model;

      public Parser(Assembly assembly, string dbContextTypeName = null)
      {
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

         ConstructorInfo constructor = contextType.GetConstructor(Type.EmptyTypes);

         // ReSharper disable once UnthrowableException
         if (constructor == null)
            throw new MissingMethodException("Can't find default constructor");

         dbContext = assembly.CreateInstance(contextType.FullName) as DbContext;
         model = dbContext.Model;
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

      private List<ModelBidirectionalAssociation> GetBidirectionalAssociations(IEntityType entityType)
      {
         List<ModelBidirectionalAssociation> result = new List<ModelBidirectionalAssociation>();

         foreach (INavigation navigationProperty in entityType.GetNavigations().Where(n => n.FindInverse() != null))
         {
            ModelBidirectionalAssociation association = new ModelBidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.ClrType.Namespace;
            association.TargetClassName = navigationProperty.ClrType.Name;
            association.TargetClassNamespace = navigationProperty.ClrType.Namespace;

            INavigation inverse = navigationProperty.FindInverse();

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.PropertyInfo.PropertyType.Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.GetTargetMultiplicity());

            //association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            //association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourcePropertyTypeName = inverse.PropertyInfo.PropertyType.Name;
            association.SourcePropertyName = inverse.Name;
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.GetSourceMultiplicity());

            //association.SourceSummary = navigationProperty.FromEndMember.Documentation?.Summary;
            //association.SourceDescription = navigationProperty.FromEndMember.Documentation?.LongDescription;

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
         //customAttributes.Remove("[System.SerializableAttribute()]");
         //customAttributes.Remove("[System.Runtime.InteropServices.ComVisibleAttribute((Boolean)True)]");
         //customAttributes.Remove("[__DynamicallyInvokableAttribute()]");
         //customAttributes.Remove("[System.Reflection.DefaultMemberAttribute(\"Chars\")]");
         //customAttributes.Remove("[System.Runtime.Versioning.NonVersionableAttribute()]");
         //customAttributes.Remove("[System.FlagsAttribute()]");

         return string.Join("", customAttributes);
      }

      private List<ModelUnidirectionalAssociation> GetUnidirectionalAssociations(IEntityType entityType)
      {
         List<ModelUnidirectionalAssociation> result = new List<ModelUnidirectionalAssociation>();

         foreach (INavigation navigationProperty in entityType.GetNavigations().Where(n => n.FindInverse() == null))
         {
            ModelUnidirectionalAssociation association = new ModelUnidirectionalAssociation();

            association.SourceClassName = navigationProperty.DeclaringType.Name;
            association.SourceClassNamespace = navigationProperty.DeclaringType.ClrType.Namespace;
            association.TargetClassName = navigationProperty.ClrType.Name;
            association.TargetClassNamespace = navigationProperty.ClrType.Namespace;

            // the property in the source class (referencing the target class)
            association.TargetPropertyTypeName = navigationProperty.PropertyInfo.PropertyType.Name;
            association.TargetPropertyName = navigationProperty.Name;
            association.TargetMultiplicity = ConvertMultiplicity(navigationProperty.GetTargetMultiplicity());

            //association.TargetSummary = navigationProperty.ToEndMember.Documentation?.Summary;
            //association.TargetDescription = navigationProperty.ToEndMember.Documentation?.LongDescription;

            // the property in the target class (referencing the source class)
            association.SourceMultiplicity = ConvertMultiplicity(navigationProperty.GetSourceMultiplicity());

            //association.SourceSummary = navigationProperty.FromEndMember.Documentation?.Summary;
            //association.SourceDescription = navigationProperty.FromEndMember.Documentation?.LongDescription;

            result.Add(association);
         }

         return result;
      }

      public string Process()
      {
         if (dbContext == null)

            // ReSharper disable once NotResolvedInText
            throw new ArgumentNullException("dbContext");

         model = dbContext.Model;

         ModelRoot modelRoot = ProcessRoot();

         List<ModelClass> modelClasses = model.GetEntityTypes()
                                              .Select(type => ProcessEntity(type, modelRoot))
                                              .Where(x => x != null)
                                              .ToList();

         modelRoot.Classes.AddRange(modelClasses);

         return JsonConvert.SerializeObject(modelRoot);
      }

      private ModelClass ProcessEntity(IEntityType entityType, ModelRoot modelRoot)
      {
         ModelClass result = new ModelClass();

         result.Name = entityType.Name;
         result.Namespace = entityType.ClrType.Namespace;
         result.IsAbstract = entityType.ClrType.IsAbstract;

         result.BaseClass = entityType.ClrType.BaseType.Name == "System.Object"
                               ? null
                               : entityType.ClrType.BaseType.Name;

         result.TableName = dbContext.Model.FindEntityType(entityType.ClrType).Relational().TableName;
         result.IsDependentType = entityType.IsOwned();
         result.CustomAttributes = GetCustomAttributes(entityType.ClrType.CustomAttributes);

         result.CustomInterfaces = entityType.ClrType.GetInterfaces().Any()
                                      ? string.Join(",", entityType.ClrType.GetInterfaces().Select(t => t.FullName))
                                      : null;

         result.Properties = entityType.GetProperties().Where(p => !p.IsShadowProperty).Select(p => ProcessProperty(p, modelRoot)).Where(x => x != null).ToList();
         result.UnidirectionalAssociations = GetUnidirectionalAssociations(entityType);
         result.BidirectionalAssociations = GetBidirectionalAssociations(entityType);

         return result;
      }

      private void ProcessEnum(Type enumType, ModelRoot modelRoot)
      {
         FlagsAttribute flagsAttr = enumType.GetTypeInfo().GetCustomAttribute(typeof(FlagsAttribute)) as FlagsAttribute;
         string customAttributes = GetCustomAttributes(enumType);

         ModelEnum result = new ModelEnum();
         result.Name = enumType.Name;
         result.Namespace = enumType.Namespace;

         if (modelRoot.Enumerations.All(e => e.FullName != result.FullName))
         {
            result.IsFlags = flagsAttr != null;
            result.ValueType = Enum.GetUnderlyingType(enumType).Name;

            result.CustomAttributes = customAttributes.Length > 2
                                         ? customAttributes
                                         : null;

            result.Values = Enum.GetNames(enumType)
                                .Select(name => new ModelEnumValue {Name = name, Value = Enum.Parse(enumType, name).ToString()})
                                .ToList();

            modelRoot.Enumerations.Add(result);
         }
      }

      private ModelProperty ProcessProperty(IProperty propertyData, ModelRoot modelRoot)
      {
         Type type = propertyData.ClrType;

         List<CustomAttributeData> attributes = type.CustomAttributes.ToList();

         ModelProperty result = new ModelProperty();

         if (propertyData.ClrType.IsEnum)
            ProcessEnum(propertyData.ClrType, modelRoot);

         result.TypeName = propertyData.ClrType.Name;
         result.Name = propertyData.Name;
         result.IsIdentity = propertyData.IsKey();
         result.Required = !propertyData.IsNullable;
         result.Indexed = propertyData.IsIndex();

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

      private ModelRoot ProcessRoot()
      {
         ModelRoot result = new ModelRoot();
         Type contextType = dbContext.GetType();

         result.Namespace = contextType.Namespace;

         return result;
      }
   }
}