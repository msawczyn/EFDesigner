using System;
using System.ComponentModel;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelRootTypeDescriptor
   {
      /// <summary>
      /// Returns the property descriptors for the described ModelRoot domain class.
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
