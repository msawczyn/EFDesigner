using System.IO;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelDiagram
   {
      public override void OnInitialize()
      {
         base.OnInitialize();

         // because we can hide elements, line routing looks odd when it thinks it's jumping over lines
         // that really aren't visible. Since replacing the routing algorithm is too hard (impossible?)
         // let's just stop it from showing jumps at all. A change to the highlighting on mouseover
         // makes it easier to see which lines are which in complex diagrams, so this doesn't hurt anything.
         RouteJumpType = VGPageLineJumpCode.NoJumps;
      }

      public override void OnDragOver(DiagramDragEventArgs e)
      {
         base.OnDragOver(e);

         if (e.Effect == System.Windows.Forms.DragDropEffects.None && IsAcceptableDropItem(e)) // To be defined
            e.Effect = System.Windows.Forms.DragDropEffects.Copy;
      }

      private bool IsAcceptableDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         return diagramDragEventArgs.Data.GetData("Text", false) is string filename && File.Exists(filename);
      }

      public override void OnDragDrop(DiagramDragEventArgs e)
      {
         if (IsAcceptableDropItem(e))
            ProcessDragDropItem(e); // To be defined
         else
            base.OnDragDrop(e);
      }

      private void ProcessDragDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         string filename = diagramDragEventArgs.Data.GetData("Text", false) as string;
         FileDropHelper.HandleDrop(Store, filename);
      }
   }
}
