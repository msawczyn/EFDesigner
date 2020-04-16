using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorer
   {
      private readonly List<Type> nodeEventHandlersAdded = new List<Type>();

      private void AddedHandler(object sender, ElementAddedEventArgs e)
      {
         UpdateRoleGroupNode(e.ModelElement, 1);
      }

      public override RoleGroupTreeNode CreateRoleGroupTreeNode(DomainRoleInfo targetRoleInfo)
      {
         if (targetRoleInfo == null)
            throw new ArgumentNullException(nameof(targetRoleInfo));

         Type representedType = targetRoleInfo.LinkPropertyInfo.PropertyType;

         if (!nodeEventHandlersAdded.Contains(representedType))
         {
            DomainClassInfo diagramInfo = ModelingDocData.Store.DomainDataDirectory.FindDomainClass(representedType);
            ModelingDocData.Store.EventManagerDirectory.ElementAdded.Add(diagramInfo, new EventHandler<ElementAddedEventArgs>(AddedHandler));
            ModelingDocData.Store.EventManagerDirectory.ElementDeleted.Add(diagramInfo, new EventHandler<ElementDeletedEventArgs>(DeletedHandler));
            nodeEventHandlersAdded.Add(representedType);
         }

         RoleGroupTreeNode roleGroupTreeNode = new EFModelRoleGroupTreeNode(targetRoleInfo);

         if (ObjectModelBrowser.ImageList != null)
            roleGroupTreeNode.DefaultImageIndex = 1;

         return roleGroupTreeNode;
      }

      private void DeletedHandler(object sender, ElementDeletedEventArgs e)
      {
         UpdateRoleGroupNode(e.ModelElement, -1);
      }

      partial void Init()
      {
         ObjectModelBrowser.NodeMouseDoubleClick += ObjectModelBrowser_OnNodeMouseDoubleClick;
         ObjectModelBrowser.ItemDrag += ObjectModelBrowser_OnItemDrag;
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
         if (node.Text.StartsWith("Diagrams") && node is EFModelRoleGroupTreeNode)
         {
            // sorting Diagrams first. Normally would be alpha ordering
            siblingNodes.Insert(0, node);
         }
         else
            base.InsertTreeNode(siblingNodes, node);

         if (node.Parent is EFModelRoleGroupTreeNode roleNode)
            roleNode.Text = roleNode.GetNodeText();
      }

      private void ObjectModelBrowser_OnItemDrag(object sender, ItemDragEventArgs e)
      {
         if (e.Item is ExplorerTreeNode elementNode)
            DoDragDrop(elementNode.RepresentedElement, DragDropEffects.Copy);
      }

      private void ObjectModelBrowser_OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         if (e.Node is ExplorerTreeNode elementNode && !(e.Node is EFModelRoleGroupTreeNode))
         {
            ModelElement element = elementNode.RepresentedElement;

            if (ModelingDocData is EFModelDocData docData)
            {
               Diagram diagram = docData.CurrentDocView?.CurrentDiagram;

               if (diagram != null && diagram is EFModelDiagram efModelDiagram)
                  efModelDiagram.AddExistingModelElement(element);
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

      private void UpdateRoleGroupNode(ModelElement element, int offset)
      {
         ExplorerTreeNode elementNode = FindNodeForElement(element);

         if (elementNode?.Parent is EFModelRoleGroupTreeNode groupNode)
         {
            groupNode.Text = groupNode.GetNodeText(offset);
            Invalidate();
         }
      }

      public class EFModelRoleGroupTreeNode : RoleGroupTreeNode
      {
         private readonly string displayTextBase;

         /// <summary>Constructor</summary>
         /// <param name="metaRole">Role represented by this node</param>
         public EFModelRoleGroupTreeNode(DomainRoleInfo metaRole) : base(metaRole)
         {
            string propertyDisplayName = metaRole.OppositeDomainRole.PropertyDisplayName;

            displayTextBase = !string.IsNullOrEmpty(propertyDisplayName)
                                 ? propertyDisplayName
                                 : metaRole.OppositeDomainRole.PropertyName;
         }

         internal string GetNodeText()
         {
            return ProvideNodeText();
         }

         internal string GetNodeText(int offset)
         {
            return $"{displayTextBase} ({Nodes.Count + offset})";
         }

         /// <summary>Suppply the text for the node</summary>
         /// <returns>The text for the node</returns>
         protected override string ProvideNodeText()
         {
            return $"{displayTextBase} ({Nodes.Count})";
         }
      }
   }
}