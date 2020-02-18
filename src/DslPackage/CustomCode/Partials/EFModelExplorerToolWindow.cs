using System;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorerToolWindow
   {
      protected override void OnSelectionChanged(EventArgs e)
      {
         base.OnSelectionChanged(e);

         // select element in tree
         if (PrimarySelection != null)
         {
            if (PrimarySelection is ModelDiagramData modelDiagramData)
            {
               EFModelDocData docData = (EFModelDocData)TreeContainer.ModelingDocData;
               docData.OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(modelDiagramData.Name, typeof(EFModelDiagram), docData.RootElement));

               return;
            }

            if (PrimarySelection is ModelElement modelElement)
               modelElement.LocateInDiagram(true);
         }
      }
   }
}
