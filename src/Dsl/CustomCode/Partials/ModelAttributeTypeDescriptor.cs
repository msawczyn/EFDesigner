using System;
using System.ComponentModel;
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

         // Get a reference to the model element that is being described.  
         ModelAttribute modelAttribute = ModelElement as ModelAttribute;

         //Add the descriptor for the tracking property.  
         if (modelAttribute != null)
         {
            storeDomainDataDirectory = modelAttribute.Store.DomainDataDirectory;

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

            /********************************************************************************/

            if (modelAttribute.ModelClass.ModelRoot.EntityFrameworkVersion > EFVersion.EFCore21)
            {
               DomainPropertyInfo columnTypePropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelAttribute.ColumnTypeDomainPropertyId);
               DomainPropertyInfo isColumnTypeTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsColumnTypeTrackingDomainPropertyId);

               // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
               Attribute[] columnTypeAttributes =
               {
                  new DisplayNameAttribute("Column Type"),
                  new DescriptionAttribute("Overrides default column type"),
                  new CategoryAttribute("Database")
               };

               propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute, columnTypePropertyInfo, isColumnTypeTrackingPropertyInfo, columnTypeAttributes));
            }
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}
