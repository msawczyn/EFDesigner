using System;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Annotations;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class BidirectionalAssociation
   {
      private string GetSourcePropertyNameDisplayValue()
      {
         return TargetRole == EndpointRole.Dependent && !string.IsNullOrWhiteSpace(FKPropertyName) && Target.ModelRoot.ShowForeignKeyPropertyNames
                   ? $"{SourcePropertyName}\n[{string.Join(", ", GetForeignKeyPropertyNames().Select(n => $"{Target.Name}.{n.Trim()}"))}]"
                   : SourcePropertyName;
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      [UsedImplicitly]
      [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Called by validation")]
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (Source?.ModelRoot == null) return;

         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot?.WarnOnMissingDocumentation == true && Target != null && string.IsNullOrWhiteSpace(SourceSummary))
         {
            context.LogWarning($"{Target.Name}.{SourcePropertyName}: Association end should be documented", "AWMissingSummary", this);
            hasWarning = true;
            this.Redraw();
         }
      }

      /// <summary>
      /// Short display text for this attribute
      /// </summary>
      public override string GetDisplayText()
      {
         string sourceAutoIncluded = SourceAutoInclude ? " (AutoInclude)" : string.Empty;
         string targetAutoIncluded = TargetAutoInclude ? " (AutoInclude)" : string.Empty;

         return $"{Source.Name}.{TargetPropertyName}{targetAutoIncluded} <--> {Target.Name}.{SourcePropertyName}{sourceAutoIncluded}";
      }

      internal string SourceBackingFieldNameDefault => string.IsNullOrEmpty(SourcePropertyName) ? string.Empty : $"_{SourcePropertyName.Substring(0, 1).ToLowerInvariant()}{SourcePropertyName.Substring(1)}";

      internal string _sourceBackingFieldName;
      private string GetSourceBackingFieldNameValue() => string.IsNullOrEmpty(_sourceBackingFieldName) ? SourceBackingFieldNameDefault : _sourceBackingFieldName;
      private void SetSourceBackingFieldNameValue(string value) => _sourceBackingFieldName = value;

      #region SourceImplementNotify tracking property

      /// <summary>Storage for the SourceImplementNotify property.</summary>  
      private bool sourceImplementNotifyStorage;

      /// <summary>Gets the storage for the SourceImplementNotify property.</summary>
      /// <returns>The SourceImplementNotify value.</returns>
      public bool GetSourceImplementNotifyValue()
      {
         if (!this.IsLoading() && IsCollectionClassTracking)
         {
            try
            {
               return Source.ImplementNotify;
            }
            catch (NullReferenceException)
            {
               return false;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return false;
            }
         }

         return sourceImplementNotifyStorage;
      }

      /// <summary>Sets the storage for the SourceImplementNotify property.</summary>
      /// <param name="value">The SourceImplementNotify value.</param>
      public void SetSourceImplementNotifyValue(bool value)
      {
         sourceImplementNotifyStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsSourceImplementNotifyTracking = (sourceImplementNotifyStorage == Source.ImplementNotify);
      }

      internal sealed partial class IsSourceImplementNotifyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsSourceImplementNotifyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(BidirectionalAssociation element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(SourceImplementNotifyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsSourceImplementNotifyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(BidirectionalAssociation element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.Source?.ImplementNotify;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.SourceImplementNotify == (bool)calculatedValue)
               element.isSourceImplementNotifyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsSourceImplementNotifyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(BidirectionalAssociation element)
         {
            // Force the IsSourceImplementNotifyTracking property to false so that the value  
            // of the SourceImplementNotify property is retrieved from storage.  
            element.isSourceImplementNotifyTrackingPropertyStorage = false;
         }
      }

      #endregion

      #region SourceAutoProperty tracking property

      private bool sourceAutoPropertyStorage;

      private bool GetSourceAutoPropertyValue()
      {
         if (!this.IsLoading() && IsSourceAutoPropertyTracking)
         {
            try
            {
               return Source?.AutoPropertyDefault ?? true;
            }
            catch (NullReferenceException)
            {
               return default;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return default;
            }
         }

         return sourceAutoPropertyStorage;
      }

      private void SetSourceAutoPropertyValue(bool value)
      {
         sourceAutoPropertyStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsSourceAutoPropertyTracking = (value == Source.AutoPropertyDefault);
      }

      internal sealed partial class IsSourceAutoPropertyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsSourceAutoPropertyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(BidirectionalAssociation element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(SourceAutoPropertyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsSourceAutoPropertyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(BidirectionalAssociation element)
         {
            object calculatedValue = null;
            try
            {
               calculatedValue = element.Source?.AutoPropertyDefault;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.Source.AutoPropertyDefault == (bool)calculatedValue)
               element.isSourceAutoPropertyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsSourceAutoPropertyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(BidirectionalAssociation element) =>
            // Force the IsSourceAutoPropertyTracking property to false so that the value  
            // of the SourceAutoProperty property is retrieved from storage.  
            element.isSourceAutoPropertyTrackingPropertyStorage = false;
      }

      #endregion SourceAutoProperty tracking property

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal override void PreResetIsTrackingProperties()
      {
         base.PreResetIsTrackingProperties();
         IsSourceImplementNotifyTrackingPropertyHandler.Instance.PreResetValue(this);
         IsSourceAutoPropertyTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal override void ResetIsTrackingProperties()
      {
         base.ResetIsTrackingProperties();
         IsSourceImplementNotifyTrackingPropertyHandler.Instance.ResetValue(this);
         IsSourceAutoPropertyTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }
   }
}
