using System.ComponentModel;

using Microsoft.Msagl.Core.Layout;

namespace Microsoft.Msagl.Prototype.Ranking
{
   /// <summary>
   ///    Ranking layout settings
   /// </summary>
#if PROPERTY_GRID_SUPPORT
   [Description("Settings for the layout with ranking by y-axis")]
   [TypeConverter(typeof(ExpandableObjectConverter))]
#endif
   public class RankingLayoutSettings : LayoutAlgorithmSettings
   {
      ///<summary>
      ///</summary>
      public RankingLayoutSettings()
      {
         NodeSeparation = 0;
      }

      /// <summary>
      ///    Number of pivots in Landmark Scaling (between 3 and number of objects).
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Number of pivots")]
      [Description("Number of pivots in MDS")]
      [DefaultValue(50)]
#endif
      public int PivotNumber { set; get; } = 50;

      /// <summary>
      ///    Impact of group structure on layout in the x-axis.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Group effect by x-axis")]
      [DefaultValue(0.15)]
#endif
      public double OmegaX { set; get; } = .15;

      /// <summary>
      ///    Impact of group structure on layout in the y-axis.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Group effect by y-axis")]
      [DefaultValue(0.15)]
#endif
      public double OmegaY { set; get; } = .15;

      /// <summary>
      ///    X Scaling Factor.
      /// </summary>
#if PROPERTY_GRID_SUPPORT
      [DisplayName("Scale by x-axis")]
      [Description("The resulting layout will be scaled by x-axis by this number")]
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
      public double ScaleY { set; get; } = 200;

      /// <summary>
      ///    Clones the object
      /// </summary>
      /// <returns></returns>
      public override LayoutAlgorithmSettings Clone()
      {
         return MemberwiseClone() as LayoutAlgorithmSettings;
      }

#if PROPERTY_GRID_SUPPORT
      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString()
      {
         return "Ranking Layout Settings";
      }
#endif
   }
}