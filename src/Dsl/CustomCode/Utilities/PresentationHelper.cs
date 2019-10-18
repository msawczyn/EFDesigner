using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{

   public static class PresentationHelper
   {
      public static void UpdateAssociationDisplay(Association element
                                                , DeleteAction? sourceDeleteAction = null
                                                , DeleteAction? targetDeleteAction = null
                                                , Multiplicity? sourceMultiplicity = null
                                                , Multiplicity? targetMultiplicity = null)
      {
         if (element == null)
            return;

         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         sourceDeleteAction = sourceDeleteAction ?? element.SourceDeleteAction;
         targetDeleteAction = targetDeleteAction ?? element.TargetDeleteAction;
         sourceMultiplicity = sourceMultiplicity ?? element.SourceMultiplicity;
         targetMultiplicity = targetMultiplicity ?? element.TargetMultiplicity;

         bool persistent = element.Persistent;

         bool cascade = modelRoot.ShowCascadeDeletes
                     && persistent
                     && (sourceMultiplicity == Multiplicity.One || 
                         targetMultiplicity == Multiplicity.One || 
                         targetDeleteAction == DeleteAction.Cascade || 
                         sourceDeleteAction == DeleteAction.Cascade);

         Color black = Color.FromArgb(255, 113, 111, 110);

         List<AssociationConnector> changeColor =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (cascade && connector.Color != Color.Red)
                                                     || (persistent && connector.Color != Color.SlateGray)
                                                     || (!cascade && connector.Color != black))
                                    .ToList();

         List<AssociationConnector> changeStyle =
            PresentationViewsSubject.GetPresentation(element)
                                    .OfType<AssociationConnector>()
                                    .Where(connector => (persistent && cascade && connector.DashStyle != DashStyle.Dash)
                                                     || ((!persistent || !cascade) && connector.DashStyle != DashStyle.Solid))
                                    .ToList();

         using (Transaction trans = element.Store.TransactionManager.BeginTransaction("Display associations"))
         {
            foreach (AssociationConnector connector in changeColor)
            {
               if (persistent && !cascade)
                  connector.Color = black;
               else if (!persistent)
                  connector.Color = Color.SlateGray;
               else
                  connector.Color = Color.Red;
            }

            foreach (AssociationConnector connector in changeStyle)
            {
               if (persistent && cascade)
                  connector.DashStyle = DashStyle.Dash;
               else
                  connector.DashStyle = DashStyle.Solid;
            }

            trans.Commit();
         }
      }

      public static void UpdateClassDisplay(ModelClass element)
      {
         if (element == null)
            return;

         foreach (ClassShape classShape in PresentationViewsSubject
                                          .GetPresentation(element)
                                          .OfType<ClassShape>())
         {
            if (classShape.ColorCache == null)
            {
               if (!element.IsPersistent)
               {
                  classShape.ColorCache = new ColorCache { FillColor = classShape.FillColor, TextColor = classShape.TextColor };

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
   }

}