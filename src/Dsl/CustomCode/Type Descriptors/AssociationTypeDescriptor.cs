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
      ///    Returns the property descriptors for the described Association domain class, adding tracking property
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

            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, association);

            // only display roles for 1..1 and 0-1..0-1 associations
            if ((association.SourceMultiplicity != Multiplicity.One || association.TargetMultiplicity != Multiplicity.One)
             && (association.SourceMultiplicity != Multiplicity.ZeroOne || association.TargetMultiplicity != Multiplicity.ZeroOne))
            {
               propertyDescriptors.Remove("SourceRole");
               propertyDescriptors.Remove("TargetRole");
            }

            // only display delete behavior on the principal end
            if (association.SourceRole != EndpointRole.Principal)
               propertyDescriptors.Remove("SourceDeleteAction");

            if (association.TargetRole != EndpointRole.Principal)
               propertyDescriptors.Remove("TargetDeleteAction");

            /********************************************************************************/

            //Add the descriptors for the tracking properties 

            propertyDescriptors.Add(new TrackingPropertyDescriptor(association
                                                                 , storeDomainDataDirectory.GetDomainProperty(Association.CollectionClassDomainPropertyId)
                                                                 , storeDomainDataDirectory.GetDomainProperty(Association.IsCollectionClassTrackingDomainPropertyId)
                                                                 , new Attribute[]
                                                                   {
                                                                      new DisplayNameAttribute("Collection Class")
                                                                    , new DescriptionAttribute("Type of collections generated. Overrides the default collection class for the model")
                                                                    , new CategoryAttribute("Code Generation")
                                                                   }));

            if (association.TargetMultiplicity == Multiplicity.One || association.TargetMultiplicity == Multiplicity.ZeroOne)
            {
               propertyDescriptors.Add(new TrackingPropertyDescriptor(association
                                                                    , storeDomainDataDirectory.GetDomainProperty(Association.TargetImplementNotifyDomainPropertyId)
                                                                    , storeDomainDataDirectory.GetDomainProperty(Association.IsTargetImplementNotifyTrackingDomainPropertyId)
                                                                    , new Attribute[]
                                                                      {
                                                                         new DisplayNameAttribute("Implement INotifyPropertyChanged")
                                                                       , new DescriptionAttribute("Should this end participate in INotifyPropertyChanged activities? "
                                                                                                + "Only valid for non-collection targets.")
                                                                       , new CategoryAttribute("End 2")
                                                                      }));
            }

            if (association is BidirectionalAssociation bidirectionalAssociation && (bidirectionalAssociation.SourceMultiplicity == Multiplicity.One || 
                                                                                     bidirectionalAssociation.SourceMultiplicity == Multiplicity.ZeroOne))
            {
               propertyDescriptors.Add(new TrackingPropertyDescriptor(bidirectionalAssociation
                                                                    , storeDomainDataDirectory.GetDomainProperty(BidirectionalAssociation.SourceImplementNotifyDomainPropertyId)
                                                                    , storeDomainDataDirectory.GetDomainProperty(BidirectionalAssociation.IsSourceImplementNotifyTrackingDomainPropertyId)
                                                                    , new Attribute[]
                                                                      {
                                                                         new DisplayNameAttribute("Implement INotifyPropertyChanged")
                                                                       , new DescriptionAttribute("Should this end participate in INotifyPropertyChanged activities? "
                                                                                                + "Only valid for non-collection targets.")
                                                                       , new CategoryAttribute("End 1")
                                                                      }));
            }

         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }


   }
}
