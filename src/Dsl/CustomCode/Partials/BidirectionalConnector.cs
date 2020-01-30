using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class BidirectionalConnector
   {
      /// <summary>
      /// Get/Set whether or not the Shape shows a mouse hover tooltip by default
      /// </summary>
      public override bool HasToolTip => true;

      /// <summary>
      /// Gets the tooltip text for the PEL element under the cursor
      /// </summary>
      /// <param name="item">this contains the shape,field, and subfield under the cursor</param>
      /// <returns></returns>
      public override string GetToolTipText(DiagramItem item)
      {
         return item.Shape.ModelElement is BidirectionalAssociation association
                   ? $"{association.Source.Name}.{association.TargetPropertyName} <--> {association.Target.Name}.{association.SourcePropertyName}"
                   : string.Empty;
      }
   }
}
