using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelEnum), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelEnumDeleteRules : DeleteRule
   {
      public override void ElementDeleted(ElementDeletedEventArgs e)
      {
         base.ElementDeleted(e);
         ModelEnum element = (ModelEnum)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         string fullName = element.FullName;

         using (Transaction t1 = store.TransactionManager.BeginTransaction("Remove enum properties"))
         {
            foreach (ModelAttribute modelAttribute in element.ModelRoot.Store.ElementDirectory.AllElements.OfType<ModelAttribute>().Where(a => a.Type == fullName))
               modelAttribute.Delete();

            t1.Commit();
         }
      }
   }
}
