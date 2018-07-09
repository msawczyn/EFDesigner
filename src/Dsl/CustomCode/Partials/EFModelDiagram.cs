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

      public override void OnDragOver(DiagramDragEventArgs e)
      {
         base.OnDragOver(e);

         if (e.Effect == System.Windows.Forms.DragDropEffects.None && IsAcceptableDropItem(e))
            e.Effect = System.Windows.Forms.DragDropEffects.Copy;
      }

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
         return selectedFilePaths.Any() && selectedFilePaths.All(File.Exists);
      }

      public override void OnDragDrop(DiagramDragEventArgs e)
      {
         if (IsAcceptableDropItem(e))
         {
            int elementCount = Store.ElementDirectory.AllElements.OfType<ModelType>().Count();
            FileDropHelper.HandleMultiDrop(Store, FileDropHelper.SelectedFilePaths);
            
            // if we've increased the element count by at least 50%, autolayout
            if (Store.ElementDirectory.AllElements.OfType<ModelType>().Count() > 1.5 * elementCount)
            {
               using (Transaction tx = Store.TransactionManager.BeginTransaction("ModelAutoLayout"))
               {
                  AutoLayoutShapeElements(NestedChildShapes, VGRoutingStyle.VGRouteStraight, PlacementValueStyle.VGPlaceSN, true);
                  tx.Commit();
               }
            }
         }
         else
            base.OnDragDrop(e);
      }

      //private void ProcessDragDropItem(DiagramDragEventArgs diagramDragEventArgs)
      //{
      //   string filename = diagramDragEventArgs.Data.GetData("Text") as string;

      //   FileDropHelper.HandleDrop(Store, filename);
      //}
   }
}
