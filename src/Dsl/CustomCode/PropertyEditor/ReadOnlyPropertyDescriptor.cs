using System;
using System.ComponentModel;

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
   ///    A PropertyDescriptor for read-only properties.
   ///    This PropertyDescriptor wraps another PropertyDescriptor and delegates
   ///    all calls to this one. Only the IsReadOnly property will return always true.
   /// </summary>
   public class ReadOnlyPropertyDescriptor : PropertyDescriptor
   {
      private readonly PropertyDescriptor descriptor;

      /// <summary>
      ///    Initializes a new instance of the <see cref="ReadOnlyPropertyDescriptor" /> class.
      /// </summary>
      /// <param name="descriptor">A base descriptor for this descriptor.</param>
      public ReadOnlyPropertyDescriptor(PropertyDescriptor descriptor) : base(descriptor.Name, new Attribute[] { })
      {
         this.descriptor = descriptor;
      }

      /// <summary>
      ///    Indicates that this property is allways read only
      /// </summary>
      /// <remarks>For writable properties this ReadOnlyPropertyDescriptor must not be used.</remarks>
      public override bool IsReadOnly => true;

      public override AttributeCollection Attributes => descriptor.Attributes;

      public override string Category => descriptor.Category;

      public override string Description => descriptor.Description;

      public override bool DesignTimeOnly => descriptor.DesignTimeOnly;

      public override string DisplayName => descriptor.DisplayName;

      public override bool IsBrowsable => descriptor.IsBrowsable;

      public override string Name => descriptor.Name;

      public override string ToString()
      {
         return descriptor.ToString();
      }

#region delegate all memeber to default descriptor

      /// <summary>
      ///    Enables other objects to be notified when this property changes.
      /// </summary>
      /// <param name="component">The component to add the handler for.</param>
      /// <param name="handler">The delegate to add as a listener.</param>
      /// <exception cref="T:System.ArgumentNullException">
      ///    <paramref name="component" /> or <paramref name="handler" /> is null.
      /// </exception>
      public new void AddValueChanged(object component, EventHandler handler)
      {
         descriptor.AddValueChanged(component, handler);
      }

      /// <summary>
      ///    When overridden in a derived class, returns whether resetting an object changes its value.
      /// </summary>
      /// <param name="component">The component to test for reset capability.</param>
      /// <returns>
      ///    true if resetting the component changes its value; otherwise, false.
      /// </returns>
      public override bool CanResetValue(object component)
      {
         return descriptor.CanResetValue(component);
      }

      /// <summary>
      ///    When overridden in a derived class, gets the type of the component this property is bound to.
      /// </summary>
      /// <value></value>
      /// <returns>
      ///    A <see cref="T:System.Type" /> that represents the type of component this property is bound to. When the
      ///    <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" /> or
      ///    <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" /> methods are invoked,
      ///    the object specified might be an instance of this type.
      /// </returns>
      public override Type ComponentType => descriptor.ComponentType;

      /// <summary>
      ///    Gets the type converter for this property.
      /// </summary>
      /// <value></value>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.TypeConverter" /> that is used to convert the
      ///    <see cref="T:System.Type" /> of this property.
      /// </returns>
      /// <PermissionSet>
      ///    <IPermission
      ///       class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
      ///       version="1" Flags="UnmanagedCode" />
      /// </PermissionSet>
      public new TypeConverter Converter => descriptor.Converter;

      /// <summary>
      ///    Compares this to another object to see if they are equivalent.
      /// </summary>
      /// <param name="obj">The object to compare to this <see cref="T:System.ComponentModel.PropertyDescriptor" />.</param>
      /// <returns>
      ///    true if the values are equivalent; otherwise, false.
      /// </returns>
      public new bool Equals(object obj)
      {
         return descriptor.Equals(obj);
      }

      /// <summary>
      ///    Returns the default <see cref="T:System.ComponentModel.PropertyDescriptorCollection" />.
      /// </summary>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" />.
      /// </returns>
      /// <PermissionSet>
      ///    <IPermission
      ///       class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
      ///       version="1" Flags="UnmanagedCode, ControlEvidence" />
      /// </PermissionSet>
      public new PropertyDescriptorCollection GetChildProperties()
      {
         return descriptor.GetChildProperties();
      }

      /// <summary>
      ///    Returns a <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> for a given object using a specified
      ///    array of attributes as a filter.
      /// </summary>
      /// <param name="instance">A component to get the properties for.</param>
      /// <param name="filter">An array of type <see cref="T:System.Attribute" /> to use as a filter.</param>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that match the specified
      ///    attributes for the specified component.
      /// </returns>
      public new PropertyDescriptorCollection GetChildProperties(object instance, Attribute[] filter)
      {
         return descriptor.GetChildProperties(instance, filter);
      }

      /// <summary>
      ///    Returns a <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> using a specified array of attributes
      ///    as a filter.
      /// </summary>
      /// <param name="filter">An array of type <see cref="T:System.Attribute" /> to use as a filter.</param>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties that match the specified
      ///    attributes.
      /// </returns>
      public new PropertyDescriptorCollection GetChildProperties(Attribute[] filter)
      {
         return descriptor.GetChildProperties(filter);
      }

      /// <summary>
      ///    Returns a <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> for a given object.
      /// </summary>
      /// <param name="instance">A component to get the properties for.</param>
      /// <returns>
      ///    A <see cref="T:System.ComponentModel.PropertyDescriptorCollection" /> with the properties for the specified
      ///    component.
      /// </returns>
      public new PropertyDescriptorCollection GetChildProperties(object instance)
      {
         return descriptor.GetChildProperties(instance);
      }

      /// <summary>
      ///    Gets an editor of the specified type.
      /// </summary>
      /// <param name="editorBaseType">
      ///    The base type of editor, which is used to differentiate between multiple editors that a
      ///    property supports.
      /// </param>
      /// <returns>
      ///    An instance of the requested editor type, or null if an editor cannot be found.
      /// </returns>
      public new object GetEditor(Type editorBaseType)
      {
         return descriptor.GetEditor(editorBaseType);
      }

      /// <summary>
      ///    Returns the hash code for this object.
      /// </summary>
      /// <returns>The hash code for this object.</returns>
      public new int GetHashCode()
      {
         return descriptor.GetHashCode();
      }

      /// <summary>
      ///    When overridden in a derived class, gets the current value of the property on a component.
      /// </summary>
      /// <param name="component">The component with the property for which to retrieve the value.</param>
      /// <returns>
      ///    The value of a property for a given component.
      /// </returns>
      public override object GetValue(object component)
      {
         return descriptor.GetValue(component);
      }

      /// <summary>
      ///    Gets a value indicating whether this property should be localized, as specified in the
      ///    <see cref="T:System.ComponentModel.LocalizableAttribute" />.
      /// </summary>
      /// <value></value>
      /// <returns>
      ///    true if the member is marked with the <see cref="T:System.ComponentModel.LocalizableAttribute" /> set to true;
      ///    otherwise, false.
      /// </returns>
      public new bool IsLocalizable => descriptor.IsLocalizable;

      /// <summary>
      ///    When overridden in a derived class, gets the type of the property.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Type" /> that represents the type of the property.</returns>
      public override Type PropertyType => descriptor.PropertyType;

      /// <summary>
      ///    Enables other objects to be notified when this property changes.
      /// </summary>
      /// <param name="component">The component to remove the handler for.</param>
      /// <param name="handler">The delegate to remove as a listener.</param>
      /// <exception cref="T:System.ArgumentNullException">
      ///    <paramref name="component" /> or <paramref name="handler" /> is null.
      /// </exception>
      public new void RemoveValueChanged(object component, EventHandler handler)
      {
         descriptor.RemoveValueChanged(component, handler);
      }

      /// <summary>
      ///    When overridden in a derived class, resets the value for this property of the component to the default value.
      /// </summary>
      /// <param name="component">The component with the property value that is to be reset to the default value.</param>
      public override void ResetValue(object component)
      {
         descriptor.ResetValue(component);
      }

      /// <summary>
      ///    Gets a value indicating whether this property should be serialized, as specified in the
      ///    <see cref="T:System.ComponentModel.DesignerSerializationVisibilityAttribute" />.
      /// </summary>
      /// <value></value>
      /// <returns>
      ///    One of the <see cref="T:System.ComponentModel.DesignerSerializationVisibility" /> enumeration values that
      ///    specifies whether this property should be serialized.
      /// </returns>
      public new DesignerSerializationVisibility SerializationVisibility => descriptor.SerializationVisibility;

      /// <summary>
      ///    When overridden in a derived class, sets the value of the component to a different value.
      /// </summary>
      /// <param name="component">The component with the property value that is to be set.</param>
      /// <param name="value">The new value.</param>
      public override void SetValue(object component, object value)
      {
         descriptor.SetValue(component, value);
      }

      /// <summary>
      ///    When overridden in a derived class, determines a value indicating whether the value of this property needs to be
      ///    persisted.
      /// </summary>
      /// <param name="component">The component with the property to be examined for persistence.</param>
      /// <returns>
      ///    true if the property should be persisted; otherwise, false.
      /// </returns>
      public override bool ShouldSerializeValue(object component)
      {
         return descriptor.ShouldSerializeValue(component);
      }

      /// <summary>
      ///    Gets a value indicating whether value change notifications for this property may originate from outside the property
      ///    descriptor.
      /// </summary>
      /// <value></value>
      /// <returns>true if value change notifications may originate from outside the property descriptor; otherwise, false.</returns>
      public new bool SupportsChangeEvents => descriptor.SupportsChangeEvents;

#endregion
   }
}
