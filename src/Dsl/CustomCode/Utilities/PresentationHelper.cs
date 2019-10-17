using System.Collections.Generic;
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
                                             TextColor = classShape.TextColor
                                          };

                  classShape.FillColor = Color.White;
                  classShape.OutlineColor = Color.Gainsboro;
                  classShape.TextColor = Color.SlateGray;
               }
            }
            else
            {
               if (element.IsPersistent)
               {
                  classShape.FillColor = classShape.ColorCache.FillColor;
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

      public static void UpdateDisplayForCascadeDelete(Association element,
                                                       DeleteAction? sourceDeleteAction = null,
                                                       DeleteAction? targetDeleteAction = null,
                                                       Multiplicity? sourceMultiplicity = null,
                                                       Multiplicity? targetMultiplicity = null)
      {
         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         sourceDeleteAction = sourceDeleteAction ?? element.SourceDeleteAction;
         targetDeleteAction = targetDeleteAction ?? element.TargetDeleteAction;
         sourceMultiplicity = sourceMultiplicity ?? element.SourceMultiplicity;
         targetMultiplicity = targetMultiplicity ?? element.TargetMultiplicity;

         bool cascade = modelRoot.ShowCascadeDeletes &&
                        element.Persistent &&
                        (sourceMultiplicity == Multiplicity.One ||
                         targetMultiplicity == Multiplicity.One ||
                         targetDeleteAction == DeleteAction.Cascade ||
                         sourceDeleteAction == DeleteAction.Cascade);

         List<AssociationConnector> changeColor =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.Color != Color.Red) ||
                                                        (!cascade && connector.Color != Color.Black))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.DashStyle != DashStyle.Dash) ||
                                                        (!cascade && connector.DashStyle != DashStyle.Solid))
                                    .ToList();

         foreach (AssociationConnector connector in changeColor)
         {
            connector.Color = cascade
                                 ? Color.Red
                                 : Color.Black;
         }

         foreach (AssociationConnector connector in changeStyle)
         {
            connector.DashStyle = cascade
                                     ? DashStyle.Dash
                                     : DashStyle.Solid;
         }
      }

      public static void UpdateDisplayForPersistence(Association element)
      {
         // don't change unless necessary so as to not set the model's dirty flag without need
         bool persistent = element.Persistent;

         List<AssociationConnector> changeColors =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.Color != Color.Black) ||
                                                        (!persistent && connector.Color != Color.Gray))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && connector.DashStyle != DashStyle.Solid) ||
                                                        (!persistent && connector.DashStyle != DashStyle.Dash))
                                    .ToList();

         foreach (AssociationConnector connector in changeColors)
         {
            connector.Color = persistent
                                 ? Color.Black
                                 : Color.Gray;
         }

         foreach (AssociationConnector connector in changeStyle)
         {
            connector.DashStyle = persistent
                                     ? DashStyle.Solid
                                     : DashStyle.Dash;
         }
      }

   }
}
