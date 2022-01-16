using System;
using System.ComponentModel;
using System.Data.Entity.Design.PluralizationServices;

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

               if (modelRoot.GetEntityFrameworkPackageVersionNum() < 2.1)
                  propertyDescriptors.Remove("LazyLoadingEnabled");

               if (!modelRoot.IsEFCore5Plus)
                  propertyDescriptors.Remove("GenerateTableComments");
            }
            else
            {
               propertyDescriptors.Remove("GenerateTableComments");
               propertyDescriptors.Remove("GenerateDbContextFactory");
               propertyDescriptors.Remove("PropertyAccessModeDefault");
               propertyDescriptors.Remove("DatabaseCollationDefault");
            }

            if (!modelRoot.ShowGrid)
               propertyDescriptors.Remove("GridColor");

            // if there's no pluralization service, don't ask if we should pluralize
            if (ModelRoot.PluralizationService == null)
            {
               propertyDescriptors.Remove("PluralizeDbSetNames");
               propertyDescriptors.Remove("PluralizeTableNames");
            }

            //Add in extra custom properties here...
         }

         // Return the property descriptors for this element
         return propertyDescriptors;
      }
   }
}