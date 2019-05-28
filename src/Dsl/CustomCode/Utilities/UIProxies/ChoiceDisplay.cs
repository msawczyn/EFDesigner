using System.Collections.Generic;

namespace Sawczyn.EFDesigner
{
   public static class ChoiceDisplay
   {
      public delegate string ChoiceVisualizer(string title, IEnumerable<string> choices);
      private static ChoiceVisualizer ChoiceVisualizerMethod;

      public static string GetChoice(string title, IEnumerable<string> choices)
      {
         if (ChoiceVisualizerMethod != null)
         {
            try
            {
               return ChoiceVisualizerMethod(title, choices);
            }
            catch
            {
               return null;
            }
         }
         return null;
      }

      public static void RegisterDisplayHandler(ChoiceVisualizer method)
      {
         ChoiceVisualizerMethod = method;
      }

   }
}