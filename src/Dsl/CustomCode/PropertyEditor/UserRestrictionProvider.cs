using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
// ReSharper disable UnusedMember.Global

#region Additional License

/**************************************************************************************
 * This file is based on code in the JaDAL project by Benjamin Schroeter 
 * under New BSD license.
 *
 * Copyright (c) <year>, <copyright holder>
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * For more information please visit http://www.codeplex.com/JaDAL 
 */

#endregion

namespace Sawczyn.EFDesigner
{
   /// <summary>
   ///    This provider adds the functionality of dynamically restricted
   ///    properties to ModelElements.
   /// </summary>
   public class UserRestrictionProvider : TypeDescriptionProvider
   {
      private readonly TypeDescriptionProvider parent;

      /// <summary>
      ///    Initializes a new instance of the <see cref="UserRestrictionProvider" /> class.
      /// </summary>
      /// <param name="parent">The parent type description provider.</param>
      private UserRestrictionProvider(TypeDescriptionProvider parent) : base(parent)
      {
         this.parent = parent;
      }

      /// <summary>
      ///    Gets a custom type descriptor for the given type and object.
      /// </summary>
      /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
      /// <param name="instance">
      ///    An instance of the type. Can be null if no instance was passed to the
      ///    <see cref="T:System.ComponentModel.TypeDescriptor" />.
      /// </param>
      /// <returns>
      ///    An <see cref="T:System.ComponentModel.ICustomTypeDescriptor" /> that can provide metadata for the type.
      /// </returns>
      public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
      {
         if (instance is ModelElement e)
            return new RestrictedShapeTypeDescriptor(GetRestrictionMode(e.Store), instance, parent);

         return parent.GetTypeDescriptor(objectType, instance);
      }

      #region register providers

      /// <summary>
      ///    Registers the restricted element.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      public static void RegisterRestrictedElement<T>() where T : ModelElement
      {
         RegisterRestrictedElement(typeof(T));
      }

      /// <summary>
      ///    Registers all restricted elements in the Assembly of the given Diagram type.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      public static void RegisterAllRestrictedElements<T>() where T : Diagram
      {
         Assembly a = typeof(T).Assembly;

         foreach (Type type in a.GetTypes()
                                .Select(type => new { type, attributes1 = type.GetCustomAttributes(typeof(RestrictedPropertyAttribute), true) })
                                .Select(x => new { x, attributes2 = x.type.GetCustomAttributes(typeof(CustomRestrictedPropertyAttribute), true) })
                                .Where(y => y.x.attributes1.Length + y.attributes2.Length > 0)
                                .Select(z => z.x.type))
            RegisterRestrictedElement(type);
      }

      private static void RegisterRestrictedElement(Type t)
      {
         TypeDescriptionProvider baseprovider = TypeDescriptor.GetProvider(t);
         TypeDescriptor.AddProvider(new UserRestrictionProvider(baseprovider), t);
      }

      #endregion

      #region get and set restriction modes

      private static readonly Dictionary<Guid, int> restrictions = new Dictionary<Guid, int>();

      /// <summary>
      ///    Gets the restriction mode for the given store (and so for the diagram / model of this store)..
      /// </summary>
      /// <param name="store">The store of the diagram or model or modelelement.</param>
      /// <returns>The restriction value, if the store is unknown by the UserRestrictionProvider 0 will be returend.</returns>
      public static int GetRestrictionMode(Store store) => restrictions.ContainsKey(store.Id)
                                                              ? restrictions[store.Id]
                                                              : 0;

      /// <summary>
      ///    Sets the restriction mode for the given store (and so for the diagram / model of this store).
      /// </summary>
      /// <param name="store">The store of the diagram or model or modelelement.</param>
      /// <param name="restriction">The restriction value.</param>
      public static void SetRestrictionMode(Store store, int restriction)
      {
         restrictions[store.Id] = restriction;
      }

      #endregion

      #region AllowMerge for compartment item add

      private static readonly Dictionary<string, bool> cache = new Dictionary<string, bool>();

      public static bool AllowMerge(ModelElement element, string compartmentToAdd)
      {
         int restrictionMode = GetRestrictionMode(element.Store);

         string cacheKey = element.Store.Id
                         + compartmentToAdd
                         + element.GetType()
                                  .FullName
                         + restrictionMode;

         if (cache.ContainsKey(cacheKey))
            return cache[cacheKey];

         bool allow = true;

         foreach (RestrictElementMergeAttribute attribute in element.GetType()
                                                                    .GetCustomAttributes(typeof(RestrictElementMergeAttribute), true)
                                                                    .Cast<RestrictElementMergeAttribute>()
                                                                    .Where(attribute => (attribute.RestrictionMode == restrictionMode) &&
                                                                                        (attribute.Element == compartmentToAdd)))
         {
            allow = !attribute.IsRestricted;

            break;
         }

         cache.Add(cacheKey, allow);

         return allow;
      }

      #endregion
   }
}
