using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

using Sawczyn.EFDesigner.EFModel.Extensions;

using Cursor = System.Windows.Forms.Cursor;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelClipboardCommandSet
   {
      private bool PasteShapes(List<ModelElement> modelElements)
      {
         if (TargetElement is ModelRoot
          && CurrentModelingDocView is EFModelDocView efModelDocView
          && efModelDocView.Diagram is EFModelDiagram currentDiagram
          && currentDiagram.Store != null
          && modelElements.Any())
         {
            Store store = efModelDocView.Diagram.Store;

            List<ModelClass> modelClasses = modelElements.OfType<ModelClass>().ToList();
            List<ModelEnum> enums = modelElements.OfType<ModelEnum>().ToList();
            List<Comment> comments = modelElements.OfType<Comment>().ToList();
            List<ModelElement> everythingElse = modelElements.Except(modelClasses).Except(comments).ToList();
            List<ShapeElement> newShapes = new List<ShapeElement>();

            using (Transaction t = store.TransactionManager.BeginTransaction())
            {
               // paste classes, enums and comments first to ensure that any possible connector end is present before the connectors arrive
               newShapes.AddRange(modelClasses.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
               newShapes.AddRange(enums.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
               newShapes.AddRange(comments.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
               newShapes = newShapes.Where(s => s != null).ToList();

               // select and show the new or existing shape. Search, in order, classes, enums, comments, then everything else
               ModelElement firstElement = modelClasses.FirstOrDefault() ?? enums.FirstOrDefault() ?? comments.FirstOrDefault() ?? everythingElse.FirstOrDefault();

               if (firstElement != null)
                  currentDiagram.ActiveDiagramView.SelectModelElement(firstElement, true);

               // if nothing got pasted (because it's already there), we succeeded in our paste but didn't really change
               // the display, so nothing further needs done
               if (!newShapes.Any())
                  return false;

               t.Commit();
            }

            currentDiagram.Invalidate();

            return true;
         }

         return false;
      }

      /// <summary>Implementation of the Paste command</summary>
      protected override void ProcessOnMenuPasteCommand()
      {
         if (CurrentModelingDocView is EFModelDocView efModelDocView && efModelDocView.Diagram is EFModelDiagram currentDiagram)
         {
            Store store = currentDiagram.Store;
            IEnumerable<ModelElement> modelElements = store.GetAll<ModelElement>();
            base.ProcessOnMenuPasteCommand();
            List<ModelElement> newElements = store.GetAll<ModelElement>().Except(modelElements).ToList();
            PasteShapes(newElements);
         }
      }
   }
}