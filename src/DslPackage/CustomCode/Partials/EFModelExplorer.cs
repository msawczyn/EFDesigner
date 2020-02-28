using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{

   internal partial class EFModelExplorer
   {
      /// <summary>
      ///    Method to insert the incoming node into the TreeNodeCollection. This allows the derived class to change the sorting behavior.
      ///    N.B. This should really be protected, and is only intended as an override point. Do not call it directly, but rather call
      ///    InsertNode()
      /// </summary>
      /// <param name="siblingNodes"></param>
      /// <param name="node"></param>
      public override void InsertTreeNode(TreeNodeCollection siblingNodes, ExplorerTreeNode node)
      {
         if (node.Text == "Diagrams" && node is RoleGroupTreeNode)
            siblingNodes.Insert(0, node);
         else
            base.InsertTreeNode(siblingNodes, node);
      }

      /// <summary>Virtual method to process the menu Delete operation</summary>
      protected override void ProcessOnMenuDeleteCommand()
      {
         switch (SelectedElement)
         {
            case ModelDiagramData diagramData when BooleanQuestionDisplay.Show($"About to permanently delete diagram named {diagramData.Name} - are you sure?") == true:
            {
               base.ProcessOnMenuDeleteCommand();

               break;
            }

            case ModelEnum modelEnum when !ModelEnum.IsUsed(modelEnum)
                                       || BooleanQuestionDisplay.Show($"{modelEnum.FullName} is used as an entity property. Deleting the enumeration will remove those properties. Are you sure?") == true:
            {
               base.ProcessOnMenuDeleteCommand();

               break;
            }
         }
      }
   }

}