using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Extensions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelClass : IModelElementWithCompartments, IDisplaysWarning, IHasStore
   {
      /// <summary>
      /// True if this is a normal entity type (not aggregated and not keyless), false otherwise
      /// </summary>
      /// <returns></returns>
      [Browsable(false)]
      public bool IsEntity() => !IsDependentType && !IsKeylessType();

      /// <summary>
      /// True if this is a dependent (aggregated) entity type, false otherwise
      /// </summary>
      /// <returns></returns>
      [Browsable(false)]
      public bool IsDependent() => IsDependentType;

      /// <summary>
      /// True if this is a keyless entity type (backed by a query or a view), false otherwise
      /// </summary>
      /// <returns></returns>
      [Browsable(false)]
      public bool IsKeyless() => IsKeylessType();

      /// <summary>
      /// The default namespace for this entity, based on its kind
      /// </summary>
      [Browsable(false)]
      public string DefaultNamespace
      {
         get
         {
            if (IsDependentType && !string.IsNullOrWhiteSpace(ModelRoot?.StructNamespace))
               return ModelRoot.StructNamespace;

            if (!IsDependentType && !string.IsNullOrWhiteSpace(ModelRoot?.EntityNamespace))
               return ModelRoot.EntityNamespace;

            return ModelRoot?.Namespace;
         }
      }

      /// <summary>
      /// Namespace for generated code. Takes overrides into account.
      /// </summary>
      [Browsable(false)]
      public string EffectiveNamespace
      {
         get
         {
            return namespaceStorage ?? DefaultNamespace;
         }
      }

      /// <summary>
      /// Where in the project code would normally be generated, based on the entity type
      /// </summary>
      [Browsable(false)]
      public string DefaultOutputDirectory
      {
         get
         {
            if (IsDependentType && !string.IsNullOrWhiteSpace(ModelRoot?.StructOutputDirectory))
               return ModelRoot.StructOutputDirectory;

            if (!IsDependentType && !string.IsNullOrWhiteSpace(ModelRoot?.EntityOutputDirectory))
               return ModelRoot.EntityOutputDirectory;

            return ModelRoot?.ContextOutputDirectory;
         }
      }

      /// <summary>
      /// Output directory for generated code. Takes overrides into account.
      /// </summary>
      [Browsable(false)]
      public string EffectiveOutputDirectory
      {
         get
         {
            return outputDirectoryStorage ?? DefaultOutputDirectory;
         }
      }

      /// <summary>
      /// Human readable text for displaying this object
      /// </summary>
      /// <returns></returns>
      public string GetDisplayText() => Name;

      public string GetDefaultDbSetName(bool shouldPluralize)
      {
         return ModelRoot.PluralizationService?.IsSingular(Name) == true && shouldPluralize
                                ? ModelRoot.PluralizationService.Pluralize(Name)
                                : Name;
      }

      public string GetDefaultTableName(bool shouldPluralize)
      {
         return ModelRoot.PluralizationService?.IsSingular(Name) == true && shouldPluralize
                   ? ModelRoot.PluralizationService.Pluralize(Name)
                   : Name;
      }


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

      /// <summary>
      /// Collection of all ModelClass classes making up the inheritance for this ModelClass
      /// </summary>
      public List<ModelClass> AllSuperclasses
      {
         get
         {
            List<ModelClass> result = new List<ModelClass>();
            ModelClass s = Superclass;

            while (s != null)
            {
               result.Add(s);
               s = s.Superclass;
            }

            return result;
         }
      }

      /// <summary>
      /// Collection of all ModelClass classes where this ModelClass is in its inheritance chain
      /// </summary>
      public List<ModelClass> AllSubclasses
      {
         get
         {
            List<ModelClass> result = Store.GetAll<ModelClass>().Where(x => x.Superclass == this).ToList();

            for (int i = 0; i < result.Count; i++)
               result.AddRange(Store.GetAll<ModelClass>().Where(x => x.Superclass == result[i]));

            return result;
         }
      }

      /// <summary>
      /// Names of all properties in the class, taking into account inheritance
      /// </summary>
      public IEnumerable<string> AllPropertyNames
      {
         get
         {
            List<string> result = AllAttributes.Select(a => a.Name).ToList();

            result.AddRange(AllNavigationProperties().Where(np => !string.IsNullOrEmpty(np.PropertyName)).Select(np => np.PropertyName));

            return result;
         }
      }

      /// <summary>
      /// All required attributes defined in this class
      /// </summary>
      public IEnumerable<ModelAttribute> RequiredAttributes
      {
         get
         {
            return Attributes.Where(x => x.Required).ToList();
         }
      }

      /// <summary>
      /// All required attributes in the inheritance chain
      /// </summary>
      public IEnumerable<ModelAttribute> AllRequiredAttributes
      {
         get
         {
            return AllAttributes.Where(x => x.Required).ToList();
         }
      }

      /// <summary>
      /// All identity attributes defined in this class
      /// </summary>
      public IEnumerable<ModelAttribute> IdentityAttributes
      {
         get
         {
            return Attributes.Where(x => x.IsIdentity).ToList();
         }
      }

      /// <summary>
      /// All identity attributes in the inheritance chain
      /// </summary>
      public IEnumerable<ModelAttribute> AllIdentityAttributes
      {
         get
         {
            return AllAttributes.Where(x => x.IsIdentity).ToList();
         }
      }

      /// <summary>
      /// Names of identity attributes defined in this class
      /// </summary>
      public IEnumerable<string> IdentityAttributeNames
      {
         get
         {
            return IdentityAttributes.Select(x => x.Name).ToList();
         }
      }

      /// <summary>
      /// Names of all identity attributes in the inheritance chain
      /// </summary>
      public IEnumerable<string> AllIdentityAttributeNames
      {
         get
         {
            return AllIdentityAttributes.Select(x => x.Name).ToList();
         }
      }

      /// <summary>
      /// Class name with namespace
      /// </summary>
      public string FullName
      {
         get
         {
            if (IsPropertyBag)
               return "System.Collections.Generic.Dictionary<string, object>";

            return string.IsNullOrWhiteSpace(EffectiveNamespace)
                      ? $"global::{Name}"
                      : $"global::{EffectiveNamespace}.{Name}";
         }
      }

      #region Warning display

      // set as methods to avoid issues around serialization

      private bool hasWarning;

      /// <summary>
      /// Determines if this class has warnings being displayed.
      /// </summary>
      /// <returns>True if this class has warnings visible, false otherwise</returns>
      public bool GetHasWarningValue() => hasWarning;

      /// <summary>
      /// Clears visible warnings.
      /// </summary>
      public void ResetWarning() => hasWarning = false;

      /// <summary>
      /// Redraws this class.
      /// </summary>
      public void RedrawItem()
      {
         this.Redraw();
      }

      /// <summary>
      /// If true, diagram should show an interface lollipop on the class if it has a custom interface
      /// </summary>
      protected bool GetShouldShowInterfaceGlyphValue()
      {
         return ModelRoot.ShowInterfaceIndicators && !string.IsNullOrEmpty(CustomInterfaces);
      }

      /// <summary>
      /// Gets the glyph type value for display
      /// </summary>
      /// <returns>The type of glyph that should be displayed</returns>
      protected string GetGlyphTypeValue()
      {
         if (ModelRoot.ShowWarningsInDesigner && GetHasWarningValue())
            return "WarningGlyph";

         if (IsQueryType)
            return "SQLGlyph";

         if (!GenerateCode)
            return "NoGenGlyph";

         if (IsPropertyBag)
            return "DictionaryGlyph";

         // ReSharper disable once ConvertIfStatementToReturnStatement
         if (IsAbstract)
            return "AbstractEntityGlyph";

         return "EntityGlyph";
      }

      #endregion

      /// <summary>
      /// Concurrency type, taking into account the model's default concurrency and any override defined in this class
      /// </summary>
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

      /// <summary>
      /// All navigation properties including those in superclasses
      /// </summary>
      /// <param name="ignore">Associations to remove from the result</param>
      /// <returns>All navigation properties including those in superclasses, except those listed in the parameter</returns>
      public IEnumerable<NavigationProperty> AllNavigationProperties(params Association[] ignore)
      {
         List<NavigationProperty> result = LocalNavigationProperties(ignore).ToList();

         if (Superclass != null)
            result.AddRange(Superclass.AllNavigationProperties(ignore));

         return result;
      }

      /// <summary>
      /// All navigation properties defined in this class
      /// </summary>
      /// <param name="ignore">Associations to remove from the result</param>
      /// <returns>All navigation properties defined in this class, except those listed in the parameter</returns>
      public IEnumerable<NavigationProperty> LocalNavigationProperties(params Association[] ignore)
      {
         List<NavigationProperty> sourceProperties = Association.GetLinksToTargets(this)
                                                                .Except(ignore)
                                                                .Select(NavigationProperty.LinkToTarget)
                                                                .ToList();

         List<NavigationProperty> targetProperties = Association.GetLinksToSources(this)
                                                                .Except(ignore)
                                                                .OfType<BidirectionalAssociation>()
                                                                .Select(NavigationProperty.LinkToSource)
                                                                .ToList();

         targetProperties.AddRange(Association.GetLinksToSources(this)
                                              .Except(ignore)
                                              .OfType<UnidirectionalAssociation>()
                                              .Select(NavigationProperty.LinkToSource));
         int suffix = 0;
         foreach (NavigationProperty navigationProperty in targetProperties.Where(x => x.PropertyName == null))
         {
            navigationProperty.PropertyName = $"_{navigationProperty.ClassType.Name.ToLower()}{suffix++}";
            navigationProperty.ConstructorParameterOnly = true;
         }

         return sourceProperties.Concat(targetProperties);
      }

      /// <summary>
      /// required navigation (1.. cardinality) properties in this class
      /// </summary>
      /// <param name="ignore">Associations to remove from the result.</param>
      /// <returns>All required associations found, except for those in the [ignore] parameter</returns>
      public IEnumerable<NavigationProperty> RequiredNavigationProperties(params Association[] ignore) => LocalNavigationProperties(ignore).Where(x => x.Required).ToList();

      /// <summary>
      /// All the required navigation (1.. cardinality) properties in both this and base classes.
      /// </summary>
      /// <param name="ignore">Associations to remove from the result.</param>
      /// <returns>All required associations found, except for those in the [ignore] parameter</returns>
      public IEnumerable<NavigationProperty> AllRequiredNavigationProperties(params Association[] ignore) => AllNavigationProperties(ignore).Where(x => x.Required).ToList();

      /// <summary>
      /// Finds the association named by the value specified in the parameter
      /// </summary>
      /// <param name="identifier">Association property name to find.</param>
      /// <returns>The object representing the association, if found</returns>
      public NavigationProperty FindAssociationNamed(string identifier) => AllNavigationProperties().FirstOrDefault(x => x.PropertyName == identifier);

      /// <summary>
      /// Finds the attribute named by the value specified in the parameter 
      /// </summary>
      /// <param name="identifier">Attribute name to find.</param>
      /// <returns>The object representing the attribute, if found</returns>
      public ModelAttribute FindAttributeNamed(string identifier) => AllAttributes.FirstOrDefault(x => x.Name == identifier);

      /// <summary>
      /// Determines whether the generated code will have an association property with the name specified in the parameter
      /// </summary>
      /// <param name="identifier">Property name to find.</param>
      /// <returns>
      ///   <c>true</c> if the class will have this property; otherwise, <c>false</c>.
      /// </returns>
      public bool HasAssociationNamed(string identifier) => FindAssociationNamed(identifier) != null;

      /// <summary>
      /// Determines whether the generated code will have a scalar property with the name specified in the parameter
      /// </summary>
      /// <param name="identifier">Property name to find.</param>
      /// <returns>
      ///   <c>true</c> if the class will have this property; otherwise, <c>false</c>.
      /// </returns>
      public bool HasAttributeNamed(string identifier) => FindAttributeNamed(identifier) != null;

      /// <summary>
      /// Determines whether the generated code will have any property with the name specified in the parameter
      /// </summary>
      /// <param name="identifier">Property name to find.</param>
      /// <returns>
      ///   <c>true</c> if the class will have this property; otherwise, <c>false</c>.
      /// </returns>
      public bool HasPropertyNamed(string identifier) => HasAssociationNamed(identifier) || HasAttributeNamed(identifier);

      /// <summary>
      /// Gets the name of the superclass, if any.
      /// </summary>
      /// <returns></returns>
      private string GetBaseClassValue() => Superclass?.Name;

      internal bool IsKeylessType() => IsQueryType || IsDatabaseView;

      /// <summary>
      /// Sets the superclass to the class with the supplied name, if it exists. Sets to null if can't be found.
      /// </summary>
      /// <param name="newValue">Simple name (not FQN) of class to use as superclass.</param>
      private void SetBaseClassValue(string newValue)
      {
         ModelClass baseClass = Store.ElementDirectory.FindElements<ModelClass>().FirstOrDefault(x => x.Name == newValue);
         Superclass?.Subclasses?.Remove(this);
         baseClass?.Subclasses?.Add(this);
      }

      internal void MoveAttribute(ModelAttribute attribute, ModelClass destination)
      {
         MergeDisconnect(attribute);
         destination.MergeRelate(attribute, null);
      }

      #region Validations

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void ClassShouldHaveAttributes(ValidationContext context)
      {
         if (ModelRoot == null) return;

         if (!Attributes.Any() && !LocalNavigationProperties().Any())
         {
            context.LogWarning($"{Name}: Class has no properties", "MCWNoProperties", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Menu)]
      private void OwnedTypeCannotHaveABaseClass(ValidationContext context)
      {
         if (ModelRoot == null) return;
         if (IsDependentType && Superclass != null)
            context.LogError($"Can't make {Name} a dependent class since it has a base class", "MCEOwnedHasBaseClass", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Menu)]
      private void OwnedTypeCannotHaveASubclass(ValidationContext context)
      {
         if (ModelRoot == null) return;
         if (IsDependentType && Subclasses.Any())
            context.LogError($"Can't make {Name} a dependent class since it has subclass(es) {string.Join(", ", Subclasses.Select(s => s.Name))}", "MCEOwnedHasSubclass", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Menu)]
      private void OwnedTypeCannotBeAbstract(ValidationContext context)
      {
         if (ModelRoot == null) return;
         if (IsDependentType && IsAbstract)
            context.LogError($"Can't make {Name} a dependent class since it's abstract", "MCEOwnedIsAbstract", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Menu)]
      private void OwnedTypeCannotBePrincipal(ValidationContext context)
      {
         if (ModelRoot == null) return;

         List<Association> principalAssociations = ModelRoot.Store.GetAll<Association>().Where(a => a.Principal == this).ToList();

         if (IsDependentType && principalAssociations.Any())
         {
            string badAssociations = string.Join(", ", principalAssociations.Select(a => a.GetDisplayText()));
            context.LogError($"Can't make {Name} a dependent class since it's the principal end in: {badAssociations}", "MCEOwnedIsPrincipal", this);
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Menu)]
      private void OwnedTypeCannotBeInBidirectionalAssociation(ValidationContext context)
      {
         if (ModelRoot == null) return;

         if (IsDependentType && !ModelRoot.IsEFCore5Plus && ModelRoot.Store.GetAll<BidirectionalAssociation>().Any(a => a.Source == this || a.Target == this))
            context.LogError($"Can't make {Name} a dependent class since it's part of a bidirectional association", "MCEOwnedInBidirectional", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void AttributesCannotBeNamedSameAsEnclosingClass(ValidationContext context)
      {
         if (ModelRoot == null) return;

         if (HasPropertyNamed(Name))
            context.LogError($"{Name}: Properties can't be named the same as the enclosing class", "MCESameName", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void PersistentClassesMustHaveIdentity(ValidationContext context)
      {
         if (ModelRoot == null) return;

         if (!IsDependentType && !IsQueryType && !AllIdentityAttributes.Any())
            context.LogError($"{Name}: Class has no identity property in inheritance chain", "MCENoIdentity", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void DerivedClassesShouldNotHaveIdentity(ValidationContext context)
      {
         if (ModelRoot == null) return;

         if (Attributes.Any(x => x.IsIdentity))
         {
            ModelClass modelClass = Superclass;
            while (modelClass != null)
            {
               if (modelClass.Attributes.Any(x => x.IsIdentity))
               {
                  context.LogWarning($"{modelClass.Name}: Identity attribute(s) in derived class {Name} become a composite key", "MCWDerivedIdentity", this);
                  hasWarning = true;
                  RedrawItem();
                  return;
               }

               modelClass = modelClass.Superclass;
            }
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (ModelRoot == null) return;

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
         if (!this.IsLoading() && IsDatabaseSchemaTracking)
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

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
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

      #region DefaultConstructorVisibility tracking property

      private TypeAccessModifierExt defaultConstructorVisibilityStorage;

      private TypeAccessModifierExt GetDefaultConstructorVisibilityValue()
      {
         if (!this.IsLoading() && IsDefaultConstructorVisibilityTracking)
         {
            try
            {
               return Store.ModelRoot()?.EntityDefaultConstructorVisibilityDefault ?? TypeAccessModifierExt.Default;
            }
            catch (NullReferenceException)
            {
               return TypeAccessModifierExt.Default;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return TypeAccessModifierExt.Default;
            }
         }

         return defaultConstructorVisibilityStorage;
      }

      private void SetDefaultConstructorVisibilityValue(TypeAccessModifierExt value)
      {
         defaultConstructorVisibilityStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsDefaultConstructorVisibilityTracking = false;
      }

      internal sealed partial class IsDefaultConstructorVisibilityTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsDefaultConstructorVisibilityTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(DefaultConstructorVisibilityDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsDefaultConstructorVisibilityTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelClass element)
         {
            object calculatedValue = null;
            ModelRoot modelRoot = element.Store.ModelRoot();

            try
            {
               calculatedValue = modelRoot?.EntityDefaultConstructorVisibilityDefault;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.DefaultConstructorVisibility == (TypeAccessModifierExt)calculatedValue)
               element.isDefaultConstructorVisibilityTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsDefaultConstructorVisibilityTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelClass element) =>
            // Force the IsDefaultConstructorVisibilityTracking property to false so that the value  
            // of the DefaultConstructorVisibility property is retrieved from storage.  
            element.isDefaultConstructorVisibilityTrackingPropertyStorage = false;
      }

      #endregion

      #region Namespace tracking property

      private string namespaceStorage;

      private string GetNamespaceValue()
      {
         if (!this.IsLoading() && IsNamespaceTracking)
         {
            try
            {
               return DefaultNamespace;
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
         namespaceStorage = string.IsNullOrWhiteSpace(value) || value == DefaultNamespace ? null : value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsNamespaceTracking = namespaceStorage == null;
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
            element.isNamespaceTrackingPropertyStorage = string.IsNullOrWhiteSpace(element.namespaceStorage);
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
         if (!this.IsLoading() && IsOutputDirectoryTracking)
         {
            try
            {
               return DefaultOutputDirectory;
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

         return outputDirectoryStorage;
      }

      private void SetOutputDirectoryValue(string value)
      {
         outputDirectoryStorage = string.IsNullOrWhiteSpace(value) || value == DefaultOutputDirectory ? null : value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsOutputDirectoryTracking = (outputDirectoryStorage == null);
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

      /// <summary>
      /// Updates tracking properties when the IsImplementNotify value changes
      /// </summary>
      /// <param name="oldValue">Prior value</param>
      /// <param name="newValue">Current value</param>
      protected virtual void OnIsImplementNotifyChanged(bool oldValue, bool newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Attributes,
                                                         ModelAttribute.ImplementNotifyDomainPropertyId,
                                                         ModelAttribute.IsImplementNotifyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Store.GetAll<Association>().Where(a => a.Source?.FullName == FullName),
                                                         Association.TargetImplementNotifyDomainPropertyId,
                                                         Association.IsTargetImplementNotifyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Store.GetAll<BidirectionalAssociation>().Where(a => a.Target?.FullName == FullName),
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

      #endregion IsImplementNotify tracking property

      #region AutoPropertyDefault tracking property

      // this property is both a tracking property (dependent on ModelRoot.AutoPropertyDefault)
      // and a tracked property (ModelAttribute.AutoProperty, Association.TargetAutoProperty and BidirectionalAssociation.SourceAutoProperty depends on it)

      private bool autoPropertyDefaultStorage;

      /// <summary>Gets the storage for the AutoPropertyDefault property.</summary>
      /// <returns>The AutoPropertyDefault value.</returns>
      public bool GetAutoPropertyDefaultValue()
      {
         if (!this.IsLoading() && IsAutoPropertyDefaultTracking)
         {
            try
            {
               return ModelRoot?.AutoPropertyDefault ?? true;
            }
            catch (NullReferenceException)
            {
               return true;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return true;
            }
         }

         return autoPropertyDefaultStorage;
      }

      /// <summary>Sets the storage for the AutoPropertyDefault property.</summary>
      /// <param name="value">The AutoPropertyDefault value.</param>
      public void SetAutoPropertyDefaultValue(bool value)
      {
         autoPropertyDefaultStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsAutoPropertyDefaultTracking = (autoPropertyDefaultStorage == (ModelRoot?.AutoPropertyDefault ?? true));
      }

      /// <summary>
      /// Updates tracking properties when the AutoPropertyDefault value changes
      /// </summary>
      /// <param name="oldValue">Prior value</param>
      /// <param name="newValue">Current value</param>
      protected virtual void OnAutoPropertyDefaultChanged(bool oldValue, bool newValue)
      {
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Attributes,
                                                         ModelAttribute.AutoPropertyDomainPropertyId,
                                                         ModelAttribute.IsAutoPropertyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Store.GetAll<Association>().Where(a => a.Source?.FullName == FullName),
                                                         Association.TargetAutoPropertyDomainPropertyId,
                                                         Association.IsTargetAutoPropertyTrackingDomainPropertyId);
         TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                         Store.GetAll<BidirectionalAssociation>().Where(a => a.Target?.FullName == FullName),
                                                         BidirectionalAssociation.SourceAutoPropertyDomainPropertyId,
                                                         BidirectionalAssociation.IsSourceAutoPropertyTrackingDomainPropertyId);
      }

      internal sealed partial class AutoPropertyDefaultPropertyHandler
      {
         protected override void OnValueChanged(ModelClass element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnAutoPropertyDefaultChanged(oldValue, newValue);
         }
      }

      #endregion AutoPropertyDefault tracking property
   }
}
