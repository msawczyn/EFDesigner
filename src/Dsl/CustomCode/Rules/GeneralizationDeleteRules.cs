using Microsoft.VisualStudio.Modeling;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.LocalCommit)]
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
         // this is important for when the generalization is being deleted by itself. There's another run of this in GeneralizationDeletingRules for when we're deleting a subclass. 
         IdentityHelper identityHelper = new IdentityHelper(store.ModelRoot());
         identityHelper.FixupIdentityAssociations();
      }
   }
}