using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnvDTE;

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
         element.SetDiagram(store.ElementDirectory.AllElements.OfType<EFModelDiagram>().FirstOrDefault(d => d.Name == element.Name));
      }
   }
}
