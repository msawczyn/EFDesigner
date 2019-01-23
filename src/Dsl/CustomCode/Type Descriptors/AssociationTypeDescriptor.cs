using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class AssociationTypeDescriptor
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

         //Add the descriptor for the tracking property.  
         if (ModelElement is Association association)
         {
            storeDomainDataDirectory = association.Store.DomainDataDirectory;

            /********************************************************************************/
            /********************************************************************************/

            // only display roles for 1..1 and 0-1..0-1 associations
            if (((association.SourceMultiplicity != Multiplicity.One || association.TargetMultiplicity != Multiplicity.One) &&
                 (association.SourceMultiplicity != Multiplicity.ZeroOne || association.TargetMultiplicity != Multiplicity.ZeroOne)))
            {
               PropertyDescriptor sourceRoleTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "SourceRole");
               if (sourceRoleTypeDescriptor != null) propertyDescriptors.Remove(sourceRoleTypeDescriptor);

               PropertyDescriptor targetRoleTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "TargetRole");
               if (targetRoleTypeDescriptor != null) propertyDescriptors.Remove(targetRoleTypeDescriptor);
            }

            /********************************************************************************/

            // only display delete behavior on the principal end
            if (association.SourceRole != EndpointRole.Principal)
            {
               PropertyDescriptor sourceDeleteActionTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "SourceDeleteAction");
               if (sourceDeleteActionTypeDescriptor != null) propertyDescriptors.Remove(sourceDeleteActionTypeDescriptor);
            }

            if (association.TargetRole != EndpointRole.Principal)
            {
               PropertyDescriptor targetDeleteActionTypeDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().Single(x => x.Name == "TargetDeleteAction");
               if (targetDeleteActionTypeDescriptor != null) propertyDescriptors.Remove(targetDeleteActionTypeDescriptor);
            }

            /********************************************************************************/
            /********************************************************************************/

            DomainPropertyInfo collectionClassPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Association.CollectionClassDomainPropertyId);
            DomainPropertyInfo isCollectionClassTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Association.IsCollectionClassTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] collectionClassAttributes =
            {
               new DisplayNameAttribute("Collection Class"),
               new DescriptionAttribute("Type of collections generated. Overrides the default collection class for the model"),
               new CategoryAttribute("Code Generation")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(association, collectionClassPropertyInfo, isCollectionClassTrackingPropertyInfo, collectionClassAttributes));
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }

   }
}
