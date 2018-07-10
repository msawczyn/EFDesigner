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

         // For files to be dropped, they have to be selected in the solution explorer
         // just get anything selected there that's a .cs file and remember it
         selectedFilePaths = FileDropHelper.SelectedFilePaths.ToList();
         if (!selectedFilePaths.Any() || !selectedFilePaths.All(File.Exists))
            selectedFilePaths = null;

         if (diagramDragEventArgs.Effect == System.Windows.Forms.DragDropEffects.None && selectedFilePaths != null)
            diagramDragEventArgs.Effect = System.Windows.Forms.DragDropEffects.Copy;
      }

      private List<string> selectedFilePaths = null;

      public override void OnDragDrop(DiagramDragEventArgs diagramDragEventArgs)
      {
         if (selectedFilePaths?.Any() == true)
            FileDropHelper.HandleMultiDrop(Store, selectedFilePaths);
         else
            base.OnDragDrop(diagramDragEventArgs);

         selectedFilePaths = null;
      }
   }
}
