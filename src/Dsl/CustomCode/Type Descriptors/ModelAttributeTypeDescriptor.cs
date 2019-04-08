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
         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (ModelElement is ModelAttribute modelAttribute)
         {
            storeDomainDataDirectory = modelAttribute.Store.DomainDataDirectory;

            EFCoreValidator.RemoveHiddenProperties(propertyDescriptors, modelAttribute);

            // don't display IdentityType unless the IsIdentity is true
            if (!modelAttribute.IsIdentity)
            {
               PropertyDescriptor identityTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "IdentityType");
               propertyDescriptors.Remove(identityTypeDescriptor);
            }

            // ImplementNotify implicitly defines autoproperty as false, so we don't display it
            if (modelAttribute.ModelClass.ImplementNotify)
            {
               PropertyDescriptor autoPropertyTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "AutoProperty");
               propertyDescriptors.Remove(autoPropertyTypeDescriptor);
            }

            /********************************************************************************/

            // don't display String property modifiers unless the type is "String"
            if (modelAttribute.Type != "String")
            {
               PropertyDescriptor minLengthTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "MinLength");
               propertyDescriptors.Remove(minLengthTypeDescriptor);

               PropertyDescriptor maxLengthTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "MaxLength");
               propertyDescriptors.Remove(maxLengthTypeDescriptor);

               PropertyDescriptor stringTypeTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "StringType");
               propertyDescriptors.Remove(stringTypeTypeDescriptor);
            }

            /********************************************************************************/

            // don't display IndexedUnique unless the Indexed is true
            if (!modelAttribute.Indexed)
            {
               PropertyDescriptor indexedUniqueTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "IndexedUnique");
               propertyDescriptors.Remove(indexedUniqueTypeDescriptor);
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
