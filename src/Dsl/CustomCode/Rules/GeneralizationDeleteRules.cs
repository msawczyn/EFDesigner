using Microsoft.VisualStudio.Modeling;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.Inline)]
   public class GeneralizationDeleteRules : DeleteRule
   {
      public override void ElementDeleted(ElementDeletedEventArgs e)
      {
         base.ElementDeleted(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         // this rule can be called as a spinoff of the superclass being deleted
         if (element.Superclass.IsDeleting)
            return;

         // make sure identity associations are correct (if necessary)
         store.ModelRoot().TargetIdentityAssociations();

      }
   }
}