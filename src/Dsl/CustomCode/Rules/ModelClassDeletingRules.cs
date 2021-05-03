using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Immutability;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelClass), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelClassDeletingRules : DeletingRule
   {
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         ModelClass element = (ModelClass)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         foreach (ModelAttribute attribute in element.Attributes)
            attribute.SetLocks(Locks.None);

         List<Generalization> generalizations = store.GetAll<Generalization>().Where(g => g.Superclass == element).ToList();

         if (generalizations.Any())
         {
            string question = generalizations.Count == 1
                                 ? $"Push {element.Name} attributes and associations down its to its subclass?"
                                 : $"Push {element.Name} attributes and associations down its to {generalizations.Count} subclasses?";

            if (BooleanQuestionDisplay.Show(store, question) == true)
            {
               foreach (ModelClass subclass in generalizations.Select(g => g.Subclass))
                  element.MoveContents(subclass);
            }
         }
      }
   }
}
