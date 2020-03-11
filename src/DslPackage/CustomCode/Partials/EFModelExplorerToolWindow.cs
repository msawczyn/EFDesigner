using System;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      protected override void OnSelectionChanged(EventArgs e)
      {
         base.OnSelectionChanged(e);

         // select element in tree
         if (PrimarySelection != null && PrimarySelection is ModelElement element)
         {
            using (Transaction t = element.Store.TransactionManager.BeginTransaction("TreeSelectionChanged"))
            {
               Diagram diagram = element.GetActiveDiagramView()?.Diagram;

               switch (PrimarySelection)
               {
                  case ModelDiagramData modelDiagramData:
                     // user selected a diagram. Open it.
                     EFModelDocData docData = (EFModelDocData)TreeContainer.ModelingDocData;
                     docData.OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(modelDiagramData.Name, typeof(EFModelDiagram), docData.RootElement));

                     break;

                  case ModelClass modelClass:
                     // user selected a class. If it's in the current diagram, find it, center it and make it visible
                     ShapeElement primaryShapeElement = PresentationViewsSubject.GetPresentation(modelClass)
                                                                                .OfType<ShapeElement>()
                                                                                .FirstOrDefault(s => s.Diagram == diagram);

                     //if (primaryShapeElement == null || !primaryShapeElement.IsVisible)
                     //   break;

                     modelClass.LocateInDiagram(true);

                     // then fix up the compartments since they might need it
                     ModelElement[] classElements = {modelClass};
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AttributesCompartment", false);
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AssociationsCompartment", false);
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "SourcesCompartment", false);

                     // any associations to visible classes on this diagram need to be visible as well
                     foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties())
                     {
                        ModelClass other = navigationProperty.AssociationObject.Dependent == modelClass
                                                 ? navigationProperty.AssociationObject.Principal
                                                 : navigationProperty.AssociationObject.Dependent;
                        
                        ShapeElement shapeElement = PresentationViewsSubject.GetPresentation(other)
                                                                            .OfType<ShapeElement>()
                                                                            .FirstOrDefault(s => s.Diagram == diagram);
                        
                        if (shapeElement != null && shapeElement.IsVisible)
                        {
                           ShapeElement connectorElement = PresentationViewsSubject.GetPresentation(navigationProperty.AssociationObject)
                                                                                   .OfType<AssociationConnector>()
                                                                                   .FirstOrDefault(s => s.Diagram == diagram);
                           connectorElement?.Show();
                        }
                     }

                     FixUpAllDiagrams.FixUp(diagram, modelClass.ModelRoot, modelClass);

                     break;

                  case ModelEnum modelEnum:
                     // user selected an enum. Find it in the current diagram, center it and make it visible
                     modelEnum.LocateInDiagram(true);

                     // then fix up the compartment since it might need it
                     ModelElement[] enumElements = {modelEnum};
                     CompartmentItemAddRule.UpdateCompartments(enumElements, typeof(EnumShape), "ValuesCompartment", false);
                     FixUpAllDiagrams.FixUp(diagram, modelEnum.ModelRoot, modelEnum);

                     break;
               }

               t.Commit();
            }
         }
      }
   }
}
