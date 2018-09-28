using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Extensions {
   public static class DiagramViewExtensions
   {
      public static bool SelectModelElement(this DiagramView diagramView, ModelElement modelElement)
      {
         // Get the shape element that corresponds to the model element

         ShapeElement shapeElement = modelElement.GetFirstShapeElement();
         if (shapeElement != null)
         {
            // Make sure the shape element is visible (because connectors can be hidden)

            if (!shapeElement.IsVisible)
               shapeElement.Show();

            // Create a diagram item for this shape element and select it
            diagramView.Selection.Set(new DiagramItem(shapeElement));
            return true;
         }

         // If the model element does not have a shape, try to cast it IModelElementCompartmented

         if (modelElement is IModelElementInCompartment compartmentedModelElement)
            if (compartmentedModelElement.ParentModelElement is ModelElement parentModelElement)
            {
               // Get the compartment that stores the model element

               ElementListCompartment compartment = parentModelElement.GetCompartment(compartmentedModelElement.CompartmentName);
               if (compartment == null)
                  throw new InvalidOperationException($"Can't find compartment {compartmentedModelElement.CompartmentName}");

               // Expand the compartment?
               if (!compartment.IsExpanded)
               {
                  using (Transaction trans = modelElement.Store.TransactionManager.BeginTransaction("IsExpanded"))
                  {
                     compartment.IsExpanded = true;
                     trans.Commit();
                  }
               }

               // Find the model element in the compartment

               int index = compartment.Items.IndexOf(modelElement);
               if (index >= 0)
               {
                  // Create a diagram item and select it

                  diagramView.Selection.Set(new DiagramItem(compartment, compartment.ListField, new ListItemSubField(index)));
                  return true;
               }
            }

         return false;
      }
   }
}