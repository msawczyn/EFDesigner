using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.Generic;
using System.Drawing;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class CommentBoxShape: IHasStore, IThemeable
   {
      /// <summary>
      /// Shape instance initialization.
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
            FillColor = diagramColors.Background;
            TextColor = FillColor.LegibleTextColor();

            Invalidate();

            tx.Commit();
         }
      }

      //Called once for each shape instance. 
      protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
      {
         base.InitializeDecorators(shapeFields, decorators);

         //Look up the shape field, which is called "Comment." 
         TextField commentField = (TextField)FindShapeField(shapeFields, "Comment");
         // Allow multiple lines of text. 
         commentField.DefaultMultipleLine = true;
         // Autosize not supported for multi-line fields. 
         commentField.DefaultAutoSize = false;
         // Anchor the field slightly inside the container shape. 
         commentField.AnchoringBehavior.Clear();
         commentField.AnchoringBehavior.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0.01);
         commentField.AnchoringBehavior.SetRightAnchor(AnchoringBehavior.Edge.Right, 0.01);
         commentField.AnchoringBehavior.SetTopAnchor(AnchoringBehavior.Edge.Top, 0.01);
         commentField.AnchoringBehavior.SetBottomAnchor(AnchoringBehavior.Edge.Bottom, 0.01);
      }
   }
}
