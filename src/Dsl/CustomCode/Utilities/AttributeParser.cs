using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   class AttributeParser
   {
      private static Func<string, ParseResult>[] _parsers =
      {
            Parser1
      };

      private static ParseResult Parser1(string txt)
      {
         #region Examples
         // foo : string
         // foo : string = hello
         // foo : string?
         // foo : string? = hello
         // foo : string?[50]
         // foo : string?[50] = hello
         // foo : string[50]
         // foo : string[50] = hello
         // foo! : string
         // foo! : string = hello
         // foo! : string?
         // foo! : string? = hello
         // foo! : string?[50]
         // foo! : string?[50] = hello
         // foo! : string[50]
         // foo! : string[50] = hello
         // public foo : string
         // public foo : string = hello
         // public foo : string?
         // public foo : string? = hello
         // public foo : string?[50]
         // public foo : string?[50] = hello
         // public foo : string[50]
         // public foo : string[50] = hellos
         // public foo! : string
         // public foo! : string = hello
         // public foo! : string?
         // public foo! : string? = hello
         // public foo! : string?[50]
         // public foo! : string?[50] = hello
         // public foo! : string[50]
         // public foo! : string[50] = hello
         #endregion

         string visibility = "(public|protected|internal)";
         string name = "((?:[@A-Za-z_][A-Za-z0-9_]*))";
         string type = "((?:[A-Za-z_][A-Za-z0-9_]*))";
         string optional = @"(\?)";
         string identity = "(!)";
         string length = @"(\d+)";
         string initialValue = "((?:.+))";

         Regex r = new Regex($@"{visibility}?.*?{name}{identity}?(.*?:.*?{type}{optional}?(\[{length}\])?(.*?=.*?{initialValue})?)?", RegexOptions.Singleline);

         Match m = r.Match(txt);
         return m.Success
                      ? new ParseResult
                      {
                         SetterVisibility = m.Groups[1].ToString() == "protected"
                                                       ? SetterAccessModifier.Protected
                                                       : m.Groups[1].ToString() == "internal"
                                                             ? SetterAccessModifier.Internal
                                                             : SetterAccessModifier.Public,
                         Name = m.Groups[2].ToString(),
                         IsIdentity = !string.IsNullOrEmpty(m.Groups[3].ToString()),
                         Type = !string.IsNullOrEmpty(m.Groups[5].ToString()) ? m.Groups[5].ToString() : "String",
                         Required = string.IsNullOrEmpty(m.Groups[6].ToString()),
                         MaxLength = m.Groups[5].ToString().ToLowerInvariant() == "string" && int.TryParse(m.Groups[8].ToString(), out int _maxLength) ? _maxLength : 0,
                         InitialValue = m.Groups[10].ToString().Trim('"', ' ')
                      }
                      : null;
      }

      private static ParseResult Parser2(string txt)
      {
         #region Examples
         // foo = hello
         // foo! = hello
         // public foo = hello
         // public foo! = hello
         // public string foo = hello
         // public string foo! = hello
         // public string foo!
         // public string foo
         // public string? foo = hello
         // public string? foo! = hello
         // public string? foo!
         // public string? foo
         // public string?[50] foo = hello
         // public string?[50] foo! = hello
         // public string?[50] foo!
         // public string?[50] foo
         // public string[50] foo = hello
         // public string[50] foo! = hello
         // public string[50] foo!
         // public string[50] foo
         // string foo = hello
         // string foo! = hello
         // string foo!
         // string foo
         // string? foo = hello
         // string? foo! = hello
         // string? foo!
         // string? foo
         // string?[50] foo = hello
         // string?[50] foo! = hello
         // string?[50] foo!
         // string?[50] foo
         // string[50] foo = hello
         // string[50] foo! = hello
         // string[50] foo!
         // string[50] foo
         #endregion
         string visibility = "(public|protected|internal)";
         string name = "((?:[@A-Za-z_][A-Za-z0-9_]*))";
         string type = "((?:[A-Za-z_][A-Za-z0-9_]*))";
         string optional = @"(\?)";
         string identity = "(!)";
         string length = @"(\d+)";
         string initialValue = "((?:.+))";

         Regex r = new Regex($@"{visibility}?.*?{type}{optional}?((\[){length}(\]))?.*?{name}{identity}?(.*?(=).*?{initialValue})?", RegexOptions.Singleline);
         Match m = r.Match(txt);
         return m.Success
                      ? new ParseResult
                      {
                         SetterVisibility = m.Groups[1].ToString() == "protected"
                                                       ? SetterAccessModifier.Protected
                                                       : m.Groups[1].ToString() == "internal"
                                                             ? SetterAccessModifier.Internal
                                                             : SetterAccessModifier.Public,
                         Type = !string.IsNullOrEmpty(m.Groups[2].ToString()) ? m.Groups[2].ToString() : "String",
                         Required = string.IsNullOrEmpty(m.Groups[3].ToString()),
                         MaxLength = m.Groups[2].ToString().ToLowerInvariant() == "string" && int.TryParse(m.Groups[6].ToString(), out int _maxLength) ? _maxLength : 0,
                         Name = m.Groups[8].ToString(),
                         IsIdentity = !string.IsNullOrEmpty(m.Groups[9].ToString()),
                         InitialValue = m.Groups[12].ToString().Trim('"', ' ')
                      }
                      : null;
      }

      private static ParseResult Parser3(string txt)
      {
         string visibility = "(public|protected|internal)";
         string name = "((?:[@A-Za-z_][A-Za-z0-9_]*))";
         string identity = "(!)";

         Regex r = new Regex($@"^{visibility}?.*?{name}{identity}?", RegexOptions.Singleline);
         Match m = r.Match(txt);
         return m.Success
                      ? new ParseResult
                      {
                         SetterVisibility = m.Groups[1].ToString() == "protected"
                                                     ? SetterAccessModifier.Protected
                                                     : m.Groups[1].ToString() == "internal"
                                                           ? SetterAccessModifier.Internal
                                                           : SetterAccessModifier.Public,
                         Type = "String",
                         Required = true,
                         MaxLength = 0,
                         Name = m.Groups[2].ToString(),
                         IsIdentity = !string.IsNullOrEmpty(m.Groups[3].ToString()),
                         InitialValue = ""
                      }
                      : null;
      }

      public static ParseResult Parse(string txt)
      {
         return _parsers.Select(parser => parser(txt.Trim(';', ' '))).FirstOrDefault(result => result != null);
      }
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
}
