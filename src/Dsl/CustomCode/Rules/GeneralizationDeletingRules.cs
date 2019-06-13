using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.LocalCommit)]
   internal class GeneralizationDeletingRules : DeletingRule
   {
      /// <inheritdoc />
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         ModelRoot modelRoot = store.ModelRoot();
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         // make sure identity associations are correct (if necessary)
         // this is important for when we're deleting a subclass. There's another run of this in GeneralizationDeleteRules for when the generalization is being deleted by itself
         IdentityHelper identityHelper = new IdentityHelper(modelRoot);
         identityHelper.FixupIdentityAssociations();

         // this rule can be called as a spinoff of the superclass being deleted
         if (element.Superclass.IsDeleting)
            return;

         ModelClass superclass = element.Superclass;
         ModelClass subclass = element.Subclass;

         List<Association> associations = store.Get<Association>().Where(a => a.Source == superclass || a.Target == superclass).ToList();

         if (!superclass.AllAttributes.Any() && !associations.Any())
            return;

         if (!subclass.IsDeleting && QuestionDisplay.Show($"Push {superclass.Name} attributes and associations down to {subclass.Name}?") == true)
            superclass.PushDown(subclass);

      }
   }
}
