using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class BidirectionalConnector
   {
      public override bool HasToolTip => true;

      public override string GetToolTipText(DiagramItem item)
      {
         BidirectionalAssociation association = item.Shape.ModelElement as BidirectionalAssociation;
         return association != null
                   ? $"{association.Source.Name}.{association.TargetPropertyName} <--> {association.Target.Name}.{association.SourcePropertyName}"
                   : string.Empty;
      }
   }
}
