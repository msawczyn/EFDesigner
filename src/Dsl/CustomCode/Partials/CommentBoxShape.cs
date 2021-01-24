using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Collections.Generic;
using System.Drawing;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class CommentBoxShape: IHasStore
   {
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
