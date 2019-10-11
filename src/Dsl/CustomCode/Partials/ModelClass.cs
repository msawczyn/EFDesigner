using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

using Sawczyn.EFDesigner.EFModel.Extensions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelClass : IModelElementWithCompartments, IDisplaysWarning
   {
      /// <summary>
      /// All attributes in the class, including those inherited from base classes
      /// </summary>
      public IEnumerable<ModelAttribute> AllAttributes
      {
         get
         {
            List<ModelAttribute> result = Attributes.ToList();
            if (Superclass != null)
               result.AddRange(Superclass.AllAttributes);
            return result;
         }
      }

      public IEnumerable<ModelAttribute> RequiredAttributes
      {
         get
         {
            return Attributes.Where(x => x.Required).ToList();
         }
      }

      public IEnumerable<ModelAttribute> AllRequiredAttributes
      {
         get
         {
            return AllAttributes.Where(x => x.Required).ToList();
         }
      }

      public IEnumerable<ModelAttribute> IdentityAttributes
      {
         get
         {
            return Attributes.Where(x => x.IsIdentity).ToList();
         }
      }

      public IEnumerable<ModelAttribute> AllIdentityAttributes
      {
         get
         {
            return AllAttributes.Where(x => x.IsIdentity).ToList();
         }
      }

      public IEnumerable<string> IdentityAttributeNames
      {
         get
         {
            return IdentityAttributes.Select(x => x.Name).ToList();
         }
      }

      public IEnumerable<string> AllIdentityAttributeNames
      {
         get
         {
            return AllIdentityAttributes.Select(x => x.Name).ToList();
         }
      }

      public string FullName
      {
         get
         {
            return string.IsNullOrWhiteSpace(Namespace)
                      ? $"global::{Name}"
                      : $"global::{Namespace}.{Name}";
         }
      }

      public bool HasPersistentChildren
      {
         get
         {
            return Store.ElementDirectory
                        .AllElements
                        .OfType<Generalization>()
                        .Where(g => g.Superclass == this)
                        .Any(g => g.Subclass.IsPersistent || g.Subclass.HasPersistentChildren);
         }
      }

      #region Warning display

      // set as methods to avoid issues around serialization

      private bool hasWarning;

      public bool GetHasWarningValue() => IsPersistent && hasWarning;

      public void ResetWarning() => hasWarning = false;

      public void RedrawItem()
      {
         List<ShapeElement> shapeElements = PresentationViewsSubject.GetPresentation(this).OfType<ShapeElement>().ToList();
         foreach (ShapeElement shapeElement in shapeElements)
            shapeElement.Invalidate();
      }

      protected string GetGlyphTypeValue()
      {
         if (!IsPersistent)
            return "TransientGlyph";

         if (ModelRoot.ShowWarningsInDesigner && GetHasWarningValue())
            return "WarningGlyph";

         // ReSharper disable once ConvertIfStatementToReturnStatement
         if (IsAbstract)
            return "AbstractEntityGlyph";

         return "EntityGlyph";
      }

      #endregion

      public ConcurrencyOverride EffectiveConcurrency
      {
         get
         {
            if (Concurrency != ConcurrencyOverride.Default)
               return Concurrency;

            return ModelRoot?.ConcurrencyDefault == EFModel.Concurrency.None ? ConcurrencyOverride.None : ConcurrencyOverride.Optimistic;
         }
      }

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      public virtual void PreResetIsTrackingProperties()
      {
         IsDatabaseSchemaTrackingPropertyHandler.Instance.PreResetValue(this);
         IsNamespaceTrackingPropertyHandler.Instance.PreResetValue(this);
         IsOutputDirectoryTrackingPropertyHandler.Instance.PreResetValue(this);

         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void ResetIsTrackingProperties()
      {
         IsDatabaseSchemaTrackingPropertyHandler.Instance.ResetValue(this);
         IsNamespaceTrackingPropertyHandler.Instance.ResetValue(this);
         IsOutputDirectoryTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }

      public IEnumerable<NavigationProperty> AllNavigationProperties(params Association[] ignore)
      {
         List<NavigationProperty> result = LocalNavigationProperties(ignore).ToList();

         if (Superclass != null)
            result.AddRange(Superclass.AllNavigationProperties(ignore));

         return result;
      }

      public IEnumerable<NavigationProperty> LocalNavigationProperties(params Association[] ignore)
      {
         List<NavigationProperty> sourceProperties = Association.GetLinksToTargets(this)
                                                                .Except(ignore)
                                                                .Select(x => new NavigationProperty
                                                                {
                                                                   Cardinality = x.TargetMultiplicity,
                                                                   ClassType = x.Target,
                                                                   AssociationObject = x,
                                                                   PropertyName = x.TargetPropertyName,
                                                                   Summary = x.TargetSummary,
                                                                   Description = x.TargetDescription,
                                                                   CustomAttributes = x.TargetCustomAttributes,
                                                                   DisplayText = x.TargetDisplayText,
                                                                   IsAutoProperty = true,
                                                                   ImplementNotify = x.TargetImplementNotify
                                                                })
                                                                .ToList();

         List<NavigationProperty> targetProperties = Association.GetLinksToSources(this)
                                                                .Except(ignore)
                                                                .OfType<BidirectionalAssociation>()
                                                                .Select(x => new NavigationProperty
                                                                {
                                                                   Cardinality = x.SourceMultiplicity,
                                                                   ClassType = x.Source,
                                                                   AssociationObject = x,
                                                                   PropertyName = x.SourcePropertyName,
                                                                   Summary = x.SourceSummary,
                                                                   Description = x.SourceDescription,
                                                                   CustomAttributes = x.SourceCustomAttributes,
                                                                   DisplayText = x.SourceDisplayText,
                                                                   IsAutoProperty = true,
                                                                   ImplementNotify = x.SourceImplementNotify
                                                                })
                                                                .ToList();
         targetProperties.AddRange(Association.GetLinksToSources(this)
                                              .Except(ignore)
                                              .OfType<UnidirectionalAssociation>()
                                              .Select(x => new NavigationProperty
                                              {
                                                 Cardinality = x.SourceMultiplicity,
                                                 ClassType = x.Source,
                                                 AssociationObject = x,
                                                 PropertyName = null
                                              }));
         int index = 0;
         foreach (NavigationProperty navigationProperty in targetProperties.Where(x => x.PropertyName == null))
         {
            navigationProperty.PropertyName = $"_{navigationProperty.ClassType.Name.ToLower()}{index++}";
            navigationProperty.ConstructorParameterOnly = true;
         }

         return sourceProperties.Concat(targetProperties);
      }

      public IEnumerable<NavigationProperty> RequiredNavigationProperties(params Association[] ignore) => LocalNavigationProperties(ignore).Where(x => x.Required).ToList();

      public IEnumerable<NavigationProperty> AllRequiredNavigationProperties(params Association[] ignore) => AllNavigationProperties(ignore).Where(x => x.Required).ToList();

      public NavigationProperty FindAssociationNamed(string identifier) => AllNavigationProperties().FirstOrDefault(x => x.PropertyName == identifier);

      public ModelAttribute FindAttributeNamed(string identifier) => AllAttributes.FirstOrDefault(x => x.Name == identifier);

      public bool HasAssociationNamed(string identifier) => FindAssociationNamed(identifier) != null;

      public bool HasAttributeNamed(string identifier) => FindAttributeNamed(identifier) != null;

      public bool HasPropertyNamed(string identifier) => HasAssociationNamed(identifier) || HasAttributeNamed(identifier);

      private string GetBaseClassValue() => Superclass?.Name;

      private void SetBaseClassValue(string newValue)
      {
         ModelClass baseClass = Store.ElementDirectory.FindElements<ModelClass>().FirstOrDefault(x => x.Name == newValue);
         Superclass = baseClass;
      }

      #region Validations

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void ClassShouldHaveAttributes(ValidationContext context)
      {
         if (!Attributes.Any() && !LocalNavigationProperties().Any())
         {
            context.LogWarning($"{Name}: Class has no properties", "MCWNoProperties", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]

      private void AttributesCannotBeNamedSameAsEnclosingClass(ValidationContext context)
      {
         if (HasPropertyNamed(Name))
            context.LogError($"{Name}: Properties can't be named the same as the enclosing class", "MCESameName", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void PersistentClassesMustHaveIdentity(ValidationContext context)
      {
         if (!IsDependentType && !AllIdentityAttributes.Any())
            context.LogError($"{Name}: Class has no identity property in inheritance chain", "MCENoIdentity", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void DerivedClassesShouldNotHaveIdentity(ValidationContext context)
      {
         if (Attributes.Any(x => x.IsIdentity))
         {
            ModelClass modelClass = Superclass;
            while (modelClass != null)
            {
               if (modelClass.Attributes.Any(x => x.IsIdentity))
               {
                  context.LogWarning($"{modelClass.Name}: Identity attribute in derived class {Name} becomes a composite key", "MCWDerivedIdentity", this);
                  hasWarning = true;
                  RedrawItem();
                  return;
               }

               modelClass = modelClass.Superclass;
            }
         }
      }

      // Removed since code generation will add a Timestamp property if needed
      //
      //[ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      //private void EnsureProperNumberOfConcurrencyProperties(ValidationContext context)
      //{
      //   int tokenCount = AllAttributes.Count(x => x.IsConcurrencyToken);
      //   int shouldHave = EffectiveConcurrency == ConcurrencyOverride.Optimistic ? 1 : 0;

      //   if (tokenCount != shouldHave)
      //      context.LogWarning($"{Name}: Should have {shouldHave} concurrency properties but has {tokenCount}.", "MCEConcurrencyCount", this);
      //}

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot?.WarnOnMissingDocumentation == true && string.IsNullOrWhiteSpace(Summary))
         {
            context.LogWarning($"Class {Name} should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      #endregion Validations

      #region DatabaseSchema tracking property

      private string databaseSchemaStorage;

      private string GetDatabaseSchemaValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsDatabaseSchemaTracking)
         {
            try
            {
               return Store.ModelRoot()?.DatabaseSchema;
            }
            catch (NullReferenceException)
            {
               return null;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return null;
            }
         }

         return databaseSchemaStorage;
      }

      private void SetDatabaseSchemaValue(string value)
      {
         databaseSchemaStorage = value;

         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsDatabaseSchemaTracking = false;
      }

      internal sealed partial class IsDatabaseSchemaTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsDatabaseSchemaTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(DatabaseSchemaDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsDatabaseSchemaTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelClass element)
         {
            object calculatedValue = null;
            ModelRoot modelRoot = element.Store.ModelRoot();

            try
            {
               calculatedValue = modelRoot?.DatabaseSchema;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.DatabaseSchema == (string)calculatedValue)
               element.isDatabaseSchemaTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsDatabaseSchemaTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(ModelClass element) =>
            // Force the IsDatabaseSchemaTracking property to false so that the value  
            // of the DatabaseSchema property is retrieved from storage.  
            element.isDatabaseSchemaTrackingPropertyStorage = false;
      }

      #endregion DatabaseSchema tracking property

      #region Namespace tracking property

      private string namespaceStorage;

      private string GetNamespaceValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsNamespaceTracking)
         {
            try
            {
               return Store.ModelRoot()?.Namespace;
            }
            catch (NullReferenceException)
            {
               return null;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return null;
            }
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
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(NamespaceDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsNamespaceTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelClass element)
         {
            object calculatedValue = null;
            ModelRoot modelRoot = element.Store.ModelRoot();

            try
            {
               calculatedValue = modelRoot?.Namespace;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.Namespace == (string)calculatedValue)
               element.isNamespaceTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsNamespaceTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(ModelClass element) =>
            // Force the IsNamespaceTracking property to false so that the value  
            // of the Namespace property is retrieved from storage.  
            element.isNamespaceTrackingPropertyStorage = false;
      }

      #endregion Namespace tracking property

      #region OutputDirectory tracking property

      private string outputDirectoryStorage;

      private string GetOutputDirectoryValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!loading && IsOutputDirectoryTracking)
         {
            try
            {
               return IsDependentType 
                         ? Store.ModelRoot()?.StructOutputDirectory 
                         : Store.ModelRoot()?.EntityOutputDirectory;
            }
            catch (NullReferenceException)
            {
               return null;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return null;
            }
         }

         return outputDirectoryStorage;
      }

      private void SetOutputDirectoryValue(string value)
      {
         outputDirectoryStorage = value;

         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            IsOutputDirectoryTracking = false;
      }

      internal sealed partial class IsOutputDirectoryTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsOutputDirectoryTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(OutputDirectoryDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsOutputDirectoryTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelClass element)
         {
            object calculatedValue = null;
            ModelRoot modelRoot = element.Store.ModelRoot();

            try
            {
               calculatedValue = element.IsDependentType ? modelRoot?.StructOutputDirectory : element.ModelRoot?.EntityOutputDirectory;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.OutputDirectory == (string)calculatedValue)
               element.isOutputDirectoryTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsOutputDirectoryTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelClass element) =>
            // Force the IsOutputDirectoryTracking property to false so that the value  
            // of the OutputDirectory property is retrieved from storage.  
            element.isOutputDirectoryTrackingPropertyStorage = false;
      }

      #endregion OutputDirectory tracking property

      #region IsImplementNotify tracking property

      protected virtual void OnIsImplementNotifyChanged(bool oldValue, bool newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store, 
                                                         Attributes, 
                                                         ModelAttribute.ImplementNotifyDomainPropertyId, 
                                                         ModelAttribute.IsImplementNotifyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store, 
                                                         Store.ElementDirectory.AllElements.OfType<Association>().Where(a => a?.Source?.FullName == FullName),
                                                         Association.TargetImplementNotifyDomainPropertyId, 
                                                         Association.IsTargetImplementNotifyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store, 
                                                         Store.ElementDirectory.AllElements.OfType<BidirectionalAssociation>().Where(a => a?.Target?.FullName == FullName),
                                                         BidirectionalAssociation.SourceImplementNotifyDomainPropertyId, 
                                                         BidirectionalAssociation.IsSourceImplementNotifyTrackingDomainPropertyId);
      }

      internal sealed partial class ImplementNotifyPropertyHandler
      {
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnIsImplementNotifyChanged(oldValue, newValue);
         }
      }

      #endregion Namespace tracking property
   }
}
