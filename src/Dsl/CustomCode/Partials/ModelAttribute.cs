using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using Sawczyn.EFDesigner.EFModel.CustomCode.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{

   [ValidationState(ValidationState.Enabled)]
   public partial class ModelAttribute : IModelElementInCompartment, IDisplaysWarning
   {
      public IModelElementWithCompartments ParentModelElement => ModelClass;

      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;

      public static readonly string[] SpatialTypes =
      {
         "Geography"
       , "GeographyCollection"
       , "GeographyLineString"
       , "GeographyMultiLineString"
       , "GeographyMultiPoint"
       , "GeographyMultiPolygon"
       , "GeographyPoint"
       , "GeographyPolygon"
       , "Geometry"
       , "GeometryCollection"
       , "GeometryLineString"
       , "GeometryMultiLineString"
       , "GeometryMultiPoint"
       , "GeometryMultiPolygon"
       , "GeometryPoint"
       , "GeometryPolygon"
      };

      public static readonly string[] ValidTypes =
      {
         "Binary"
       , "Boolean"
       , "Byte"
       , "byte"
       , "DateTime"
       , "DateTimeOffset"
       , "Decimal"
       , "Double"
       , "Geography"
       , "GeographyCollection"
       , "GeographyLineString"
       , "GeographyMultiLineString"
       , "GeographyMultiPoint"
       , "GeographyMultiPolygon"
       , "GeographyPoint"
       , "GeographyPolygon"
       , "Geometry"
       , "GeometryCollection"
       , "GeometryLineString"
       , "GeometryMultiLineString"
       , "GeometryMultiPoint"
       , "GeometryMultiPolygon"
       , "GeometryPoint"
       , "GeometryPolygon"
       , "Guid"
       , "Int16"
       , "Int32"
       , "Int64"
       , "Single"
       , "String"
       , "Time"
      };

      public static readonly string[] ValidCLRTypes =
      {
         "Binary",
         "Boolean", "Boolean?", "Nullable<Boolean>",
         "Byte", "Byte?", "Nullable<Byte>",
         "DateTime", "DateTime?", "Nullable<DateTime>",
         "DateTimeOffset", "DateTimeOffset?", "Nullable<DateTimeOffset>",
         "DbGeography",
         "DbGeometry",
         "Decimal", "Decimal?", "Nullable<Decimal>",
         "Double", "Double?", "Nullable<Double>",
         "Geography",
         "GeographyCollection",
         "GeographyLineString",
         "GeographyMultiLineString",
         "GeographyMultiPoint",
         "GeographyMultiPolygon",
         "GeographyPoint",
         "GeographyPolygon",
         "Geometry",
         "GeometryCollection",
         "GeometryLineString",
         "GeometryMultiLineString",
         "GeometryMultiPoint",
         "GeometryMultiPolygon",
         "GeometryPoint",
         "GeometryPolygon",
         "Guid", "Guid?", "Nullable<Guid>",
         "Int16", "Int16?", "Nullable<Int16>",
         "Int32", "Int32?", "Nullable<Int32>",
         "Int64", "Int64?", "Nullable<Int64>",
         "Single", "Single?", "Nullable<Single>",
         "String",
         "Time",
         "TimeSpan", "TimeSpan?", "Nullable<TimeSpan>",
         "bool", "bool?", "Nullable<bool>",
         "byte", "byte?", "Nullable<byte>",
         "byte[]",
         "decimal", "decimal?", "Nullable<decimal>",
         "double", "double?", "Nullable<double>",
         "int", "int?", "Nullable<int>",
         "long", "long?", "Nullable<long>",
         "short", "short?", "Nullable<short>",
         "string"
      };

      public static readonly string[] ValidIdentityAttributeTypes =
      {
         "Int16",
         "Int32",
         "Int64",
         "Byte",
         "Guid"
      };

      #region Warning display

      // set as methods to avoid issues around serialization

      private bool hasWarning;

      public bool GetHasWarningValue() => hasWarning;

      public void ResetWarning() => hasWarning = false;

      public void RedrawItem()
      {
         List<ShapeElement> shapeElements = PresentationViewsSubject.GetPresentation(ParentModelElement as ModelElement).OfType<ShapeElement>().ToList();
         foreach (ShapeElement shapeElement in shapeElements)
            shapeElement.Invalidate();
      }

      #endregion

      /// <summary>
      /// Tests if the InitialValue property is valid for the type indicated
      /// </summary>
      /// <param name="typeName">Name of type to test. If typeName is null, Type property will be used. If initialValue is null, InitialValue property will be used</param>
      /// <param name="initialValue">Initial value to test</param>
      /// <returns>true if InitialValue is a valid value for the type, or if initialValue is null or empty</returns>
#pragma warning disable 168
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
               return false; //string.IsNullOrEmpty(initialValue);
            case "Boolean":
               return bool.TryParse(initialValue, out bool _bool);
            case "Byte":
               return byte.TryParse(initialValue, out byte _byte);
            case "DateTime":
               switch (initialValue?.Trim())
               {
                  case "DateTime.Now":
                  case "DateTime.MinValue":
                  case "DateTime.MaxValue":
                     return true;
                  default:
                     return DateTime.TryParse(initialValue, out DateTime _dateTime);
               }
            case "DateTimeOffset":
               return DateTimeOffset.TryParse(initialValue, out DateTimeOffset _dateTimeOffset);
            case "Decimal":
               return decimal.TryParse(initialValue, out decimal _decimal);
            case "Double":
               return double.TryParse(initialValue, out double _double);
            case "Guid":
               return Guid.TryParse(initialValue, out Guid _guid);
            case "Int16":
               return short.TryParse(initialValue, out short _int16);
            case "Int32":
               return int.TryParse(initialValue, out int _int32);
            case "Int64":
               return long.TryParse(initialValue, out long _int64);
            case "Single":
               return float.TryParse(initialValue, out float _single);
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

      public static bool IsValidCLRType(string type)
      {
         return ValidCLRTypes.Contains(type);
      }

      public string PrimitiveType => ToPrimitiveType(Type);

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
      // ReSharper disable once UnusedMember.Global
      public string CLRType => ToCLRType(Type);

      /// <summary>
      /// From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity
      /// </summary>
      /// <param name="typeName"></param>
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
            case "Geography":
            case "GeographyPoint":
            case "GeographyLineString":
            case "GeographyPolygon":
            case "GeographyMultiPoint":
            case "GeographyMultiLineString":
            case "GeographyMultiPolygon":
            case "GeographyCollection":
               return "DbGeography";
            case "Geometry":
            case "GeometryPoint":
            case "GeometryLineString":
            case "GeometryPolygon":
            case "GeometryMultiPoint":
            case "GeometryMultiLineString":
            case "GeometryMultiPolygon":
            case "GeometryCollection":
               return "DbGeometry";
            case "Int16":
               return "short";
            case "Int32":
               return "int";
            case "Int64":
               return "long";
            case "String":
               return "string";
         }

         return typeName;
      }

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
            case "int":
               return "Int32";
            case "long":
               return "Int64";
            case "string":
               return "String";
         }

         return typeName;
      }

      /// <summary>Storage for the ColumnName property.</summary>  
      private string columnNameStorage; 

      public string GetColumnNameValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         return !loading && IsColumnNameTracking ? Name : columnNameStorage;
      }

      public void SetColumnNameValue(string value)
      {
         columnNameStorage = value == Name ? null : value;
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            // ReSharper disable once ArrangeRedundantParentheses
            IsColumnNameTracking = (columnNameStorage == null);
      }

      /// <summary>Storage for the ColumnType property.</summary>  
      private string columnTypeStorage; 

      public string GetColumnTypeValue()
      {
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         return !loading && IsColumnTypeTracking ? "default" : columnTypeStorage;
      }

      public void SetColumnTypeValue(string value)
      {
         columnTypeStorage = value.ToLowerInvariant() == "default" ? null : value;
         bool loading = Store.TransactionManager.InTransaction && Store.TransactionManager.CurrentTransaction.IsSerializing;

         if (!Store.InUndoRedoOrRollback && !loading)
            // ReSharper disable once ArrangeRedundantParentheses
            IsColumnTypeTracking = (columnTypeStorage == null);
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
            element.isColumnTypeTrackingPropertyStorage = (element.ColumnType == "default");
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

      /// <summary>
      ///    Calls the pre-reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void PreResetIsTrackingProperties()
      {
         IsColumnNameTrackingPropertyHandler.Instance.PreResetValue(this);
         IsColumnTypeTrackingPropertyHandler.Instance.PreResetValue(this);
         // same with other tracking properties as they get added
      }

      /// <summary>
      ///    Calls the reset method on the associated property value handler for each
      ///    tracking property of this model element.
      /// </summary>
      internal virtual void ResetIsTrackingProperties()
      {
         IsColumnNameTrackingPropertyHandler.Instance.ResetValue(this);
         IsColumnTypeTrackingPropertyHandler.Instance.ResetValue(this);
         // same with other tracking properties as they get added
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void StringsShouldHaveLength(ValidationContext context)
      {
         if (Type == "String" && MaxLength == 0)
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
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation)
         {
            if (string.IsNullOrWhiteSpace(Summary))
            {
               context.LogWarning($"{ModelClass.Name}.{Name}: Property should be documented", "AWMissingSummary", this);
               hasWarning = true;
               RedrawItem();
            }
         }
      }

      #region To/From String

      /// <summary>Returns a string that represents the current object.</summary>
      /// <remarks>Output is, in order:
      /// <ul>
      /// <li>Visibility</li>
      /// <li>Type (with optional '?' if not a required field</li>
      /// <li>Max length in brackets, if a string field and length is specified</li>
      /// <li>Name (with optional '!' if an identity field</li>
      /// <li>an equal sign (=) followed by an initializer, if an initializer is specified</li>
      /// </ul>
      /// </remarks>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         List<string> parts = new List<string>
                              {
                                 SetterVisibility.ToString().ToLower(),
                                 $"{Type}{(Required ? string.Empty : "?")}"
                              };

         if (Type?.ToLower() == "string")
         {
            // if a min length is present, output both the min and max
            // otherwise, just the max, if present
            if (MinLength > 0)
               parts.Add($"[{MinLength}-{MaxLength}]");
            else if (MaxLength > 0)
               parts.Add($"[{MaxLength}]");
         }

         parts.Add($"{Name}{(IsIdentity ? "!" : string.Empty)}");

         if (!string.IsNullOrEmpty(InitialValue))
         {
            string initialValue = InitialValue;

            // make sure string initial values are in quotes, but don't duplicate quotes if already present
            if (Type?.ToLower() == "string")
               initialValue = $"\"{InitialValue.Trim('"')}\"";

            parts.Add($"= {initialValue}");
         }

         // get rid of the space between type name and length, if any
         return string.Join(" ", parts).Replace(" [", "[");
      }

      public static ParseResult Parse(ModelRoot modelRoot, string input)
      {
         string _input = input?.Split('{')[0].Trim(';');
         if (_input == null) return null;

         ParseResult result = AttributeParser.Parse(_input);
         if (result != null)
         {
            result.Type = ToCLRType(result.Type);

            if (result.Type != null && !ValidTypes.Contains(result.Type))
            {
               result.Type = ToCLRType(result.Type);
               if (!ValidTypes.Contains(result.Type) && !modelRoot.Enums.Select(e => e.Name).Contains(result.Type))
               {
                  result.Type = null;
                  result.Required = null;
               }
            }
         }
         else
         {
            throw new ArgumentException(AttributeParser.FailMessage);
         }

         return result;
      }

      #endregion Parse string
   }
}
