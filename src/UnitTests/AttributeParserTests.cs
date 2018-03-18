using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitTests.Properties;

namespace UnitTests
{
   [TestClass]
   public class AttributeParserTests
   {
      [TestMethod]
      public void TestVariations()
      {
         string[] testStrings = Resources.AttributeTestStrings.Split('\n');
         for (int i = 0; i < testStrings.Length; i++)
            testStrings[i] = testStrings[i].Trim('\r');

         Console.WriteLine("Error,Input,SetterVisibility,Type,Required,MaxLength,Name,IsIdentity,InitialValue");
         foreach (string s in testStrings)
         {
               ParseResult result = AttributeParser.Parse(s);
               Console.WriteLine($"{(result == null ? "\"True\"" : "")},\"{s}\",\"{result?.SetterVisibility}\",\"{result?.Type}\",\"{result?.Required}\",\"{result?.MaxLength}\",\"{result?.Name}\",\"{result?.IsIdentity}\",\"{result?.InitialValue}\"");
         }
      }
   }
}
