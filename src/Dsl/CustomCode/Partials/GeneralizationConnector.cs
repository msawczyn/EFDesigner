using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class GeneralizationConnector: IHasStore
   {
      public override bool HasToolTip => true;

      public override string GetToolTipText(DiagramItem item)
      {
         return item.Shape.ModelElement is Generalization generalization
                   ? $"{generalization.Subclass.Name} inherits from {generalization.Superclass.Name}"
                   : string.Empty;
      }

      protected override int ModifyLuminosity(int currentLuminosity, DiagramClientView view)
      {
         if (!view.HighlightedShapes.Contains(new DiagramItem(this)))
            return currentLuminosity;

         int baseCalculation = base.ModifyLuminosity(currentLuminosity, view);

         // black (luminosity == 0) will be changed to luminosity 40, which doesn't show up.
         // so if it's black we're highlighting, return 130, since that looks ok.
         return baseCalculation == 40 ? 130 : baseCalculation;
      }
   }
}