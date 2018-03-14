using System;
using System.Windows.Forms;
using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorer
   {
      private readonly ModelSearch searchControl;

      public EFModelExplorer(EFModelExplorerToolWindowBase serviceProvider)
         : this((IServiceProvider)serviceProvider)
      {
         SuspendLayout();

         searchControl = new ModelSearch {Dock = DockStyle.Top};
         Controls.Add(searchControl);
         Controls.SetChildIndex(searchControl, 0);

         ResumeLayout(false);
         PerformLayout();
      }
      
      
   }
}
