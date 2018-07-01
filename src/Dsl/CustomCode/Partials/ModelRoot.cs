using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelRoot
   {
      public static readonly PluralizationService PluralizationService;

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

      #region Nuget

      public NuGetDisplay NuGetPackageVersion
      {
         get
         {
            return NuGetHelper.NuGetPackageDisplay.FirstOrDefault(x => x.EFVersion == EntityFrameworkVersion && 
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
