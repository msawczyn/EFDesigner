using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;

using EnvDTE;

using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocView 
   {
      ///// <summary>
      ///// </summary>
      //protected override void OnCreate()
      //{
      //   base.OnCreate();
      //   EnvDTE.DTE dte = (EnvDTE.DTE)GetService(typeof(EnvDTE.DTE));
      //   EnvDTE80.Events2 events = (EnvDTE80.Events2)dte.Events;
      //   events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;
      //   events.WindowEvents.WindowClosing += WindowsEvents_WindowClosing;
      //}

      public override void SetInfo()
      {
         base.SetInfo();
         Messages.AddStatus(Messages.LastStatusMessage);
      }

      protected EFModelExplorerToolWindow ModelExplorerWindow
      {
         get
         {
            return EFModelPackage.Instance?.GetToolWindow(typeof(EFModelExplorerToolWindow), true) as EFModelExplorerToolWindow;
         }
      }

      /// <summary>
      /// Called to initialize the view after the corresponding document has been loaded.
      /// </summary>
      protected override bool LoadView()
      {
         DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
         dte.Events.WindowEvents.WindowActivated += OnWindowActivated;
         dte.Events.WindowEvents.WindowClosing += OnWindowClosing;

         bool result = base.LoadView();
         if (result)
            Frame.SetProperty((int)__VSFPROPID.VSFPROPID_EditorCaption, $" [{Diagram.Name}]");

         return result;
      }

      private void OnWindowClosing(Window window)
      {
         DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
         dte.Events.WindowEvents.WindowActivated -= OnWindowActivated;
         dte.Events.WindowEvents.WindowClosing -= OnWindowClosing;
      }

      private void OnWindowActivated(Window gotFocus, Window lostFocus)
      {
         TreeView treeView = ModelExplorerWindow.TreeContainer.ObjectModelBrowser;
         List<EFModelExplorer.EFModelElementTreeNode> classNodes = treeView.GetAllNodes().OfType<EFModelExplorer.EFModelElementTreeNode>().Where(n => n.RepresentedElement is ModelClass).ToList();
         List<EFModelExplorer.EFModelElementTreeNode> enumNodes = treeView.GetAllNodes().OfType<EFModelExplorer.EFModelElementTreeNode>().Where(n => n.RepresentedElement is ModelEnum).ToList();
         List<ModelClass> classesInDiagram = CurrentDiagram.NestedChildShapes.OfType<ClassShape>().Select(s => s.ModelElement as ModelClass).Where(c => c != null).ToList();
         List<ModelEnum> enumsInDiagram = CurrentDiagram.NestedChildShapes.OfType<EnumShape>().Select(s => s.ModelElement as ModelEnum).Where(c => c != null).ToList();

         treeView.BeginUpdate();

         foreach (EFModelExplorer.EFModelElementTreeNode classNode in classNodes)
         {
            ModelClass classNodeRepresentedElement = (ModelClass)classNode.RepresentedElement;

            classNode.ForeColor = classesInDiagram.Contains(classNodeRepresentedElement)
                                     ? Color.ForestGreen
                                     : Color.Black;

            classesInDiagram.Remove(classNodeRepresentedElement);
         }

         foreach (EFModelExplorer.EFModelElementTreeNode enumNode in enumNodes)
         {
            ModelEnum enumNodeRepresentedElement = (ModelEnum)enumNode.RepresentedElement;

            enumNode.ForeColor = enumsInDiagram.Contains(enumNodeRepresentedElement)
                                    ? Color.ForestGreen
                                    : Color.Black;

            enumsInDiagram.Remove(enumNodeRepresentedElement);
         }
         
         treeView.EndUpdate();
      }

      /// <summary>
      /// Called when window is closed. Overridden here to remove our objects from the selection context so that
      /// the property browser doesn't call back on our objects after the window is closed.
      /// </summary>
      protected override void OnClose()
      {
         bool dirty = DocData.IsDirty(out int isDirty) == 0 && isDirty == 1;

         if (!DocData.DocViews.Except(new[] {this}).Any() && dirty && DocData.QuerySaveFile().CanSaveFile)
            DocData.Save("", 1, 0);

         base.OnClose();
      }
   }
}
