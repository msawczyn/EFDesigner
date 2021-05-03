using Microsoft.VisualStudio.Modeling;
using System.Collections.Generic;
using System.Linq;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Extension methods for Sawczyn.EFDesigner.EFModel.ModelClass
   /// </summary>
   public static class ModelClassExtensions
   {
      /// <summary>
      /// Moves all attributes and associations from a superclass to its subclass
      /// </summary>
      /// <param name="source">Source ModelClass</param>
      /// <param name="target">Target ModelClass</param>
      public static void MoveContents(this ModelClass source, ModelClass target)
      {
         Store store = source.Store;

         using (Transaction transaction = store.TransactionManager.BeginTransaction("PushDown"))
         {
            List<ModelAttribute> newAttributes = source.AllAttributes
                                                           .Select(modelAttribute => (ModelAttribute)modelAttribute.Copy(new[] {ClassHasAttributes.AttributeDomainRoleId}))
                                                           .Distinct()
                                                           .ToList();

            foreach (ModelAttribute newAttribute in newAttributes)
               newAttribute.ModelClass = target;

            List<Association> associations = new List<Association>(); 
            ModelClass src = source;
            while (src != null)
            {
               associations.AddRange(store.GetAll<Association>().Where(a => a.Source == src || a.Target == src));
               src = src.Superclass;
            }

            associations = associations.Distinct().ToList();

            foreach (UnidirectionalAssociation association in associations.OfType<UnidirectionalAssociation>())
            {
               if (association.Source == source)
                  UnidirectionalAssociationBuilder.Connect(target, association.Target);
               else
                  UnidirectionalAssociationBuilder.Connect(association.Source, target);
            }

            foreach (BidirectionalAssociation association in associations.OfType<BidirectionalAssociation>())
            {
               if (association.Source == source)
                  BidirectionalAssociationBuilder.Connect(target, association.Target);
               else
                  BidirectionalAssociationBuilder.Connect(association.Source, target);
            }

            transaction.Commit();
         }
      }
   }
}
