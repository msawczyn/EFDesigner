using System.Drawing;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ColorExtensions
   {
      public static Color LegibleTextColor(this Color color)
      {
         // Counting the perceptive luminance - human eye favors green color... 
         double a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;

         // bright colors (a < 0.5) - black font
         // dark colors (a >= 0.5) - white font

         return a < 0.5
                   ? Color.Black
                   : Color.White;
      }
   }
}
