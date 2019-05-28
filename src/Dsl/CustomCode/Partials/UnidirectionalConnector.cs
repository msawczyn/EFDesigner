using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class UnidirectionalConnector
   {
      public override bool HasToolTip => true;

      public override string GetToolTipText(DiagramItem item)
      {
         UnidirectionalAssociation association = item.Shape.ModelElement as UnidirectionalAssociation;
         return association != null
                   ? $"{association.Source.Name}.{association.TargetPropertyName}"
                   : string.Empty;
      }
   }
}
