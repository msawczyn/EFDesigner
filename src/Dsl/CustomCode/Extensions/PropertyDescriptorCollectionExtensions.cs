using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class PropertyDescriptorCollectionExtensions
   {
      public static void Remove(this PropertyDescriptorCollection propertyDescriptors, string name)
      {
         PropertyDescriptor propertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == name);

         if (propertyDescriptor != null)
            propertyDescriptors.Remove(propertyDescriptor);
      }
   }
}
