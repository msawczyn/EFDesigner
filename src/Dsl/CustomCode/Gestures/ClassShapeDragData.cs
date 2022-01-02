using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using Microsoft.VisualStudio.Modeling.Diagrams;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel
{
   internal class ClassShapeDragData
   {
      private readonly RectangleD initialBoundingBox;
      private readonly double cursorXOffset;
      private readonly double cursorYOffset;
      private readonly List<BidirectionalConnector> priorHighlightedConnectors = new List<BidirectionalConnector>();

      public ClassShape ClassShape { get; }

      public ClassShapeDragData([NotNull] ClassShape classShape, PointD startingMousePosition)
      {
         ClassShape = classShape ?? throw new ArgumentNullException(nameof(classShape));

         cursorXOffset = startingMousePosition.X - classShape.AbsoluteBoundingBox.Left;
         cursorYOffset = startingMousePosition.Y - classShape.AbsoluteBoundingBox.Top;
         initialBoundingBox = classShape.AbsoluteBoundingBox;
      }

      private RectangleD BoundingBoxWithMouseAt(PointD mousePosition)
      {
         double left = mousePosition.X - cursorXOffset;
         double top = mousePosition.Y - cursorYOffset;
         RectangleD boundingBox = new RectangleD(left, top, initialBoundingBox.Width, initialBoundingBox.Height);

         return boundingBox;
      }

      public List<BidirectionalConnector> GetBidirectionalConnectorsUnderShape(PointD mousePosition)
      {
         ModelClass modelClass = (ModelClass)ClassShape?.ModelElement;

         if (modelClass == null)
            return new List<BidirectionalConnector>();

         RectangleD boundingBox = BoundingBoxWithMouseAt(mousePosition);

         List<Guid> connectionObjectsWithAssociationClass = modelClass.Store.ElementDirectory.AllElements
                                                                      .OfType<ModelClass>()
                                                                      .Where(x => x.IsAssociationClass)
                                                                      .Select(x => x.DescribedAssociationElementId)
                                                                      .ToList();

         List<BidirectionalConnector> connectors = ClassShape.Store.ElementDirectory.AllElements
                                                             .OfType<BidirectionalConnector>()
                                                             .Where(c => ((BidirectionalAssociation)c.ModelElement).SourceMultiplicity == Multiplicity.ZeroMany
                                                                      && ((BidirectionalAssociation)c.ModelElement).TargetMultiplicity == Multiplicity.ZeroMany
                                                                      && !connectionObjectsWithAssociationClass.Contains(((BidirectionalAssociation)c.ModelElement).Id)
                                                                      && c.Diagram.Id == ClassShape.Diagram.Id
                                                                      && c.AbsoluteBoundingBox.IntersectsWith(boundingBox))
                                                             .ToList();

         Debug.WriteLine($"{connectors.Count} potential drop target(s)");
         return connectors;
      }

      internal void HighlightActionableClassShapes(PointD mousePosition)
      {
         List<BidirectionalConnector> connectors = GetBidirectionalConnectorsUnderShape(mousePosition);
         HighlightedShapesCollection highlightedShapes = ClassShape.Diagram.ActiveDiagramView.DiagramClientView.HighlightedShapes;

         DiagramItem classShapeItem = new DiagramItem(ClassShape);

         if (highlightedShapes.Contains(classShapeItem))
         {
            highlightedShapes.Remove(classShapeItem);
            ClassShape.Invalidate();
         }

         foreach (BidirectionalConnector connector in priorHighlightedConnectors)
         {
            highlightedShapes.Remove(new DiagramItem(connector));
            connector.Invalidate();
         }

         priorHighlightedConnectors.Clear();

         if (connectors.Any())
         {
            priorHighlightedConnectors.AddRange(connectors);

            highlightedShapes.Add(classShapeItem);
            ClassShape.Invalidate();

            foreach (BidirectionalConnector connector in connectors.Where(c => !highlightedShapes.Contains(new DiagramItem(c))))
            {
               highlightedShapes.Add(new DiagramItem(connector));
               connector.Invalidate();
            }
         }
      }
   }
}