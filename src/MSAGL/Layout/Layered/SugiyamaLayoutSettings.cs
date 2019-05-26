using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Msagl.Core;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
#if USE_PHYLOTREE
using Microsoft.Msagl.Prototype.Phylo;

#endif

namespace Microsoft.Msagl.Layout.Layered
{
   public enum SnapToGridByY
   {
      None,
      Top,
      Bottom
   }

   /// <summary>
   ///    controls many properties of the layout algorithm
   /// </summary>
   public sealed class SugiyamaLayoutSettings : LayoutAlgorithmSettings
   {
      private int brandesThreshold = 600;

      private int groupSplit = 2;

      private double labelCornersPreserveCoefficient = 0.1;

      private int maxNumberOfPassesInOrdering = 24;

      private double minimalHeight;

      private double minimalWidth;

      private double minNodeHeight = 2;
      private double minNodeWidth = 2;

      private int noGainStepsBound = 5;

      private int randomSeedForOrdering = 1;

      private int repetitionCoefficentForOrdering = 2;

      /// <summary>
      ///    Constructor
      /// </summary>
      public SugiyamaLayoutSettings()
      {
         EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.SugiyamaSplines;
      }

      /// <summary>
      ///    Separation between to neighboring layers
      /// </summary>
      public double LayerSeparation { get; set; } = 1;

      /// <summary>
      ///    minLayer, maxLayer, same layer, up-down, up-down vertical and left-right constraints are supported by this class
      /// </summary>
      internal VerticalConstraintsForSugiyama VerticalConstraints { get; } = new VerticalConstraintsForSugiyama();

      internal HorizontalConstraintsForSugiyama HorizontalConstraints { get; } = new HorizontalConstraintsForSugiyama();

      /// <summary>
      ///    if set to true the algorithm only calculates the layers and exits
      /// </summary>
      public bool LayeringOnly { get; set; }

      /// <summary>
      ///    This coefficient if set to the value greater than 1 will force the algorithm to search for layouts with fewer edge
      ///    crossings
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Repetition coefficient for ordering")]
      [Description("This coefficient if set to the value greater than 1 will force the algorithm to search for layouts with fewer edge crossings")]
      [DefaultValue(2)]
#endif
      public int RepetitionCoefficientForOrdering
      {
         get { return repetitionCoefficentForOrdering; }
         set { repetitionCoefficentForOrdering = Math.Max(value, 1); }
      }

      /// <summary>
      ///    The seed for the random element inside of the ordering
      /// </summary>

#if PROPERTY_GRID_SUPPORT
      [DisplayName("Random seed for ordering")]
      [Description("The seed for the random element inside of the ordering")]
      [DefaultValue(2)]
#endif
      public int RandomSeedForOrdering
      {
         get { return randomSeedForOrdering; }
         set { randomSeedForOrdering = value; }
      }

      /// <summary>
      ///    Maximal number of sequential steps without gain in the adjacent swap process
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("No gain steps bound")]
      [Description("Maximal number of sequential steps without gain in the adjacent swap process")]
      [DefaultValue(5)]
#endif
      public int NoGainAdjacentSwapStepsBound
      {
         get { return noGainStepsBound; }
         set { noGainStepsBound = value; }
      }

      /// <summary>
      ///    Maximal number of passes over layers applying the median algorithm in ordering
      /// </summary>

#if PROPERTY_GRID_SUPPORT
      [DisplayName("Maximal number of passes in ordering")]
      [Description("Maximal number of passes over layers applying the median algorithm in ordering")]
      [DefaultValue(24)]
#endif
      public int MaxNumberOfPassesInOrdering
      {
         get { return maxNumberOfPassesInOrdering; }
         set { maxNumberOfPassesInOrdering = value; }
      }

      /// <summary>
      ///    The ratio of the group splitting algorithm used in the spatial hierarchy constructions for edge routing
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Group split ratio")]
      [Description("The ratio of the group splitting algorithm used in the spatial hierarchy constructions")]
      [DefaultValue(2)]
#endif
      public int GroupSplit
      {
         get { return groupSplit; }
         set { groupSplit = value; }
      }

      /// <summary>
      ///    We create a hexagon for a label boundary: this coeffiecient defines the ratio of the top and the bottom side to the
      ///    width of the label
      /// </summary>

#if PROPERTY_GRID_SUPPORT
      [DisplayName("Edge label crossing coefficient")]
      [Description("The coefficient from 0 to 1 controlling the edge crossing with its label.\n 1 - allowing only a shallow crossing which can cause a sharp edge bend\n" +
                   "0 - allowing a deeper crossing resulting in a smoother edge")]
      [DefaultValue(0.1)]
#endif
      public double LabelCornersPreserveCoefficient
      {
         get { return labelCornersPreserveCoefficient; }
         set { labelCornersPreserveCoefficient = value; }
      }

