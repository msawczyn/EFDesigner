using System;

using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocView
   {
      //internal static bool ChangingSelection { get; private set; }

      /// <summary>
      /// Called when selection changes in this window.
      /// </summary>
      /// <remarks>
      /// Overriden to update the F1 help keyword for the selection.
      /// </remarks>
      /// <param name="e"></param>
      protected override void OnSelectionChanged(EventArgs e)
      {
         base.OnSelectionChanged(e);

         // TODO: Finish this

         //ChangingSelection = true;

         //try
         //{
         //   EFModelExplorerToolWindow.SetSelectedComponents(SelectedElements);
         //}
         //finally
         //{
         //   ChangingSelection = false;
         //}
      }

      protected EFModelExplorerToolWindow EFModelExplorerToolWindow
      {
         get
         {
            EFModelExplorerToolWindow explorerWindow = null;

            if (ServiceProvider.GetService(typeof(Package)) is ModelingPackage package)
               explorerWindow = package.GetToolWindow(typeof(EFModelExplorerToolWindow), true) as EFModelExplorerToolWindow;

            return explorerWindow;
         }
      }
   }
}
