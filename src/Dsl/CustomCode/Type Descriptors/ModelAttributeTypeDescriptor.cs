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

            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, modelAttribute);

            // if parent ModelClass is not persistent and has no persistent children, the following aren't relevant
            if (!modelAttribute.ModelClass.IsPersistent && !modelAttribute.ModelClass.HasPersistentChildren)
            {
               string[] propertyNames = { "Persistent", "Indexed", "IndexedUnique", "ColumnName", "ColumnType", "PersistencePoint", "IsIdentity", "IdentityType" };

               foreach (PropertyDescriptor propertyDescriptor in propertyNames.Select(propertyName => propertyDescriptors.OfType<PropertyDescriptor>()
                                                                                                                         .SingleOrDefault(x => x.Name == propertyName))
                                                                              .Where(descriptor => descriptor != null))
                  propertyDescriptors.Remove(propertyDescriptor);
            }

            // No sense asking for initial values if we won't use them
            if (!modelAttribute.SupportsInitialValue)
            {
               PropertyDescriptor initialValuePropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "InitialValue");

               if (initialValuePropertyDescriptor != null)
                  propertyDescriptors.Remove(initialValuePropertyDescriptor);
            }

            // don't display IdentityType unless the IsIdentity is true
            if (!modelAttribute.IsIdentity)
            {
               PropertyDescriptor identityPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "IdentityType");

               if (identityPropertyDescriptor != null)
                  propertyDescriptors.Remove(identityPropertyDescriptor);
            }

            // ImplementNotify implicitly defines autoproperty as false, so we don't display it
            if (modelAttribute.ModelClass.ImplementNotify)
            {
               PropertyDescriptor autoPropertyPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "AutoProperty");

               if (autoPropertyPropertyDescriptor != null)
                  propertyDescriptors.Remove(autoPropertyPropertyDescriptor);
            }

            // don't need a persistence point type if it's not persistent
            if (!modelAttribute.Persistent)
            {
               PropertyDescriptor persistencePointPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "PersistencePoint");

               if (persistencePointPropertyDescriptor != null)
                  propertyDescriptors.Remove(persistencePointPropertyDescriptor);
            }

            /********************************************************************************/

            // don't display String property modifiers unless the type is "String"
            if (modelAttribute.Type != "String")
            {
               PropertyDescriptor minLengthPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "MinLength");

               if (minLengthPropertyDescriptor != null)
                  propertyDescriptors.Remove(minLengthPropertyDescriptor);

               PropertyDescriptor maxLengthPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "MaxLength");

               if (maxLengthPropertyDescriptor != null)
                  propertyDescriptors.Remove(maxLengthPropertyDescriptor);

               PropertyDescriptor stringTypePropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "StringType");

               if (stringTypePropertyDescriptor != null)
                  propertyDescriptors.Remove(stringTypePropertyDescriptor);
            }

            /********************************************************************************/

            // don't display IndexedUnique unless the Indexed is true
            if (!modelAttribute.Indexed)
            {
               PropertyDescriptor indexedUniquePropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "IndexedUnique");

               if (indexedUniquePropertyDescriptor != null)
                  propertyDescriptors.Remove(indexedUniquePropertyDescriptor);
            }

            /********************************************************************************/

            //Add the descriptors for the tracking properties 

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.ColumnNameDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsColumnNameTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Column Name")
                                                                    , new DescriptionAttribute("Overrides default column name")
                                                                    , new CategoryAttribute("Database")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.ColumnTypeDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsColumnTypeTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Column Type")
                                                                    , new DescriptionAttribute("Overrides default column type")
                                                                    , new CategoryAttribute("Database")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.AutoPropertyDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsAutoPropertyTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("AutoProperty")
                                                                    , new DescriptionAttribute("Overrides default autoproperty setting")
                                                                    , new CategoryAttribute("Code Generation")
                                                                   }));

            propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.ImplementNotifyDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsImplementNotifyTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Implement INotifyPropertyChanged")
                                                                    , new DescriptionAttribute("Should this attribute implement INotifyPropertyChanged?")
                                                                    , new CategoryAttribute("Code Generation")
                                                                   }));
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}