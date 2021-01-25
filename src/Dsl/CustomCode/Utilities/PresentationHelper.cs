using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Methods to shortcut working with diagram views
   /// </summary>
   public static class PresentationHelper
   {
      /// <summary>
      /// Redraws the association on every open diagram
      /// </summary>
      /// <param name="element"></param>
      /// <param name="sourceDeleteAction"></param>
      /// <param name="targetDeleteAction"></param>
      /// <param name="sourceMultiplicity"></param>
      /// <param name="targetMultiplicity"></param>
      public static void UpdateAssociationDisplay(Association element
                                                , DeleteAction? sourceDeleteAction = null
                                                , DeleteAction? targetDeleteAction = null)
      {
         // redraw on every diagram
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

      /// <summary>
      /// Redraws the association on every open diagram
      /// </summary>
      /// <param name="connector"></param>
      /// <param name="sourceDeleteAction"></param>
      /// <param name="targetDeleteAction"></param>
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

         Color lineColor = !persistent
                              ? Color.SlateGray
                              : cascade
                                 ? Color.Red
                                 : Color.FromArgb(255, 113, 111, 110);

         DashStyle lineStyle = cascade
                                  ? DashStyle.Dash
                                  : DashStyle.Solid;

         using (Transaction trans = element.Store.TransactionManager.BeginTransaction("Display associations"))
         {
            //SetConnectorWidth(connector);

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

      //private static void SetConnectorWidth(AssociationConnector connector)
      //{
      //   if (!(connector?.ModelElement is Association element))
      //      return;

      //   BidirectionalAssociation bidirectionalElement = connector.ModelElement as BidirectionalAssociation;
      //   PenSettings settings = connector.StyleSet.GetOverriddenPenSettings(DiagramPens.ConnectionLine) ?? new PenSettings();

      //   bool hasAutoInclude = element.TargetAutoInclude || (bidirectionalElement?.SourceAutoInclude == true);
      //   settings.Width = hasAutoInclude ? 0.04f : 0.01f;

      //   connector.StyleSet.OverridePen(DiagramPens.ConnectionLine, settings);
      //}

      /// <summary>
      /// Redraws the class on every open diagram
      /// </summary>
      /// <param name="element"></param>
      public static void UpdateClassDisplay(ModelClass element)
      {
         if (element == null)
            return;

         // ensure foreign key attributes have the proper setting to surface the right glyph
         foreach (var data in element.Store.ElementDirectory.AllElements
                                     .OfType<Association>()
                                     .Where(a => a.Dependent == element && !string.IsNullOrEmpty(a.FKPropertyName))
                                     .SelectMany(association => association.FKPropertyName.Split(',')
                                                                           .Where(propertyName => element.Attributes.Any(attr => attr.Name == propertyName))
                                                                           .Select(propertyName => new
                                                                                                   {
                                                                                                         Assoc = association
                                                                                                       , Attr = element.Attributes.FirstOrDefault(attr => attr.Name == propertyName)
                                                                                                   })))
            data.Attr.IsForeignKeyFor = data.Assoc.Id;

         // update on every diagram
         foreach (ClassShape classShape in PresentationViewsSubject
                                          .GetPresentation(element)
                                          .OfType<ClassShape>())
            classShape.Invalidate();

         // ensure any associations have the correct end for composition ownership
         foreach (AssociationConnector connector in element.Store.ElementDirectory
                                                           .AllElements.OfType<Association>()
                                                           .Where(a => a.Dependent == element)
                                                           .SelectMany(association => PresentationViewsSubject.GetPresentation(association)
                                                                                                              .OfType<AssociationConnector>()))
            connector.Invalidate();
      }
   }
}