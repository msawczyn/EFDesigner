using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

using Sawczyn.EFDesigner.EFModel.Extensions;

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
         foreach (AssociationConnector connector in PresentationViewsSubject.GetPresentation(element).OfType<AssociationConnector>())
            UpdateAssociationDisplay(connector, sourceDeleteAction, targetDeleteAction, sourceMultiplicity, targetMultiplicity);
      }

      public static void UpdateAssociationDisplay(AssociationConnector connector
                                                , DeleteAction? sourceDeleteAction = null
                                                , DeleteAction? targetDeleteAction = null
                                                , Multiplicity? sourceMultiplicity = null
                                                , Multiplicity? targetMultiplicity = null)
      {
         if (!(connector?.ModelElement is Association element))
            return;

         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         sourceDeleteAction = sourceDeleteAction ?? element.SourceDeleteAction;
         targetDeleteAction = targetDeleteAction ?? element.TargetDeleteAction;
         sourceMultiplicity = sourceMultiplicity ?? element.SourceMultiplicity;
         targetMultiplicity = targetMultiplicity ?? element.TargetMultiplicity;

         bool persistent = element.Persistent;

         bool cascade = modelRoot.ShowCascadeDeletes
                     && persistent
                     && (sourceMultiplicity == Multiplicity.One
                      || targetMultiplicity == Multiplicity.One
                      || targetDeleteAction == DeleteAction.Cascade
                      || sourceDeleteAction == DeleteAction.Cascade);

         Color black = Color.FromArgb(255, 113, 111, 110);

         Color lineColor = !persistent
                              ? Color.SlateGray
                              : cascade
                                 ? Color.Red
                                 : black;

         DashStyle lineStyle = cascade
                                  ? DashStyle.Dash
                                  : DashStyle.Solid;

         using (Transaction trans = element.Store.TransactionManager.BeginTransaction("Display associations"))
         {
            if (connector.Color != lineColor)
            {
               connector.Color = lineColor;
               element.InvalidateDiagrams();
            }

            if (connector.DashStyle != lineStyle)
            {
               connector.DashStyle = lineStyle;
               element.InvalidateDiagrams();
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