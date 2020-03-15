using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel.Extensions
{
   public static class ListExtensions
   {
      /// <summary>
      /// Reconciles two collections based on a comparison method, indicating what should be added to or removed from the target collection in order to satisfy the reconciliation
      /// </summary>
      /// <typeparam name="T">Type of element contained in targetCollection</typeparam>
      /// <typeparam name="U">Type of element contained in sourceCollection</typeparam>
      /// <param name="targetCollection">Collection of elements to act upon</param>
      /// <param name="sourceCollection">Collection of elements to check against target</param>
      /// <param name="comparer">Method to check "equality" of source element against target element</param>
      /// <returns>Tuple indicating which source elements should be added to the target and which target elements should be removed to reconcile the two collections</returns>
      public static (IEnumerable<U> add, IEnumerable<T> remove) Synchronize<T, U>(this IEnumerable<T> targetCollection, IEnumerable<U> sourceCollection, Func<T, U, bool> comparer)
      {
         List<T> targets = targetCollection.ToList(); // prevent multiple enumeration
         List<U> add = new List<U>();
         List<T> keep = new List<T>();

         foreach (U source in sourceCollection)
         {
            T keeper = targets.FirstOrDefault(target => comparer(target, source));
           
            if (keeper != null) 
               keep.Add(keeper);
            else
               add.Add(source);
         }

         List<T> remove = targets.Except(keep).ToList();

         return (add, remove);
      }
   }
}
