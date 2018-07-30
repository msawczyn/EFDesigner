using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelAttributeTypeDescriptor
   {
      private DomainDataDirectory storeDomainDataDirectory;

      /// <summary>
      ///    Returns the property descriptors for the described ModelAttribute domain class, adding tracking property
      ///    descriptor(s).
      /// </summary>
      private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
      {
         ModelAttribute modelAttribute = ModelElement as ModelAttribute;

         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (modelAttribute != null)
         {
            storeDomainDataDirectory = modelAttribute.Store.DomainDataDirectory;

            EFCoreValidator.RemoveHiddenProperties(propertyDescriptors, modelAttribute);

            // dono't display IdentityType unless the IsIdentity is true
            if (!modelAttribute.IsIdentity)
            {
               PropertyDescriptor identityTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "IdentityType");
               propertyDescriptors.Remove(identityTypeDescriptor);
            }

            /********************************************************************************/

            DomainPropertyInfo columnNamePropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelAttribute.ColumnNameDomainPropertyId);
            DomainPropertyInfo isColumnNameTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsColumnNameTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] columnNameAttributes =
            {
               new DisplayNameAttribute("Column Name"),
               new DescriptionAttribute("Overrides default column name"),
               new CategoryAttribute("Database")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute, columnNamePropertyInfo, isColumnNameTrackingPropertyInfo, columnNameAttributes));

         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}
