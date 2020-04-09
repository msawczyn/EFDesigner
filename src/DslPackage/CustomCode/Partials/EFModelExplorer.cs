using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{

   internal partial class EFModelExplorer
   {
      partial void Init()
      {
         ObjectModelBrowser.NodeMouseDoubleClick += ObjectModelBrowser_OnNodeMouseDoubleClick;
         ObjectModelBrowser.DragEnter += ObjectModelBrowser_OnDragEnter;
         ObjectModelBrowser.DragOver += ObjectModelBrowser_OnDragOver;
         ObjectModelBrowser.ItemDrag += ObjectModelBrowser_OnItemDrag;
      }

      private void ObjectModelBrowser_OnDragOver(object sender, DragEventArgs e)
      {
         if (e.Data.GetDataPresent(typeof(ModelElement)))
            e.Effect = e.AllowedEffect;
      }

      private void ObjectModelBrowser_OnItemDrag(object sender, ItemDragEventArgs e)
      {
         if (e.Item is ExplorerTreeNode elementNode)
            DoDragDrop(elementNode.RepresentedElement, DragDropEffects.Copy);
      }

      private void ObjectModelBrowser_OnDragEnter(object sender, DragEventArgs e)
      {
         e.Effect = e.AllowedEffect;
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

      private void ObjectModelBrowser_OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         switch (e.Node)
         {
            case RoleGroupTreeNode roleNode:
               roleNode.Expand();

               break;

            case ExplorerTreeNode elementNode:
            {
               ModelElement element = elementNode.RepresentedElement;
               Diagram diagram = element.GetActiveDiagramView()?.Diagram;
               ModelRoot parent = element.Store.ModelRoot();
               FixUpAllDiagrams.FixUp(diagram, parent, element);

               break;
            }
         }
      }

      /// <summary>Virtual method to process the menu Delete operation</summary>
      protected override void ProcessOnMenuDeleteCommand()
      {
         TreeNode diagramRoot = ObjectModelBrowser.SelectedNode?.Parent;

         switch (SelectedElement)
         {
            case ModelDiagramData diagramData:
            {
               if (BooleanQuestionDisplay.Show($"About to permanently delete diagram named {diagramData.Name} - are you sure?") == true)
               {
                  base.ProcessOnMenuDeleteCommand();
                  ObjectModelBrowser.SelectedNode = null;
               }

               break;
            }

            case ModelEnum modelEnum:
            {
               string fullName = modelEnum.FullName.Split('.').Last();

               if (!ModelEnum.IsUsed(modelEnum)
                || BooleanQuestionDisplay.Show($"{fullName} is used as an entity property. Deleting the enumeration will remove those properties. Are you sure?") == true)

               {
                  base.ProcessOnMenuDeleteCommand();
                  ObjectModelBrowser.SelectedNode = null;
               }

               break;
            }

            default:
               base.ProcessOnMenuDeleteCommand();
               ObjectModelBrowser.SelectedNode = null;

               break;
         }

         diagramRoot?.Expand();
      }
   }

}