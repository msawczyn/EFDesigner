// https://stackoverflow.com/questions/54968182/ef-core-get-navigation-properties-of-an-entity-from-model-with-multiplicity-zero

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFCoreParser
{
   public static class IModelExtensions
   {
      /// <summary>
      /// Extension method used to get from the entity all navigation properties by multiplicity
      /// </summary>
      /// <typeparam name="T">Entity from where the navigation properties are taken</typeparam>
      /// <param name="model">Context Model</param>
      /// <param name="multiplicity">Type of multiplicity to use</param>
      /// <returns>List of PropertyInfo of Navigation Properties</returns>
      public static IEnumerable<PropertyInfo> GetNavigationProperties<T>(this IModel model, RelationshipMultiplicity multiplicity)
      {
         IEnumerable<INavigation> navigations = model.GetEntityTypes().FirstOrDefault(m => m.ClrType == typeof(T))?.GetNavigations();

         switch (multiplicity)
         {
            case RelationshipMultiplicity.Many:
               return navigations?
                      .Where(nav => nav.IsCollection())
                      .Select(nav => nav.PropertyInfo);
            case RelationshipMultiplicity.One:
               return navigations?
                      .Where(nav => !nav.IsCollection() && nav.ForeignKey.IsRequired)
                      .Select(nav => nav.PropertyInfo);
            case RelationshipMultiplicity.ZeroOrOne:
               return navigations?
                      .Where(nav => !nav.IsCollection())
                      .Select(nav => nav.PropertyInfo);
            default:
               return null;
         }
      }

      public static RelationshipMultiplicity GetTargetMultiplicity(this INavigation navigation)
      {
         if (navigation.IsCollection())
            return RelationshipMultiplicity.Many;

         if (navigation.ForeignKey.IsRequired)
            return RelationshipMultiplicity.One;

         return RelationshipMultiplicity.ZeroOrOne;
      }

      public static RelationshipMultiplicity GetSourceMultiplicity(this INavigation navigation)
      {
         INavigation inverse = navigation.FindInverse();

         if (inverse == null)
            return RelationshipMultiplicity.One;

         return inverse.GetTargetMultiplicity();
      }

      public static Type Unwrap(this Type type)
      {
         if (type.IsGenericType &&
             (type.GetGenericTypeDefinition() == typeof(Nullable<>) ||
              type.GetGenericTypeDefinition() == typeof(IEnumerable<>) ||
              type.GetGenericTypeDefinition() == typeof(ICollection<>)))
            type = type.GetGenericArguments()[0];
         return type;
      }

      public static IEntityType GetSourceType(this INavigation navigation)
      {
         return navigation.DeclaringType as IEntityType;
      }
   }
}