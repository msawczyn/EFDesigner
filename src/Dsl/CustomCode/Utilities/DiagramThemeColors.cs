using System.Drawing;

namespace Sawczyn.EFDesigner.EFModel
{
   public class DiagramThemeColors
   {
      public DiagramThemeColors(Color background)
      {
         Background = background;
      }

      public Color Background { get; }
      public Color Text => Background.LegibleTextColor();
      public Color HeaderBackground => Background.IsDark() ? Color.Gray : Color.LightGray;
      public Color HeaderText => HeaderBackground.LegibleTextColor();
   }
}