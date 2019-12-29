// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Miscellaneous;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;
using Microsoft.VisualStudio.Modeling.Extensibility;

using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

using Sawczyn.EFDesigner.EFModel.Extensions;

using LineSegment = Microsoft.Msagl.Core.Geometry.Curves.LineSegment;

namespace Sawczyn.EFDesigner.EFModel
{
   public class FileDotEngine : IDotEngine
   {    
      public string Run(GraphvizImageType imageType, string dot, string outputFileName)
      {
         using (StreamWriter writer = new StreamWriter(outputFileName))
         {
            writer.Write(dot);    
         }

         return Path.GetFileName(outputFileName);
      }
   }
   internal class Commands
   {
      internal static void LayoutDiagram(EFModelDiagram diagram)
      {
         try
         {
            Cursor.Current = Cursors.WaitCursor;

            ModelRoot modelRoot = diagram.Store.ModelRoot();

            using (Transaction tx = diagram.Store.TransactionManager.BeginTransaction("ModelAutoLayout"))
            {
               List<NodeShape> nodeShapes = diagram.NestedChildShapes.Where(s => s.IsVisible).OfType<NodeShape>().ToList();
               List<BinaryLinkShape> linkShapes = diagram.NestedChildShapes.Where(s => s.IsVisible).OfType<BinaryLinkShape>().ToList();

               // The standard DSL layout method was selected. Just do the deed and be done with it.
               // otherwise, we need to run an MSAGL layout
               if (modelRoot.LayoutAlgorithm == LayoutAlgorithm.Default || modelRoot.LayoutAlgorithmSettings == null)
               {
                  DoGraphvizLayout(nodeShapes, linkShapes, diagram);
                  DoStandardLayout(linkShapes, diagram);
               }
               else
                  DoCustomLayout(nodeShapes, linkShapes, modelRoot);

               tx.Commit();
            }
         }
         finally
         {
            Cursor.Current = Cursors.Default;
         }
      }

      private static void DoGraphvizLayout(List<NodeShape> nodeShapes, List<BinaryLinkShape> linkShapes, EFModelDiagram diagram)
      {
         SEdge<NodeShape>[] edges = linkShapes.Select(s => new SEdge<NodeShape>(s.FromShape, s.ToShape)).ToArray();
         BidirectionalGraph<NodeShape, SEdge<NodeShape>> graph = edges.ToBidirectionalGraph<NodeShape, SEdge<NodeShape>>(true);
         graph.AddVertexRange(nodeShapes.Except(edges.Select(e => e.Source).Union(edges.Select(e => e.Target))));
         graph.RemoveVertexIf(n => n is CommentBoxShape);

         GraphvizAlgorithm<NodeShape, SEdge<NodeShape>> graphviz = new GraphvizAlgorithm<NodeShape, SEdge<NodeShape>>(graph);

         graphviz.FormatVertex += (sender, args) =>
                                  {
                                     if (args.Vertex is ClassShape classShape)
                                        args.VertexFormatter.Label = ((ModelClass)classShape.ModelElement).Name;
                                     else if (args.Vertex is EnumShape enumShape)
                                        args.VertexFormatter.Label = ((ModelEnum)enumShape.ModelElement).Name;
                                     else
                                        args.VertexFormatter.Label = Guid.NewGuid().ToString();

                                     args.VertexFormatter.FixedSize = true;
                                     args.VertexFormatter.Size = new GraphvizSizeF((float)args.Vertex.Size.Width, (float)args.Vertex.Size.Height);
                                  };

         graphviz.Generate(new FileDotEngine(), @"C:\Temp\GraphML.dot");
      }

      private static void DoCustomLayout(List<NodeShape> nodeShapes, List<BinaryLinkShape> linkShapes, ModelRoot modelRoot)
      {
         GeometryGraph graph = new GeometryGraph();

         CreateDiagramNodes(nodeShapes, graph);
         CreateDiagramLinks(linkShapes, graph);

         AddDesignConstraints(linkShapes, modelRoot, graph);

         LayoutHelpers.CalculateLayout(graph, modelRoot.LayoutAlgorithmSettings, null);

         // Move model to positive axis.
         graph.UpdateBoundingBox();
         graph.Translate(new Point(-graph.Left, -graph.Bottom));

         UpdateNodePositions(graph);
         UpdateConnectors(graph);
      }

