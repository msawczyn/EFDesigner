#region Using directives

using System;
using System.ComponentModel;

#endregion

namespace Microsoft.Msagl.Core.Routing
{
   ///<summary>
   ///</summary>
#if !SILVERLIGHT
   [Description("Specifies the edge bundling settings")]
   [TypeConverter(typeof(ExpandableObjectConverter))]
   [DisplayName("Edge bundling settings")]
#endif
   [Browsable(false)]
   public sealed class BundlingSettings
   {
      /// <summary>
      ///    the default value of CapacityOverflowCoefficient
      /// </summary>
      public static double DefaultCapacityOverflowCoefficientMultiplier = 1000;

      /// <summary>
      ///    the default path lenght importance coefficient
      /// </summary>
      public static double DefaultPathLengthImportance = 500;

      /// <summary>
      ///    the default ink importance
      /// </summary>
      public static double DefaultInkImportance = 0.01;

      /// <summary>
      ///    default edge separation
      /// </summary>
      public static double DefaultEdgeSeparation = 0.5;

      /// <summary>
      ///    the upper bound of the virtual node radius
      /// </summary>
      internal double MaxHubRadius = 50.0;

      /// <summary>
      ///    the lower bound of the virtual node radius
      /// </summary>
      internal double MinHubRadius = 0.1;

      /// <summary>
      ///    this number is muliplied by the overflow penalty cost and by the sum of the LengthImportanceCoefficient
      ///    and InkImportanceCoefficient, and added to the routing price
      /// </summary>
      public double CapacityOverflowCoefficient
      {
         get;
         set;
      } = DefaultCapacityOverflowCoefficientMultiplier;

      /// <summary>
      /// </summary>
      public bool CreateUnderlyingPolyline { get; set; }

      /// <summary>
      ///    the importance of path lengths coefficient
      /// </summary>
      public double PathLengthImportance
      {
         get;
         set;
      } = DefaultPathLengthImportance;

      ///<summary>
      ///</summary>
      public double InkImportance
      {
         get;
         set;
      } = DefaultInkImportance;

      /// <summary>
      ///    Separation between to neighboring edges within a bundle
      /// </summary>
      public double EdgeSeparation
      {
         get;
         set;
      } = DefaultEdgeSeparation;

      /// <summary>
      ///    if is set to true will be using Cubic Bezie Segments inside of hubs, otherwise will be using Biarcs
      /// </summary>
      public bool UseCubicBezierSegmentsInsideOfHubs
      {
         get;
         set;
      }

      /// <summary>
      ///    if is set to true will be using greedy ordering algorithm, otherwise will be using linear
      /// </summary>
      public bool UseGreedyMetrolineOrdering
      {
         get;
         set;
      } = true;

      /// <summary>
      ///    min angle for gluing edges
      /// </summary>
      public double AngleThreshold
      {
         get;
         set;
      } = Math.PI / 180 * 45;

      /// <summary>
      ///    the importance of hub repulsion coefficient
      /// </summary>
      public double HubRepulsionImportance
      {
         get;
         set;
      } = 100;

      /// <summary>
      ///    the importance of bundle repulsion coefficient
      /// </summary>
      public double BundleRepulsionImportance
      {
         get;
         set;
      } = 100;

      /// <summary>
      ///    minimal ration of cdt edges with satisfied capacity needed to perform bundling
      ///    (otherwise bundling will not be executed)
      /// </summary>
      public double MinimalRatioOfGoodCdtEdges
      {
         get;
         set;
      } = 0.9;

      /// <summary>
      ///    speed vs quality of the drawing
      /// </summary>
      public bool HighestQuality
      {
         get;
         set;
      } = true;

      /// <summary>
      ///    if is set to true the original spline before the trimming should be kept under the corresponding EdgeGeometry
      /// </summary>
      public bool KeepOriginalSpline { get; set; }

      /// <summary>
      ///    if set to true then the edges will be routed one on top of each other with no gap inside of a bundle
      /// </summary>
      public bool KeepOverlaps { get; set; }

      /// <summary>
      ///    calculates the routes that just follow the visibility graph
      /// </summary>
      public bool StopAfterShortestPaths { get; set; }

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "Bundling Settings";
      }
   }
}