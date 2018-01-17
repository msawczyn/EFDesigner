using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Modeling.Validation;
using Sawczyn.EFDesigner.EFModel.CustomCode.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Tag interface indicating diagram items for this element are compartments in a parent element
   /// </summary>
   public interface IModelElementCompartmented
   {
      IModelElementWithCompartments ParentModelElement { get; }
      string CompartmentName { get; }
   }

   [ValidationState(ValidationState.Enabled)]
   public partial class ModelAttribute : IModelElementCompartmented
   {
      public IModelElementWithCompartments ParentModelElement => ModelClass;

      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;

      public static readonly string[] ValidTypes =
      {
         "Binary",
         "Boolean",
         "Byte",
         "DateTime",
         "DateTimeOffset",
         "Decimal",
         "Double",
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
         "Guid",
         "Int16",
         "Int32",
         "Int64",
         //"SByte",
         "Single",
         "String",
         "Time"
      };

      public string CLRType => ToCLRType(Type);

#pragma warning disable 168
      public bool HasValidDefault()
      {
         if (string.IsNullOrEmpty(InitialValue))
            return true;

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
               return string.IsNullOrEmpty(InitialValue);
            case "Boolean":
               return bool.TryParse(InitialValue, out bool _bool);
            case "Byte":
               return byte.TryParse(InitialValue, out byte _byte);
            case "DateTime":
               if (InitialValue?.Trim() == "DateTime.Now") return true;
               return DateTime.TryParse(InitialValue, out DateTime _dateTime);
            case "DateTimeOffset":
               return DateTimeOffset.TryParse(InitialValue, out DateTimeOffset _dateTimeOffset);
            case "Decimal":
               return decimal.TryParse(InitialValue, out decimal _decimal);
            case "Double":
               return double.TryParse(InitialValue, out double _double);
            case "Guid":
               return Guid.TryParse(InitialValue, out Guid _guid);
            case "Int16":
               return short.TryParse(InitialValue, out short _int16);
            case "Int32":
               return int.TryParse(InitialValue, out int _int32);
            case "Int64":
               return long.TryParse(InitialValue, out long _int64);
            //case "SByte":
            //   return sbyte.TryParse(InitialValue, out sbyte _sbyte);
            case "Single":
               return float.TryParse(InitialValue, out float _single);
            case "String":
               return true;
            case "Time":
               return DateTime.TryParseExact(
                                             InitialValue,
                                             new[] { "HH:mm:ss", "H:mm:ss", "HH:mm", "H:mm", "HH:mm:ss tt", "H:mm:ss tt", "HH:mm tt", "H:mm tt" },
                                             CultureInfo.InvariantCulture,
                                             DateTimeStyles.None,
                                             out DateTime _time);
            default:
               if (InitialValue.Contains("."))
               {
                  string[] parts = InitialValue.Split('.');
                  ModelEnum enumType = ModelClass.ModelRoot.Enums.FirstOrDefault(x => x.Name == parts[0]);
                  return enumType != null && parts.Length == 2 && enumType.Values.Any(x => x.Name == parts[1]);
               }

               break;
         }

         return false;
      }
