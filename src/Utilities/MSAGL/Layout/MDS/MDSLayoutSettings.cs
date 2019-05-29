using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Layout.ProximityOverlapRemoval.MinimumSpanningTree;

namespace Microsoft.Msagl.Layout.MDS
{
   /// <summary>
   ///    MDL layout settings
   /// </summary>
#if PROPERTY_GRID_SUPPORT
   [DisplayName("MDS layout settings")]
   [Description("Setting for Multi Dimensional Scaling algorithm")]
   [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
   public class MdsLayoutSettings : LayoutAlgorithmSettings
   {
      // private double epsilon = Math.Pow(10,-8);
      private int pivotNumber = 50;
      private int iterationsWithMajorization = 30;
      private double scaleY = 200;
      private double rotationAngle;

      /// <summary>
      /// </summary>
#if SHARPKIT // no multithreading in JS
        public bool RunInParallel = false;
#else
      public bool RunInParallel = true;
#endif

      /// remove overlaps between node boundaries
      public bool RemoveOverlaps
      {
         set;
         get;
      } = true;

      /*
      /// <summary>
      /// Level of convergence accuracy (the closer to zero, the more accurate).
      /// </summary>
      [Description("this is the epsilon")]
      public double Epsilon {
          set { epsilon=value; }
          get { return epsilon; }
      }
      */

      /// <summary>
      ///    Number of pivots in Landmark Scaling (between 3 and number of objects).
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Number of pivots")]
      [Description("Number of pivots in MDS")]
      [DefaultValue(50)]
#endif
      public int PivotNumber
      {
         set { pivotNumber = Math.Max(value, 3); }
         get { return pivotNumber; }
      }

      /// <summary>
      ///    Number of iterations in distance scaling
      /// </summary>

      [SuppressMessage("Microsoft.Naming",
         "CA1704:IdentifiersShouldBeSpelledCorrectly",
         MessageId = "Majorization")]
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Number of iterations with majorization")]
      [Description("If this number is positive then the majorization method will be used with the initial solution taken from the landmark method")]
      [DefaultValue(0)]
#endif
      public int IterationsWithMajorization
      {
         set { iterationsWithMajorization = value; }
         get { return iterationsWithMajorization; }
      }

      /// <summary>
      ///    X Scaling Factor.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Scale by x-axis")]
      [Description("The resulting layout will be scaled in the x-axis by this number")]
      [DefaultValue(200.0)]
#endif
      public double ScaleX { set; get; } = 200;

      /// <summary>
      ///    Y Scaling Factor.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Scale by y-axis")]
      [Description("The resulting layout will be scaled by y-axis by this number")]
      [DefaultValue(200.0)]
#endif
      public double ScaleY
      {
         set { scaleY = value; }
         get { return scaleY; }
      }

      /// <summary>
      ///    Weight matrix exponent.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [Description("The power to raise the distances to in the majorization step")]
      [DefaultValue(-2.00)]
#endif
      public double Exponent { set; get; } = -2;

      /// <summary>
      ///    rotation angle
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Rotation angle")]
      [Description("The resulting layout will be rotated by this angle")]
      [DefaultValue(0.0)]
#endif
      public double RotationAngle
      {
         set { rotationAngle = value % 360; }
         get { return rotationAngle; }
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
      ///    Settings for calculation of ideal edge length
      /// </summary>
      public IdealEdgeLengthSettings IdealEdgeLength { get; set; }

      /// <summary>
      ///    Adjust the scale of the graph if there is not enough whitespace between nodes
      /// </summary>
      public bool AdjustScale { get; set; }

      /// <summary>
      ///    The method which should be used to remove the overlaps.
      /// </summary>
      public OverlapRemovalMethod OverlapRemovalMethod
      {
         get;
         set;
      } = OverlapRemovalMethod.MinimalSpanningTree;

      public int GetNumberOfIterationsWithMajorization(int nodeCount)
      {
         return nodeCount > CallIterationsWithMajorizationThreshold
                   ? 0
                   : IterationsWithMajorization;
      }

      public int CallIterationsWithMajorizationThreshold { get; set; } = 3000;

#if PROPERTY_GRID_SUPPORT
      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "MDS Layout Settings";
      }
#endif
   }
}