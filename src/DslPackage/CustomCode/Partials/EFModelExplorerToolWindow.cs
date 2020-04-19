using System;
using System.Linq;

using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell.Interop;

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
                     if (!modelClass.LocateInActiveDiagram(true))
                        break;

                     // then fix up the compartments since they might need it
                     ModelElement[] classElements = {modelClass};
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AttributesCompartment", false);
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AssociationsCompartment", false);
                     CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "SourcesCompartment", false);

                     // any associations to visible classes on this diagram need to be visible as well
                     foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties())
                     {
                        ModelClass other = navigationProperty.AssociationObject.Source == modelClass
                                                 ? navigationProperty.AssociationObject.Target
                                                 : navigationProperty.AssociationObject.Source;

                        // should never happen
                        if (other == null)
                           continue;

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

                     // so do generalizations, as long as both classes are available
                     foreach (Generalization generalization in modelClass.Store.ElementDirectory.AllElements
                                                                         .OfType<Generalization>()
                                                                         .Where(g => g.Superclass == modelClass 
                                                                                  || g.Subclass == modelClass))
                     {
                        ModelClass other = generalization.Superclass == modelClass
                                              ? generalization.Subclass
                                              : generalization.Superclass;

                        // should never happen
                        if (other == null)
                           continue;

                        ShapeElement shapeElement = PresentationViewsSubject.GetPresentation(other)
                                                                            .OfType<ShapeElement>()
                                                                            .FirstOrDefault(s => s.Diagram == diagram);
                        
                        if (shapeElement != null && shapeElement.IsVisible)
                        {
                           ShapeElement connectorElement = PresentationViewsSubject.GetPresentation(generalization)
                                                                                   .OfType<GeneralizationConnector>()
                                                                                   .FirstOrDefault(s => s.Diagram == diagram);
                           connectorElement?.Show();
                        }
                     }

                     FixUpAllDiagrams.FixUp(diagram, modelClass.ModelRoot, modelClass);

                     break;

                  case ModelEnum modelEnum:
                     // user selected an enum. Find it in the current diagram, center it and make it visible
                     if (!modelEnum.LocateInActiveDiagram(true))
                        break;

                     // then fix up the compartment since it might need it
                     ModelElement[] enumElements = {modelEnum};
                     CompartmentItemAddRule.UpdateCompartments(enumElements, typeof(EnumShape), "ValuesCompartment", false);
                     FixUpAllDiagrams.FixUp(diagram, modelEnum.ModelRoot, modelEnum);

                     break;
               }

               // make sure the explorer window is still visible
               Show();

               t.Commit();
            }
         }
      }

   }
}
