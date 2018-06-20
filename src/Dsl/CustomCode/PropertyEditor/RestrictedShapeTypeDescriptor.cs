using System;
using System.ComponentModel;
using System.Linq;

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
   ///    A CustomTypeDescriptor for Shapes that will be used to restrict
   ///    the properties list.
   ///    All other methodes will be delegated to the default TypeDescriptor.
   /// </summary>
   public class RestrictedShapeTypeDescriptor : CustomTypeDescriptor
   {
      private readonly object instance;
      private readonly TypeDescriptionProvider parentTypeDescriptionProvider;
      private readonly int RestrictionMode;

      /// <summary>
      ///    Initializes a new instance of the <see cref="RestrictedShapeTypeDescriptor" /> class.
      /// </summary>
      /// <param name="restricionMode">The selected restricion mode. Based on this value the properties list will be manipulated.</param>
      /// <param name="instance">The instance to create a TypeDescriptor for.</param>
      /// <param name="parent">The parent TypeDescriptorProvider.</param>
      public RestrictedShapeTypeDescriptor(int restricionMode, object instance, TypeDescriptionProvider parent)
      {
         RestrictionMode               = restricionMode;
         this.instance                 = instance;
         parentTypeDescriptionProvider = parent;
      }

      /// <summary>
      ///    Changes the property in the propertycollection.
      ///    For Restriction.Full everythings stays the same.
      ///    For Restriction.Hidden the property will be removed.
      ///    For Restriction.ReadOnly the property will be set to read only
      /// </summary>
      private void ChangeProperty(Restriction restriction, string propertyName, PropertyDescriptorCollection properycollection)
      {
         if (restriction == Restriction.Full)
            return;

         PropertyDescriptor prop = properycollection.Find(propertyName, true);

         if (prop != null)
         {
            if (restriction == Restriction.Hidden)
               properycollection.Remove(prop);
            else if (restriction == Restriction.ReadOnly)
            {
               PropertyDescriptor propreadonly = new ReadOnlyPropertyDescriptor(prop);
               properycollection.Remove(prop);
               properycollection.Add(propreadonly);
            }
         }
      }

      public override PropertyDescriptorCollection GetProperties(Attribute[] atr)
      {
         PropertyDescriptorCollection properties = parentTypeDescriptionProvider.GetTypeDescriptor(instance)
                                                                                .GetProperties(atr);

         // the RestrictedPropertyAttributes that use the RestrictionMode
         if (RestrictionMode > 0)
         {
            object[] attributes = instance.GetType()
                                          .GetCustomAttributes(typeof(RestrictedPropertyAttribute), true);

            foreach (RestrictedPropertyAttribute attr in attributes.Cast<RestrictedPropertyAttribute>()
                                                                   .Where(attr => attr.RestrictionMode == RestrictionMode))
               ChangeProperty(attr.Restriction, attr.PropertyName, properties);
         }

         // the CustomRestrictedPropertyAttributes using the ISupportsUserRestrictions interface

         if (instance is ISupportsUserRestrictions restrictionProvider)
         {
            object[] customattributes = instance.GetType()
                                                .GetCustomAttributes(typeof(CustomRestrictedPropertyAttribute), true);

            foreach (CustomRestrictedPropertyAttribute attr in customattributes)
            {
               // ask the instance for the restriction
               Restriction restriction = restrictionProvider.GetRestriction(attr.PropertyName);
               ChangeProperty(restriction, attr.PropertyName, properties);
            }
         }

         return properties;
      }

      /// <summary>
      ///    Returns a collection of property descriptors for the object represented by this type descriptor.
      /// </summary>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> containing the property descriptions for the
      ///    object represented by this type descriptor. The default is
      ///    <see cref="F:System.ComponentModel.PropertyDescriptorCollection.Empty" />.
      /// </returns>
      public override PropertyDescriptorCollection GetProperties()
      {
         return GetProperties(new Attribute[] { });
      }
   }
}
