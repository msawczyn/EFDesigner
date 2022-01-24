using System.Drawing;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Extension methods for System.Drawing.Color
   /// </summary>
   public static class ColorExtensions
   {
      /// <summary>
      /// Calculates a readable color for text against a given background
      /// </summary>
      /// <param name="color">The background color</param>
      public static Color LegibleTextColor(this Color color)
      {
         return color.IsLight() ? Color.Black : Color.White;
      }

      public static bool IsDark(this Color color)
      {
         // Counting the perceptive luminance - human eye favors green color... 
         double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

         // bright colors (a < 0.5)
         // dark colors (a >= 0.5)

         return a >= 0.5;
      }

      public static bool IsLight(this Color color)
      {
         return !color.IsDark();
      }
   }
}
