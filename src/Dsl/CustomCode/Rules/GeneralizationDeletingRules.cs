using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.TopLevelCommit)]
   internal class GeneralizationDeletingRules : DeletingRule
   {
      /// <inheritdoc />
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (element.Superclass.IsDeleting)
            return;

         ModelClass subclass = element.Subclass;
         ModelClass superclass = element.Superclass;

         List<Association> associations = superclass.AllNavigationProperties()
                                                    .Select(n => n.AssociationObject)
                                                    .Distinct()
                                                    .ToList();

         if (!superclass.AllAttributes.Any() && !associations.Any())
            return;

         if (!subclass.IsDeleting && BooleanQuestionDisplay.Show(store, $"Push {superclass.Name} attributes and associations down to {subclass.Name}?") == true)
            superclass.MoveContents(subclass);
      }
   }
}
