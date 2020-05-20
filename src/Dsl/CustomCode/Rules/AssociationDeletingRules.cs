using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

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

         List<ModelAttribute> unnecessaryProperties = element.Dependent?.AllAttributes?.Where(x => x.IsForeignKeyFor == element.Id && !x.IsIdentity).ToList();

         if (unnecessaryProperties?.Any() != true)
         {
            WarningDisplay.Show($"{element.GetDisplayText()} doesn't specify defined foreign keys. Removing foreign key attribute(s) {string.Join(", ", unnecessaryProperties.Select(x => x.GetDisplayText()))}");

            foreach (ModelAttribute fkProperty in unnecessaryProperties)
            {
               fkProperty.ClearFKMods();
               fkProperty.ModelClass.Attributes.Remove(fkProperty);
               fkProperty.Delete();
            }
         }
      }
   }
}