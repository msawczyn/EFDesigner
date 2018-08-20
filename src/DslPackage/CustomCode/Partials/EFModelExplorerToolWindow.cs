using System;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      protected override void OnSelectionChanged(EventArgs e)
      {
         base.OnSelectionChanged(e);

         // select element on active diagram
         if (PrimarySelection != null && PrimarySelection is ModelElement modelElement)
            modelElement.LocateInDiagram(true);
      }
   }
}
