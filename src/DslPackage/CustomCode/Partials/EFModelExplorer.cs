using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

using Microsoft.Internal.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorer : IVsWindowSearch
   {
      private readonly List<Type> nodeEventHandlersAdded = new List<Type>();

      #region Search

      private ElementHost SearchControlHost;

      //private System.Windows.Forms.Integration.ElementHost SearchControlHost;

      /// <summary>
      /// Perform initializaton.  ToolWindow base class set frame properties here.
      /// </summary>
      protected void InitSearch()
      {
         IVsWindowSearchHostFactory windowSearchHostFactory = ServiceProvider.GetService(typeof(SVsWindowSearchHostFactory)) as IVsWindowSearchHostFactory;
         IVsWindowSearchHost windowSearchHost = windowSearchHostFactory.CreateWindowSearchHost(SearchControlHost);
         windowSearchHost.SetupSearch(this);
         windowSearchHost.Activate();
      }

      /// <summary>Creates a new search task object. The task is cold-started - Start() needs to be called on the task object to begin the search.</summary>
      /// <param name="dwCookie">The search cookie.</param>
      /// <param name="pSearchQuery">The search query.</param>
      /// <param name="pSearchCallback">The search callback.</param>
      /// <returns>The search task.</returns>
      public IVsSearchTask CreateSearch(uint dwCookie, IVsSearchQuery pSearchQuery, IVsSearchCallback pSearchCallback)
      {
         return new ModelExplorerSearchTask(this, dwCookie, pSearchQuery, pSearchCallback);
      }

      /// <summary>Clears the search result, for example, after the user has cleared the content of the search edit box.</summary>
      public void ClearSearch()
      {
         RefreshBrowserView();
      }

      /// <summary>Allows the window search host to obtain overridable search options.</summary>
      /// <param name="pSearchSettings">The search options.</param>
      public void ProvideSearchSettings(IVsUIDataSource pSearchSettings)
      {
         Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.ControlMaxWidth, (uint)10000);
         Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.SearchStartDelay, (uint)250);
         Utilities.SetValue(pSearchSettings, SearchSettingsDataSource.PropertyNames.SearchWatermark, "Search model");
      }

      /// <summary>Allows the window to preview some keydown events that can be used to navigate between the search results or take action on them</summary>
      /// <param name="dwNavigationKey">The navigation <see cref="T:Microsoft.VisualStudio.Shell.Interop.__VSSEARCHNAVIGATIONKEY" />.</param>
      /// <param name="dwModifiers">The key <see cref="T:Microsoft.VisualStudio.Shell.Interop.__VSUIACCELMODIFIERS" />.</param>
      /// <returns>True if the event was handled, otherwise false.</returns>
      public bool OnNavigationKeyDown(uint dwNavigationKey, uint dwModifiers)
      {
         return false;
      }

      /// <summary>Determines whether the search should be enabled for the window. </summary>
      /// <returns>True if search should be enabled, otherwise false.</returns>
      public bool SearchEnabled => true;

      /// <summary>Gets the GUID of the search provider. For a tool window search provider, if the category is not returned the tool window guid will be used by default.</summary>
      /// <returns>The GUID.</returns>
      public Guid Category => Guid.Empty;

      /// <summary>Returns an interface that can be used to enumerate search filters. </summary>
      /// <returns>The search filters.</returns>
      public IVsEnumWindowSearchFilters SearchFiltersEnum => null;

      /// <summary>Allows the window search host to obtain overridable search options.</summary>
      /// <returns>The search options.</returns>
      public IVsEnumWindowSearchOptions SearchOptionsEnum => null;

      #endregion Search

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
         //// shoehorn the search widget into the list
         SuspendLayout();

         Controls.Remove(ObjectModelBrowser);
         Control label = Controls[0];

         Controls.Add(SearchControlHost = new ElementHost
                                          {
                                             Location = new System.Drawing.Point(3, label.Height)
                                           , Name = "SearchControlHost"
                                           , Size = new System.Drawing.Size(Width, 25)
                                           , Dock = DockStyle.Top
                                           , Padding = new Padding(0, 3, 0, 0)
                                           , TabIndex = 1
                                           , Text = ""
                                           , Child = null
                                          });

         SearchControlHost.BringToFront();

         ObjectModelBrowser.TabIndex = 2;
         ObjectModelBrowser.Location = new System.Drawing.Point(3, label.Height);
         Controls.Add(ObjectModelBrowser);
         ObjectModelBrowser.BringToFront();

         ResumeLayout(false);
         PerformLayout();

         ObjectModelBrowser.NodeMouseDoubleClick += ObjectModelBrowser_OnNodeMouseDoubleClick;
         ObjectModelBrowser.ItemDrag += ObjectModelBrowser_OnItemDrag;

         InitSearch();
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
         if (e.Item is ExplorerTreeNode elementNode && elementNode.RepresentedElement != null)
            DoDragDrop(elementNode.RepresentedElement, DragDropEffects.Copy);
      }

      private void ObjectModelBrowser_OnNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
      {
         if (e.Node is ExplorerTreeNode elementNode && elementNode.RepresentedElement != null)
         {
            ModelElement element = elementNode.RepresentedElement;

            if (ModelingDocData is EFModelDocData docData)
            {
               Diagram diagram = docData.CurrentDocView?.CurrentDiagram;

               if (diagram != null && diagram is EFModelDiagram efModelDiagram)
                  EFModelDiagram.AddExistingModelElement(efModelDiagram, element);
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
            groupNode.Text = groupNode.GetNodeText(element);
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

         //internal string GetNodeText(int offset)
         //{
         //   return $"{displayTextBase} ({Nodes.Count + offset})";
         //}

         /// <summary>Suppply the text for the node</summary>
         /// <returns>The text for the node</returns>
         protected override string ProvideNodeText()
         {
            return $"{displayTextBase} ({Nodes.Count})";
         }

         public string GetNodeText(ModelElement member)
         {
            int count = member.Store.ElementDirectory.FindElements(member.GetDomainClass()).Count;
            return $"{displayTextBase} ({count})";
         }
      }

      internal class ModelExplorerSearchTask : VsSearchTask
      {
         private readonly EFModelExplorer modelExplorer;

         public ModelExplorerSearchTask(EFModelExplorer modelExplorer
                                      , uint dwCookie
                                      , IVsSearchQuery pSearchQuery
                                      , IVsSearchCallback pSearchCallback)
            : base(dwCookie, pSearchQuery, pSearchCallback)
         {
            this.modelExplorer = modelExplorer;
         }

         /// <summary>
         /// Called to remove an ExplorerTreeNode from the explorer. Also removes empty attribute group nodes.
         /// </summary>
         /// <param name="node">Node to remove</param>
         private void Remove(TreeNode node)
         {
            if (node is ExplorerTreeNode explorerNode)
            {
               TreeNode parent = node.Parent; // will be a group node
               node.Remove();

               if (explorerNode.RepresentedElement is ModelAttribute && parent.Nodes.Count == 0)
                  parent.Remove();
            }
         }

         protected override void OnStartSearch()
         {
            // note that this will be run from a background thread. Must context switch to the UI thread to manipulate the tree.
            // use ThreadHelper.Generic.BeginInvoke for that 

            TreeView treeView = modelExplorer.ObjectModelBrowser;

            ThreadHelper.Generic.BeginInvoke(() =>
                                             {
                                                modelExplorer.RefreshBrowserView();
                                                treeView.SelectedNode = null;
                                                PerformSearch(treeView);
                                             });

            SearchResults = (uint)treeView.GetAllNodes().Count(n => n is ExplorerTreeNode explorerNode && explorerNode.RepresentedElement != null);

            // Call to base will report completion
            base.OnStartSearch();
         }

         private void PerformSearch(TreeView treeView)
         {
            List<string> searchTexts = SearchUtilities.ExtractSearchTokens(SearchQuery).Select(token => token.ParsedTokenText).ToList();

            // if nothing to search for, everything's a hit
            if (searchTexts.Any())
            {
               // prune tree to remove non-hits

               // 1) remove attribute and diagram nodes that aren't matches. This removes attribute group nodes when necessary
               List<ExplorerTreeNode> diagramNodes = treeView.GetAllNodes()
                                                             .OfType<ExplorerTreeNode>()
                                                             .Where(n => n.RepresentedElement is ModelDiagramData)
                                                             .ToList();

               List<ExplorerTreeNode> attributeNodes = treeView.GetAllNodes()
                                                               .OfType<ExplorerTreeNode>()
                                                               .Where(n => n.RepresentedElement is ModelAttribute)
                                                               .ToList();

               foreach (ExplorerTreeNode node in attributeNodes.Union(diagramNodes).Where(node => searchTexts.All(t => node.Text.IndexOf(t, StringComparison.CurrentCultureIgnoreCase) == -1)))
                  Remove(node);

               // 2) there are cases where a class has no attributes. Find those group nodes and remove them, since we couldn't get to them via their attribute nodes
               List<EFModelRoleGroupTreeNode> emptyAttributeGroupNodes = treeView.GetAllNodes()
                                                                                 .OfType<EFModelRoleGroupTreeNode>()
                                                                                 .Where(n => n.Parent is ExplorerTreeNode explorerNode 
                                                                                          && explorerNode.RepresentedElement is ModelClass 
                                                                                          && n.Nodes.Count == 0)
                                                                                 .ToList();

               foreach (EFModelRoleGroupTreeNode emptyAttributeGroupNode in emptyAttributeGroupNodes)
                  emptyAttributeGroupNode.Remove();

               // 3) remove childless class nodes that aren't matches. 
               List<ExplorerTreeNode> classNodes = treeView.GetAllNodes()
                                                           .OfType<ExplorerTreeNode>()
                                                           .Where(n => n.RepresentedElement is ModelClass)
                                                           .ToList();

               foreach (ExplorerTreeNode node in classNodes.Where(node => node.Nodes.Count == 0 && searchTexts.All(t => node.Text.IndexOf(t, StringComparison.CurrentCultureIgnoreCase) == -1)))
                  Remove(node);

               // 4) update the text on all group nodes left in the tree
               List<EFModelRoleGroupTreeNode> groupNodes = treeView.GetAllNodes()
                                                                   .OfType<EFModelRoleGroupTreeNode>()
                                                                   .Where(n => n != treeView.Nodes[0])
                                                                   .ToList();

               foreach (EFModelRoleGroupTreeNode groupNode in groupNodes)
                  groupNode.Text = groupNode.GetNodeText();

               treeView.ExpandAll();
            }
         }
      }
   }
}