using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
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

      public override void OnDragOver(DiagramDragEventArgs diagramDragEventArgs)
      {
         base.OnDragOver(diagramDragEventArgs);
         draggedFile = null;

         if (diagramDragEventArgs.Effect == System.Windows.Forms.DragDropEffects.None && IsAcceptableDropItem(diagramDragEventArgs))
            diagramDragEventArgs.Effect = System.Windows.Forms.DragDropEffects.Copy;
      }

      private string draggedFile = null;

      // ReSharper disable once UnusedParameter.Local
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

         // for single file - insufficient for the use case
         //return diagramDragEventArgs.Data.GetData("Text") is string filename && File.Exists(filename);

         // ok, let's try this. For files to be dropped, they have to be selected in the solution explorer
         // just get anything selected there that's a project item with a file path and process it

         // make sure we're dragging a file
         if (!diagramDragEventArgs.Data.GetDataPresent("Text"))
            return false;

         List<string> selectedFilePaths = FileDropHelper.SelectedFilePaths.ToList();
         if (selectedFilePaths.Any())
            return selectedFilePaths.All(File.Exists);

         // oddly enough, if you select just one file and drag it to the model, the model's selected
         // we know at least one file was selected or we wouldn't be here, so just use that one filename
         if (diagramDragEventArgs.Data.GetData("Text") is string filename && File.Exists(filename))
         {
            draggedFile = filename;
            return true;
         }

         return false;
      }

      public override void OnDragDrop(DiagramDragEventArgs diagramDragEventArgs)
      {
         if (draggedFile != null)
         {
            List<string> selectedFilePaths = FileDropHelper.SelectedFilePaths
                                                           .Where(p => p.EndsWith(".cs"))
                                                           .ToList();

            if (selectedFilePaths.Any())
               FileDropHelper.HandleMultiDrop(Store, selectedFilePaths);
            else
               FileDropHelper.HandleDrop(Store, draggedFile);
         }
         else
            base.OnDragDrop(diagramDragEventArgs);
      }

      //private void ProcessDragDropItem(DiagramDragEventArgs diagramDragEventArgs)
      //{
      //   string filename = diagramDragEventArgs.Data.GetData("Text") as string;

      //   FileDropHelper.HandleDrop(Store, filename);
      //}
   }
}