      /// <summary>
      ///    When the number of vertices in the proper layered graph is the threshold or more we switch to
      ///    the faster, but not so accurate, method for x-coordinates calculations.
      /// </summary>

#if PROPERTY_GRID_SUPPORT
      [DisplayName("Fast x-coordinate assignment algorithm threshold")]
      [Description("If the number of vertices in the proper layered graph is more than the value we switch to the fast x-coordinate assignment algorithm of Brandes and Kopf")]
      [DefaultValue(600)]
#endif
      [SuppressMessage("Microsoft.Naming",
         "CA1704:IdentifiersShouldBeSpelledCorrectly",
         MessageId = "Brandes")]
      public int BrandesThreshold
      {
         get { return brandesThreshold; }
         set { brandesThreshold = value; }
      }

#if PROPERTY_GRID_SUPPORT
      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "Sugiyama Layout Settings";
      }
#endif

      /// <summary>
      ///    The resulting layout should be not more narrow than this value.
      /// </summary>
      public double MinimalWidth
      {
         get { return minimalWidth; }
         set { minimalWidth = Math.Max(value, 0); }
      }

      /// <summary>
      ///    The resulting layout should at least as high as this this value
      /// </summary>
      public double MinimalHeight
      {
         get { return minimalHeight; }
         set { minimalHeight = Math.Max(value, 0); }
      }

      //double minNodeHeight = 72*0.5/4;
      //double minNodeWidth = 72*0.75/4;

      /// <summary>
      ///    The minimal node height
      /// </summary>
      public double MinNodeHeight
      {
         get { return minNodeHeight; }
         set { minNodeHeight = Math.Max(0.4, value); }
      }

      /// <summary>
      ///    The minimal node width
      /// </summary>
      public double MinNodeWidth
      {
         get { return minNodeWidth; }
         set { minNodeWidth = Math.Max(0.8, value); }
      }

      /// <summary>
      ///    This transformation is to be applied to the standard top - bottom layout.
      ///    However, node boundaries remain unchanged.
      /// </summary>
      public PlaneTransformation Transformation { get; set; } = PlaneTransformation.UnitTransformation;

      /// <summary>
      ///    The ratio width/height of the final layout.
      ///    The value zero means that the aspect ratio has not been set.
      /// </summary>
      public double AspectRatio { get; set; }

      /// <summary>
      ///    InitialLayoutByCluster requires the layered layout aspect ratio
      ///    to be in the range [1/MaxAspectRatioEccentricity, MaxAspectRatioEccentricity]
      ///    before switching to FallbackLayoutSettings
      /// </summary>
      public double MaxAspectRatioEccentricity { get; set; } = 5;

      /// <summary>
      ///    If the aspect ratio of Layered layout is too slow, use the following layout settings instead
      /// </summary>
      public LayoutAlgorithmSettings FallbackLayoutSettings { get; set; }

      public SnapToGridByY SnapToGridByY { get; set; } = SnapToGridByY.None;

      public double GridSizeByY { get; set; } = 0;

      public double GridSizeByX { get; set; } = 0;

      internal double ActualLayerSeparation(bool layersAreDoubled)
      {
         return layersAreDoubled
                   ? LayerSeparation / 2.0
                   : LayerSeparation;
      }

      /// <summary>
      ///    adds a constraint to keep one node to the left of another on the same layer
      /// </summary>
      /// <param name="leftNode"></param>
      /// <param name="rightNode"></param>
      public void AddLeftRightConstraint(Node leftNode, Node rightNode)
      {
         HorizontalConstraints.LeftRightConstraints.Insert(new Tuple<Node, Node>(leftNode, rightNode));
      }

      /// <summary>
      ///    these nodes belong to the same layer and are adjacent positioned from left to right
      /// </summary>
      /// <param name="neighbors"></param>
      public void AddSameLayerNeighbors(params Node[] neighbors)
      {
         AddSameLayerNeighbors(new List<Node>(neighbors));
      }

      /// <summary>
      ///    these nodes belong to the same layer and are adjacent positioned from left to right
      /// </summary>
      /// <param name="neighbors"></param>
      public void AddSameLayerNeighbors(IEnumerable<Node> neighbors)
      {
         List<Node> neibs = new List<Node>(neighbors);
         HorizontalConstraints.AddSameLayerNeighbors(neibs);

         for (int i = 0; i < neibs.Count - 1; i++)
            VerticalConstraints.SameLayerConstraints.Insert(new Tuple<Node, Node>(neibs[i], neibs[i + 1]));
      }

      /// <summary>
      ///    adds a pair of adjacent neighbors
      /// </summary>
      /// <param name="leftNode"></param>
      /// <param name="rightNode"></param>
      public void AddSameLayerNeighbors(Node leftNode, Node rightNode)
      {
         HorizontalConstraints.AddSameLayerNeighborsPair(leftNode, rightNode);
         VerticalConstraints.SameLayerConstraints.Insert(new Tuple<Node, Node>(leftNode, rightNode));
      }

      /// <summary>
      ///    adds an up-down constraint to the couple of nodes
      /// </summary>
      /// <param name="upperNode"></param>
      /// <param name="lowerNode"></param>
      public void AddUpDownConstraint(Node upperNode, Node lowerNode)
      {
         VerticalConstraints.UpDownConstraints.Insert(new Tuple<Node, Node>(upperNode, lowerNode));
      }

      /// <summary>
      ///    adds a constraint where the top node center is positioned exactly above the lower node center
      /// </summary>
      /// <param name="upperNode"></param>
      /// <param name="lowerNode"></param>
      public void AddUpDownVerticalConstraint(Node upperNode, Node lowerNode)
      {
         VerticalConstraints.UpDownConstraints.Insert(new Tuple<Node, Node>(upperNode, lowerNode));
         HorizontalConstraints.UpDownVerticalConstraints.Insert(new Tuple<Node, Node>(upperNode, lowerNode));
      }

      /// <summary>
      ///    adds vertical up down constraints udDownIds[0]->upDownIds[1]-> ... -> upDownsIds[upDownIds.Length-1]
      /// </summary>
      /// <param name="upDownNodes"></param>
      public void AddUpDownVerticalConstraints(params Node[] upDownNodes)
      {
         ValidateArg.IsNotNull(upDownNodes, "upDownNodes");

         for (int i = 1; i < upDownNodes.Length; i++)
            AddUpDownVerticalConstraint(upDownNodes[i - 1], upDownNodes[i]);
      }

      /// <summary>
      ///    The only function one needs to call to calculate the layout.
      /// </summary>
      /// <param name="msaglGraph">The layout info will be inserted in to the corresponding fields of the graph</param>
      /// <param name="settings">The settings for the algorithm.</param>
      /// <param name="cancelToken"></param>
      [SuppressMessage("Microsoft.Reliability",
         "CA2000:Dispose objects before losing scope")]
      [SuppressMessage("Microsoft.Globalization",
         "CA1305:SpecifyIFormatProvider",
         MessageId = "System.String.Format(System.String,System.Object)")]
      internal static LayeredLayoutEngine CalculateLayout(GeometryGraph msaglGraph, SugiyamaLayoutSettings settings, CancelToken cancelToken)
      {
         LayeredLayoutEngine engine = new LayeredLayoutEngine(msaglGraph, settings);
#if USE_PHYLOTREE
         PhyloTree phyloTree = msaglGraph as PhyloTree;

         if (phyloTree != null)
         {
            PhyloTreeLayoutCalclulation pc = new PhyloTreeLayoutCalclulation(phyloTree, settings, engine.IntGraph, engine.Database);
            pc.Run();
         }
         else
#endif
            engine.Run(cancelToken);

         return engine;
      }

      /// <summary>
      ///    Clones the object
      /// </summary>
      /// <returns></returns>
      public override LayoutAlgorithmSettings Clone()
      {
         return MemberwiseClone() as LayoutAlgorithmSettings;
      }

      /// <summary>
      ///    Pins the nodes of the list to the max layer and
      /// </summary>
      public void PinNodesToMaxLayer(params Node[] nodes)
      {
         ValidateArg.IsNotNull(nodes, "nodes");

         for (int i = 0; i < nodes.Length; i++)
            VerticalConstraints.PinNodeToMaxLayer(nodes[i]);
      }

      /// <summary>
      ///    Pins the nodes of the list to the min layer and
      /// </summary>
      [SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "ToMin")]
      public void PinNodesToMinLayer(params Node[] nodes)
      {
         ValidateArg.IsNotNull(nodes, "nodes");

         for (int i = 0; i < nodes.Length; i++)
            VerticalConstraints.PinNodeToMinLayer(nodes[i]);
      }

      /// <summary>
      ///    adds a same layer constraint
      /// </summary>
      public void PinNodesToSameLayer(params Node[] nodes)
      {
         ValidateArg.IsNotNull(nodes, "nodes");

         for (int i = 1; i < nodes.Length; i++)
            VerticalConstraints.SameLayerConstraints.Insert(new Tuple<Node, Node>(nodes[0], nodes[i]));
      }

      /// <summary>
      /// </summary>
      public void RemoveAllConstraints()
      {
         HorizontalConstraints.Clear();
         VerticalConstraints.Clear();
      }

      /// <summary>
      ///    removes a left-right constraint from
      /// </summary>
      /// <param name="leftNode"></param>
      /// <param name="rightNode"></param>
      public void RemoveLeftRightConstraint(Node leftNode, Node rightNode)
      {
         HorizontalConstraints.LeftRightConstraints.Remove(new Tuple<Node, Node>(leftNode, rightNode));
      }

   }
}