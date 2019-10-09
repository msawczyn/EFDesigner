using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class PresentationHelper
   {
      public static void SetClassVisuals(ModelClass element)
      {
         foreach (ClassShape classShape in PresentationViewsSubject
                                          .GetPresentation(element)
                                          .OfType<ClassShape>())
         {
            if (classShape.ColorCache == null)
            {
               if (!element.IsPersistent)
               {
                  classShape.ColorCache = new ColorCache
                                          {
                                             FillColor = classShape.FillColor, 
                                             LineColor = classShape.OutlineColor, 
                                             TextColor = classShape.TextColor
                                          };
               }

               classShape.FillColor = Color.White;
               classShape.OutlineColor = Color.Gainsboro;
            }
            else
            {
               if (element.IsPersistent)
               {
                  classShape.FillColor = classShape.ColorCache.FillColor;
                  classShape.OutlineColor = classShape.ColorCache.LineColor;
                  classShape.TextColor = classShape.ColorCache.TextColor;
               }

               classShape.ColorCache = null;
            }

            if (element.IsPersistent)
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
}
