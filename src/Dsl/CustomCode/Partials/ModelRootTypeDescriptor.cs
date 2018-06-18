using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sawczyn.EFDesigner.EFModel.CustomCode.Utilities;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelRootTypeDescriptor
   {
      /// <summary>
      /// Returns a collection of property descriptors an instance of ModelRoot.
      /// </summary>
      private global::System.ComponentModel.PropertyDescriptorCollection GetCustomProperties(global::System.Attribute[] attributes)
      {
         ModelRoot modelRoot = ModelElement as ModelRoot;

         if (modelRoot!= null)
         {
            PropertyGridUtility.FixupBrowsability(modelRoot);
            PropertyGridUtility.FixupReadability(modelRoot);
         }

         // Get the default property descriptors from the base class
         global::System.ComponentModel.PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (modelRoot != null)
         {
            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}
