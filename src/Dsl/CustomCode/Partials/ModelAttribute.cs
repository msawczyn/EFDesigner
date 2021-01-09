using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Immutability;
using Microsoft.VisualStudio.Modeling.Validation;

using Sawczyn.EFDesigner.EFModel.Annotations;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   [SuppressMessage("ReSharper", "ArrangeAccessorOwnerBody")]
   public partial class ModelAttribute : IModelElementInCompartment, IDisplaysWarning, IHasStore
   {
      internal const int MAXLENGTH_MAX = -1;
      internal const int MAXLENGTH_UNDEFINED = 0;

      /// <summary>Gets the parent model element (ModelClass).</summary>
      /// <value>The parent model element.</value>
      public IModelElementWithCompartments ParentModelElement => ModelClass;

      /// <summary>Gets the name of the compartment holding this model element</summary>
      /// <value>The name of the compartment holding this model element.</value>
      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;

      /// <summary>
      /// Short display text for this attribute
      /// </summary>
      public string GetDisplayText() => $"{ModelClass.Name}.{Name}";

      /// <summary>
      /// If true, this property is an exposed foreign key for an association.
      /// </summary>
      public bool IsForeignKeyProperty => IsForeignKeyFor != Guid.Empty;

      internal string BackingFieldNameDefault => string.IsNullOrEmpty(Name) ? string.Empty : $"_{Name.Substring(0, 1).ToLowerInvariant()}{Name.Substring(1)}";

      internal string _backingFieldName;
      private string GetBackingFieldNameValue() => string.IsNullOrEmpty(_backingFieldName) ? BackingFieldNameDefault : _backingFieldName;
      private void SetBackingFieldNameValue(string value) => _backingFieldName = value;

      #region Warning display

      // set as methods to avoid issues around serialization

      private bool hasWarning;


      /// <summary>Indicates if there are any model warnings in this element.</summary>
      /// <returns>True if model warnings exist, false otherwise.</returns>
      public bool GetHasWarningValue() => hasWarning;


      /// <summary>Resets the warning indicator</summary>
      public void ResetWarning() => hasWarning = false;


      /// <summary>Redraws the presentation element on any diagram rendering this model element</summary>
      public void RedrawItem()
      {
         if (ParentModelElement != null)
         {
            foreach (ShapeElement shapeElement in
                  PresentationViewsSubject.GetPresentation(ParentModelElement as ModelElement).OfType<ShapeElement>().Distinct())
               shapeElement.Invalidate();
         }
      }

      #endregion

      /// <summary>Gets a value indicating whether this attribute supports initial values.</summary>
      /// <value>True if supports initial values, false if not.</value>
      public bool SupportsInitialValue
      {
         get
         {
            switch (Type)
            {
               case "Binary":
               case "Geography":
               case "GeographyCollection":
               case "GeographyLineString":
               case "GeographyMultiLineString":
               case "GeographyMultiPoint":
               case "GeographyMultiPolygon":
               case "GeographyPoint":
               case "GeographyPolygon":
               case "Geometry":
               case "GeometryCollection":
               case "GeometryLineString":
               case "GeometryMultiLineString":
               case "GeometryMultiPoint":
               case "GeometryMultiPolygon":
               case "GeometryPoint":
               case "GeometryPolygon":
               case "LineString":
               case "MultiLineString":
               case "MultiPoint":
               case "MultiPolygon":
               case "Point":
               case "Polygon":
                  return false;
            }

            return true;
         }
      }

      /// <summary>
      /// Tests if the InitialValue property is valid for the type indicated
      /// </summary>
      /// <param name="typeName">Name of type to test. If typeName is null, Type property will be used. If initialValue is null, InitialValue property will be used</param>
      /// <param name="initialValue">Initial value to test</param>
      /// <returns>true if InitialValue is a valid value for the type, or if initialValue is null or empty</returns>
#pragma warning disable 168
      [SuppressMessage("ReSharper", "BuiltInTypeReferenceStyle")]
      public bool IsValidInitialValue(string typeName = null, string initialValue = null)
      {
         typeName = typeName ?? Type;
         initialValue = initialValue ?? InitialValue;

         if (string.IsNullOrEmpty(initialValue))
            return true;

         // ReSharper disable UnusedVariable
         switch (typeName)
         {
            case "Binary":
            case "Geography":
            case "GeographyCollection":
            case "GeographyLineString":
            case "GeographyMultiLineString":
            case "GeographyMultiPoint":
            case "GeographyMultiPolygon":
            case "GeographyPoint":
            case "GeographyPolygon":
            case "Geometry":
            case "GeometryCollection":
            case "GeometryLineString":
            case "GeometryMultiLineString":
            case "GeometryMultiPoint":
            case "GeometryMultiPolygon":
            case "GeometryPoint":
            case "GeometryPolygon":
            case "LineString":
            case "MultiLineString":
            case "MultiPoint":
            case "MultiPolygon":
            case "Point":
            case "Polygon":
               return false; //string.IsNullOrEmpty(initialValue);
            case "Boolean":
               return bool.TryParse(initialValue, out bool _bool);
            case "Byte":
               return byte.TryParse(initialValue, out byte _byte);
            case "DateTime":
               switch (initialValue?.Trim())
               {
                  case "DateTime.Now":
                  case "DateTime.UtcNow":
                  case "DateTime.MinValue":
                  case "DateTime.MaxValue":
                     return true;
                  default:
                     return DateTime.TryParse(initialValue, out DateTime _dateTime);
               }
            case "DateTimeOffset":
               return DateTimeOffset.TryParse(initialValue, out DateTimeOffset _dateTimeOffset);
            case "Decimal":
               return Decimal.TryParse(initialValue, out Decimal _decimal);
            case "Double":
               return Double.TryParse(initialValue, out Double _double);
            case "Guid":
               return Guid.TryParse(initialValue, out Guid _guid);
            case "Int16":
               return Int16.TryParse(initialValue, out Int16 _int16);
            case "UInt16":
               return UInt16.TryParse(initialValue, out UInt16 _uint16);
            case "Int32":
               return Int32.TryParse(initialValue, out Int32 _int32);
            case "UInt32":
               return UInt32.TryParse(initialValue, out UInt32 _uint32);
            case "Int64":
               return Int64.TryParse(initialValue, out Int64 _int64);
            case "UInt64":
               return UInt64.TryParse(initialValue, out UInt64 _uint64);
            case "SByte":
               return SByte.TryParse(initialValue, out SByte _sbyte);
            case "Single":
               return Single.TryParse(initialValue, out Single _single);
            case "String":
               return true;
            case "Time":
               return DateTime.TryParseExact(initialValue,
                                             new[] { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm", "HH:mm:ss tt", "H:mm:ss tt", "HH:mm tt", "H:mm tt" },
                                             CultureInfo.InvariantCulture,
                                             DateTimeStyles.None,
                                             out DateTime _time);
            // ReSharper restore UnusedVariable
            default:
               if (initialValue.Contains("."))
               {
                  string[] parts = initialValue.Split('.');
                  ModelEnum enumType = ModelClass.ModelRoot.Enums.FirstOrDefault(x => x.Name == parts[0]);
                  return enumType != null && parts.Length == 2 && enumType.Values.Any(x => x.Name == parts[1]);
               }

               break;
         }

         return false;
      }
#pragma warning restore 168

      /// <summary>
      /// From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity. Converts the attribute's CLR type to a C# primitive type.
      /// </summary>
      /// <value>Name of primitive type</value>
      public string PrimitiveType
      {
         get
         {
            if (ModelClass.ModelRoot.EntityFrameworkVersion == EFVersion.EF6)
            {
               if (Type.StartsWith("Geography"))
                  return "DbGeography";

               if (Type.StartsWith("Geometry"))
                  return "DbGeometry";
            }

            return ToPrimitiveType(Type);
         }
      }

      /// <summary>Converts the attribute's CLR type to a C# primitive type.</summary>
      ///
      /// <value>Name of primitive type, or the fully qualified name if the attribute is an enumeration</value>
      // ReSharper disable once UnusedMember.Global
      public string FQPrimitiveType
      {
         get
         {
            string result = PrimitiveType;
            ModelEnum modelEnum = ModelClass.ModelRoot.Enums.FirstOrDefault(x => x.Name == result);

            return modelEnum != null
                      ? modelEnum.FullName
                      : result;
         }
      }

      /// <summary>Converts a C# primitive type to a CLR type.</summary>
      // ReSharper disable once UnusedMember.Global
      public string CLRType => ToCLRType(Type);

      /// <summary>
      /// From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity. Converts a CLR type to a C# primitive type.
      /// </summary>
      /// <param name="typeName">CLR type</param>
      /// <returns>Name of primitive type given </returns>
      // ReSharper disable once UnusedMember.Global
      public static string ToPrimitiveType(string typeName)
      {
         switch (typeName)
         {
            case "Binary":
               return "byte[]";
            case "Boolean":
               return "bool";
            case "Byte":
               return "byte";
            case "Time":
               return "TimeSpan";
            case "Decimal":
               return "decimal";
            case "Double":
               return "double";
            //case "Geography":
            //case "GeographyPoint":
            //case "GeographyLineString":
            //case "GeographyPolygon":
            //case "GeographyMultiPoint":
            //case "GeographyMultiLineString":
            //case "GeographyMultiPolygon":
            //case "GeographyCollection":
            //   return "DbGeography";
            //case "Geometry":
            //case "GeometryPoint":
            //case "GeometryLineString":
            //case "GeometryPolygon":
            //case "GeometryMultiPoint":
            //case "GeometryMultiLineString":
            //case "GeometryMultiPolygon":
            //case "GeometryCollection":
            //   return "DbGeometry";
            case "Int16":
               return "short";
            case "UInt16":
               return "ushort";
            case "Int32":
               return "int";
            case "UInt32":
               return "uint";
            case "Int64":
               return "long";
            case "UInt64":
               return "ulong";
            case "SByte":
               return "sbyte";
            case "String":
               return "string";
         }

         return typeName;
      }

      /// <summary>Converts a C# primitive type to a CLR type.</summary>
      /// <param name="typeName">C# type</param>
      /// <returns>Matching CLR type.</returns>
      public static string ToCLRType(string typeName)
      {
         switch (typeName)
         {
            case "byte[]":
               return "Binary";
            case "bool":
               return "Boolean";
            case "byte":
               return "Byte";
            case "TimeSpan":
               return "Time";
            case "decimal":
               return "Decimal";
            case "double":
               return "Double";
            case "DbGeography":
               return "Geography";
            case "DbGeometry":
               return "Geometry";
            case "short":
               return "Int16";
            case "ushort":
               return "UInt16";
            case "int":
               return "Int32";
            case "uint":
               return "UInt32";
            case "long":
               return "Int64";
            case "ulong":
               return "UInt64";
            case "sbyte":
               return "SByte";
            case "string":
               return "String";
         }

         return typeName;
      }

      internal void ClearFKMods(string summaryBoilerplate = null)
      {
         this.SetLocks(Locks.None);
         IsForeignKeyFor = Guid.Empty;

         if (!string.IsNullOrWhiteSpace(summaryBoilerplate))
         {
            int boilerplateLength = summaryBoilerplate.Length;
            Summary = !string.IsNullOrWhiteSpace(Summary) && Summary.Length >= boilerplateLength
                            ? Summary.Substring(boilerplateLength).TrimStart('.', ' ')
                            : null;
         }

         RedrawItem();
      }

      internal void SetFKMods(Association association, string summaryBoilerplate = null, bool? required = null, string type = null)
      {
         this.SetLocks(Locks.None);

         if (!string.IsNullOrWhiteSpace(summaryBoilerplate) && !Summary.StartsWith(summaryBoilerplate))
            Summary = $"{summaryBoilerplate}. {Summary}";

         if (type != null)
            Type = type;

         if (required != null)
            Required = required.Value;

         Indexed = true;
         IsForeignKeyFor = association.Id;

         this.SetLocks(Locks.Delete);
         RedrawItem();
      }

      #region ColumnName

      /// <summary>Storage for the ColumnName property.</summary>  
      private string columnNameStorage;

      /// <summary>Gets the storage for the ColumnName property.</summary>
      /// <returns>The ColumnName value.</returns>
      public string GetColumnNameValue()
      {
         if (!this.IsLoading() && IsColumnNameTracking)
         {
            try
            {
               return Name;
            }
            catch (NullReferenceException)
            {
               return Name;
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return Name;
            }
         }

         return columnNameStorage ?? Name;
      }

      /// <summary>Sets the storage for the ColumnName property.</summary>
      /// <param name="value">The ColumnName value.</param>
      public void SetColumnNameValue(string value)
      {
         columnNameStorage = value == Name ? null : value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            // ReSharper disable once ArrangeRedundantParentheses
            IsColumnNameTracking = (columnNameStorage == null);
      }

      #endregion

      #region ImplementNotify

      /// <summary>Storage for the ImplementNotify property.</summary>  
      private bool implementNotifyStorage;

      /// <summary>Gets the storage for the ImplementNotify property.</summary>
      /// <returns>The ImplementNotify value.</returns>
      public bool GetImplementNotifyValue()
      {
         if (!this.IsLoading() && IsImplementNotifyTracking)
         {
            try
            {
               return ModelClass?.ImplementNotify ?? false;
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

         return implementNotifyStorage;
      }

      /// <summary>Sets the storage for the ImplementNotify property.</summary>
      /// <param name="value">The ImplementNotify value.</param>
      public void SetImplementNotifyValue(bool value)
      {
         implementNotifyStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            // ReSharper disable once ArrangeRedundantParentheses
            IsImplementNotifyTracking = (implementNotifyStorage == (ModelClass?.ImplementNotify ?? false));
      }

      #endregion

      #region DatabaseCollation

      private string databaseCollationStorage;

      /// <summary>
      /// Getter for DatabaseCollation custom storage property
      /// </summary>
      public string GetDatabaseCollationValue()
      {
         if (!this.IsLoading() && IsDatabaseCollationTracking)
         {
            try
            {
               return ModelClass?.ModelRoot?.DatabaseCollationDefault ?? "default";
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return "default";
            }
         }

         return databaseCollationStorage ?? "default";
      }

      /// <summary>
      /// Setter for DatabaseCollation custom storage property
      /// </summary>
      /// <param name="value"></param>
      public void SetDatabaseCollationValue(string value)
      {
         databaseCollationStorage = string.IsNullOrEmpty(value) ? null : value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsDatabaseCollationTracking = ((databaseCollationStorage ?? "default") == "default");
      }

      #endregion

      #region AutoProperty

      /// <summary>Storage for the AutoProperty property.</summary>  
      private bool autoPropertyStorage;

      /// <summary>Gets the storage for the AutoProperty property.</summary>
      /// <returns>The AutoProperty value.</returns>
      public bool GetAutoPropertyValue()
      {
         if (!this.IsLoading() && IsAutoPropertyTracking)
         {
            try
            {
               return ModelClass?.AutoPropertyDefault ?? true;
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

         return autoPropertyStorage;
      }

      /// <summary>Sets the storage for the AutoProperty property.</summary>
      /// <param name="value">The AutoProperty value.</param>
      public void SetAutoPropertyValue(bool value)
      {
         autoPropertyStorage = value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            IsAutoPropertyTracking = (autoPropertyStorage == (ModelClass?.AutoPropertyDefault ?? true));
      }

      #endregion

      #region ColumnType

      /// <summary>Storage for the ColumnType property.</summary>  
      private string columnTypeStorage;

      /// <summary>Gets the storage for the ColumnType property.</summary>
      /// <returns>The ColumnType value.</returns>
      public string GetColumnTypeValue()
      {
         if (!this.IsLoading() && IsColumnTypeTracking)
         {
            try
            {
               return "default";
            }
            catch (NullReferenceException)
            {
               return "default";
            }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;

               return "default";
            }
         }

         return columnTypeStorage ?? "default";
      }

      /// <summary>Sets the storage for the ColumnType property.</summary>
      /// <param name="value">The ColumnType value.</param>
      public void SetColumnTypeValue(string value)
      {
         columnTypeStorage = value.ToLowerInvariant() == "default" ? null : value;

         if (!Store.InUndoRedoOrRollback && !this.IsLoading())
            // ReSharper disable once ArrangeRedundantParentheses
            IsColumnTypeTracking = (columnTypeStorage == null);
      }

      #endregion

      #region Tracking Properties

      internal sealed partial class IsDatabaseCollationTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsDatabaseCollationTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelAttribute element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(DatabaseCollationDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsDatabaseCollationTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelAttribute element)
         {
            string calculatedValue = null;

            try
            {
               calculatedValue = "default";
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.DatabaseCollation == calculatedValue)
               element.isDatabaseCollationTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsDatabaseCollationTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelAttribute element)
         {
            // Force the IsDatabaseCollationTracking property to false so that the value  
            // of the DatabaseCollation property is retrieved from storage.  
            element.isDatabaseCollationTrackingPropertyStorage = false;
         }
      }

      internal sealed partial class IsColumnNameTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsColumnNameTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelAttribute element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(ColumnNameDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsColumnNameTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelAttribute element)
         {
            string calculatedValue = null;

            try
            {
               calculatedValue = element.Name;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.ColumnName == calculatedValue)
               element.isColumnNameTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsColumnNameTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelAttribute element)
         {
            // Force the IsColumnNameTracking property to false so that the value  
            // of the ColumnName property is retrieved from storage.  
            element.isColumnNameTrackingPropertyStorage = false;
         }
      }

      internal sealed partial class IsColumnTypeTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsColumnTypeTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelAttribute element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(ColumnTypeDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsColumnTypeTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelAttribute element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = "default";
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.ColumnType == (string)calculatedValue)
               element.isColumnTypeTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsColumnTypeTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelAttribute element)
         {
            // Force the IsColumnTypeTracking property to false so that the value  
            // of the ColumnType property is retrieved from storage.  
            element.isColumnTypeTrackingPropertyStorage = false;
         }
      }

      internal sealed partial class IsImplementNotifyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsImplementNotifyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelAttribute element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(ImplementNotifyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsColumnTypeTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelAttribute element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.ModelClass?.ImplementNotify;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.ImplementNotify == (bool)calculatedValue)
               element.isImplementNotifyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsImplementNotifyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelAttribute element)
         {
            // Force the IsImplementNotifyTracking property to false so that the value  
            // of the ImplementNotify property is retrieved from storage.  
            element.isImplementNotifyTrackingPropertyStorage = false;
         }
      }

      internal sealed partial class IsAutoPropertyTrackingPropertyHandler
      {
         /// <summary>
         ///    Called after the IsAutoPropertyTracking property changes.
         /// </summary>
         /// <param name="element">The model element that has the property that changed. </param>
         /// <param name="oldValue">The previous value of the property. </param>
         /// <param name="newValue">The new value of the property. </param>
         protected override void OnValueChanged(ModelAttribute element, bool oldValue, bool newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);
            if (!element.Store.InUndoRedoOrRollback && newValue)
            {
               DomainPropertyInfo propInfo = element.Store.DomainDataDirectory.GetDomainProperty(AutoPropertyDomainPropertyId);
               propInfo.NotifyValueChange(element);
            }
         }

         /// <summary>Performs the reset operation for the IsAutoPropertyTracking property for a model element.</summary>
         /// <param name="element">The model element that has the property to reset.</param>
         internal void ResetValue(ModelAttribute element)
         {
            object calculatedValue = null;

            try
            {
               calculatedValue = element.ModelClass?.AutoPropertyDefault;
            }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
               if (CriticalException.IsCriticalException(e))
                  throw;
            }

            if (calculatedValue != null && element.AutoProperty == (bool)calculatedValue)
               element.isAutoPropertyTrackingPropertyStorage = true;
         }

         /// <summary>
         ///    Method to set IsAutoPropertyTracking to false so that this instance of this tracking property is not
         ///    storage-based.
         /// </summary>
         /// <param name="element">
         ///    The element on which to reset the property value.
         /// </param>
         internal void PreResetValue(ModelAttribute element)
         {
            // Force the IsAutoPropertyTracking property to false so that the value  
            // of the AutoProperty property is retrieved from storage.  
            element.isAutoPropertyTrackingPropertyStorage = false;
         }
      }

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal virtual void PreResetIsTrackingProperties()
      {
         IsColumnNameTrackingPropertyHandler.Instance.PreResetValue(this);
         IsColumnTypeTrackingPropertyHandler.Instance.PreResetValue(this);
         IsImplementNotifyTrackingPropertyHandler.Instance.PreResetValue(this);
         IsAutoPropertyTrackingPropertyHandler.Instance.PreResetValue(this);
         IsDatabaseCollationTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      // ReSharper disable once UnusedMember.Global
      internal virtual void ResetIsTrackingProperties()
      {
         IsColumnNameTrackingPropertyHandler.Instance.ResetValue(this);
         IsColumnTypeTrackingPropertyHandler.Instance.ResetValue(this);
         IsImplementNotifyTrackingPropertyHandler.Instance.ResetValue(this);
         IsAutoPropertyTrackingPropertyHandler.Instance.ResetValue(this);
         IsDatabaseCollationTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }

      #endregion Tracking Properties

      #region Validation methods

      internal static (string ef6Version, string efCoreVersion)[] GeometryTypes = {
                                                                                        (ef6Version : "Geometry", efCoreVersion : "Geometry")
                                                                                      , (ef6Version : "GeometryPoint", efCoreVersion : "Point")
                                                                                      , (ef6Version : "GeometryLineString", efCoreVersion : "LineString")
                                                                                      , (ef6Version : "GeometryPolygon", efCoreVersion : "Polygon")
                                                                                      , (ef6Version : "GeometryCollection", efCoreVersion : "GeometryCollection")
                                                                                      , (ef6Version : "GeometryMultiPoint", efCoreVersion : "MultiPoint")
                                                                                      , (ef6Version : "GeometryMultiLineString", efCoreVersion : "MultiLineString")
                                                                                      , (ef6Version : "GeometryMultiPolygon", efCoreVersion : "MultiPolygon")
                                                                                  };


      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      [UsedImplicitly]
      private void GeographyTypeDoesNotMatchEFVersion(ValidationContext context)
      {
         if (ModelClass?.ModelRoot == null) return;

         if (ModelClass.ModelRoot.EntityFrameworkVersion == EFVersion.EFCore)
         {
            if (Type.StartsWith("Geography"))
            {
               context.LogError($"{ModelClass.Name}.{Name}: Geography type invalid for EF Core. Use Geometry instead.", "AEInvalidGeography", this);
               hasWarning = true;
               RedrawItem();
            }
            else if (GeometryTypes.Any(g => g.efCoreVersion != g.ef6Version && g.ef6Version == Type))
            {
               context.LogError($"{ModelClass.Name}.{Name}: {Type} type invalid for EF Core.", "AEInvalidGeometry", this);
               hasWarning = true;
               RedrawItem();
            }
         }
         else
         {
            if (GeometryTypes.Any(g => g.efCoreVersion != g.ef6Version && g.efCoreVersion == Type))
            {
               context.LogError($"{ModelClass.Name}.{Name}: {Type} type invalid for EF6.", "AEInvalidGeometry", this);
               hasWarning = true;
               RedrawItem();
            }
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void StringsShouldHaveLength(ValidationContext context)
      {
         if (ModelClass?.ModelRoot == null) return;

         if (ModelClass != null && Type == "String" && (!MaxLength.HasValue || MaxLength.Value == MAXLENGTH_UNDEFINED))
         {
            context.LogWarning($"{ModelClass.Name}.{Name}: String length not specified", "MWStringNoLength", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (ModelClass?.ModelRoot == null) return;

         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (ModelClass != null && modelRoot?.WarnOnMissingDocumentation == true && string.IsNullOrWhiteSpace(Summary))
         {
            context.LogWarning($"{ModelClass.Name}.{Name}: Property should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      #endregion Validation Rules

      #region ColumnName tracking property

      /// <summary>
      /// Change the column name, if it's tracking the name of the property
      /// </summary>
      /// <param name="oldValue"></param>
      /// <param name="newValue"></param>
      protected virtual void OnNameChanged(string oldValue, string newValue)
      {
         // not really a "tracking property" since we're tracking in the same class, so we're handling it a bit differently
         if (ColumnName == oldValue)
            ColumnName = newValue;
      }

      internal sealed partial class NamePropertyHandler
      {
         protected override void OnValueChanged(ModelAttribute element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnNameChanged(oldValue, newValue);
         }
      }

      #endregion ColumnName tracking property

      #region ColumnType tracking property

      /// <summary>
      /// Notify watchers of change to Type property
      /// </summary>
      /// <param name="oldValue"></param>
      /// <param name="newValue"></param>
      protected virtual void OnTypeChanged(string oldValue, string newValue)
      {
         if (ModelClass != null)
         {
            TrackingHelper.UpdateTrackingCollectionProperty(Store,
                                                            ModelClass.Attributes,
                                                            ModelAttribute.ColumnTypeDomainPropertyId,
                                                            ModelAttribute.IsColumnTypeTrackingDomainPropertyId);
         }
      }

      internal sealed partial class TypePropertyHandler
      {
         protected override void OnValueChanged(ModelAttribute element, string oldValue, string newValue)
         {
            base.OnValueChanged(element, oldValue, newValue);

            if (!element.Store.InUndoRedoOrRollback)
               element.OnTypeChanged(oldValue, newValue);
         }
      }

      #endregion ColumnType tracking property

      #region To/From String

      /// <summary>Returns a string that represents the current object.</summary>
      /// <remarks>Output is, in order:
      /// <ul>
      /// <li>Visibility</li>
      /// <li>Type (with optional '?' if not a required field</li>
      /// <li>Min/Max length in brackets, if a string field and length(s) is/are specified</li>
      /// <li>Name (with optional '!' if an identity field</li>
      /// <li>an equal sign (=) followed by an initializer, if an initializer is specified</li>
      /// </ul>
      /// </remarks>
      /// <returns>A string that represents the current object. Note that this is a parsable string when turning back to a ModelAttribute.</returns>
      public override string ToString()
      {
         string visibility = SetterVisibility.ToString().ToLower();
         string identity = IsIdentity ? "!" : string.Empty;

         string nullable = Required ? string.Empty : "?";
         string initial = !string.IsNullOrEmpty(InitialValue) ? $" = {InitialValue.Trim('"')}" : string.Empty;

         return $"{visibility} {Type}{nullable}{LengthDisplay()} {Name}{identity}{initial}";
      }

      private string LengthDisplay()
      {
         string result = string.Empty;

         if (Type != "String")
            return result;

         if (MinLength > 0)
         {
            switch (MaxLength)
            {
               case null:
               case MAXLENGTH_UNDEFINED:
                  result = $"[{MinLength}-]";

                  break;
               case MAXLENGTH_MAX:
                  result = $"[{MinLength}-max]";

                  break;
               default:
                  result = $"[{MinLength}-{MaxLength}]";

                  break;
            }
         }
         else if (MaxLength == MAXLENGTH_MAX)
            result = "[max]";
         else if (MaxLength != null && MaxLength != MAXLENGTH_UNDEFINED)
            result = $"[{MaxLength}]";

         return result;
      }

      /// <summary>
      /// Full display name for this attribute
      /// </summary>
      /// <returns></returns>
      public string ToDisplayString()
      {
         string nullable = Required ? string.Empty : "?";
         string initial = !string.IsNullOrEmpty(InitialValue) ? $" = {InitialValue.Trim('"')}" : string.Empty;

         return $"{Name}: {Type}{nullable}{LengthDisplay()}{initial}";
      }

      /// <summary>
      /// Parses the input string to check for type validity.
      /// </summary>
      /// <param name="modelRoot">Context in which to parse the input</param>
      /// <param name="input">String to parse</param>
      /// <returns>ParseResult object if successful</returns>
      /// <exception cref="ArgumentException">Thrown if unable to parse inut string to a valid type for the model</exception>
      public static ParseResult Parse(ModelRoot modelRoot, string input)
      {
         string _input = input?.Split('{')[0].Trim(';');
         if (_input == null) return null;

         ParseResult result = AttributeParser.Parse(_input);
         if (result != null)
         {
            result.Type = ToCLRType(result.Type);

            if (result.Type != null && !modelRoot.ValidTypes.Contains(result.Type))
            {
               result.Type = ToCLRType(result.Type);
               if (!modelRoot.ValidTypes.Contains(result.Type) && !modelRoot.Enums.Select(e => e.Name).Contains(result.Type))
               {
                  result.Type = null;
                  result.Required = null;
               }
            }
         }
         else
            throw new ArgumentException(AttributeParser.FailMessage);

         return result;
      }

      #endregion Parse string
   }
}
