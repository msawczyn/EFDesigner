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
            ModelRoot modelRoot = association.Source.ModelRoot;
            storeDomainDataDirectory = association.Store.DomainDataDirectory;
            BidirectionalAssociation bidirectionalAssociation = association as BidirectionalAssociation;
            
            // show FKPropertyName only when possible and required
            if (!modelRoot.ExposeForeignKeys
             || (association.SourceRole != EndpointRole.Dependent && association.TargetRole != EndpointRole.Dependent))
               propertyDescriptors.Remove("FKPropertyName");

            // EF6 can't have declared foreign keys for 1..1 / 0-1..1 / 1..0-1 / 0-1..0-1 relationships
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6
             && association.SourceMultiplicity != Multiplicity.ZeroMany
             && association.TargetMultiplicity != Multiplicity.ZeroMany)
               propertyDescriptors.Remove("FKPropertyName");

            // no FKs for aggregates
            if (association.Source.IsDependentType || association.Target.IsDependentType)
               propertyDescriptors.Remove("FKPropertyName");

            // only display roles for 1..1 and 0-1..0-1 associations
            if ((association.SourceMultiplicity != Multiplicity.One || association.TargetMultiplicity != Multiplicity.One)
             && (association.SourceMultiplicity != Multiplicity.ZeroOne || association.TargetMultiplicity != Multiplicity.ZeroOne))
            {
               propertyDescriptors.Remove("SourceRole");
               propertyDescriptors.Remove("TargetRole");
            }

            // only display delete behavior on the principal end
            // except that owned types don't have deletiion behavior choices
            if (association.SourceRole != EndpointRole.Principal || association.Source.IsDependentType || association.Target.IsDependentType)
               propertyDescriptors.Remove("SourceDeleteAction");

            if (association.TargetRole != EndpointRole.Principal || association.Source.IsDependentType || association.Target.IsDependentType)
               propertyDescriptors.Remove("TargetDeleteAction");

            // only show JoinTableName if is *..* association
            if (association.SourceMultiplicity != Multiplicity.ZeroMany || association.TargetMultiplicity != Multiplicity.ZeroMany)
               propertyDescriptors.Remove("JoinTableName");

            // implementNotify implicitly defines autoproperty as false, so we don't display it
            if (association.TargetImplementNotify)
               propertyDescriptors.Remove("TargetAutoProperty");
            if (bidirectionalAssociation != null && bidirectionalAssociation.SourceImplementNotify)
               propertyDescriptors.Remove("SourceAutoProperty");

            // we're only allowing ..1 and ..0-1 associations to have backing fields
            if (association.TargetMultiplicity == Multiplicity.ZeroMany)
               propertyDescriptors.Remove("TargetAutoProperty");
            if (bidirectionalAssociation != null && bidirectionalAssociation.SourceMultiplicity == Multiplicity.ZeroMany)
               propertyDescriptors.Remove("SourceAutoProperty");

            // EF6 doesn't support property access modes
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
            {
               propertyDescriptors.Remove("TargetPropertyAccessMode");
               propertyDescriptors.Remove("SourcePropertyAccessMode");
            }

            // only show backing field name and property access mode if not an autoproperty
            if (association.TargetAutoProperty)
            {
               propertyDescriptors.Remove("TargetBackingFieldName");
               propertyDescriptors.Remove("TargetPropertyAccessMode");
            }

            if (bidirectionalAssociation == null || bidirectionalAssociation.SourceAutoProperty)
            {
               propertyDescriptors.Remove("SourceBackingFieldName");
               propertyDescriptors.Remove("SourcePropertyAccessMode");
            }

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

               propertyDescriptors.Add(new TrackingPropertyDescriptor(association
                                                                    , storeDomainDataDirectory.GetDomainProperty(Association.TargetAutoPropertyDomainPropertyId)
                                                                    , storeDomainDataDirectory.GetDomainProperty(Association.IsTargetAutoPropertyTrackingDomainPropertyId)
                                                                    , new Attribute[]
                                                                      {
                                                                         new DisplayNameAttribute("End1 Is Auto Property")
                                                                       , new DescriptionAttribute("If false, generates a backing field and a partial method to hook getting and setting the property. "
                                                                                                + "If true, generates a simple auto property. Only valid for non-collection properties.")
                                                                       , new CategoryAttribute("End 2")
                                                                      }));
            }

            if (bidirectionalAssociation?.SourceMultiplicity == Multiplicity.One || bidirectionalAssociation?.SourceMultiplicity == Multiplicity.ZeroOne)
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

               propertyDescriptors.Add(new TrackingPropertyDescriptor(association
                                                                    , storeDomainDataDirectory.GetDomainProperty(BidirectionalAssociation.SourceAutoPropertyDomainPropertyId)
                                                                    , storeDomainDataDirectory.GetDomainProperty(BidirectionalAssociation.IsSourceAutoPropertyTrackingDomainPropertyId)
                                                                    , new Attribute[]
                                                                      {
                                                                         new DisplayNameAttribute("End2 Is Auto Property")
                                                                       , new DescriptionAttribute("If false, generates a backing field and a partial method to hook getting and setting the property. "
                                                                                                + "If true, generates a simple auto property. Only valid for non-collection properties.")
                                                                       , new CategoryAttribute("End 1")
                                                                      }));
            }

         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }


   }
}
