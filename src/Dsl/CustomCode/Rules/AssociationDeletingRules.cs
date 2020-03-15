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

         ModelAttribute[] fkProperties = element.Dependent.AllAttributes.Where(x => x.IsForeignKeyFor == element.Id).ToArray();
         WarningDisplay.Show($"Removing foreign key attribute(s) {string.Join(", ", fkProperties.Select(x => x.GetDisplayText()))}");

         foreach (ModelAttribute fkProperty in fkProperties)
         {
            fkProperty.ClearFKMods();
            fkProperty.ModelClass.Attributes.Remove(fkProperty);
            fkProperty.Delete();
         }
      }
   }
}