using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   /// <summary>
   /// Rules used in various places, consolidated here to remove technical debt stemming from copy/paste
   /// </summary>
   public static class CommonRules
   {
      public static string ValidateNamespace(string ns, Func<string, bool> isValidLanguageIndependentIdentifier)
      {
         bool isBad = string.IsNullOrWhiteSpace(ns);
         
         if (!isBad)
         {
            string[] namespaceParts = ns.Split('.');
            foreach (string namespacePart in namespaceParts)
               isBad &= isValidLanguageIndependentIdentifier(namespacePart);
         }

         return isBad ? "Namespace must exist and consist of valid .NET identifiers" : null;
      }
   }
}
