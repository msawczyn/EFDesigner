using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Nuget;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelRoot
   {
      public static readonly PluralizationService PluralizationService;

      public static string[] EFVersions { get; }
      public static string[] EFCoreVersions { get; }
      private static readonly HttpClient client = new HttpClient();
      private static string nugetURL = "https://api-v2v3search-0.nuget.org/query?q={0}&prerelease=false";

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

         EFVersions = GetVersions("entityframework");
         EFCoreVersions = GetVersions("microsoft.entityframeworkcore");
      }

      private static string[] GetVersions(string packageId)
      {
         string jsonString = client.GetAsync(string.Format(nugetURL, packageId)).Result.Content.ReadAsStringAsync().Result;
         NugetPackages nugetPackages = NugetPackages.FromJson(jsonString);
         string id = packageId.ToLower();

         return nugetPackages.Data
                             .Where(x => x.Title.ToLower() == id)
                             .SelectMany(x => x.Versions)
                             .OrderBy(v => v.VersionVersion)
                             .Select(v => v.VersionVersion)
                             .ToArray();
      }

      internal double EFPackageVersionNum
      {
         get
         {
            string ver = EntityFrameworkPackageVersion;

            if (ver == "Latest")
               ver = EntityFrameworkVersion == EFVersion.EF6
                        ? EFVersions.Last()
                        : EFCoreVersions.Last();

            return double.TryParse(ver, out double result)
                      ? result
                      : 0;
         }
      }

      /// <summary>
      /// Package might set this to false depending on whether or not it can find the resources needed to load Nuget packages
      /// </summary>
      internal bool CanLoadNugetPackages { get; set; } = true;

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void ConnectionStringMustExist(ValidationContext context)
      {
         if (!Types.OfType<ModelRoot>().Any() && !Types.OfType<ModelEnum>().Any())
            return;

         if (string.IsNullOrEmpty(ConnectionString) && string.IsNullOrEmpty(ConnectionStringName))
            context.LogWarning("Model: Default connection string missing", "MRWConnectionString", this);

         if (string.IsNullOrEmpty(EntityContainerName))
            context.LogError("Model: Entity container needs a name", "MREContainerNameEmpty", this);
      }

      #region DatabaseSchema tracking property

      protected virtual void OnDatabaseSchemaChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Types, ModelClass.DatabaseSchemaDomainPropertyId, ModelClass.IsDatabaseSchemaTrackingDomainPropertyId);
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

      #region Namespace tracking property

      protected virtual void OnNamespaceChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Types, ModelClass.NamespaceDomainPropertyId, ModelClass.IsNamespaceTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Types, ModelEnum.NamespaceDomainPropertyId, ModelEnum.IsNamespaceTrackingDomainPropertyId);
      }

      internal sealed partial class NamespacePropertyHandler
      {
         protected override void OnValueChanged(ModelRoot element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnNamespaceChanged(oldValue, newValue);
         }
      }

      #endregion Namespace tracking property

      #region DefaultCollectionClass tracking property

      protected virtual void OnCollectionClassChanged(string oldValue, string newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, Types, Association.CollectionClassDomainPropertyId, Association.IsCollectionClassTrackingDomainPropertyId);
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
   }
}