#pragma warning restore 168

      /// <summary>
      /// From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity
      /// </summary>
      /// <param name="typeName"></param>
      /// <returns></returns>
      public static string ToCLRType(string typeName)
      {
         switch (typeName)
         {
            case "Binary":
               return "byte[]";
            case "Boolean":
               return "bool";
            case "Byte":
               return "byte";
            case "DateTime":
               return "DateTime";
            case "Time":
               return "TimeSpan";
            case "DateTimeOffset":
               return "DateTimeOffset";
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
            case "Guid":
               return "Guid";
            case "Single":
               return "Single";
            //case "SByte":
            //   return "sbyte";
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

      public static string FromCLRType(string typeName)
      {
         switch (typeName)
         {
            case "byte[]":
               return "Binary";
            case "bool":
               return "Boolean";
            case "byte":
               return "Byte";
            case "DateTime":
               return "DateTime";
            case "TimeSpan":
               return "Time";
            case "DateTimeOffset":
               return "DateTimeOffset";
            case "decimal":
               return "Decimal";
            case "double":
               return "Double";
            case "DbGeography":
               return "Geography";
            case "DbGeometry":
               return "Geometry";
            case "Guid":
               return "Guid";
            case "Single":
               return "Single";
            //case "sbyte":
            //   return "SByte";
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

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void StringsShouldHaveLength(ValidationContext context)
      {
         if (Type == "String" && MaxLength == 0)
            context.LogWarning("String length not specified", "MWStringNoLength", this);
      }

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation)
         {
            if (string.IsNullOrWhiteSpace(Summary))
               context.LogWarning($"Attribute {ModelClass.Name}.{Name} should be documented", "AWMissingSummary", this);
         }
      }

      #region Parse string

      // Note: gave some thought to making this be an LALR parser, but that's WAY overkill for what needs done here. Regex is good enough.

      private const string NAME        = "(?<name>[A-Za-z_][A-za-z0-9_]*[!]?)";
      private const string TYPE        = "(?<type>[A-Za-z_][A-za-z0-9_]*[?]?)";
      private const string LENGTH      = @"(?<length>\d+)";
      private const string STRING_TYPE = "(?<type>[Ss]tring)";
      private const string VISIBILITY  = @"(?<visibility>public\s+|protected\s+)";
      private const string INITIAL     = @"(=\s*(?<initialValue>.+))";
      private const string WS          = @"\s*";
      private const string BODY        = @"(\{.+)";

      private static readonly Regex Pattern = new Regex($@"^{WS}{VISIBILITY}?{NAME}{WS}{INITIAL}?$|" +
                                                        $@"^{WS}{VISIBILITY}?{STRING_TYPE}\[{LENGTH}\]\s+{NAME}{WS}{INITIAL}?$|" +
                                                        $@"^{WS}{VISIBILITY}?{STRING_TYPE}\({LENGTH}\)\s+{NAME}{WS}{INITIAL}?$|" +
                                                        $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{STRING_TYPE}\[{LENGTH}\]{WS}{INITIAL}?$|" +
                                                        $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{STRING_TYPE}\({LENGTH}\){WS}{INITIAL}?$|" +
                                                        $@"^{WS}{VISIBILITY}?{TYPE}\s+{NAME}{WS}({INITIAL}?;?|{BODY})?$|" +
                                                        $@"^{WS}{VISIBILITY}?{NAME}{WS}:{WS}{TYPE}{WS}{INITIAL}?$", RegexOptions.Compiled);

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         List<string> parts = new List<string>
                              {
                                 SetterVisibility.ToString().ToLower(),
                                 $"{Type}{(Required ? "" : "?")}",
                                 $"{Name}{(IsIdentity ? "!" : "")}"
                              };

         if (Type?.ToLower() == "string" && MaxLength > 0)
            parts.Add($"[{MaxLength}]");
         
         if (!string.IsNullOrEmpty(InitialValue))
         {
            string initialValue = InitialValue;
            if (Type?.ToLower() == "string") initialValue = $@"""{InitialValue}""";
            parts.Add($"= {initialValue}");
         }

         return string.Join(" ", parts);
      }

      public class ParseResult
      {
         public SetterAccessModifier? SetterVisibility { get; set; }
         public string Name { get; set; }
         public string Type { get; set; }
         public bool? Required { get; set; }
         public int? MaxLength { get; set; }
         public string InitialValue { get; set; }
         public bool IsIdentity { get; set; }
      }

      public static ParseResult Parse(ModelRoot modelRoot, string input)
      {
         Match match = Pattern.Match(input);
         if (match.Success)
         {
            ParseResult result = new ParseResult();

            if (match.Groups["visibility"].Success)
               result.SetterVisibility = match.Groups["visibility"].Value.Trim() == "protected" ? SetterAccessModifier.Protected : SetterAccessModifier.Public;

            if (match.Groups["name"].Success)
            {
               result.Name = match.Groups["name"].Value.Trim();
               if (result.Name.EndsWith("!")) result.IsIdentity = true;
               result.Name = result.Name.Trim('!');
            }

            if (match.Groups["type"].Success)
            {
               result.Type = match.Groups["type"].Value.Trim();
               result.Required = !result.Type.EndsWith("?");
               result.Type = result.Type.Trim('?');
               if (!ValidTypes.Contains(result.Type))
               {
                  result.Type = FromCLRType(result.Type);
                  if (!ValidTypes.Contains(result.Type) && !modelRoot.Enums.Select(e => e.Name).Contains(result.Type))
                  {
                     result.Type = null;
                     result.Required = null;
                  }
               }
            }

            if (result.Type == "String" && match.Groups["length"].Success && !string.IsNullOrWhiteSpace(match.Groups["length"].Value.Trim()))
               result.MaxLength = int.Parse(match.Groups["length"].Value.Trim());

            if (match.Groups["initialValue"].Success)
               result.InitialValue = match.Groups["initialValue"].Value.Trim();

            return result;
         }

         return null; // couldn't parse
      }

      #endregion Parse string
   }
}
