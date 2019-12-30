// 

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using QuickGraph.Serialization;

using Sawczyn.EFDesigner.EFModel.Extensions;

using Shields.GraphViz.Models;

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
   public class DotEngine : IDotEngine
   {    
      public string Run(GraphvizImageType imageType, string dot, string outputFileName)
      {
         return dot;
      }
   }

   public class DotNode
   {
      public double X { get; set; }
      public double Y { get; set; }
      public NodeShape Shape { get; set; }
   }

   public class DotEdge : IEdge<DotNode>
   {
      public BinaryLinkShape Shape { get; set; }
      public List<(double X, double Y)> EndPoints { get; } = new List<(double X, double Y)>();
      public DotNode Source { get; set; }
      public DotNode Target { get; set; }
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
               List<DotNode> vertices = diagram.NestedChildShapes
                                               .Where(s => s.IsVisible)
                                               .OfType<NodeShape>()
                                               .Select(node => new DotNode {Shape = node})
                                               .ToList();

               List<DotEdge> edges = diagram.NestedChildShapes
                                            .Where(s => s.IsVisible)
                                            .OfType<BinaryLinkShape>()
                                            .Select(link => new DotEdge
                                                            {
                                                               Shape = link
                                                             , Source = vertices.Single(vertex => vertex.Shape.Id == link.FromShape.Id)
                                                             , Target = vertices.Single(vertex => vertex.Shape.Id == link.ToShape.Id)
                                                            })
                                            .ToList();

               // The standard DSL layout method was selected. Just do the deed and be done with it.
               // otherwise, we need to run an MSAGL layout
               if (modelRoot.LayoutAlgorithm == LayoutAlgorithm.Default || modelRoot.LayoutAlgorithmSettings == null)
               {
                  // use graphviz as the default if available
                  if (File.Exists(EFModelPackage.Options.DotExePath))
                     DoGraphvizLayout(vertices, edges, diagram);
                  else
                     DoStandardLayout(edges.Select(edge => edge.Shape).ToList(), diagram);
               }
               else
                  DoMSAGLLayout(vertices.Select(node => node.Shape).ToList(), edges.Select(edge => edge.Shape).ToList(), modelRoot);

               tx.Commit();
            }
         }
         finally
         {
            Cursor.Current = Cursors.Default;
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

      private static void DoGraphvizLayout(List<DotNode> vertices, List<DotEdge> edges, EFModelDiagram diagram)
      {
         // set up to be a bidirectional graph with the edges we found
         BidirectionalGraph<DotNode, DotEdge> graph = edges.ToBidirectionalGraph<DotNode, DotEdge>(true);

         // add all the vertices that aren't connected by edges
         graph.AddVertexRange(vertices.Except(edges.Select(e => e.Source).Union(edges.Select(e => e.Target))));

         // we'll process as Graphviz
         GraphvizAlgorithm<DotNode, DotEdge> graphviz = new GraphvizAlgorithm<DotNode, DotEdge>(graph);
         graphviz.GraphFormat.NodeSeparation = 1.0;
         graphviz.CommonVertexFormat.Shape = GraphvizVertexShape.Record;

         // labels will be the Id of the underlying Shape
         graphviz.FormatVertex += (sender, args) =>
                                  {
                                     args.VertexFormatter.Label = args.Vertex.Shape.ModelElement is ModelClass modelClass
                                                                     ? modelClass.Name
                                                                     : args.Vertex.Shape.ModelElement is ModelEnum modelEnum
                                                                        ? modelEnum.Name
                                                                        : args.Vertex.Shape.ModelElement.Id.ToString();
                                     args.VertexFormatter.FixedSize = true;
                                     args.VertexFormatter.Size = new GraphvizSizeF((float)args.Vertex.Shape.Size.Width, 
                                                                                   (float)args.Vertex.Shape.Size.Height);

                                     args.VertexFormatter.Label = args.Vertex.Shape.Id.ToString();
                                  };

         graphviz.FormatEdge += (sender, args) =>
                                {
                                   args.EdgeFormatter.Label.Value = args.Edge.Shape.Id.ToString();
                                };
         // generate the commands
         string dotCommands = graphviz.Generate(new DotEngine(), null);

         // splines doesn't appear to be available in the QuickGraph implementation for GraphViz
         dotCommands = dotCommands.Replace("nodesep=1", "graph [splines=ortho, nodesep=1] ");
         Debug.WriteLine(dotCommands);

         ProcessStartInfo dotStartInfo = new ProcessStartInfo(EFModelPackage.Options.DotExePath, "-T plain")
                                         {
                                            RedirectStandardInput = true, 
                                            RedirectStandardOutput = true, 
                                            UseShellExecute = false,
                                            CreateNoWindow = true
                                         };
         string graphOutput;

         using (Process dotProcess = Process.Start(dotStartInfo))
         {
            // stdin is redirected to our stream, so pump the commands in through that
            dotProcess.StandardInput.WriteLine(dotCommands);
            // closing the stream starts the process
            dotProcess.StandardInput.Close();
            // stdout is redirected too, so capture the output
            graphOutput = dotProcess.StandardOutput.ReadToEnd();
            dotProcess.WaitForExit();
         }
         Debug.WriteLine(graphOutput);

         // break it up into lines of text for processing
         string[] outputLines = graphOutput.Split(new []{'\r','\n'}, StringSplitOptions.RemoveEmptyEntries);
         SizeD graphSize = SizeD.Empty;

         // ReSharper disable once LoopCanBePartlyConvertedToQuery
         foreach (string outputLine in outputLines)
         {
            // spaces aren't valid in any of the data, so we can treat them as delimiters
            string[] parts = outputLine.Split(' ');
            string id;
            double x, y;

            // graphviz coordinates have 0,0 at the bottom left, positive means moving up
            // our coordinates have 0,0 at the top left, positive means moving down
            // so we need to transform them
            switch (parts[0])
            {
               case "graph":
                  // 0     1 2      3
                  // graph 1 109.38 92.681
                  graphSize = new SizeD(double.Parse(parts[2]), double.Parse(parts[3]));
                  break;

               case "node":
                  // 0    1  2      3      4   5      6                                      7
                  // node 71 78.514 93.639 1.5 3.3056 "0f651fe7-da0f-453f-a08a-ec1d31ec0e71" solid record black lightgrey
                  id = parts[6].Trim('"');
                  DotNode dotNode = vertices.Single(v => v.Shape.Id.ToString() == id); // label

                  x = double.Parse(parts[2]);
                  y = graphSize.Height - double.Parse(parts[3]);

                  dotNode.Shape.Bounds = new RectangleD(x, y, dotNode.Shape.Size.Width, dotNode.Shape.Size.Height);

                  break;

               case "edge":
                  // 0    1 2  3 4      5      6      7      8      9     10     11    12
                  // edge 6 18 4 34.926 77.518 34.926 77.518 34.926 75.88 34.926 75.88 "567b5db7-7591-4aa7-845c-76635bf56f28" 36.083 77.16 solid black
                  id = parts[4 + int.Parse(parts[3]) * 2].Trim('"');
                  DotEdge edge = edges.Single(e => e.Shape.Id.ToString() == id);

                  // need to mark the connector as dirty. this is the easiest way to do this
                  BinaryLinkShape linkShape = edge.Shape;
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

                  int pointCount = int.Parse(parts[3]);

                  for (int index = 4; index < 4 + pointCount * 2; index += 2)
                  {
                     x = double.Parse(parts[index]);
                     y = graphSize.Height - double.Parse(parts[index + 1]);
                     linkShape.EdgePoints.Add(new EdgePoint(x, y, VGPointType.Normal));
                  }
                 
                  // since we're not changing the nodes this edge connects, this really doesn't do much.
                  // what it DOES do, however, is call ConnectEdgeToNodes, which is an internal method we'd otherwise
                  // be unable to access
                  linkShape.Connect(linkShape.FromShape, linkShape.ToShape);
                  linkShape.ManuallyRouted = false;
                  
                  break;
            }
         }
      }

      private static void DoMSAGLLayout(List<NodeShape> nodeShapes, List<BinaryLinkShape> linkShapes, ModelRoot modelRoot)
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

      #region MSAGL support

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

      #endregion

   }
}