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
    public partial class ModelEnum : IModelElementWithCompartments, IDisplaysWarning
    {
        public static string[] ValidValueTypes = {/*"SByte", */"Int16", "Int32", "Int64" };

        public string FullName => string.IsNullOrWhiteSpace(Namespace) ? $"global::{Name}" : $"global::{Namespace}.{Name}";

        #region Warning display

        // set as methods to avoid issues around serialization

        private bool hasWarning;

        public bool GetHasWarningValue() => hasWarning;

        public void ResetWarning() => hasWarning = false;

        public void RedrawItem()
        {
            List<ShapeElement> shapeElements = PresentationViewsSubject.GetPresentation(this).OfType<ShapeElement>().ToList();
            foreach (ShapeElement shapeElement in shapeElements)
                shapeElement.Invalidate();
        }

        protected string GetGlyphTypeValue() => ModelRoot.ShowWarningsInDesigner && GetHasWarningValue()
                     ? "WarningGlyph"
                     : "EnumGlyph";

        #endregion

        /// <summary>
        /// If enum is flags, renumbers all enum values starting at 1 without regard to its current value
        /// </summary>
        public void SetFlagValues()
        {
            if (IsFlags)
            {
                for (int i = 0; i < Values.Count; i++)
                    SetFlagValue(Values[i]);
            }
        }

        /// <summary>
        /// If enum is flags and no value is set, sets the indicated EnumValue to the next binary number.
        /// Takes into account that flags may be valued as combined binaries in the existing list.
        /// </summary>
        /// <param name="value"></param>
        public void SetFlagValue(ModelEnumValue value)
        {
            if (IsFlags && string.IsNullOrWhiteSpace(value.Value))
            {
                List<ModelEnumValue> modelEnumValues = Values.Where(v => long.TryParse(v.Value, out long _)).ToList();

                long maxValue = modelEnumValues.Any()
                                    ? modelEnumValues.Max(v => long.Parse(v.Value))
                                    : -1;

                long nextValue = maxValue <= 0
                                     ? 1
                                     : (long)Math.Pow(2, (int)Math.Log(maxValue, 2) + 1);

                value.Value = nextValue.ToString();
            }
        }

        /// <summary>
        ///    Calls the pre-reset method on the associated property value handler for each
        ///    tracking property of this model element.
        /// </summary>
        internal virtual void PreResetIsTrackingProperties()
        {
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
            IsNamespaceTrackingPropertyHandler.Instance.ResetValue(this);
            IsOutputDirectoryTrackingPropertyHandler.Instance.ResetValue(this);
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
            {
                context.LogWarning($"{Name}: Enum has some, but not all, values initialized. Please ensure this is what was intended.", "MWPartialEnumValueInitialization", this);
                hasWarning = true;
                RedrawItem();
            }
        }

        [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
        // ReSharper disable once UnusedMember.Local
        private void SummaryDescriptionIsEmpty(ValidationContext context)
        {
            ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
            if (modelRoot?.WarnOnMissingDocumentation == true && string.IsNullOrWhiteSpace(Summary))
            {
                context.LogWarning($"{Name}: Enum should be documented", "AWMissingSummary", this);
                hasWarning = true;
                RedrawItem();
            }
        }
        #region Namespace tracking property

        private string namespaceStorage;

        private string GetNamespaceValue()
        {
           bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

            if (!loading && IsNamespaceTracking)
                try
                {
                    return ModelRoot?.Namespace;
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

            /// <summary>Performs the reset operation for the IsNamespaceTracking property for a model element.</summary>
            /// <param name="element">The model element that has the property to reset.</param>
            internal void ResetValue(ModelEnum element)
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
            internal void PreResetValue(ModelEnum element) =>
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
                try
                {
                    return ModelRoot?.EnumOutputDirectory;
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
            protected override void OnValueChanged(ModelEnum element, bool oldValue, bool newValue)
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
            internal void ResetValue(ModelEnum element)
            {
                object calculatedValue = null;
                ModelRoot modelRoot = element.Store.ModelRoot();

                try
                {
                    calculatedValue = modelRoot?.EnumOutputDirectory;
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
            ///    The element on which to reset the property
            ///    value.
            /// </param>
            internal void PreResetValue(ModelEnum element) =>
               // Force the IsOutputDirectoryTracking property to false so that the value  
               // of the OutputDirectory property is retrieved from storage.  
               element.isOutputDirectoryTrackingPropertyStorage = false;
        }

        #endregion OutputDirectory tracking property
    }
}
