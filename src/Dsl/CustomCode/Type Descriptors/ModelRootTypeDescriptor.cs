using System;
using System.ComponentModel;

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
            if (!modelRoot.IsEFCore5Plus)
               propertyDescriptors.Remove("DatabaseCollation");

            if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore)
            {
               propertyDescriptors.Remove("DatabaseInitializerType");
               propertyDescriptors.Remove("AutomaticMigrationsEnabled");
               propertyDescriptors.Remove("ProxyGenerationEnabled");
               propertyDescriptors.Remove("DatabaseType");
               propertyDescriptors.Remove("InheritanceStrategy");

               if (modelRoot.GetEntityFrameworkPackageVersionNum() < 2.1)
                  propertyDescriptors.Remove("LazyLoadingEnabled");
            }

            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}