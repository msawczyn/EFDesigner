﻿using System;
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
            ModelRoot modelRoot = modelAttribute.ModelClass.ModelRoot;

            // keyless classes don't get identities
            if (modelAttribute.ModelClass.IsKeylessType())
            {
               propertyDescriptors.Remove("IsIdentity");
               propertyDescriptors.Remove("IdentityType");
            }

            // No sense asking for initial values if we won't use them
            if (!modelAttribute.SupportsInitialValue)
               propertyDescriptors.Remove("InitialValue");

            // don't display IdentityType if IsIdentity is false
            if (!modelAttribute.IsIdentity)
               propertyDescriptors.Remove("IdentityType");

            // if IsIdentity is true, since we don't encourage changing identity properties (!), don't show the implement notify switch 
            if (modelAttribute.IsIdentity)
               propertyDescriptors.Remove("ImplementNotify");

            // don't display SetterVisibility if IsIdentity is true and automatically generated
            if (modelAttribute.IsIdentity && modelAttribute.IdentityType == IdentityType.AutoGenerated)
               propertyDescriptors.Remove("SetterVisibility");

            // ImplementNotify implicitly defines autoproperty as false, so we don't display it
            if (modelAttribute.ImplementNotify)
               propertyDescriptors.Remove("AutoProperty");

            if (modelAttribute.Type != "String")
            {
               // don't display String property modifiers unless the type is "String"
               propertyDescriptors.Remove("MinLength");
               propertyDescriptors.Remove("MaxLength");
               propertyDescriptors.Remove("StringType");
            }

            // don't display IndexedUnique unless the Indexed is true
            if (!modelAttribute.Indexed)
               propertyDescriptors.Remove("IndexedUnique");

            // EF6 doesn't support various attribute features
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
            {
               propertyDescriptors.Remove("PropertyAccessMode");
            }

            // don't display BackingField or PropertyAccessMode unless AutoProperty is false
            if (modelAttribute.AutoProperty)
            {
               propertyDescriptors.Remove("BackingFieldName");
               propertyDescriptors.Remove("PropertyAccessMode");
            }

            // only abstract classes can have abstract properties
            if (!modelAttribute.ModelClass.IsAbstract)
               propertyDescriptors.Remove("IsAbstract");

            // abstract properties don't get lots of stuff
            if (modelAttribute.IsAbstract)
            {
               propertyDescriptors.Remove("IsIdentity");
               propertyDescriptors.Remove("IdentityType");
               propertyDescriptors.Remove("AutoProperty");
               propertyDescriptors.Remove("InitialValue");
               propertyDescriptors.Remove("BackingFieldName");
               propertyDescriptors.Remove("PropertyAccessMode");
               propertyDescriptors.Remove("Indexed");
               propertyDescriptors.Remove("IndexedUnique");
            }

            // Only transient properties can be made ReadOnly (i.e., have no setter)
            if (modelAttribute.Persistent)
               propertyDescriptors.Remove("ReadOnly");

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

            if (modelRoot.IsEFCore5Plus)
            {
               if (modelAttribute.Type == "String")
               {
                  propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                       , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.DatabaseCollationDomainPropertyId)
                                                                       , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsDatabaseCollationTrackingDomainPropertyId)
                                                                       , new Attribute[]
                                                                         {
                                                                            new DisplayNameAttribute("Database Collation")
                                                                          , new DescriptionAttribute("Overrides the default database collation setting for the column that persists this attribute")
                                                                          , new CategoryAttribute("Database")
                                                                         }));
               }

               propertyDescriptors.Add(new TrackingPropertyDescriptor(modelAttribute
                                                                    , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.PropertyAccessModeDomainPropertyId)
                                                                    , storeDomainDataDirectory.GetDomainProperty(ModelAttribute.IsPropertyAccessModeTrackingDomainPropertyId)
                                                                    , new Attribute[]
                                                                      {
                                                                         new DisplayNameAttribute("Property Access Mode")
                                                                       , new DescriptionAttribute("Defines how EF reads and write this property or its backing field. See  https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.propertyaccessmode")
                                                                       , new CategoryAttribute("Code Generation")
                                                                      }));
            }
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }
   }
}