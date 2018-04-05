using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sawczyn.EFDesigner.EFModel;

namespace UnitTests
{
   [TestClass]
   public class AttributeParserTests
   {
      private readonly string[] input =
      {
         "foo : int",
         "foo : string",
         "foo : string?",
         "foo : string?(0-50)",
         "foo : string?(0-50)= \"hello\"",
         "foo : string?(50)",
         "foo : string?(50)= \"hello\"",
         "foo : string?(50-0)",
         "foo : string?(50-0)= \"hello\"",
         "foo : string?[0-50]",
         "foo : string?[0-50]= \"hello\"",
         "foo : string?[50-0]",
         "foo : string?[50-0]= \"hello\"",
         "foo : string?[50]",
         "foo : string?[50]= \"hello\"",
         "foo! : int = 12",
         "foo! : int",
         "foo",
         "int foo! = 12",
         "int foo!",
         "int foo",
         "public foo : int = 12",
         "public foo : int",
         "public foo : string",
         "public foo : string(0-50)",
         "public foo : string(0-50)= \"hello\"",
         "public foo : string(50)",
         "public foo : string(50)= \"hello\"",
         "public foo : string(50-0)",
         "public foo : string(50-0)= \"hello\"",
         "public foo : string?",
         "public foo : string?(0-50)",
         "public foo : string?(0-50)= \"hello\"",
         "public foo : string?(50)",
         "public foo : string?(50)= \"hello\"",
         "public foo : string?(50-0)",
         "public foo : string?(50-0)= \"hello\"",
         "public foo : string?= \"hello\"",
         "public foo : string?[0-50]",
         "public foo : string?[0-50]= \"hello\"",
         "public foo : string?[50-0]",
         "public foo : string?[50-0]= \"hello\"",
         "public foo : string?[50]",
         "public foo : string?[50]= \"hello\"",
         "public foo : string[0-50]",
         "public foo : string[0-50]= \"hello\"",
         "public foo : string[50-0]",
         "public foo : string[50-0]= \"hello\"",
         "public foo : string[50]",
         "public foo : string[50]= \"hello\"",
         "public foo = \"hello\"",
         "public foo = \"hello\"",
         "public foo! : int = 12",
         "public foo! : int",
         "public foo! = 12",
         "public foo! = 12",
         "public foo!",
         "public foo",
         "public int foo = 12",
         "public int foo! = 12",
         "public int foo!",
         "public int foo",
         "public string foo",
         "public string(0-50) foo = \"hello\"",
         "public string(0-50) foo",
         "public string(50) foo = \"hello\"",
         "public string(50) foo",
         "public string(50-0) foo = \"hello\"",
         "public string(50-0) foo",
         "public string? foo = \"hello\"",
         "public string? foo",
         "public string?(0-50) foo = \"hello\"",
         "public string?(0-50) foo",
         "public string?(50) foo = \"hello\"",
         "public string?(50) foo",
         "public string?(50-0) foo = \"hello\"",
         "public string?(50-0) foo",
         "public string?[0-50] foo = \"hello\"",
         "public string?[0-50] foo",
         "public string?[50-0] foo = \"hello\"",
         "public string?[50-0] foo",
         "public string?[50] foo = \"hello\"",
         "public string?[50] foo",
         "public string[0-50] foo = \"hello\"",
         "public string[0-50] foo",
         "public string[50-0] foo = \"hello\"",
         "public string[50-0] foo",
         "public string[50] foo = \"hello\"",
         "public string[50] foo",
         "string foo",
         "string? foo",
         "string?(0-50) foo = \"hello\"",
         "string?(0-50) foo",
         "string?(50) foo = \"hello\"",
         "string?(50) foo",
         "string?(50-0) foo = \"hello\"",
         "string?(50-0) foo",
         "string?[0-50] foo = \"hello\"",
         "string?[0-50] foo",
         "string?[50-0] foo = \"hello\"",
         "string?[50-0] foo",
         "string?[50] foo = \"hello\"",
         "string?[50] foo"
      };

      private readonly string MissingName = "";
      private readonly string InvalidIdentifier = "1abc";
      private readonly string InvertedLengths = "string[50-1] foo";
      private readonly string NegativeLength = "string[-50] foo";
      private readonly string BadVisibility = "pubic string foo";

      [TestMethod]
      public void TestParserWithValidInput()
      {
         Console.WriteLine("Input,Visibility,Name,Type,Min,Max,Required,Identity,Value");
         foreach (string s in input)
         {
            string _input = s?.Split('{')[0].Trim(';');
            if (s == null)
               throw new Exception("bad input");

            ParseResult result = AttributeParser.Parse(_input);
            Assert.IsNotNull(result, s);
            Console.WriteLine($"{s},{result.SetterVisibility ?? SetterAccessModifier.Public},{result.Name},{result.Type ?? "string"},{result.MinLength ?? 0},{result.MaxLength ?? 0},{result.Required ?? false},{result.IsIdentity},{result.InitialValue ?? "(null)"}");
         }
      }

      [TestMethod]
      public void MustHaveName()
      {
         ParseResult result = AttributeParser.Parse(MissingName);
         Assert.IsNull(result);
      }

      [TestMethod]
      public void MustBeValidIdentifier()
      {
         ParseResult result = AttributeParser.Parse(InvalidIdentifier);
         Assert.IsNull(result);
      }

      [TestMethod]
      public void MinMustBeLessThanMaxLength()
      {
         ParseResult result = AttributeParser.Parse(InvertedLengths);
         Assert.IsNull(result);
      }

      [TestMethod]
      public void LengthsCannotBeNegative()
      {
         ParseResult result = AttributeParser.Parse(NegativeLength);
         Assert.IsNull(result);
      }

      [TestMethod]
      public void VisibilityMustBeValid()
      {
         ParseResult result = AttributeParser.Parse(BadVisibility);
         Assert.IsNull(result);
      }
   }
}
