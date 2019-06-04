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
      /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick" /> event.</summary>
      /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
      protected override void OnDoubleClick(EventArgs e)
      {
         base.OnDoubleClick(e);

         // let's make this a switch so we can more easily extend it to different element types later
         switch (SelectedElement)
         {
            case ModelClass modelClass:
               EFModelDocData.OpenFileFor(modelClass);
               break;
            case ModelAttribute modelAttribute:
               EFModelDocData.OpenFileFor(modelAttribute.ModelClass);
               break;
            case ModelEnum modelEnum:
               EFModelDocData.OpenFileFor(modelEnum);
               break;
            case ModelEnumValue modelEnumValue:
               EFModelDocData.OpenFileFor(modelEnumValue.Enum);
               break;
         }
      }

      protected override void ProcessOnStatusDeleteCommand(MenuCommand cmd)
      {
         OnStatusDeleteCommandLogic.ForExplorerDelete(cmd,
                                                      SelectedElement,
                                                      base.ProcessOnStatusDeleteCommand);
      }

      protected override void ProcessOnStatusDeleteAllCommand(MenuCommand cmd)
      {
         OnStatusDeleteCommandLogic.ForExplorerDeleteAll(cmd,
                                                         ObjectModelBrowser.SelectedNode,
                                                         base.ProcessOnStatusDeleteAllCommand);
      }

   }

   /// <summary>
   /// Implements the logic for the ProcessOnStatus callback
   /// methods for the commands.
   /// </summary>
   /// <remarks>
   /// https://archive.codeplex.com/?p=JaDAL
   /// </remarks>
   public static class OnStatusDeleteCommandLogic
   {
      /// <summary>
      /// Use this method from within the MyLanguageCommandSet class
      /// in the ProcessOnStatusDeleteCommand() method.
      /// </summary>
      /// <example>
      /// partial class MyLanguageCommandSet
      /// {
      ///    protected override void ProcessOnStatusDeleteCommand(MenuCommand command)
      ///    {
      ///        OnStatusDeleteCommandLogic.ForEditor(command, this.CurrentDocumentSelection, base.ProcessOnStatusDeleteCommand);
      ///    }
      /// }
      /// </example>
      public static void ForEditor(MenuCommand command, ICollection currentDocumentSelection, Action<MenuCommand> baseFunction)
      {
         if (currentDocumentSelection.OfType<IDynamicCanDelete>().Any(x => !x.CanDelete()) ||
             currentDocumentSelection.OfType<PresentationElement>().Select(x => x.ModelElement).OfType<IDynamicCanDelete>().Any(x => !x.CanDelete()))
         {
            command.Visible = false;
            command.Enabled = false;
            return;
         }

         baseFunction(command);
      }

      /// <summary>
      /// Use this method from within the MyLanguageExplorer class
      /// in the ProcessOnStatusDeleteCommand() method.
      /// </summary>
      /// <example>
      /// partial class MyLanguageExplorer
      /// {
      ///    protected override void ProcessOnStatusDeleteCommand(MenuCommand cmd)
      ///    {
      ///        OnStatusDeleteCommandLogic.ForExplorerDelete(cmd, this.SelectedElement, base.ProcessOnStatusDeleteCommand);
      ///    }
      /// }
      /// </example>
      public static void ForExplorerDelete(MenuCommand cmd, ModelElement selectedElement, Action<MenuCommand> baseFunction)
      {
         // check the selected element
         if (selectedElement is IDynamicCanDelete dynamicCanDelete && !dynamicCanDelete.CanDelete())
         {
            // hide the command
            cmd.Visible = false;
            cmd.Enabled = false;
            return;
         }

         // call the base implementation
         baseFunction(cmd);
      }

      /// <summary>
      /// Use this method from within the MyLanguageExplorer class
      /// in the ProcessOnStatusDeleteAllCommand() method.
      /// </summary>
      /// <example>
      /// partial class MyLanguageExplorer
      /// {
      ///    protected override void ProcessOnStatusDeleteAllCommand(MenuCommand cmd)
      ///    {
      ///        OnStatusDeleteCommandLogic.ForExplorerDeleteAll(cmd, this.ObjectModelBrowser.SelectedNode, base.ProcessOnStatusDeleteAllCommand);
      ///    }
      /// }
      /// </example>
      public static void ForExplorerDeleteAll(MenuCommand cmd, TreeNode selectedNode, Action<MenuCommand> baseFunction)
      {
         if (selectedNode != null &&
             selectedNode.Nodes
                         .OfType<ModelElementTreeNode>()
                         .Any(x => x.ModelElement is IDynamicCanDelete dynamicCanDelete &&
                                 !dynamicCanDelete.CanDelete()))
         {
            // hide the command
            cmd.Visible = false;
            cmd.Enabled = false;
            return;
         }

         // call the base implementation
         baseFunction(cmd);
      }
   }

   public static class OnStatusCopyCommandLogic
   {
      public static void ForEditor(MenuCommand command, ICollection currentDocumentSelection, Action<MenuCommand> baseFunction)
      {
         if (currentDocumentSelection.OfType<IDynamicCanCopy>().Any(x => !x.CanCopy()) ||
             currentDocumentSelection.OfType<PresentationElement>().Select(x => x.ModelElement).OfType<IDynamicCanCopy>().Any(x => !x.CanCopy()))
         {
            command.Visible = false;
            command.Enabled = false;
            return;
         }

         baseFunction(command);
      }

      public static void ForExplorerCopy(MenuCommand cmd, ModelElement selectedElement, Action<MenuCommand> baseFunction)
      {
         // check the selected element
         if (selectedElement is IDynamicCanCopy dynamicCanCopy && !dynamicCanCopy.CanCopy())
         {
            // hide the command
            cmd.Visible = false;
            cmd.Enabled = false;
            return;
         }

         // call the base implementation
         baseFunction(cmd);
      }

   }

   public static class OnStatusPasteCommandLogic
   {
      public static void ForEditor(MenuCommand command, ICollection currentDocumentSelection, Action<MenuCommand> baseFunction)
      {
         if (currentDocumentSelection.OfType<IDynamicCanPaste>().Any(x => !x.CanPaste()) ||
             currentDocumentSelection.OfType<PresentationElement>().Select(x => x.ModelElement).OfType<IDynamicCanPaste>().Any(x => !x.CanPaste()))
         {
            command.Visible = false;
            command.Enabled = false;
            return;
         }

         baseFunction(command);
      }

      public static void ForExplorerPaste(MenuCommand cmd, ModelElement selectedElement, Action<MenuCommand> baseFunction)
      {
         // check the selected element
         if (selectedElement is IDynamicCanPaste dynamicCanPaste && !dynamicCanPaste.CanPaste())
         {
            // hide the command
            cmd.Visible = false;
            cmd.Enabled = false;
            return;
         }

         // call the base implementation
         baseFunction(cmd);
      }

   }
}
