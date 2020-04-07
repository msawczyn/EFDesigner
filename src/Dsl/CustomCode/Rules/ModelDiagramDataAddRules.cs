using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelDiagramData), FireTime = TimeToFire.LocalCommit)]
   public class ModelDiagramDataAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelDiagramData element = (ModelDiagramData)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         ModelDiagramData.OpenDiagram?.Invoke(element);
         if (element.GetDiagram() == null)
            element.SetDiagram(store.GetAll<EFModelDiagram>().FirstOrDefault(d => d.Name == store.ModelRoot().GetFileName()));
      }
   }
}
