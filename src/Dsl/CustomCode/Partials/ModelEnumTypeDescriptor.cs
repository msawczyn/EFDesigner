using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelEnumTypeDescriptor
   {
      private DomainDataDirectory storeDomainDataDirectory;

      /// <summary>
      ///    Returns the property descriptors for the described ModelEnum domain class, adding tracking property
      ///    descriptor(s).
      /// </summary>
      private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
      {
         // Get a reference to the model element that is being described.  
         ModelEnum modelEnum = ModelElement as ModelEnum;

         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         //Add the descriptor for the tracking property.  
         if (modelEnum != null)
         {
            storeDomainDataDirectory = modelEnum.Store.DomainDataDirectory;

            /********************************************************************************/

            DomainPropertyInfo namespacePropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelEnum.NamespaceDomainPropertyId);
            DomainPropertyInfo isNamespaceTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelEnum.IsNamespaceTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] namespaceAttributes =
            {
               new DisplayNameAttribute("Namespace"),
               new DescriptionAttribute("Overrides default namespace"),
               new CategoryAttribute("Code Generation")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelEnum, namespacePropertyInfo, isNamespaceTrackingPropertyInfo, namespaceAttributes));
          
            /********************************************************************************/

            DomainPropertyInfo outputDirectoryPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.OutputDirectoryDomainPropertyId);
            DomainPropertyInfo isOutputDirectoryTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.IsOutputDirectoryTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] outputDirectoryAttributes =
            {
               new DisplayNameAttribute("Output Directory"),
               new DescriptionAttribute("Overrides default output directory"),
               new CategoryAttribute("Code Generation")
            };

            TrackingPropertyDescriptor outputDirectoryTypeDescriptor = new TrackingPropertyDescriptor(modelEnum, outputDirectoryPropertyInfo, isOutputDirectoryTrackingPropertyInfo, outputDirectoryAttributes);
            propertyDescriptors.Add(outputDirectoryTypeDescriptor);
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}
