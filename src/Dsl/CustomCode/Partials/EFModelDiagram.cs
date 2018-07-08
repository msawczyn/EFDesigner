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

         if (e.Effect == System.Windows.Forms.DragDropEffects.None && IsAcceptableDropItem(e)) 
            e.Effect = System.Windows.Forms.DragDropEffects.Copy;
      }

      private bool IsAcceptableDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         // attempting drag/drop from explorer - multiple files: doesn't work
         //string[] explorerFiles = diagramDragEventArgs.Data.GetData(DataFormats.FileDrop) as string[];
         //string[] solutionFiles = diagramDragEventArgs.Data.GetData("Text") as string[];
         //string[] filenames = explorerFiles ?? solutionFiles;
         //return filenames != null && filenames.All(File.Exists);

         // attempting drag/drop from explorer - single file: explorer doesn't work
         //string explorerFile = diagramDragEventArgs.Data.GetData("FileNameW") as string;
         //string solutionFile = diagramDragEventArgs.Data.GetData("Text") as string;
         //string filename = explorerFile ?? solutionFile;
         //return filename != null && File.Exists(filename);

         string filename = diagramDragEventArgs.Data.GetData("Text") as string;
         return filename != null && File.Exists(filename);
      }

      public override void OnDragDrop(DiagramDragEventArgs e)
      {
         if (IsAcceptableDropItem(e))
            ProcessDragDropItem(e); 
         else
            base.OnDragDrop(e);
      }

      private void ProcessDragDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         string filename = diagramDragEventArgs.Data.GetData("Text") as string;

         FileDropHelper.HandleDrop(Store, filename);
      }
   }
}
