using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

using Sawczyn.EFDesigner.EFModel.Annotations;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class Association : IDisplaysWarning, IHasStore
   {
      #region Value changed handlers

      //partial class FKPropertyNamePropertyHandler
      //{
      //   protected override void OnValueChanged(Association element, string oldValue, string newValue)
      //   {
      //      base.OnValueChanged(element, oldValue, newValue);

      //      ModelRoot modelRoot = element.Store.ModelRoot();

      //      if (!element.Store.InUndoRedoOrRollback)
      //      {
      //         if (modelRoot.EntityFrameworkVersion == EFVersion.EF6 && element.SourceMultiplicity != Multiplicity.ZeroMany && element.TargetMultiplicity != Multiplicity.ZeroMany && newValue != null)
      //         {
      //            element.FKPropertyName = null;
      //            AssociationChangedRules.FixupForeignKeys(element);
      //         }
      //      }
      //   }

      //}

      #endregion

      /// <summary>
      /// Gets a human-readable value for source multiplicity
      /// </summary>
      /// <returns></returns>
      public string GetSourceMultiplicityDisplayValue() => MultiplicityDisplayValue(SourceMultiplicity);

      /// <summary>
      /// Gets a human-readable value for target multiplicity
      /// </summary>
      /// <returns></returns>
      public string GetTargetMultiplicityDisplayValue() => MultiplicityDisplayValue(TargetMultiplicity);

      public bool LinksDependentType => Source.IsDependentType || Target.IsDependentType;

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

      /// <summary>
      /// Gets the principal ModelClass of this association, if any
      /// </summary>
      public ModelClass Principal
      {
         get
         {
            return SourceRole == EndpointRole.Principal
                      ? Source
                      : TargetRole == EndpointRole.Principal
                         ? Target
                         : null;
         }
      }

      /// <summary>
      /// Gets the dependent ModelClass of this association, if any
      /// </summary>
      public ModelClass Dependent
      {
         get
         {
            return SourceRole == EndpointRole.Dependent
                      ? Source
                      : TargetRole == EndpointRole.Dependent
                         ? Target
                         : null;
         }
      }

      /// <summary>
      /// Gets the individual foreign key property names defined in the FKPropertyName property
      /// </summary>
      public string[] GetForeignKeyPropertyNames()
      {
         return FKPropertyName?.Split(',').Select(n => n.Trim()).ToArray() ?? new string[0];
      }

      /// <summary>
      /// Short display text for this attribute
      /// </summary>
      public virtual string GetDisplayText()
      {
         return $"{Source.Name}.{TargetPropertyName} --> {Target.Name}";
      }

      internal string TargetBackingFieldNameDefault => string.IsNullOrEmpty(TargetPropertyName) ? string.Empty : $"_{TargetPropertyName.Substring(0, 1).ToLowerInvariant()}{TargetPropertyName.Substring(1)}";

#pragma warning disable 1591
      internal string _targetBackingFieldName;
      protected string GetTargetBackingFieldNameValue() => string.IsNullOrEmpty(_targetBackingFieldName) ? TargetBackingFieldNameDefault : _targetBackingFieldName;
      protected void SetTargetBackingFieldNameValue(string value) => _targetBackingFieldName = value;
#pragma warning restore 1591

      private string GetNameValue()
      {
         return GetDisplayText();
      }

      internal string GetSummaryBoilerplate()
      {
         return $"Foreign key for {GetDisplayText()}";
      }

      private string GetTargetPropertyNameDisplayValue()
      {
         return SourceRole == EndpointRole.Dependent && !string.IsNullOrWhiteSpace(FKPropertyName) && Source.ModelRoot.ShowForeignKeyPropertyNames
                   ? $"{TargetPropertyName}\n[{string.Join(", ", GetForeignKeyPropertyNames().Select(n => $"{Source.Name}.{n.Trim()}"))}]"
                   : TargetPropertyName;
      }

      #region Warning display

      // set as methods to avoid issues around serialization

      /// <summary>
      /// If true, this association has a warning and might show a glyph in the diagram
      /// </summary>
      protected bool hasWarning;

      /// <inheritdoc />
      public bool GetHasWarningValue() => hasWarning;

      /// <inheritdoc />
      public void ResetWarning() => hasWarning = false;

      /// <inheritdoc />
      public void RedrawItem()
      {
         ModelElement[] modelElements = { this, Source, Target };

         // redraw on every diagram
         foreach (ShapeElement shapeElement in
               modelElements.SelectMany(modelElement => PresentationViewsSubject.GetPresentation(modelElement)
                                                                                .OfType<ShapeElement>()
                                                                                .Distinct()))
            shapeElement.Invalidate();
      }

      #endregion

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
      private void UnidirectionalManyToManyUnsupported(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.IsEFCore5Plus
          && this is UnidirectionalAssociation
          && SourceMultiplicity == Multiplicity.ZeroMany
          && TargetMultiplicity == Multiplicity.ZeroMany)
            context.LogError($"{Source.Name} <=> {Target.Name}: Many-to-many unidirectional associations are not yet supported in Entity Framework Core.", "AEUnidirectionalManyToMany", this);
      }

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

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Load | ValidationCategories.Menu)]
      [UsedImplicitly]
      private void MustDetermineEndpointRoles(ValidationContext context)
      {
         if (Source?.ModelRoot == null)
            return;

         if (Source != null
          && Target != null
          && (SourceRole == EndpointRole.NotSet || TargetRole == EndpointRole.NotSet)
          && ((SourceMultiplicity == Multiplicity.One && TargetMultiplicity == Multiplicity.One)
           || (SourceMultiplicity == Multiplicity.ZeroOne && TargetMultiplicity == Multiplicity.ZeroOne)))
            context.LogError($"{Source.Name} <=> {Target.Name}: Principal/dependent designations must be manually set for 1..1 and 0-1..0-1 associations.", "AEEndpointRoles", this);
      }

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Load | ValidationCategories.Menu)]
      [UsedImplicitly]
      private void FKPropertiesCannotBeStoreGeneratedIdentifiers(ValidationContext context)
      {

         foreach (ModelAttribute attribute in GetFKAutoIdentityErrors())
         {
            context.LogError($"{Source.Name} <=> {Target.Name}: FK property {attribute.Name} in {Dependent.FullName} is an auto-generated identity. Migration will fail."
                           , "AEIdentityFK"
                           , this);
         }
      }

      internal IEnumerable<ModelAttribute> GetFKAutoIdentityErrors()
      {
         if (string.IsNullOrWhiteSpace(FKPropertyName))
            return new ModelAttribute[0];

         return FKPropertyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                              .Select(name => Dependent.Attributes.FirstOrDefault(a => a.Name == name.Trim()))
                              .Where(a => a != null && a.IsIdentity && a.IdentityType == IdentityType.AutoGenerated)
                              .ToList();
      }

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Load | ValidationCategories.Menu)]
      [UsedImplicitly]
      private void FKPropertiesInvalidWithoutDependentEnd(ValidationContext context)
      {
         if (string.IsNullOrWhiteSpace(FKPropertyName))
            return;

         if (Dependent == null)
         {
            context.LogError($"{Source.Name} <=> {Target.Name}: FK property set without association having a Dependent end."
                           , "AEFKWithNoDependent"
                           , this);
         }
      }

      [ValidationMethod(ValidationCategories.Save | ValidationCategories.Load | ValidationCategories.Menu)]
      [UsedImplicitly]
      private void FKPropertiesMatchIdentityProperties(ValidationContext context)
      {
         if (string.IsNullOrWhiteSpace(FKPropertyName) || Principal == null)
            return;

         int identityCount = Principal.AllIdentityAttributes.Count();
         int fkCount = GetForeignKeyPropertyNames().Length;

         if (fkCount != identityCount)
         {
            context.LogError($"{Source.Name} <=> {Target.Name}: Wrong number of FK properties. Should be {identityCount} to match identity count in {Principal.Name} - currently is {fkCount}."
                           , "AEFKWrongCount"
                           , this);
         }
      }

      #region TargetImplementNotify tracking property

      /// <summary>Storage for the TargetImplementNotify property.</summary>  
      private bool targetImplementNotifyStorage;

      /// <summary>Gets the storage for the TargetImplementNotify property.</summary>
      /// <returns>The TargetImplementNotify value.</returns>
      public bool GetTargetImplementNotifyValue()
      {
         if (!this.IsLoading() && IsTargetImplementNotifyTracking)
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

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
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
         if (!this.IsLoading() && IsCollectionClassTracking)
         {
            try
            {
               return Source?.ModelRoot?.DefaultCollectionClass;
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

         return collectionClassStorage;
      }

      private void SetCollectionClassValue(string value)
      {
         collectionClassStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
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

      #region TargetAutoProperty tracking property

      private bool targetAutoPropertyStorage;

      private bool GetTargetAutoPropertyValue()
      {
         if (!this.IsLoading() && IsTargetAutoPropertyTracking)
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

         return targetAutoPropertyStorage;
      }

      private void SetTargetAutoPropertyValue(bool value)
      {
         targetAutoPropertyStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsTargetAutoPropertyTracking = (value == Source.AutoPropertyDefault);
      }

      internal sealed partial class IsTargetAutoPropertyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsTargetAutoPropertyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(Association element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(TargetAutoPropertyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsTargetAutoPropertyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(Association element)
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

            if (calculatedValue != null && element.TargetAutoProperty == (bool)calculatedValue)
               element.isTargetAutoPropertyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsTargetAutoPropertyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(Association element) =>
            // Force the IsTargetAutoPropertyTracking property to false so that the value  
            // of the TargetAutoProperty property is retrieved from storage.  
            element.isTargetAutoPropertyTrackingPropertyStorage = false;
      }

      #endregion TargetAutoProperty tracking property


      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal virtual void PreResetIsTrackingProperties()
      {
         IsCollectionClassTrackingPropertyHandler.Instance.PreResetValue(this);
         IsTargetImplementNotifyTrackingPropertyHandler.Instance.PreResetValue(this);
         IsTargetAutoPropertyTrackingPropertyHandler.Instance.PreResetValue(this);
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
         IsTargetAutoPropertyTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }

      internal sealed partial class SourceMultiplicityPropertyHandler
      {
         /// <summary>Called after property value has been changed.</summary>
         /// <param name="element">Element which owns the property.</param>
         /// <param name="oldValue">Old value of the property.</param>
         /// <param name="newValue">New value of the property.</param>
         protected override void OnValueChanged(Association element, Multiplicity oldValue, Multiplicity newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback)
               element.Store.DomainDataDirectory.GetDomainProperty(SourceMultiplicityDisplayDomainPropertyId).NotifyValueChange(element);
         }
      }

      internal sealed partial class TargetMultiplicityPropertyHandler
      {
         /// <summary>Called after property value has been changed.</summary>
         /// <param name="element">Element which owns the property.</param>
         /// <param name="oldValue">Old value of the property.</param>
         /// <param name="newValue">New value of the property.</param>
         protected override void OnValueChanged(Association element, Multiplicity oldValue, Multiplicity newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback)
               element.Store.DomainDataDirectory.GetDomainProperty(TargetMultiplicityDisplayDomainPropertyId).NotifyValueChange(element);
         }
      }

   }
}
