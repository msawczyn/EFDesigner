using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Extensions
{
   public static class ModelElementExtensions
   {
      public static bool LocateInDiagram(this ModelElement element)
      {
         DiagramView diagramView = element.GetShapeElement()?.Diagram?.ActiveDiagramView;
         return diagramView != null && diagramView.SelectModelElement(element);
      }

      private static ShapeElement GetShapeElement(this ModelElement element)
      {
         // Get the first shape
         // If the model element is in a compartment the result will be null

         return element.GetFirstShapeElement() ?? element.GetCompartmentElementFirstParentElement()?.GetFirstShapeElement();
      }

      public static ShapeElement GetFirstShapeElement(this ModelElement element)
      {
         return PresentationViewsSubject.GetPresentation(element).OfType<ShapeElement>().FirstOrDefault();
      }

      /// <summary>
      /// Gets the named compartment in this element
      /// </summary>
      /// <param name="element"></param>
      /// <param name="compartmentName"></param>
      /// <returns></returns>
      public static ElementListCompartment GetCompartment(this ModelElement element, string compartmentName)
      {
         return element.GetFirstShapeElement()
                       .NestedChildShapes
                       .OfType<ElementListCompartment>()
                       .FirstOrDefault(s => s.Name == compartmentName);
      }

      /// <summary>
      /// Causes all diagrams to redraw
      /// </summary>
      /// <param name="element"></param>
      public static void InvalidateDiagrams(this ModelElement element)
      {
         List<EFModelDiagram> diagrams = element.Store
                                                .DefaultPartitionForClass(EFModelDiagram.DomainClassId)
                                                .ElementDirectory
                                                .AllElements
                                                .OfType<EFModelDiagram>()
                                                .ToList();

         foreach (EFModelDiagram diagram in diagrams)
         {
            diagram.Invalidate();
         }

      }

      private static ModelElement GetCompartmentElementFirstParentElement(this ModelElement modelElement)
      {
         // Get the domain class associated with model element.

         DomainClassInfo domainClass = modelElement.GetDomainClass();
         if (domainClass?.AllEmbeddedByDomainRoles?.Count == 1)
         {
            DomainRoleInfo roleInfo = domainClass.AllEmbeddedByDomainRoles[0];

            // Get a collection of all the links to this model element
            // Since this is in a compartment there should be at least one
            // There can be only one.

            ElementLink elementLink = roleInfo.GetElementLinks(modelElement)?.FirstOrDefault();
            return elementLink?.LinkedElements?.FirstOrDefault(e => !modelElement.Equals(e));
         }

         return null;
      }
   }
}
