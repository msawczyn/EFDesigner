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
      ///// <summary>Not doing anything?</summary>
      //protected override void OnDoubleClick(EventArgs e)
      //{
      //   base.OnDoubleClick(e);

      //   // let's make this a switch so we can more easily extend it to different element types later
      //   switch (SelectedElement)
      //   {
      //      case ModelClass modelClass:
      //         EFModelDocData.OpenFileFor(modelClass);

      //         break;

      //      case ModelAttribute modelAttribute:
      //         EFModelDocData.OpenFileFor(modelAttribute.ModelClass);

      //         break;

      //      case ModelEnum modelEnum:
      //         EFModelDocData.OpenFileFor(modelEnum);

      //         break;

      //      case ModelEnumValue modelEnumValue:
      //         EFModelDocData.OpenFileFor(modelEnumValue.Enum);

      //         break;

      //      //case ModelDiagramData modelDiagram:
      //      //   ((EFModelDocData)ModelingDocData).OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(modelDiagram.Name, typeof(EFModelDiagram), ModelingDocData.RootElement));

      //      //   break;
      //   }
      //}

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
   }
}