using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelRoot
   {
      public static readonly PluralizationService PluralizationService = PluralizationService.CreateService(CultureInfo.CurrentCulture);

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void ConnectionStringMustExist(ValidationContext context)
      {
         if (!Types.OfType<ModelRoot>().Any() && !Types.OfType<ModelEnum>().Any())
            return;

         if (string.IsNullOrEmpty(ConnectionString) && string.IsNullOrEmpty(ConnectionStringName))
            context.LogWarning("Default connection string missing", "MRWConnectionString", this);

         if (string.IsNullOrEmpty(EntityContainerName))
            context.LogError("Entity container needs a name", "MREContainerNameEmpty", this);
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
