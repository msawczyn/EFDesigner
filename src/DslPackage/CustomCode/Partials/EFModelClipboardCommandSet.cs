using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class EFModelClipboardCommandSet
   {
      private bool PasteShapes()
      {
         // Get prototypes from clipboard, if they exist
         IDataObject dataObject = Clipboard.GetDataObject();
         ElementGroupPrototype elementGroupPrototype = ElementOperations.GetElementGroupPrototype(ServiceProvider, dataObject);

         // if we're not pasting on the diagram (i.e., the ModelRoot), we're pasting the prototype
         if (TargetElement is ModelRoot
          && CurrentModelingDocView is EFModelDocView efModelDocView
          && efModelDocView.Diagram is EFModelDiagram currentDiagram
          && currentDiagram?.Store != null
          && elementGroupPrototype != null)
         {
            Store store = efModelDocView.Diagram.Store;

            // get matching elements from the store
            List<ModelElement> modelElements = elementGroupPrototype.ProtoElements
                                                                    .Select(p => store.ElementDirectory.FindElement(p.ElementId))
                                                                    .Where(e => e != null)
                                                                    .ToList();

            if (modelElements.Any())
            {
               List<ModelClass> modelClasses = modelElements.OfType<ModelClass>().ToList();
               List<Comment> comments = modelElements.OfType<Comment>().ToList();
               List<ModelElement> everythingElse = modelElements.Except(modelClasses).Except(comments).ToList();
               List<ShapeElement> newShapes = new List<ShapeElement>();

               using (Transaction t = store.TransactionManager.BeginTransaction())
               {
                  // paste classes and comments first to ensure that any possible connector end is present before the connectors arrive
                  newShapes.AddRange(modelClasses.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
                  newShapes.AddRange(comments.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
                  newShapes.AddRange(everythingElse.Select(e => EFModelDiagram.AddExistingModelElement(currentDiagram, e)));
                  newShapes = newShapes.Where(s => s != null).ToList();

                  // if nothing got pasted (because it's already there), we succeeded in our paste but didn't really change
                  // the display, so nothing further needs done
                  if (!newShapes.Any())
                     return true;

                  t.Commit();
               }

               using (Transaction t = store.TransactionManager.BeginTransaction())
               {
                  Commands.LayoutDiagram(currentDiagram, newShapes);
                  t.Commit();
               }

               currentDiagram.Invalidate();

               return true;
            }
         }

         return false;
      }

      /// <summary>Implementation of the Paste command</summary>
      protected override void ProcessOnMenuPasteCommand()
      {
         if (!PasteShapes())
            base.ProcessOnMenuPasteCommand();
      }
   }
}