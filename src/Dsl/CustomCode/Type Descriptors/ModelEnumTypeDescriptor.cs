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
         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         //Add the descriptor for the tracking property.  
         if (ModelElement is ModelEnum modelEnum)
         {
            storeDomainDataDirectory = modelEnum.Store.DomainDataDirectory;

            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, modelEnum);

            //Add the descriptors for the tracking properties 

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelEnum
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelEnum.NamespaceDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelEnum.IsNamespaceTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Namespace")
                                                                    , new DescriptionAttribute("Overrides default namespace")
                                                                    , new CategoryAttribute("Code Generation")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelEnum
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelEnum.OutputDirectoryDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelEnum.IsOutputDirectoryTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Output Directory")
                                                                    , new DescriptionAttribute("Overrides default output directory")
                                                                    , new CategoryAttribute("Code Generation")
                                                                    , new TypeConverterAttribute(typeof(ProjectDirectoryTypeConverter))
                                                                   }));
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}
