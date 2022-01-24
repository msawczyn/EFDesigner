using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class CommentConnector: IHasStore, IThemeable
   {
      /// <summary>
      /// This method is called when a shape is inititially created, derived classes can
      /// override to perform shape instance initialization.  This method is always called within a transaction.
      /// </summary>
      public override void OnInitialize()
      {
         base.OnInitialize();
         if (ModelDisplay.GetDiagramColors != null)
            SetThemeColors(ModelDisplay.GetDiagramColors());
      }

      public void SetThemeColors(DiagramThemeColors diagramColors)
      {
         using (Transaction tx = Store.TransactionManager.BeginTransaction("Set diagram colors"))
         {
            Color = diagramColors.Background.LegibleTextColor();
            TextColor = diagramColors.Text;

            Invalidate();

            tx.Commit();
         }
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
