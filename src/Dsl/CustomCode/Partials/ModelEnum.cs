using System;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelEnum : IModelElementWithCompartments
   {
      public static string[] ValidValueTypes = {/*"SByte", */"Int16", "Int32", "Int64"};

      public void SetFlagValues()
      {
         if (IsFlags)
         {
            for (int i = 0; i < Values.Count; i++)
               Values[i].Value = Math.Pow(2, i).ToString();
         }
      }

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void PreResetIsTrackingProperties()
      {
         IsNamespaceTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void ResetIsTrackingProperties()
      {
         IsNamespaceTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void EnumMustHaveValues(ValidationContext context)
      {
         if (!Values.Any())
            context.LogError($"{Name}: Enum has no values", "MEENoValues", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void EnumValueInitializationsShouldBeAllOrNothing(ValidationContext context)
      {
         if (Values.Any(x => !string.IsNullOrEmpty(x.Value)) && Values.Any(x => string.IsNullOrEmpty(x.Value)))
            context.LogWarning($"{Name}: Enum has some, but not all, values initialized. Please ensure this is what was intended.", "MWPartialEnumValueInitialization", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation)
         {
            if (string.IsNullOrWhiteSpace(Summary))
               context.LogWarning($"{Name}: Enum should be documented", "AWMissingSummary", this);
         }
      }
      #region Namespace tracking property

      private string namespaceStorage;

      private string GetNamespaceValue()
      {
         Transaction transactionManagerCurrentTransaction = Store.TransactionManager.CurrentTransaction;
         bool loading = Store.TransactionManager.InTransaction && transactionManagerCurrentTransaction.IsSerializing;

         if (!loading && IsNamespaceTracking)
            try
            {
               return ModelRoot.Namespace;
            }
            catch (NullReferenceException)
            {
               return default(string);
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return default(string);
            }

         return namespaceStorage;
      }

      private void SetNamespaceValue(string value)
      {
         namespaceStorage = value;

         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsNamespaceTracking = false;
      }

      internal sealed partial class IsNamespaceTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsNamespaceTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelEnum element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(NamespaceDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsDatabaseSchemaTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelEnum element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.ModelRoot.Namespace;
            }
            catch (NullReferenceException) {}
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.Namespace == (string)calculatedValue)
               element.isNamespaceTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsDatabaseSchemaTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(ModelEnum element)
         {
            // Force the IsDatabaseSchemaTracking property to false so that the value  
            // of the DatabaseSchema property is retrieved from storage.  
            element.isNamespaceTrackingPropertyStorage = false;
         }
      }

      #endregion Namespace tracking property
   }
}
