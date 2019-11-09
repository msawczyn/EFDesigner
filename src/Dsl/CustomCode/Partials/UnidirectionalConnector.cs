using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class UnidirectionalConnector
   {
      public override bool HasToolTip => true;

      public override string GetToolTipText(DiagramItem item)
      {
         return item.Shape.ModelElement is UnidirectionalAssociation association
                   ? $"{association.Source.Name}.{association.TargetPropertyName}"
                   : string.Empty;
      }
   }
}
