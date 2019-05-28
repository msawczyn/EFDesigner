using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class PresentationHelper
   {
      public static void ColorShapeOutline(ModelClass element)
      {
         foreach (ClassShape classShape in PresentationViewsSubject
                                          .GetPresentation(element)
                                          .OfType<ClassShape>())
         {
            if (element.IsAbstract)
            {
               classShape.OutlineColor = Color.OrangeRed;
               classShape.OutlineThickness = 0.03f;
               classShape.OutlineDashStyle = DashStyle.Dot;
            }
            else if (element.IsDependentType)
            {
               classShape.OutlineColor = Color.ForestGreen;
               classShape.OutlineThickness = 0.03f;
               classShape.OutlineDashStyle = DashStyle.Dot;
            }
            else if (element.ImplementNotify)
            {
               classShape.OutlineColor = Color.CornflowerBlue;
               classShape.OutlineThickness = 0.03f;
               classShape.OutlineDashStyle = DashStyle.Dot;
            }
            else
            {
               classShape.OutlineColor = Color.Black;
               classShape.OutlineThickness = 0.01f;
               classShape.OutlineDashStyle = DashStyle.Solid;
            }
         }
      }
   }
}
