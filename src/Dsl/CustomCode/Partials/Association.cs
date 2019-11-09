using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class Association : IDisplaysWarning, IHasStore
   {
      public string GetSourceMultiplicityDisplayValue() => MultiplicityDisplayValue(SourceMultiplicity);

      public string GetTargetMultiplicityDisplayValue() => MultiplicityDisplayValue(TargetMultiplicity);

      private static string MultiplicityDisplayValue(Multiplicity multiplicity)
      {
         switch (multiplicity)
         {
            case Multiplicity.One:
               return "1";
            //case Multiplicity.OneMany:
            //   return "1..*";
            case Multiplicity.ZeroMany:
               return "*";
            case Multiplicity.ZeroOne:
               return "0..1";
         }

         return "?";
      }

      //internal static (EndpointRole sourceRole, EndpointRole targetRole)? GetEndpointRoles(Multiplicity sourceMultiplicity, Multiplicity targetMultiplicity)
      //{
      //   switch (targetMultiplicity)
      //   {
      //      case Multiplicity.ZeroMany:

      //         switch (sourceMultiplicity)
      //         {
      //            case Multiplicity.ZeroMany:
      //               return (EndpointRole.NotApplicable, EndpointRole.NotApplicable);
      //            case Multiplicity.One:
      //               return (EndpointRole.Principal, EndpointRole.Dependent);
      //            case Multiplicity.ZeroOne:
      //               return (EndpointRole.Principal, EndpointRole.Dependent);
      //         }

      //         break;
      //      case Multiplicity.One:

      //         switch (sourceMultiplicity)
      //         {
      //            case Multiplicity.ZeroMany:
      //               return (EndpointRole.Dependent, EndpointRole.Principal);
      //            case Multiplicity.One:
      //               return null; // must be manually set
      //            case Multiplicity.ZeroOne:
      //               return (EndpointRole.Dependent, EndpointRole.Principal);
      //         }

      //         break;
      //      case Multiplicity.ZeroOne:

      //         switch (sourceMultiplicity)
      //         {
      //            case Multiplicity.ZeroMany:
      //               return (EndpointRole.Dependent, EndpointRole.Principal);
      //            case Multiplicity.One:
      //               return (EndpointRole.Principal, EndpointRole.Dependent);
      //            case Multiplicity.ZeroOne:
      //               return null; // must be manually set
      //         }

      //         break;
      //   }

      //   return null;
      //}


      #region Warning display

      // set as methods to avoid issues around serialization

      protected bool hasWarning;

      public bool GetHasWarningValue() => hasWarning;

      public void ResetWarning() => hasWarning = false;

      public void RedrawItem()
      {
         ModelElement[] modelElements = { this, Source, Target };

         List<ShapeElement> shapeElements =
            modelElements.SelectMany(modelElement => PresentationViewsSubject.GetPresentation(modelElement)
                                                                             .OfType<ShapeElement>())
                         .ToList();

         foreach (ShapeElement shapeElement in shapeElements)
            shapeElement.Invalidate();
      }

      #endregion

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (Source?.ModelRoot == null) return;

         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot?.WarnOnMissingDocumentation == true && Source != null && string.IsNullOrWhiteSpace(TargetSummary))
         {
            context.LogWarning($"{Source.Name}.{TargetPropertyName}: Association end should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
            Source.RedrawItem();
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void TPCEndpointsOnlyOnLeafNodes(ValidationContext context)
      {
         if (Source?.ModelRoot == null) return;

         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot?.InheritanceStrategy == CodeStrategy.TablePerConcreteType && 
             (Target?.Subclasses.Any() == true || Source?.Subclasses.Any() == true))
            context.LogError($"{Source.Name} <=> {Target.Name}: Association endpoints can only be to most-derived classes in TPC inheritance strategy", "AEWrongEndpoints", this);
      }

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void MustDetermineEndpointRoles(ValidationContext context)
      {
         if (Source?.ModelRoot == null) return;

         if (Source != null && Target != null && (SourceRole == EndpointRole.NotSet || TargetRole == EndpointRole.NotSet))
            context.LogError($"{Source.Name} <=> {Target.Name}: Principal/dependent designations must be manually set for 1..1 and 0-1..0-1 associations.", "AEEndpointRoles", this);
      }

      #region TargetImplementNotify tracking property

      /// <summary>Storage for the TargetImplementNotify property.</summary>  
      private bool targetImplementNotifyStorage;

      /// <summary>Gets the storage for the TargetImplementNotify property.</summary>
      /// <returns>The TargetImplementNotify value.</returns>
      public bool GetTargetImplementNotifyValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsTargetImplementNotifyTracking)
         {
            try
            {
               return Target?.ImplementNotify ?? false;
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

         return targetImplementNotifyStorage;
      }

      /// <summary>Sets the storage for the TargetImplementNotify property.</summary>
      /// <param name="value">The TargetImplementNotify value.</param>
      public void SetTargetImplementNotifyValue(bool value)
      {
         targetImplementNotifyStorage = value;
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            // ReSharper disable once ArrangeRedundantParentheses
            IsTargetImplementNotifyTracking = (targetImplementNotifyStorage == Target.ImplementNotify);
      }

      internal sealed partial class IsTargetImplementNotifyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsTargetImplementNotifyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(Association element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(TargetImplementNotifyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsTargetImplementNotifyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(Association element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.Target?.ImplementNotify;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.TargetImplementNotify == (bool)calculatedValue)
               element.isTargetImplementNotifyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsTargetImplementNotifyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(Association element)
         {
            // Force the IsTargetImplementNotifyTracking property to false so that the value  
            // of the TargetImplementNotify property is retrieved from storage.  
            element.isTargetImplementNotifyTrackingPropertyStorage = false;
         }
      }

      #endregion

      #region CollectionClass tracking property

      private string collectionClassStorage;

      private string GetCollectionClassValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsCollectionClassTracking)
         {
            try
            {
               return Source?.ModelRoot?.DefaultCollectionClass;
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
         }

         return collectionClassStorage;
      }

      private void SetCollectionClassValue(string value)
      {
         collectionClassStorage = value;

         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsCollectionClassTracking = (value == Source.ModelRoot.DefaultCollectionClass);
      }

      internal sealed partial class IsCollectionClassTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsCollectionClassTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(Association element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(CollectionClassDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsCollectionClassTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(Association element)
         {
            object calculatedValue = null;
            try
            {
               ModelRoot modelRoot = element.Store.ModelRoot();
               calculatedValue = modelRoot.DefaultCollectionClass;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.CollectionClass == (string)calculatedValue)
               element.isCollectionClassTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsCollectionClassTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(Association element) =>
            // Force the IsCollectionClassTracking property to false so that the value  
            // of the CollectionClass property is retrieved from storage.  
            element.isCollectionClassTrackingPropertyStorage = false;
      }

      #endregion CollectionClass tracking property

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal virtual void PreResetIsTrackingProperties()
      {
         IsCollectionClassTrackingPropertyHandler.Instance.PreResetValue(this);
         IsTargetImplementNotifyTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal virtual void ResetIsTrackingProperties()
      {
         IsCollectionClassTrackingPropertyHandler.Instance.ResetValue(this);
         IsTargetImplementNotifyTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }
   }
}
