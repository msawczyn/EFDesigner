using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelRootTypeDescriptor
   {
      /// <summary>
      /// Returns a collection of property descriptors an instance of ModelRoot.
      /// </summary>
      private global::System.ComponentModel.PropertyDescriptorCollection GetCustomProperties(global::System.Attribute[] attributes)
      {
         // Get the default property descriptors from the base class
         global::System.ComponentModel.PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         // Get a reference to the model element that is being described.
         ModelRoot source = this.ModelElement as ModelRoot;
         if (source != null)
         {
            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}
