using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelDiagramData), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelDiagramDataDeleteRules : DeleteRule
   {
      /// <summary>
      /// public virtual method for the client to have his own user-defined delete rule class
      /// </summary>
      /// <param name="e"></param>
      public override void ElementDeleted(ElementDeletedEventArgs e)
      {
         base.ElementDeleted(e);
         ModelDiagramData element = (ModelDiagramData)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         EFModelDiagram diagram = element.GetDiagram();
         if (diagram != null)
         {
            ModelDiagramData.CloseDiagram?.Invoke(diagram);
            diagram.Delete();
         }
      }
   }
}
