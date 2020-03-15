using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelEnum), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelEnumDeleteRules : DeletingRule
   {
      /// <summary>
      /// public virtual method for the client to have his own user-defined delete rule class
      /// </summary>
      /// <param name="e"></param>
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);
         ModelEnum element = (ModelEnum)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         string fullName = element.FullName;

         using (Transaction t1 = store.TransactionManager.BeginTransaction("Remove enum properties"))
         {
            foreach (ModelAttribute modelAttribute in store.ElementDirectory.AllElements.OfType<ModelAttribute>().Where(a => a.Type == fullName))
               modelAttribute.Delete();

            t1.Commit();
         }
      }
   }
}
