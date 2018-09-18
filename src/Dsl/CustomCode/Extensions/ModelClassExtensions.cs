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
            List<ModelAttribute> newAttributes = superclass.AllAttributes
                                                           .Select(modelAttribute => (ModelAttribute)modelAttribute.Copy(new[]
                                                                                                                         {
                                                                                                                            ClassHasAttributes.AttributeDomainRoleId
                                                                                                                         }))
                                                           .Distinct()
                                                           .ToList();

            foreach (ModelAttribute newAttribute in newAttributes)
               newAttribute.ModelClass = subclass;

            List<Association> associations = new List<Association>(); 
            ModelClass src = superclass;
            while (src != null)
            {
               associations.AddRange(store.ElementDirectory.AllElements.OfType<Association>().Where(a => a.Source == src || a.Target == src));
               src = src.Superclass;
            }

            associations = associations.Distinct().ToList();

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