      private static void CreateDiagramNodes(List<NodeShape> nodeShapes, GeometryGraph graph)
      {
         foreach (NodeShape nodeShape in nodeShapes)
         {
            ICurve graphRectangle = CurveFactory.CreateRectangle(nodeShape.Bounds.Width,
                                                                 nodeShape.Bounds.Height,
                                                                 new Point(nodeShape.Bounds.Center.X,
                                                                           nodeShape.Bounds.Center.Y));

            Node diagramNode = new Node(graphRectangle, nodeShape);
            graph.Nodes.Add(diagramNode);
         }
      }

      private static void CreateDiagramLinks(List<BinaryLinkShape> linkShapes, GeometryGraph graph)
      {
         foreach (BinaryLinkShape linkShape in linkShapes)
         {
            graph.Edges.Add(new Edge(graph.FindNodeByUserData(linkShape.Nodes[0]),
                                     graph.FindNodeByUserData(linkShape.Nodes[1]))
            {
               UserData = linkShape
            });
         }
      }

      private static void AddDesignConstraints(List<BinaryLinkShape> linkShapes, ModelRoot modelRoot, GeometryGraph graph)
      {
         // Sugiyama allows for layout constraints, so we can make sure that base classes are above derived classes,
         // and put classes derived from the same base in the same vertical layer. Unfortunately, other layout strategies
         // don't have that ability.
         if (modelRoot.LayoutAlgorithmSettings is SugiyamaLayoutSettings sugiyamaSettings)
         {
            // ensure generalizations are vertically over each other
            foreach (GeneralizationConnector linkShape in linkShapes.OfType<GeneralizationConnector>())
            {
               if (modelRoot.LayoutAlgorithm == LayoutAlgorithm.Sugiyama)
               {
                  int upperNodeIndex = linkShape.Nodes[1].ModelElement.GetBaseElement() == linkShape.Nodes[0].ModelElement ? 0 : 1;
                  int lowerNodeIndex = upperNodeIndex == 0 ? 1 : 0;

                  sugiyamaSettings.AddUpDownConstraint(graph.FindNodeByUserData(linkShape.Nodes[upperNodeIndex]),
                                                       graph.FindNodeByUserData(linkShape.Nodes[lowerNodeIndex]));
               }
            }

            // add constraints ensuring descendents of a base class are on the same level
            Dictionary<string, List<NodeShape>> derivedClasses = linkShapes.OfType<GeneralizationConnector>()
                                                                           .SelectMany(ls => ls.Nodes)
                                                                           .Where(n => n.ModelElement is ModelClass mc && mc.BaseClass != null)
                                                                           .GroupBy(n => ((ModelClass)n.ModelElement).BaseClass)
                                                                           .ToDictionary(n => n.Key, n => n.ToList());

            foreach (KeyValuePair<string, List<NodeShape>> derivedClassData in derivedClasses)
            {
               Node[] siblingNodes = derivedClassData.Value.Select(graph.FindNodeByUserData).ToArray();
               sugiyamaSettings.AddSameLayerNeighbors(siblingNodes);
            }
         }
      }

      private static void UpdateNodePositions(GeometryGraph graph)
      {
         foreach (Node node in graph.Nodes)
         {
            NodeShape nodeShape = (NodeShape)node.UserData;
            nodeShape.Bounds = new RectangleD(node.BoundingBox.Left, node.BoundingBox.Top, node.BoundingBox.Width, node.BoundingBox.Height);
         }
      }

      //private static void AutoRouteConnector(BinaryLinkShape connector)
      //{
      //   if (connector != null)
      //   {
      //      connector.ManuallyRouted = false;
      //      connector.FixedFrom = VGFixedCode.NotFixed;
      //      connector.FixedTo = VGFixedCode.NotFixed;

      //      foreach (ShapeElement element in connector.RelativeChildShapes)
      //      {
      //         if (element is LineLabelShape lineLabelShape)
      //         {
      //            lineLabelShape.ManuallySized = false;
      //            lineLabelShape.ManuallyPlaced = false;
      //         }
      //      }

      //      connector.RecalculateRoute();
      //   }
      //}

