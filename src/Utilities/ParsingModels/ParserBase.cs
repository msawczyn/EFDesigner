using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ParsingModels
{
   public abstract class ParserBase
   {
      protected static readonly Regex TypeNameRegex = new Regex(@"([^`]+)`\d\[(\[[^\]]+\])(,(\[[^\]]+\]))*\]", RegexOptions.Compiled);

      protected static string GetTypeFullName(Type type)
      {
         return GetTypeFullName(type?.FullName);
      }

      protected static string GetTypeFullName(string fullName)
      {
         if (string.IsNullOrWhiteSpace(fullName))
            return null;

         Match m = TypeNameRegex.Match(fullName);

         if (m.Success)
         {
            List<string> typeNames = new List<string>();
            string baseName = m.Groups[1].Value;
            typeNames.Add(m.Groups[2].Value.Trim('[', ']').Split(',')[0]);

            if (m.Groups.Count > 2)
            {
               foreach (Capture capture in m.Groups[3].Captures)
                  typeNames.Add(capture.Value.Trim(',', '[', ']').Split(',')[0]);
            }

            return $"{baseName}<{string.Join(",", typeNames)}>";
         }

         return fullName;
      }

      protected static string GetCustomAttributes(Type type)
      {
         return type == null
                   ? string.Empty
                   : GetCustomAttributes(type.CustomAttributes);
      }

      protected static readonly List<string> IgnoreAttributes = new List<string>(new[]
                                                                                 {
                                                                                    "System.SerializableAttribute"
                                                                                  , "System.Runtime.InteropServices.ComVisibleAttribute"
                                                                                  , "__DynamicallyInvokableAttribute"
                                                                                  , "System.Reflection.DefaultMemberAttribute"
                                                                                  , "System.Runtime.Versioning.NonVersionableAttribute"
                                                                                  , "System.FlagsAttribute"
                                                                                  , "TableAttribute("
                                                                                  , "IsReadOnlyAttribute("
                                                                                  , "NullableAttribute("
                                                                                  , "NullableContextAttribute("
                                                                                 });
      protected static string GetCustomAttributes(IEnumerable<CustomAttributeData> customAttributeData)
      {
         List<string> customAttributes = customAttributeData.Select(a => a.ToString()).ToList();
         customAttributes.RemoveAll(s => IgnoreAttributes.Select(s.Contains).Any());

         return string.Join("", customAttributes);
      }

      protected static Multiplicity ConvertMultiplicity(RelationshipMultiplicity relationshipMultiplicity)
      {
         Multiplicity multiplicity = Multiplicity.ZeroOne;

         switch (relationshipMultiplicity)
         {
            case RelationshipMultiplicity.ZeroOrOne:
               multiplicity = Multiplicity.ZeroOne;

               break;

            case RelationshipMultiplicity.One:
               multiplicity = Multiplicity.One;

               break;

            case RelationshipMultiplicity.Many:
               multiplicity = Multiplicity.ZeroMany;

               break;
         }

         return multiplicity;
      }

   }
}
