using System.Drawing;
using System.Drawing.Drawing2D;

namespace Sawczyn.EFDesigner.EFModel {
   public interface IHighlightFromModelExplorer
   {
      Color OutlineColor { get; set; }
      DashStyle OutlineDashStyle { get; set; }
      float OutlineThickness { get; set; }
   }
}