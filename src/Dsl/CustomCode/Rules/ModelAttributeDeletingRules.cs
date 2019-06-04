using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelAttribute), FireTime = TimeToFire.TopLevelCommit)]
   public class ModelAttributeDeletingRules : DeletingRule
   {
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         ModelAttribute element = (ModelAttribute)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (element.ModelClass.ReadOnly)
         {
            ErrorDisplay.Show($"{element.ModelClass.Name} is read-only; can't delete any of its properties");
            current.Rollback();

            return;
         }
      }
   }
}