using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Extensions;

#pragma warning disable 1591

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelRoot : IHasStore
   {
      public static readonly PluralizationService PluralizationService;

      internal static bool BatchUpdating = false;

      public static Action ExecuteValidator { get; set; }

      public static Func<Diagram> GetCurrentDiagram;

      public static Func<bool> WriteDiagramAsBinary = () => false;

      static ModelRoot()
      {
         try
         {
            PluralizationService = PluralizationService.CreateService(CultureInfo.CurrentCulture);
         }
         catch (NotImplementedException)
         {
            PluralizationService = null;
         }
      }


      // ReSharper disable once UnusedMember.Global
      public string FullName => string.IsNullOrWhiteSpace(Namespace) ? $"global::{EntityContainerName}" : $"global::{Namespace}.{EntityContainerName}";

      /// <summary>
      /// True if the model is EFCore and the Entity Framework version is >= 5
      /// </summary>
      public bool IsEFCore5Plus => EntityFrameworkVersion == EFVersion.EFCore && GetEntityFrameworkPackageVersionNum() >= 5;

      [Obsolete("Use ModelRoot.Classes instead")]
      public LinkedElementCollection<ModelClass> Types => Classes;

      public EFModelDiagram[] GetDiagrams()
      {
         return Store
               .DefaultPartitionForClass(EFModelDiagram.DomainClassId)
               .ElementDirectory
               .AllElements
               .OfType<EFModelDiagram>()
               .ToArray();
      }

      #region OutputLocations

      private OutputLocations outputLocationsStorage;

      private OutputLocations GetOutputLocationsValue()
      {
         return outputLocationsStorage ?? (outputLocationsStorage = new OutputLocations(this));
      }

      private void SetOutputLocationsValue(OutputLocations value)
      {
         outputLocationsStorage = value;
      }

      #endregion OutputLocations

      #region Namespaces

      private Namespaces namespacesStorage;

      private Namespaces GetNamespacesValue()
      {
         return namespacesStorage ?? (namespacesStorage = new Namespaces(this));
      }

      private void SetNamespacesValue(Namespaces value)
      {
         namespacesStorage = value;
      }

      #endregion Namespaces

      #region Valid types based on EF version

      public string[] SpatialTypes
      {
         get
         {
            return EntityFrameworkVersion == EFVersion.EF6
                         ? new[]
                           {
                                 "Geography"
                               , "GeographyCollection"
                               , "GeographyLineString"
                               , "GeographyMultiLineString"
                               , "GeographyMultiPoint"
                               , "GeographyMultiPolygon"
                               , "GeographyPoint"
                               , "GeographyPolygon"
                               , "Geometry"
                               , "GeometryCollection"
                               , "GeometryLineString"
                               , "GeometryMultiLineString"
                               , "GeometryMultiPoint"
                               , "GeometryMultiPolygon"
                               , "GeometryPoint"
                               , "GeometryPolygon"
                           }
                         : new[]
                           {
                                 "Geometry"
                               , "GeometryCollection"
                               , "LineString"
                               , "MultiLineString"
                               , "MultiPoint"
                               , "MultiPolygon"
                               , "Point"
                               , "Polygon"
                           };
         }
      }

      /// <summary>
      /// Class types that can be used in the model
      /// </summary>
      public string[] ValidTypes
      {
         get
         {
            List<string> validTypes = new List<string>(new[]
                                                       {
                                                          "Binary"
                                                        , "Boolean"
                                                        , "Byte"
                                                        , "byte"
                                                        , "DateTime"
                                                        , "DateTimeOffset"
                                                        , "Decimal"
                                                        , "Double"
                                                        , "Guid"
                                                       });

            if (IsEFCore5Plus)
               validTypes.Add("System.Net.IPAddress");

            validTypes.AddRange(new[]
                                {
                                   "Int16"
                                 , "Int32"
                                 , "Int64"
                                 , "Single"
                                 , "String"
                                 , "Time"
                                });

            return validTypes.Union(SpatialTypes).ToArray();
         }
      }

      /// <summary>
      /// CLR Types that can be used in the model
      /// </summary>
      public string[] ValidCLRTypes
      {
         get
         {
            List<string> validClrTypes = new List<string>(new[]
                                                          {
                                                             "Binary",
                                                             "Boolean", "Boolean?", "Nullable<Boolean>",
                                                             "Byte", "Byte?", "Nullable<Byte>",
                                                             "DateTime", "DateTime?", "Nullable<DateTime>",
                                                             "DateTimeOffset", "DateTimeOffset?", "Nullable<DateTimeOffset>",
                                                             "DbGeography",
                                                             "DbGeometry",
                                                             "Decimal", "Decimal?", "Nullable<Decimal>",
                                                             "Double", "Double?", "Nullable<Double>",
                                                             "Guid", "Guid?", "Nullable<Guid>"
                                                          });
            if (IsEFCore5Plus)
               validClrTypes.Add("System.Net.IPAddress");

            validClrTypes.AddRange(new[]
                                   {
                                      "Int16", "Int16?", "Nullable<Int16>",
                                      "Int32", "Int32?", "Nullable<Int32>",
                                      "Int64", "Int64?", "Nullable<Int64>",
                                      "Single", "Single?", "Nullable<Single>",
                                      "String",
                                      "Time",
                                      "TimeSpan", "TimeSpan?", "Nullable<TimeSpan>",
                                      "bool", "bool?", "Nullable<bool>",
                                      "byte", "byte?", "Nullable<byte>",
                                      "byte[]",
                                      "decimal", "decimal?", "Nullable<decimal>",
                                      "double", "double?", "Nullable<double>",
                                      "int", "int?", "Nullable<int>",
                                      "long", "long?", "Nullable<long>",
                                      "short", "short?", "Nullable<short>",
                                      "string"
                                   });

            return validClrTypes.Union(SpatialTypes).ToArray();
         }
      }

      /// <summary>
      /// Validates that the type in question can be used as an identity.
      /// EF6 is constrained as to identity types, as is EFCore before v5.
      /// EFCore v5+ has no constraints, other than what's put on by the database type
      /// </summary>
      /// <param name="typename">Name of type to check for use as identity</param>
      /// <returns>True if valid, false otherwise</returns>
      public bool IsValidIdentityAttributeType(string typename)
      {
         return IsEFCore5Plus || ValidIdentityAttributeTypes.Contains(typename);
      }

      public string[] ValidIdentityAttributeTypes
      {
         get
         {
            List<string> baseResult = ValidIdentityTypeAttributesBaseList;

            baseResult.AddRange(Store.ElementDirectory
                                     .AllElements
                                     .OfType<ModelEnum>()
                                     .Where(e => baseResult.Contains(e.ValueType.ToString()))
                                     .Select(e => e.Name)
                                     .OrderBy(n => n));

            return baseResult.ToArray();
         }
      }

      private static List<string> ValidIdentityTypeAttributesBaseList
      {
         get
         {
            return new List<string>
                   {
                      "Int16",
                      "Int32",
                      "Int64",
                      "Byte",
                      "String",
                      "Guid"
                   };
         }
      }

      public bool IsValidCLRType(string type)
      {
         return ValidCLRTypes.Contains(type);
      }

      #endregion

      #region Nuget

      public NuGetDisplay NuGetPackageVersion
      {
         get
         {
            return NuGetHelper.NuGetPackageDisplay.FirstOrDefault(x => x.EFVersion == EntityFrameworkVersion &&
                                                                           x.DisplayVersion == EntityFrameworkPackageVersion);
         }
      }

      public double GetEntityFrameworkPackageVersionNum()
      {
         string[] parts = EntityFrameworkPackageVersion.Split('.');

         string resultString = parts.Length > 1
                                  ? $"{parts[0]}.{parts[1]}"
                                  : parts.FirstOrDefault();

         return double.TryParse(resultString, out double result)
                   ? result
                   : 0;
      }

      #endregion Nuget

      #region Validation methods

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void ConnectionStringMustExist(ValidationContext context)
      {
         if (!Classes.Any() && !Enums.Any())
            return;

         if (string.IsNullOrEmpty(ConnectionString) && string.IsNullOrEmpty(ConnectionStringName))
            context.LogWarning("Model: Default connection string missing", "MRWConnectionString", this);

         if (string.IsNullOrEmpty(EntityContainerName))
            context.LogError("Model: Entity container needs a name", "MREContainerNameEmpty", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (string.IsNullOrWhiteSpace(Summary) && WarnOnMissingDocumentation)
            context.LogWarning("Model: Summary documentation missing", "AWMissingSummary", this);
      }

      #endregion Validation methods

      #region DatabaseSchema tracking property

      protected virtual void OnDatabaseSchemaChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Classes,
                                                         ModelClass.DatabaseSchemaDomainPropertyId,
                                                         ModelClass.IsDatabaseSchemaTrackingDomainPropertyId);
      }

      internal sealed partial class DatabaseSchemaPropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnDatabaseSchemaChanged(oldValue, newValue);
         }
      }

      #endregion DatabaseSchema tracking property

      #region DatabaseCollationDefault tracking property

      protected virtual void OnDatabaseCollationDefaultChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Classes,
                                                         ModelAttribute.DatabaseCollationDomainPropertyId,
                                                         ModelAttribute.IsDatabaseCollationTrackingDomainPropertyId);
      }

      internal sealed partial class DatabaseCollationDefaultPropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnDatabaseCollationDefaultChanged(oldValue, newValue);
         }
      }

      #endregion DatabaseCollationDefault tracking property

      #region DefaultCollectionClass tracking property

      protected virtual void OnCollectionClassChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Store.GetAll<Association>().ToList(),
                                                         Association.CollectionClassDomainPropertyId,
                                                         Association.IsCollectionClassTrackingDomainPropertyId);
      }

      internal sealed partial class DefaultCollectionClassPropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnCollectionClassChanged(oldValue, newValue);
         }
      }

      #endregion DefaultCollectionClass tracking property

      #region Entity Output Directory tracking property

      protected virtual void OnEntityOutputDirectoryChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Classes, ModelClass.OutputDirectoryDomainPropertyId, ModelClass.IsOutputDirectoryTrackingDomainPropertyId);
      }

      internal sealed partial class EntityOutputDirectoryPropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnEntityOutputDirectoryChanged(oldValue, newValue);
         }
      }

      #endregion

      #region Enum Output Directory tracking property

      protected virtual void OnEnumOutputDirectoryChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Classes, ModelEnum.OutputDirectoryDomainPropertyId, ModelEnum.IsOutputDirectoryTrackingDomainPropertyId);
      }

      internal sealed partial class EnumOutputDirectoryPropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnEnumOutputDirectoryChanged(oldValue, newValue);
         }
      }

      #endregion

      #region Namespace tracking property

      internal sealed partial class NamespacePropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
            {
               if (string.IsNullOrWhiteSpace(element.EntityNamespace))
               {
                  TrackingHelper.UpdateTrackingCollectionProperty(element.Store
                                                                , element.Classes.Where(c => !c.IsDependentType)
                                                                , ModelClass.NamespaceDomainPropertyId
                                                                , ModelClass.IsNamespaceTrackingDomainPropertyId);
               }

               if (string.IsNullOrWhiteSpace(element.StructNamespace))
               {
                  TrackingHelper.UpdateTrackingCollectionProperty(element.Store
                                                                , element.Classes.Where(c => c.IsDependentType)
                                                                , ModelClass.NamespaceDomainPropertyId
                                                                , ModelClass.IsNamespaceTrackingDomainPropertyId);
               }

               if (string.IsNullOrWhiteSpace(element.EnumNamespace))
                  TrackingHelper.UpdateTrackingCollectionProperty(element.Store, element.Enums, ModelEnum.NamespaceDomainPropertyId, ModelEnum.IsNamespaceTrackingDomainPropertyId);
            }
         }
      }

      internal sealed partial class EntityNamespacePropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
            {
               TrackingHelper.UpdateTrackingCollectionProperty(element.Store,
                                                               element.Classes.Where(c => !c.IsDependentType),
                                                               ModelClass.NamespaceDomainPropertyId,
                                                               ModelClass.IsNamespaceTrackingDomainPropertyId);
            }
         }
      }

      internal sealed partial class EnumNamespacePropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               TrackingHelper.UpdateTrackingCollectionProperty(element.Store, element.Enums, ModelEnum.NamespaceDomainPropertyId, ModelEnum.IsNamespaceTrackingDomainPropertyId);
         }
      }

      internal sealed partial class StructNamespacePropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
            {
               TrackingHelper.UpdateTrackingCollectionProperty(element.Store
                                                             , element.Classes.Where(c => c.IsDependentType)
                                                             , ModelClass.NamespaceDomainPropertyId
                                                             , ModelClass.IsNamespaceTrackingDomainPropertyId);
            }
         }
      }

      #endregion Namespace tracking property

      private string filename;

      public void SetFileName(string fileName)
      {
         filename = fileName;
      }

      public string GetFileName()
      {
         return filename;
      }
   }
}
