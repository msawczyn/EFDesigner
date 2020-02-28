using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{

   internal partial class EFModelExplorer
   {
      /// <summary>
      /// Method to insert the incoming node into the TreeNodeCollection. This allows the derived class to change the sorting behavior.
      /// N.B. This should really be protected, and is only intended as an override point. Do not call it directly, but rather call
      /// InsertNode()
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
         if (SelectedElement is ModelDiagramData diagramData 
          && BooleanQuestionDisplay.Show($"About to permanently delete diagram named {diagramData.Name} - are you sure?") == true)
            base.ProcessOnMenuDeleteCommand();
      }
   }

}