using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelDiagram: IHasStore
   {
      public override void OnInitialize()
      {
         base.OnInitialize();
         ModelRoot modelRoot = Store.ModelRoot();

         // because we can hide elements, line routing looks odd when it thinks it's jumping over lines
         // that really aren't visible. Since replacing the routing algorithm is too hard (impossible?)
         // let's just stop it from showing jumps at all. A change to the highlighting on mouseover
         // makes it easier to see which lines are which in complex diagrams, so this doesn't hurt anything.
         RouteJumpType = VGPageLineJumpCode.NoJumps;

         ShowGrid = modelRoot?.ShowGrid ?? true;
         GridColor = modelRoot?.GridColor ?? Color.Gainsboro;
         SnapToGrid = modelRoot?.SnapToGrid ?? true;
         GridSize = modelRoot?.GridSize ?? 0.125;

      }

      /// <summary>
      /// Called during view fixup to ask the parent whether a shape should be created for the given child element.
      /// </summary>
      /// <remarks>
      /// Always return true, since we assume there is only one diagram per model file for DSL scenarios.
      /// </remarks>
      protected override bool ShouldAddShapeForElement(ModelElement element)
      {
         return base.ShouldAddShapeForElement(element) || NestedChildShapes.Any(s => s.ModelElement == element);
      }

      public static bool IsDropping { get; private set; }

      public override void OnDragOver(DiagramDragEventArgs diagramDragEventArgs)
      {
         base.OnDragOver(diagramDragEventArgs);

         if (diagramDragEventArgs.Handled)
            return;

         if (diagramDragEventArgs.Data.GetData(typeof(ModelElement)) is ModelElement)
            diagramDragEventArgs.Effect = DragDropEffects.Move;
         else if (IsAcceptableDropItem(diagramDragEventArgs))
            diagramDragEventArgs.Effect = DragDropEffects.Copy;
         else
            diagramDragEventArgs.Effect = DragDropEffects.None;
      }

      private bool IsAcceptableDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         IsDropping = (diagramDragEventArgs.Data.GetData("Text") is string filename && File.Exists(filename)) || 
                      (diagramDragEventArgs.Data.GetData("FileDrop") is string[] filenames && filenames.All(File.Exists));

         return IsDropping;
      }

      public bool DropTarget { get; private set; }
      public override void OnDragDrop(DiagramDragEventArgs diagramDragEventArgs)
      {
         try
         {
            DropTarget = true;
            base.OnDragDrop(diagramDragEventArgs);
         }
         catch (ArgumentException)
         {
            // ignore. byproduct of multiple diagrams
         }
         finally
         {
            DropTarget = false;
         }

         // came from model explorer?
         if (diagramDragEventArgs.Effect == DragDropEffects.Move && diagramDragEventArgs.Data.GetData(typeof(ModelElement)) is ModelElement element)
         {
            FixUpAllDiagrams.FixUp(this, Store.ModelRoot(), element);

            return;
         }

         if (IsDropping)
         {
            string[] missingFiles = null;

            if (diagramDragEventArgs.Data.GetData("Text") is string filename)
            {
               if (!File.Exists(filename)) 
                  missingFiles = new[] {filename};
               else
                  FileDropHelper.HandleDrop(Store, filename);
            }
            else if (diagramDragEventArgs.Data.GetData("FileDrop") is string[] filenames)
            {
               string[] existingFiles = filenames.Where(File.Exists).ToArray();
               FileDropHelper.HandleMultiDrop(Store, existingFiles);
               missingFiles = filenames.Except(existingFiles).ToArray();
            }
            else
            {
               try
               {
                  base.OnDragDrop(diagramDragEventArgs);
               }
               catch (ArgumentException)
               {
                  // ignore. byproduct of multiple diagrams
               }
            }

            if (missingFiles != null && missingFiles.Any())
            {
               if (missingFiles.Length > 1)
                  missingFiles[missingFiles.Length - 1] = "and " + missingFiles[missingFiles.Length - 1];
               ErrorDisplay.Show($"Can't find files {string.Join(", ", missingFiles)}");
            }
         }

         IsDropping = false;
      }

      /// <summary>Called by the control's OnMouseUp().</summary>
      /// <param name="e">A DiagramMouseEventArgs that contains event data.</param>
      public override void OnMouseUp(DiagramMouseEventArgs e)
      {
         IsDropping = false;
         base.OnMouseUp(e);
      }

      /// <summary>
      /// Called when a key is pressed when the Diagram itself has the focus.
      /// </summary>
      /// <param name="e">A DiagramKeyEventArgs that contains event data.</param>
      public override void OnKeyDown(DiagramKeyEventArgs e)
      {
         //using (Transaction t = Store.TransactionManager.BeginTransaction("Diagram.OnKeyDown"))
         //{
            
         //   if (e.KeyCode == Keys.Delete)
         //   {
         //      SelectedShapesCollection selection = FocusedDiagramView.Selection;

         //      string message = selection.Count == 1
         //                          ? "Delete multiple elements from model? Are you sure?"
         //                          : "Delete element from model? Are you sure?";

         //      if (e.Control && BooleanQuestionDisplay.Show(message) == true)
         //      {
         //         foreach (ModelElement modelElement in selection.RepresentedElements)
         //            modelElement.Delete();
         //         t.Commit();
         //         e.Handled = true;
         //      }
         //      else if (!e.Control)
         //      {
         //         foreach (DiagramItem diagramItem in selection)
         //         {
         //            if (diagramItem.Shape is NodeShape nodeShape)
         //               nodeShape.Delete();
         //         }
                  
         //         t.Commit();
         //         e.Handled = true;
         //      }
         //   }
         //}

         base.OnKeyDown(e);
      }
   }
}
