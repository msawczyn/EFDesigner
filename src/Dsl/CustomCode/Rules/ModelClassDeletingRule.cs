using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelClassDeletingRule : DeletingRule
   {
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         // todo: see GeneralizationDeletingRule
      }
   }
}
