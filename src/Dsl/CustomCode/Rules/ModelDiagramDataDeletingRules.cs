using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelDiagramData), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelDiagramDataDeletingRules : DeletingRule
   {
      /// <inheritdoc />
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         ModelDiagramData element = (ModelDiagramData)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (BooleanQuestionDisplay.Show($"About to permanently delete diagram named {element.Name} - are you sure?") != true)
         {
            current.Rollback();

            return;
         }

         EFModelDiagram diagram = element.GetDiagram();
         ModelDiagramData.CloseDiagram?.Invoke(diagram);
         diagram.Delete();
      }
   }
}