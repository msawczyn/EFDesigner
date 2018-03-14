using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Tag interface indicating that the diagram items for this element has compartments
   /// </summary>
   public interface IModelElementWithCompartments { }

   [ValidationState(ValidationState.Enabled)]
   public partial class ModelClass : IModelElementWithCompartments
   {
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

      public IEnumerable<ModelAttribute> RequiredAttributes => Attributes.Where(x => x.Required).ToList();

      public IEnumerable<ModelAttribute> AllRequiredAttributes
      {
         get
         {
            List<ModelAttribute> result = RequiredAttributes.ToList();
            if (Superclass != null)
               result.AddRange(Superclass.RequiredAttributes);
            return result;
         }
      }

      public IEnumerable<string> IdentityPropertyNames => Attributes.Where(x => x.IsIdentity).Select(x => x.Name).ToList();

      public IEnumerable<string> AllIdentityPropertyNames
      {
         get
         {
            List<string> result = Attributes.Where(x => x.IsIdentity).Select(x => x.Name).ToList();
            if (Superclass != null)
               result.AddRange(Superclass.AllIdentityPropertyNames);
            return result;
         }
      }


      public ConcurrencyOverride EffectiveConcurrency
      {
         get
         {
            if (Concurrency != ConcurrencyOverride.Default)
               return Concurrency;

            return ModelRoot.ConcurrencyDefault == EFModel.Concurrency.None ? ConcurrencyOverride.None : ConcurrencyOverride.Optimistic;
         }
      }

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void PreResetIsTrackingProperties()
      {
         IsDatabaseSchemaTrackingPropertyHandler.Instance.PreResetValue(this);
         IsNamespaceTrackingPropertyHandler.Instance.PreResetValue(this);
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
         // same with other tracking properties as they get added
      }

      public IEnumerable<NavigationProperty> AllNavigationProperties(params Association[] ignore)
      {
         List<NavigationProperty> result = LocalNavigationProperties(ignore).ToList();

         if (Superclass != null)
            result.AddRange(Superclass.AllNavigationProperties());

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
                                                                   Description = x.TargetDescription
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
                                                                   Description = x.SourceDescription
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

      public IEnumerable<NavigationProperty> RequiredNavigationProperties(params Association[] ignore)
      {
         return LocalNavigationProperties(ignore).Where(x => x.Required).ToList();
      }

      public IEnumerable<NavigationProperty> AllRequiredNavigationProperties(params Association[] ignore)
      {
         return AllNavigationProperties(ignore).Where(x => x.Required).ToList();
      }

      public NavigationProperty FindAssociationNamed(string identifier)
      {
         return AllNavigationProperties().FirstOrDefault(x => x.PropertyName == identifier);
      }

      public ModelAttribute FindAttributeNamed(string identifier)
      {
         return AllAttributes.FirstOrDefault(x => x.Name == identifier);
      }

      public bool HasAssociationNamed(string identifier)
      {
         return FindAssociationNamed(identifier) != null;
      }

      public bool HasAttributeNamed(string identifier)
      {
         return FindAttributeNamed(identifier) != null;
      }

      public bool HasPropertyNamed(string identifier)
      {
         return HasAssociationNamed(identifier) || HasAttributeNamed(identifier);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void ClassShouldHaveAttributes(ValidationContext context)
      {
         if (!Attributes.Any() && !LocalNavigationProperties().Any())
            context.LogWarning($"{Name}: Class has no properties", "MCWNoProperties", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]

      // ReSharper disable once UnusedMember.Local
      private void AttributesCannotBeNamedSameAsEnclosingClass(ValidationContext context)
      {
         if (HasPropertyNamed(Name))
            context.LogError($"{Name}: Properties can't be named the same as the enclosing class", "MCESameName", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void PersistentClassesMustHaveIdentity(ValidationContext context)
      {
         if (!AllIdentityPropertyNames.Any())
            context.LogError($"{Name}: Class has no identity property in inheritance chain", "MCENoIdentity", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
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
                  return;
               }

               modelClass = modelClass.Superclass;
            }
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void EnsureProperNumberOfConcurrencyProperties(ValidationContext context)
      {
         int tokenCount = AllAttributes.Count(x => x.IsConcurrencyToken);
         int shouldHave = EffectiveConcurrency == ConcurrencyOverride.Optimistic ? 1 : 0;

         if (tokenCount != shouldHave)
            context.LogError($"{Name}: Should have {shouldHave} concurrency properties but has {tokenCount}", "MCEConcurrencyCount", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation)
         {
            if (string.IsNullOrWhiteSpace(Summary))
               context.LogWarning($"Class {Name} should be documented", "AWMissingSummary", this);
         }
      }
      #region DatabaseSchema tracking property

      private string databaseSchemaStorage;

      private string GetDatabaseSchemaValue()
      {
         Transaction transactionManagerCurrentTransaction = Store.TransactionManager.CurrentTransaction;
         bool loading = Store.TransactionManager.InTransaction && transactionManagerCurrentTransaction.IsSerializing;

         if (!loading && IsDatabaseSchemaTracking)
            try
            {
               return ModelRoot.DatabaseSchema;
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

            try
            {
               calculatedValue = element.ModelRoot.DatabaseSchema;
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
         internal void PreResetValue(ModelClass element)
         {
            // Force the IsDatabaseSchemaTracking property to false so that the value  
            // of the DatabaseSchema property is retrieved from storage.  
            element.isDatabaseSchemaTrackingPropertyStorage = false;
         }
      }

      #endregion DatabaseSchema tracking property

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
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
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
         internal void ResetValue(ModelClass element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.ModelRoot.Namespace;
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
         ///    Method to set IsDatabaseSchemaTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property
         ///    value.
         /// </param>
         internal void PreResetValue(ModelClass element)
         {
            // Force the IsDatabaseSchemaTracking property to false so that the value  
            // of the DatabaseSchema property is retrieved from storage.  
            element.isNamespaceTrackingPropertyStorage = false;
         }
      }

      #endregion Namespace tracking property
   }
}
