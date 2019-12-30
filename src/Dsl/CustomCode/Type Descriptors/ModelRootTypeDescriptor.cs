using System;
using System.ComponentModel;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel
{

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

            // default layout algorithm doesn't have configuration properties
            if (modelRoot.LayoutAlgorithm == LayoutAlgorithm.Default)
               propertyDescriptors.Remove("LayoutAlgorithmSettings");

            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}