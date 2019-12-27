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
         foreach (AssociationConnector connector in PresentationViewsSubject.GetPresentation(element).OfType<AssociationConnector>().Distinct())
            UpdateAssociationDisplay(connector, sourceDeleteAction, targetDeleteAction);
      }

      /// <summary>
      /// Determine delete behavior on the principal end
      /// </summary>
      /// <param name="association">Association to interrogate</param>
      /// <param name="sourceAction">Result of calculation for source side</param>
      /// <param name="targetAction">Result of calculation for target side</param>
      private static void GetEffectiveDeleteAction(Association association, out DeleteAction? sourceAction, out DeleteAction? targetAction)
      {
         sourceAction = null;
         if (association.SourceRole == EndpointRole.Principal)
         {
            if (association.SourceDeleteAction == DeleteAction.Cascade || association.SourceDeleteAction == DeleteAction.None)
               sourceAction = association.SourceDeleteAction;
            else if (association.SourceMultiplicity == Multiplicity.One)
               sourceAction = DeleteAction.Cascade;
            else
               sourceAction = DeleteAction.None;
         }

         targetAction = null;
         if (association.TargetRole == EndpointRole.Principal)
         {
            if (association.TargetDeleteAction == DeleteAction.Cascade || association.TargetDeleteAction == DeleteAction.None)
               targetAction = association.TargetDeleteAction;
            else if (association.TargetMultiplicity == Multiplicity.One)
               targetAction = DeleteAction.Cascade;
            else
               targetAction = DeleteAction.None;
         }
      }

      public static void UpdateAssociationDisplay(AssociationConnector connector
                                                , DeleteAction? sourceDeleteAction = null
                                                , DeleteAction? targetDeleteAction = null)
      {
         if (!(connector?.ModelElement is Association element))
            return;

         ModelRoot modelRoot = element.Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();

         GetEffectiveDeleteAction(element, out DeleteAction? calculatedSourceDeleteAction, out DeleteAction? calculatedTargetDeleteAction);

         sourceDeleteAction = sourceDeleteAction != null && sourceDeleteAction != DeleteAction.Default 
                                 ? sourceDeleteAction 
                                 : calculatedSourceDeleteAction;

         targetDeleteAction = targetDeleteAction != null && targetDeleteAction != DeleteAction.Default
                                 ? targetDeleteAction
                                 : calculatedTargetDeleteAction;

         bool persistent = element.Persistent;

         bool cascade = modelRoot.ShowCascadeDeletes
                     && persistent
                     && (targetDeleteAction == DeleteAction.Cascade
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
            SetConnectorWidth(connector);

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

      private static void SetConnectorWidth(AssociationConnector connector)
      {
         PenSettings settings = connector.StyleSet.GetOverriddenPenSettings(DiagramPens.ConnectionLine) ?? new PenSettings();

         settings.Width = connector.ManuallyRouted
                             ? 0.02f
                             : 0.01f;

         connector.StyleSet.OverridePen(DiagramPens.ConnectionLine, settings);
      }

      public static void UpdateClassDisplay(ModelClass element)
      {
         if (element == null)
            return;

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