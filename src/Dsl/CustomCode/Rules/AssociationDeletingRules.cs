using System.Data.Entity.Design.PluralizationServices;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Immutability;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Association), FireTime = TimeToFire.TopLevelCommit)]
   internal class AssociationDeletingRules : DeletingRule
   {
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         Association element = (Association)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         foreach (ModelAttribute fkAttribute in element.ForeignKeyPropertyNames
                                                       .Select(propertyName => element.Dependent?.Attributes?.FirstOrDefault(a => a.Name == propertyName))
                                                       .Where(x => x != null))
            fkAttribute.SetLocks(Locks.None);
      }
   }
}