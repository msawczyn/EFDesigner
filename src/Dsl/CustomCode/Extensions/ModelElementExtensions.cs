using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;


namespace Sawczyn.EFDesigner.EFModel.Extensions
{
   public static class ModelElementExtensions
   {
      internal static bool IsVisible(this ModelElement modelElement, Diagram diagram = null)
      {
         return modelElement.GetShapeElement(diagram)?.IsVisible(diagram) == true;
      }

      internal static bool IsVisible(this ShapeElement shapeElement, Diagram diagram = null)
      {
         Diagram targetDiagram = diagram ?? EFModel.ModelRoot.GetCurrentDiagram();
         return targetDiagram != null && shapeElement.Diagram == targetDiagram && shapeElement.IsVisible;
      }

      private static ShapeElement GetShapeElement(this ModelElement element, Diagram diagram = null)
      {
         ShapeElement result = null;

         // Get the shape on the diagram. If not specified, pick the current one
         if (diagram == null)
            diagram = EFModel.ModelRoot.GetCurrentDiagram();

         if (diagram != null)
         {
            result = diagram.Store.GetAll<ShapeElement>().FirstOrDefault(x => x.ModelElement == element && x.Diagram == diagram);

            // If the model element is in a compartment the result should be null? Check for Compartment type just in case
            if (result is Compartment)
            {
               ModelElement parentElement = element.GetCompartmentElementFirstParentElement();
               result = parentElement?.GetShapeElement(diagram);
            }
         }

         return result;
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

      public static bool IsLoading(this ModelElement element)
      {
         TransactionManager transactionManager = element.Store.TransactionManager;

         Transaction currentTransaction = transactionManager.CurrentTransaction;

         return transactionManager.InTransaction
             && (currentTransaction.IsSerializing
              || currentTransaction.Name.ToLowerInvariant() == "paste"
              || currentTransaction.Name.ToLowerInvariant() == "local merge transaction");
      }

      public static ModelRoot ModelRoot(this Store store)
      {
         return store.GetAll<ModelRoot>().FirstOrDefault();
      }

      public static List<(string propertyName, object thisValue, object otherValue)> GetDifferences<T>(this T This, T Other) where T:ModelElement
      {
         List<(string propertyName, object thisValue, object otherValue)> result = new List<(string propertyName, object thisValue, object otherValue)>();
         ReadOnlyCollection<DomainPropertyInfo> domainProperties = This.GetDomainClass().AllDomainProperties;

         foreach (DomainPropertyInfo domainProperty in domainProperties)
         {
            object thisProperty = domainProperty.GetValue(This);
            object otherProperty = domainProperty.GetValue(Other);

            if (thisProperty == null && otherProperty == null)
               continue;

            if (thisProperty != null && otherProperty != null && thisProperty.Equals(otherProperty))
               continue;

            result.Add((domainProperty.Name, thisProperty, otherProperty));
         }

         return result;
      }

      public static IEnumerable<T> GetAll<T>(this Store store)
      {
         return store?.ElementDirectory?.AllElements?.OfType<T>() ?? new T[0];
      }

      public static ShapeElement GetFirstShapeElement(this ModelElement element)
      {
         return PresentationViewsSubject.GetPresentation(element)
                                        .OfType<ShapeElement>()
                                        .FirstOrDefault(s => s.Diagram == EFModel.ModelRoot.GetCurrentDiagram());
      }

      /// <summary>
      /// Gets the named compartment in this element
      /// </summary>
      /// <param name="element"></param>
      /// <param name="compartmentName"></param>
      /// <returns></returns>
      public static ElementListCompartment GetCompartment(this ModelElement element, string compartmentName)
      {
         return element?.GetFirstShapeElement()
                       ?.NestedChildShapes
                       ?.OfType<ElementListCompartment>()
                        .FirstOrDefault(s => s.Name == compartmentName);
      }

      /// <summary>
      /// Causes all diagrams to redraw
      /// </summary>
      /// <param name="element"></param>
      public static void InvalidateDiagrams(this ModelElement element)
      {
         if (element != null)
         {
            List<EFModelDiagram> diagrams = element.Store
                                                   .DefaultPartitionForClass(EFModelDiagram.DomainClassId)
                                                   .ElementDirectory
                                                   .AllElements
                                                   .OfType<EFModelDiagram>()
                                                   .ToList();

            foreach (EFModelDiagram diagram in diagrams)
               diagram.Invalidate();
         }
      }

      public static void Redraw(this ModelElement element)
      {
         // redraw on every diagram
         foreach (ShapeElement shapeElement in PresentationViewsSubject.GetPresentation(element).OfType<ShapeElement>().Distinct().ToList())
            shapeElement.Invalidate();
      }

      public static Diagram GetActiveDiagram(this ModelElement element)
      {
         ShapeElement shapeElement = element.GetShapeElement();

         return shapeElement?.Diagram;
      }
   }
}
