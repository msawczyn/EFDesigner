using System;
using System.ComponentModel;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel
{
   public class NamespacesPropertyDescriptor : PropertyDescriptor
   {
      /// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyDescriptor" /> class with the specified name and attributes.</summary>
      /// <param name="name">The name of the property. </param>
      /// <param name="attrs">An array of type <see cref="T:System.Attribute" /> that contains the property attributes. </param>
      public NamespacesPropertyDescriptor([NotNull] string name, Attribute[] attrs) : base(name, attrs) { }

      /// <summary>When overridden in a derived class, gets the type of the component this property is bound to.</summary>
      /// <returns>
      ///    A <see cref="T:System.Type" /> that represents the type of component this property is bound to. When the
      ///    <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)" /> or
      ///    <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)" /> methods are invoked, the object specified might be an instance of this type.
      /// </returns>
      public override Type ComponentType { get; } = typeof(ModelRoot);

      /// <summary>When overridden in a derived class, gets a value indicating whether this property is read-only.</summary>
      /// <returns>
      ///    <see langword="true" /> if the property is read-only; otherwise, <see langword="false" />.
      /// </returns>
      public override bool IsReadOnly { get; } = true;

      /// <summary>When overridden in a derived class, gets the type of the property.</summary>
      /// <returns>A <see cref="T:System.Type" /> that represents the type of the property.</returns>
      public override Type PropertyType { get; } = typeof(Namespaces);

      /// <summary>When overridden in a derived class, returns whether resetting an object changes its value.</summary>
      /// <param name="component">The component to test for reset capability. </param>
      /// <returns>
      ///    <see langword="true" /> if resetting the component changes its value; otherwise, <see langword="false" />.
      /// </returns>
      public override bool CanResetValue(object component)
      {
         return false;
      }

      /// <summary>When overridden in a derived class, gets the current value of the property on a component.</summary>
      /// <param name="component">The component with the property for which to retrieve the value. </param>
      /// <returns>The value of a property for a given component.</returns>
      public override object GetValue(object component)
      {
         if (!(component is ModelRoot modelRoot))
            throw new ArgumentException(nameof(component));

         return modelRoot.Namespaces;
      }

      /// <summary>When overridden in a derived class, resets the value for this property of the component to the default value.</summary>
      /// <param name="component">The component with the property value that is to be reset to the default value. </param>
      public override void ResetValue(object component)
      {
         throw new NotImplementedException();
      }

      /// <summary>When overridden in a derived class, sets the value of the component to a different value.</summary>
      /// <param name="component">The component with the property value that is to be set. </param>
      /// <param name="value">The new value. </param>
      public override void SetValue(object component, object value)
      {
         throw new NotImplementedException();
      }

      /// <summary>When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.</summary>
      /// <param name="component">The component with the property to be examined for persistence. </param>
      /// <returns>
      ///    <see langword="true" /> if the property should be persisted; otherwise, <see langword="false" />.
      /// </returns>
      public override bool ShouldSerializeValue(object component)
      {
         return false;
      }
   }

   public partial class ModelRootTypeDescriptor
   {
      /// <summary>
      ///    Returns the property descriptors for the described ModelRoot domain class.
      /// </summary>
      private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
      {
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (ModelElement is ModelRoot modelRoot)
         {
            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, modelRoot);

            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}