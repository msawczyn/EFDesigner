using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Nuget;

namespace Sawczyn.EFDesigner.EFModel
{
   public class NuGetDisplay
   {
      public EFVersion EFVersion { get; }
      public string ActualPackageVersion { get; }
      public string DisplayVersion { get; }
      public string MajorMinorVersion { get; }

      public double MajorMinorVersionNum => double.TryParse(MajorMinorVersion, out double result)
                                               ? result
                                               : 0;

      public NuGetDisplay(EFVersion efVersion, string packageVersion, string display, string majorMinorVersion)
      {
         EFVersion = efVersion;
         ActualPackageVersion = packageVersion;
         DisplayVersion = display;
         MajorMinorVersion = majorMinorVersion;
      }
   }

   [ValidationState(ValidationState.Enabled)]
   public partial class ModelRoot
   {
      private static readonly HttpClient restClient = new HttpClient();
      private static string nugetURL = "https://api-v2v3search-0.nuget.org/query?q={0}&prerelease=false";

      internal static List<NuGetDisplay> NuGetPackageDisplay { get; }

      public static readonly PluralizationService PluralizationService;
      public static Dictionary<EFVersion, IEnumerable<string>> EFPackageVersions { get; }

      static ModelRoot()
      {
         EFPackageVersions = new Dictionary<EFVersion, IEnumerable<string>>();
         NuGetPackageDisplay = new List<NuGetDisplay>();

         LoadNuGetVersions(EFVersion.EF6, "entityframework");
         LoadNuGetVersions(EFVersion.EFCore, "microsoft.entityframeworkcore");

         try
         {
            PluralizationService = PluralizationService.CreateService(CultureInfo.CurrentCulture);
         }
         catch (NotImplementedException)
         {
            PluralizationService = null;
         }
      }

      #region Nuget

      private static void LoadNuGetVersions(EFVersion efVersion, string packageId)
      {
         // get NuGet packages with that package id
         string jsonString = restClient.GetAsync(string.Format(nugetURL, packageId)).Result.Content.ReadAsStringAsync().Result;
         NuGetPackages nugetPackages = NuGetPackages.FromJson(jsonString);
         string id = packageId.ToLower();

         // get their versions
         List<string> result = nugetPackages.Data
                                            .Where(x => x.Title.ToLower() == id)
                                            .SelectMany(x => x.Versions)
                                            .OrderBy(v => v.VersionVersion)
                                            .Select(v => v.VersionVersion)
                                            .ToList();

         // find the major.minor versions
         List<string> majorVersions = result.Select(v => v.Substring(0, v.LastIndexOf(".")))
                                            .OrderBy(v => v)
                                            .Distinct()
                                            .ToList();

         // do the trivial mapping of the full version to the full display name
         foreach (string v in result)
            NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, v, v, string.Join(".", v.Split('.').Take(2))));

         // figure out which one is the latest in the major.minor set and add its mapping
         foreach (string v in majorVersions)
            NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, result.FindLast(x => x.StartsWith($"{v}.")), $"{v}.Latest", v));

         // figure out which is the overall latest and map it
         NuGetPackageDisplay.Add(new NuGetDisplay(efVersion, result.FindLast(x => !x.EndsWith(".Latest")), "Latest", majorVersions.Last()));

         // tuck it away
         EFPackageVersions.Add(efVersion, result);
      }

      public NuGetDisplay NuGetPackageVersion
      {
         get
         {
            return NuGetPackageDisplay.FirstOrDefault(x => x.EFVersion == EntityFrameworkVersion && 
                                                                           x.DisplayVersion == EntityFrameworkPackageVersion);
         }
      }

      /// <summary>
      /// DslPackage might set this to false depending on whether or not it can find the resources needed to load Nuget packages
      /// </summary>
      public static bool CanLoadNugetPackages { get; set; } = true;

      // ReSharper disable once UnusedMember.Global
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
         if (!Types.OfType<ModelRoot>().Any() && !Types.OfType<ModelEnum>().Any())
            return;

         if (string.IsNullOrEmpty(ConnectionString) && string.IsNullOrEmpty(ConnectionStringName))
            context.LogWarning("Model: Default connection string missing", "MRWConnectionString", this);

         if (string.IsNullOrEmpty(EntityContainerName))
            context.LogError("Model: Entity container needs a name", "MREContainerNameEmpty", this);
      }

      #endregion Validation methods

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
