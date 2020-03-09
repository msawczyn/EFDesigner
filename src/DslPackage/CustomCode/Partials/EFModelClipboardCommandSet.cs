using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class EFModelClipboardCommandSet
   {
      protected override void ProcessOnMenuCopyCommand()
      {
         List<ShapeElement> selectedShapes = SelectedElements.OfType<ShapeElement>().ToList();

         // has to be at least one node shape; can't just copy connectors
         if (!selectedShapes.OfType<NodeShape>().Any())
            return;

         Diagram diagram = selectedShapes.FirstOrDefault()?.Diagram;
         List<ModelElement> modelElements = selectedShapes.Where(e => e.ModelElement != null).Select(e => e.ModelElement).ToList();

         if (diagram == null || !modelElements.Any())
            return;

         ElementGroup elementGroup = new ElementGroup(diagram.Partition);

         foreach (ModelElement element in modelElements)
            elementGroup.AddGraph(element, true);

         IDataObject data = new DataObject();
         data.SetData(elementGroup.CreatePrototype());

         Clipboard.SetDataObject(data, false, 10, 50);
      }

      protected override void ProcessOnMenuCutCommand()
      {
         ProcessOnMenuCopyCommand();
         DeleteSelectedItems("Cut");
      }

      /// <summary>Implementation of the Paste command</summary>
      protected override void ProcessOnMenuPasteCommand()
      {
         if (CurrentModelingDocView is EFModelDocView efModelDocView)
         {
            Diagram diagram = efModelDocView.CurrentDiagram;

            if (diagram == null)
               return;

            IDataObject data = Clipboard.GetDataObject();
            DesignSurfaceElementOperations op = diagram.ElementOperations;

            if (op.CanMerge(diagram, data))
            {
               using (Transaction t = diagram.Store.TransactionManager.BeginTransaction("paste"))
               {
                  // Find a suitable place to position the new shape.
                  PointD place = CurrentSelection.OfType<ShapeElement>().FirstOrDefault()?.AbsoluteBoundingBox.Center ?? new PointD(0, 0);

                  op.Merge(diagram, data, PointD.ToPointF(place));
                  t.Commit();
               }
            }
         }
      }
   }
}