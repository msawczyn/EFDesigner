using System;
using System.ComponentModel;

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
            /********************************************************************************/

            DomainPropertyInfo databaseSchemaPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.DatabaseSchemaDomainPropertyId);
            DomainPropertyInfo isDatabaseSchemaTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.IsDatabaseSchemaTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] databaseSchemaAttributes =
            {
               new DisplayNameAttribute("Database Schema"),
               new DescriptionAttribute("The schema to use for table creation. Overrides default schema for model if present."),
               new CategoryAttribute("Database")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass, databaseSchemaPropertyInfo, isDatabaseSchemaTrackingPropertyInfo, databaseSchemaAttributes));

            /********************************************************************************/

            DomainPropertyInfo namespacePropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.NamespaceDomainPropertyId);
            DomainPropertyInfo isNamespaceTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.IsNamespaceTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] namespaceAttributes =
            {
               new DisplayNameAttribute("Namespace"),
               new DescriptionAttribute("Overrides default namespace"),
               new CategoryAttribute("Code Generation")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass, namespacePropertyInfo, isNamespaceTrackingPropertyInfo, namespaceAttributes));

            /********************************************************************************/

            DomainPropertyInfo outputDirectoryPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.OutputDirectoryDomainPropertyId);
            DomainPropertyInfo isOutputDirectoryTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(ModelClass.IsOutputDirectoryTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] outputDirectoryAttributes =
            {
               new DisplayNameAttribute("Output Directory"),
               new DescriptionAttribute("Overrides default output directory"),
               new CategoryAttribute("Code Generation"),
               new TypeConverterAttribute(typeof(ProjectDirectoryTypeConverter))
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelClass, outputDirectoryPropertyInfo, isOutputDirectoryTrackingPropertyInfo, outputDirectoryAttributes));
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}
