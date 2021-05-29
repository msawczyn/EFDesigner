using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParsingModels
{
   public abstract class ParserBase
   {
      protected static readonly Regex TypeNameRegex = new Regex(@"([^`]+)`\d\[(\[[^\]]+\])(,(\[[^\]]+\]))*\]", RegexOptions.Compiled);

      protected static string GetTypeFullName(Type type)
      {
         return GetTypeFullName(type.FullName);
      }

      protected static string GetTypeFullName(string fullName)
      {
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

   }
}
