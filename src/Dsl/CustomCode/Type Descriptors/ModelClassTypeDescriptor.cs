using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class ModelClassTypeDescriptor
   {
      private DomainDataDirectory storeDomainDataDirectory;

      /// <summary>
      ///    Returns the property descriptors for the described ModelClass domain class, adding tracking property
      ///    descriptor(s).
      /// </summary>
      private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
      {
         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         if (ModelElement is ModelClass modelClass)
         {
            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, modelClass);

            storeDomainDataDirectory = modelClass.Store.DomainDataDirectory;

            //Add the descriptors for the tracking properties 

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.DatabaseSchemaDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.IsDatabaseSchemaTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Database Schema")
                                                                    , new DescriptionAttribute("The schema to use for table creation. Overrides default schema for model if present.")
                                                                    , new CategoryAttribute("Database")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.NamespaceDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.IsNamespaceTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Namespace")
                                                                    , new DescriptionAttribute("Overrides default namespace")
                                                                    , new CategoryAttribute("Code Generation")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.OutputDirectoryDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelClass.IsOutputDirectoryTrackingDomainPropertyId)
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
