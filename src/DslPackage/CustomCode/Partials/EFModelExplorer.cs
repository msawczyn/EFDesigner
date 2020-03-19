using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{

   internal partial class EFModelExplorer
   {
      partial void Init()
      {
         //ObjectModelBrowser.NodeMouseDoubleClick += ObjectModelBrowser_OnNodeMouseDoubleClick;
      }

      /// <summary>
      ///    Method to insert the incoming node into the TreeNodeCollection. This allows the derived class to change the sorting behavior.
      ///    N.B. This should really be protected, and is only intended as an override point. Do not call it directly, but rather call
      ///    InsertNode()
      /// </summary>
      /// <param name="siblingNodes"></param>
      /// <param name="node"></param>
      public override void InsertTreeNode(TreeNodeCollection siblingNodes, ExplorerTreeNode node)
      {
         // sorting Diagrams first. Normally would be alpha ordering

         if (node.Text == "Diagrams" && node is RoleGroupTreeNode)
            siblingNodes.Insert(0, node);
         else
            base.InsertTreeNode(siblingNodes, node);
      }

      //private void ObjectModelBrowser_OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
      //{
      //   // select element in tree
      //   if (PrimarySelection != null && PrimarySelection is ModelElement element)
      //   {
      //      using (Transaction t = element.Store.TransactionManager.BeginTransaction("TreeSelectionChanged"))
      //      {
      //         Diagram diagram = element.GetActiveDiagramView()?.Diagram;

      //         switch (PrimarySelection)
      //         {
      //            case ModelDiagramData modelDiagramData:
      //               // user selected a diagram. Open it.
      //               EFModelDocData docData = (EFModelDocData)TreeContainer.ModelingDocData;
      //               docData.OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(modelDiagramData.Name, typeof(EFModelDiagram), docData.RootElement));

      //               break;

      //            case ModelClass modelClass:
      //               // user selected a class. If it's in the current diagram, find it, center it and make it visible
      //               ShapeElement primaryShapeElement = PresentationViewsSubject.GetPresentation(modelClass)
      //                                                                          .OfType<ShapeElement>()
      //                                                                          .FirstOrDefault(s => s.Diagram == diagram);

      //               if (primaryShapeElement == null || !primaryShapeElement.IsVisible)
      //                  break;

      //               modelClass.LocateInDiagram(true);

      //               // then fix up the compartments since they might need it
      //               ModelElement[] classElements = {modelClass};
      //               CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AttributesCompartment", false);
      //               CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "AssociationsCompartment", false);
      //               CompartmentItemAddRule.UpdateCompartments(classElements, typeof(ClassShape), "SourcesCompartment", false);

      //               // any associations to visible classes on this diagram need to be visible as well
      //               foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties())
      //               {
      //                  ModelClass other = navigationProperty.AssociationObject.Dependent == modelClass
      //                                           ? navigationProperty.AssociationObject.Principal
      //                                           : navigationProperty.AssociationObject.Dependent;

      //                  ShapeElement shapeElement = PresentationViewsSubject.GetPresentation(other)
      //                                                                      .OfType<ShapeElement>()
      //                                                                      .FirstOrDefault(s => s.Diagram == diagram);

      //                  if (shapeElement != null && shapeElement.IsVisible)
      //                  {
      //                     ShapeElement connectorElement = PresentationViewsSubject.GetPresentation(navigationProperty.AssociationObject)
      //                                                                             .OfType<AssociationConnector>()
      //                                                                             .FirstOrDefault(s => s.Diagram == diagram);
      //                     connectorElement?.Show();
      //                  }
      //               }

      //               FixUpAllDiagrams.FixUp(diagram, modelClass.ModelRoot, modelClass);

      //               break;

      //            case ModelEnum modelEnum:
      //               // user selected an enum. Find it in the current diagram, center it and make it visible
      //               modelEnum.LocateInDiagram(true);

      //               // then fix up the compartment since it might need it
      //               ModelElement[] enumElements = {modelEnum};
      //               CompartmentItemAddRule.UpdateCompartments(enumElements, typeof(EnumShape), "ValuesCompartment", false);
      //               FixUpAllDiagrams.FixUp(diagram, modelEnum.ModelRoot, modelEnum);

      //               break;
      //         }

      //         t.Commit();
      //      }
      //   }

      //}

      /// <summary>Virtual method to process the menu Delete operation</summary>
      protected override void ProcessOnMenuDeleteCommand()
      {
         if (SelectedElement is ModelDiagramData diagramData)
         {
            if (BooleanQuestionDisplay.Show($"About to permanently delete diagram named {diagramData.Name} - are you sure?") == true)
            {
               base.ProcessOnMenuDeleteCommand();
               ObjectModelBrowser.SelectedNode = null;
            }
         }
         else if (SelectedElement is ModelEnum modelEnum)
         {
            string fullName = modelEnum.FullName.Split('.').Last();

            if (!ModelEnum.IsUsed(modelEnum)
             || BooleanQuestionDisplay.Show($"{fullName} is used as an entity property. Deleting the enumeration will remove those properties. Are you sure?") == true)

            {
               base.ProcessOnMenuDeleteCommand();
               ObjectModelBrowser.SelectedNode = null;
            }
         }
         else
         {
            base.ProcessOnMenuDeleteCommand();
            ObjectModelBrowser.SelectedNode = null;
         }
      }
   }

}