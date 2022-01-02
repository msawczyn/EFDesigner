using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   // ReSharper disable once UnusedMember.Global
   public static class ModelElementExtensions
   {
      public static DiagramView GetActiveDiagramView(this ModelElement element)
      {
         ShapeElement shapeElement = element.GetShapeElement();

         return shapeElement?.GetActiveDiagramView();
      }

      public static DiagramView GetActiveDiagramView(this ShapeElement shape)
      {
         return shape.Diagram?.ActiveDiagramView;
      }

      private static ModelElement GetCompartmentElementFirstParentElement(this ModelElement modelElement)
      {
         // Get the domain class associated with model element.
         DomainClassInfo domainClass = modelElement.GetDomainClass();

         if (domainClass != null)
         {
            // A element is only considered to be in a compartment if it participates in only 1 embedding relationship
            // This might be wrong for some models

            if (domainClass.AllEmbeddedByDomainRoles.Count == 1)
            {
               DomainRoleInfo roleInfo = domainClass.AllEmbeddedByDomainRoles[0];

               // Get a collection of all the links to this model element
               // Since this is in a compartment there should be at least one
               // There can be only one.
               if (roleInfo != null)
               {
                  ElementLink link = roleInfo.GetElementLinks(modelElement).FirstOrDefault();

                  // Get the model element participating in the link that isn't the current one
                  // That will be the parent
                  // Probably there is a better way to achieve the same result
                  return link?.LinkedElements?.FirstOrDefault(linkedElement => !modelElement.Equals(linkedElement));
               }
            }
         }

         return null;
      }

      public static ShapeElement GetShapeElement(this ModelElement element)
      {
         // If the model element is in a compartment the result will be null
         ShapeElement shape = PresentationViewsSubject.GetPresentation(element)
                                                      .OfType<ShapeElement>()
                                                      .FirstOrDefault(s => s.Diagram == ModelRoot.GetCurrentDiagram());

         if (shape == null)
         {
            // If the element is in a compartment, try to get the parent model element to select that
            ModelElement parentElement = element.GetCompartmentElementFirstParentElement();

            if (parentElement != null)
            {
               shape = PresentationViewsSubject.GetPresentation(parentElement)
                                               .OfType<ShapeElement>()
                                               .FirstOrDefault(s => s.Diagram == ModelRoot.GetCurrentDiagram());
            }
         }

         return shape;
      }

      // the following is based on code at https://stackoverflow.com/questions/44876242/center-a-dsl-shape-on-diagram-screen

      public static bool LocateInActiveDiagram(this ModelElement element, bool ensureVisible)
      {
         DiagramView diagramView = element?.GetActiveDiagramView();
         return diagramView != null && diagramView.SelectModelElement(element, ensureVisible);
      }

      public static bool SelectModelElement(this DiagramView diagramView, ModelElement modelElement, bool ensureVisible)
      {
         // Get the shape element in diagramView that corresponds to the model element 

         ShapeElement shapeElement = PresentationViewsSubject.GetPresentation(modelElement)
                                                             .OfType<ShapeElement>()
                                                             .FirstOrDefault(s => s.Diagram == diagramView.Diagram);

         if (shapeElement != null)
         {
            // Make sure the shape element is visible (because connectors can be hidden)
            if (!shapeElement.IsVisible)
               shapeElement.Show();

            // Create a diagram item for this shape element and select it
            diagramView.Selection.Set(new DiagramItem(shapeElement));

            if (ensureVisible)
            {
               diagramView.Selection.EnsureVisible(DiagramClientView.EnsureVisiblePreferences.ScrollIntoViewCenter);
               diagramView.ZoomAtViewCenter(1);
            }

            shapeElement.Invalidate();
            return true;
         }

         // If the model element does not have a shape, try to cast it IModelElementCompartment

         if (modelElement is IModelElementInCompartment compartmentedModelElement)
         {
            // Get the parent
            IModelElementWithCompartments parentModelElement = compartmentedModelElement.ParentModelElement;

            if (parentModelElement != null)
            {
               // Get the compartment that stores the model element
               CompartmentShape parentShapeElement = PresentationViewsSubject.GetPresentation((ModelElement)parentModelElement)
                                                                             .OfType<CompartmentShape>()
                                                                             .FirstOrDefault(s => s.Diagram == diagramView.Diagram);

               ElementListCompartment compartment = parentShapeElement?.GetCompartment(compartmentedModelElement.CompartmentName);

               if (compartment != null)
               {
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

                     if (ensureVisible)
                     {
                        diagramView.Selection.EnsureVisible(DiagramClientView.EnsureVisiblePreferences.ScrollIntoViewCenter);
                        diagramView.ZoomAtViewCenter(1);
                     }

                     compartment.Invalidate();
                     return true;
                  }
               }
            }
         }

         return false;
      }
   }
}