      private static void UpdateConnectors(GeometryGraph graph)
      {

         foreach (Edge edge in graph.Edges)
         {
            BinaryLinkShape linkShape = (BinaryLinkShape)edge.UserData;

            // need to mark the connector as dirty. this is the easiest way to do this
            linkShape.ManuallyRouted = !linkShape.ManuallyRouted;
            linkShape.FixedFrom = VGFixedCode.NotFixed;
            linkShape.FixedTo = VGFixedCode.NotFixed;

            // make the labels follow the lines
            foreach (LineLabelShape lineLabelShape in linkShape.RelativeChildShapes.OfType<LineLabelShape>())
            {
               lineLabelShape.ManuallySized = false;
               lineLabelShape.ManuallyPlaced = false;
            }

            linkShape.EdgePoints.Clear();

            // MSAGL deals in line segments; DSL deals in points
            // with the segments, tne end of one == the beginning of the next, so we can use just the beginning point
            // of each segment. 
            // But we have to hang on to the end point so that, when we hit the last segment, we can finish off the
            // set of points
            if (edge.Curve is LineSegment lineSegment)
            {
               // When curve is a single line segment.
               linkShape.EdgePoints.Add(new EdgePoint(lineSegment.Start.X, lineSegment.Start.Y, VGPointType.Normal));
               linkShape.EdgePoints.Add(new EdgePoint(lineSegment.End.X, lineSegment.End.Y, VGPointType.Normal));
            }
            else if (edge.Curve is Curve curve)
            {
               // When curve is a complex segment.
               EdgePoint lastPoint = null;

               foreach (ICurve segment in curve.Segments)
               {
                  switch (segment.GetType().Name)
                  {
                     case "LineSegment":
                        LineSegment line = segment as LineSegment;
                        linkShape.EdgePoints.Add(new EdgePoint(line.Start.X, line.Start.Y, VGPointType.Normal));
                        lastPoint = new EdgePoint(line.End.X, line.End.Y, VGPointType.Normal);

                        break;

                     case "CubicBezierSegment":
                        CubicBezierSegment bezier = segment as CubicBezierSegment;

                        // there are 4 segments. Store all but the last one
                        linkShape.EdgePoints.Add(new EdgePoint(bezier.B(0).X, bezier.B(0).Y, VGPointType.Normal));
                        linkShape.EdgePoints.Add(new EdgePoint(bezier.B(1).X, bezier.B(1).Y, VGPointType.Normal));
                        linkShape.EdgePoints.Add(new EdgePoint(bezier.B(2).X, bezier.B(2).Y, VGPointType.Normal));
                        lastPoint = new EdgePoint(bezier.B(3).X, bezier.B(3).Y, VGPointType.Normal);

                        break;

                     case "Ellipse":
                        // rather than draw a curved line, we'll bust the curve into 5 parts and draw those as straight lines
                        Ellipse ellipse = segment as Ellipse;
                        double interval = (ellipse.ParEnd - ellipse.ParStart) / 5.0;
                        lastPoint = null;

                        for (double i = ellipse.ParStart; i <= ellipse.ParEnd; i += interval)
                        {
                           Point p = ellipse.Center
                                   + (Math.Cos(i) * ellipse.AxisA)
                                   + (Math.Sin(i) * ellipse.AxisB);

                           // we'll remember the one we just calculated, but store away the one we calculated last time around
                           // (if there _was_ a last time around). That way, when we're done, we'll have stored all of them except
                           // for the last one
                           if (lastPoint != null)
                              linkShape.EdgePoints.Add(lastPoint);

                           lastPoint = new EdgePoint(p.X, p.Y, VGPointType.Normal);
                        }

                        break;
                  }
               }

               // finally tuck away the last one. Now we don't have duplicate points in our list
               if (lastPoint != null)
                  linkShape.EdgePoints.Add(lastPoint);
            }

            // since we're not changing the nodes this edge connects, this really doesn't do much.
            // what it DOES do, however, is call ConnectEdgeToNodes, which is an internal method we'd otherwise
            // be unable to access
            linkShape.Connect(linkShape.FromShape, linkShape.ToShape);
            linkShape.ManuallyRouted = false;
         }
      }

      private static void DoStandardLayout(List<BinaryLinkShape> linkShapes, EFModelDiagram diagram)
      {
         // first we need to mark all the connectors as dirty so they'll route. Easiest way is to flip their 'ManuallyRouted' flag
         foreach (BinaryLinkShape linkShape in linkShapes)
            linkShape.ManuallyRouted = !linkShape.ManuallyRouted;

         // now let the layout mechanism route the connectors by setting 'ManuallyRouted' to false, regardless of what it was before
         foreach (BinaryLinkShape linkShape in linkShapes)
            linkShape.ManuallyRouted = false;

         diagram.AutoLayoutShapeElements(diagram.NestedChildShapes.Where(s => s.IsVisible).ToList(),
                                         VGRoutingStyle.VGRouteStraight,
                                         PlacementValueStyle.VGPlaceSN,
                                         true);
      }
   }
}