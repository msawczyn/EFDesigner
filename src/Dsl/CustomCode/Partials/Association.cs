using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class Association : IDisplaysWarning
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
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation && string.IsNullOrWhiteSpace(TargetSummary))
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
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot?.InheritanceStrategy == CodeStrategy.TablePerConcreteType && (Target.Subclasses.Any() || Source.Subclasses.Any()))
            context.LogError($"{Source.Name} <=> {Target.Name}: Association endpoints can only be to most-derived classes in TPC inheritance strategy", "AEWrongEndpoints", this);
      }

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void MustDetermineEndpointRoles(ValidationContext context)
      {
         if (SourceRole == EndpointRole.NotSet || TargetRole == EndpointRole.NotSet)
            context.LogError($"{Source.Name} <=> {Target.Name}: Principal/dependent designations must be manually set for 1..1 and 0-1..0-1 associations.", "AEEndpointRoles", this);
      }

      #region CollectionClass tracking property

      private string collectionClassStorage;

      private string GetCollectionClassValue()
      {
         Transaction transactionManagerCurrentTransaction = Store.TransactionManager.CurrentTransaction;
         bool loading = Store.TransactionManager.InTransaction && transactionManagerCurrentTransaction.IsSerializing;

         if (!loading && IsCollectionClassTracking)
            try
            {
               ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
               return modelRoot?.DefaultCollectionClass;
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

         return collectionClassStorage;
      }

      private void SetCollectionClassValue(string value)
      {
         collectionClassStorage = value;

         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsCollectionClassTracking = false;
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
               ModelRoot modelRoot = element.Store.ElementDirectory.AllElements.OfType<ModelRoot>().FirstOrDefault();
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
   }
}
