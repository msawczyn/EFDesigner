using Microsoft.VisualStudio.Modeling;
using System.Collections.Generic;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ModelClassExtensions
   {
      public static void PushDown(this ModelClass superclass, ModelClass subclass)
      {
         Store store = superclass.Store;

         using (Transaction transaction = store.TransactionManager.BeginTransaction("PushDown"))
         {
            List<Association> associations = store.ElementDirectory.AllElements.OfType<Association>().Where(a => a.Source == superclass || a.Target == superclass).ToList();

            foreach (ModelAttribute modelAttribute in superclass.AllAttributes.Select(x => (ModelAttribute)x.Copy()))
            {
               modelAttribute.ModelClass = null;
               subclass.Attributes.Add(modelAttribute);
            }

            foreach (UnidirectionalAssociation association in associations.OfType<UnidirectionalAssociation>())
            {
               if (association.Source == superclass)
                  UnidirectionalAssociationBuilder.Connect(subclass, association.Target);
               else
                  UnidirectionalAssociationBuilder.Connect(association.Source, subclass);
            }

            foreach (BidirectionalAssociation association in associations.OfType<BidirectionalAssociation>())
            {
               if (association.Source == superclass)
                  BidirectionalAssociationBuilder.Connect(subclass, association.Target);
               else
                  BidirectionalAssociationBuilder.Connect(association.Source, subclass);
            }

            transaction.Commit();
         }
      }
   }
}
