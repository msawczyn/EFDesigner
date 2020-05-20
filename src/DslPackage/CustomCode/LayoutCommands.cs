using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

using QuickGraph;
using QuickGraph.Graphviz;
using QuickGraph.Graphviz.Dot;

namespace Sawczyn.EFDesigner.EFModel
{
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
      public DotNode Source { get; set; }
      public DotNode Target { get; set; }
   }

   internal class Commands
   {
      internal static void LayoutDiagram(EFModelDiagram diagram)
      {
         using (WaitCursor _ = new WaitCursor())
         {
            IEnumerable<ShapeElement> shapeElements = diagram.NestedChildShapes.Where(s => s.IsVisible);

            LayoutDiagram(diagram, shapeElements);
         }
      }

      internal static void LayoutDiagram(EFModelDiagram diagram, IEnumerable<ShapeElement> shapeElements)
      {
         using (Transaction tx = diagram.Store.TransactionManager.BeginTransaction("ModelAutoLayout"))
         {
            List<ShapeElement> shapeList = shapeElements.ToList();

            List<DotNode> vertices = shapeList
                                    .OfType<NodeShape>()
                                    .Select(node => new DotNode {Shape = node})
                                    .ToList();

            List<DotEdge> edges = shapeList
                                 .OfType<BinaryLinkShape>()
                                 .Select(link => new DotEdge
                                                 {
                                                    Shape = link
                                                  , Source = vertices.Single(vertex => vertex.Shape.Id == link.FromShape.Id)
                                                  , Target = vertices.Single(vertex => vertex.Shape.Id == link.ToShape.Id)
                                                 })
                                 .ToList();

            // use graphviz as the default if available
            if (File.Exists(EFModelPackage.Options.DotExePath))
               DoGraphvizLayout(vertices, edges, diagram);
            else
               DoStandardLayout(edges.Select(edge => edge.Shape).ToList(), diagram);

            tx.Commit();
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

      // ReSharper disable once UnusedParameter.Local
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
   }
}