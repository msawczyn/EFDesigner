using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelRootTypeDescriptor
   {
      /// <summary>
      /// Returns the property descriptors for the described ModelRoot domain class.
      /// </summary>
      private global::System.ComponentModel.PropertyDescriptorCollection GetCustomProperties(global::System.Attribute[] attributes)
      {
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (ModelElement is ModelRoot modelRoot)
         {
            EFCoreValidator.RemoveHiddenProperties(propertyDescriptors, modelRoot);

            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }

   }
}
