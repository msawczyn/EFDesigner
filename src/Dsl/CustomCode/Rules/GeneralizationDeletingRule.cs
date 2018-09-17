using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.TopLevelCommit)]
   internal class GeneralizationDeletingRule : DeletingRule
   {
      /// <inheritdoc />
      public override void ElementDeleting(ElementDeletingEventArgs e)
      {
         base.ElementDeleting(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (!element.Superclass.Attributes.Any() && 
             !element.Superclass.UnidirectionalSources.Any() && 
             !element.Superclass.BidirectionalSources.Any())
            return;

         if (QuestionDisplay.Show($"Push {element.Superclass.Name} properties and associations down to {element.Subclass.Name}?") == true)
         {
            using (Transaction transaction = store.TransactionManager.BeginTransaction("CopyProperties"))
            {
               foreach (ModelAttribute attribute in element.Superclass.Attributes)
                  element.Subclass.Attributes.Add((ModelAttribute)attribute.Copy());

               //ReadOnlyCollection<Association> linksToSources = Association.GetLinksToSources(element.Superclass);
               //ReadOnlyCollection<Association> linksToTargets = Association.GetLinksToTargets(element.Superclass);

               //foreach (Association association in linksToSources)
               //{
               //   // todo: copy
               //}
               //foreach (Association association in linksToTargets)
               //{
               //   // todo: copy
               //}

               transaction.Commit();
            }
         }
      }
   }
}
