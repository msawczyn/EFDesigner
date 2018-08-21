using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocView
   {
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

         List<ModelElement> selected_diagram = SelectedElements.OfType<ModelElement>().ToList();
         List<ModelElement> selected_explorer = ModelExplorerWindow?.GetSelectedComponents()?.OfType<ModelElement>() != null
                                                   ? ModelExplorerWindow.GetSelectedComponents().OfType<ModelElement>().ToList()
                                                   : null;

         if (selected_explorer != null)
         {
            if (selected_diagram.Count != 1)
               ModelExplorerWindow.SetSelectedComponents(null);
            else if (selected_diagram[0] != selected_explorer.FirstOrDefault())
               ModelExplorerWindow.SetSelectedComponents(selected_diagram);
         }
      }

      protected EFModelExplorerToolWindow ModelExplorerWindow => EFModelPackage.Instance?.GetToolWindow(typeof(EFModelExplorerToolWindow), true) as EFModelExplorerToolWindow;
   }
}
