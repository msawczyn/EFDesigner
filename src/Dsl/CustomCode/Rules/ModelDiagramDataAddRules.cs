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
   [RuleOn(typeof(ModelDiagramData), FireTime = TimeToFire.TopLevelCommit)]
   public class ModelDiagramDataAddRules : AddRule
   {
      public static bool DisableLoad { get; set; }

      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelDiagramData element = (ModelDiagramData)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (ModelDiagramData.DisplayDiagram != null && !DisableLoad)
            ModelDiagramData.DisplayDiagram(element.Name);
      }
   }
}
