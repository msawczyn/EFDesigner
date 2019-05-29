using System;
using System.ComponentModel;

namespace Microsoft.Msagl.Core.Routing
{
   /// <summary>
   ///    defines egde routing behaviour
   /// </summary>
#if !SILVERLIGHT
   [DisplayName("Edge routing settings")]
   [Description("Sets the edge routing method")]
   [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
   public class EdgeRoutingSettings
   {
      /// <summary>
      ///    defines the way edges are routed
      /// </summary>
      public EdgeRoutingMode EdgeRoutingMode
      {
         get;
         set;
      } = EdgeRoutingMode.SugiyamaSplines;

      /// <summary>
      ///    the angle in degrees of the cones in the routing fith the spanner
      /// </summary>
      public double ConeAngle
      {
         get;
         set;
      } = 30 * Math.PI / 180;

      /// <summary>
      ///    Amount of space to leave around nodes
      /// </summary>
      public double Padding
      {
         get;
         set;
      } = 3;

      /// <summary>
      ///    Additional amount of padding to leave around nodes when routing with polylines
      /// </summary>
      public double PolylinePadding
      {
         get;
         set;
      } = 1.5;

      /// <summary>
      ///    For rectilinear, the degree to round the corners
      /// </summary>
      public double CornerRadius { get; set; }

      /// <summary>
      ///    For rectilinear, the penalty for a bend, as a percentage of the Manhattan distance between the source and target
      ///    ports.
      /// </summary>
      public double BendPenalty { get; set; }

      /// <summary>
      ///    the settings for general edge bundling
      /// </summary>
      public BundlingSettings BundlingSettings { get; set; }

      /// <summary>
      ///    For rectilinear, whether to use obstacle bounding boxes in the visibility graph.
      /// </summary>
      public bool UseObstacleRectangles { get; set; }

      /// <summary>
      ///    this is a cone angle to find a relatively close point on the parent boundary
      /// </summary>
      public double RoutingToParentConeAngle
      {
         get;
         set;
      } = Math.PI / 6;

      /// <summary>
      ///    if the number of the nodes participating in the routing of the parent edges is less than the threshold
      ///    then the parent edges are routed avoiding the nodes
      /// </summary>
      public int SimpleSelfLoopsForParentEdgesThreshold
      {
         get;
         set;
      } = 200;

      /// <summary>
      ///    defines the size of the changed graph that could be routed fast with the standard spline routing when dragging
      /// </summary>
      public int IncrementalRoutingThreshold
      {
         get;
         set;
      } = 5000000;

      /// <summary>
      ///    if set to true the original spline is kept under the corresponding EdgeGeometry
      /// </summary>
      public bool KeepOriginalSpline { get; set; }

      /// <summary>
      ///    if set to true routes multi edges as ordered bundles, when routing in a spline mode
      /// </summary>
      /// <exception cref="NotImplementedException"></exception>
      public bool RouteMultiEdgesAsBundles
      {
         get;
         set;
      } = true;

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "Edge Routing Settings";
      }
   }
}