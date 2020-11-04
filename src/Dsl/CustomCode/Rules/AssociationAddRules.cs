using System.Data.Entity.Design.PluralizationServices;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(UnidirectionalAssociation), FireTime = TimeToFire.Inline)]
   [RuleOn(typeof(BidirectionalAssociation), FireTime = TimeToFire.Inline)]
   internal class AssociationAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         Association element = (Association)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;
         PluralizationService pluralizationService = ModelRoot.PluralizationService;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         // add unidirectional
         //    if not EFCore5+, source can't be dependent (connection builder handles this)
         // if target is dependent,
         //    source cardinality is 1
         //    target cardinality is 1 
         //    source is principal
         if (store.ModelRoot().IsEFCore5Plus)
         {
            if (element is UnidirectionalAssociation)
            {
               if (element.Target.IsDependentType)
               {
                  if (element.Target.AllIdentityAttributes.Any())
                  {
                     element.SourceMultiplicity = Multiplicity.One;
                     element.TargetMultiplicity = Multiplicity.ZeroMany;
                  }
                  else
                  {
                     element.SourceMultiplicity = Multiplicity.One;
                     element.TargetMultiplicity = Multiplicity.One;
                  }

                  element.SourceRole = EndpointRole.Principal;
                  element.TargetRole = EndpointRole.Dependent;
               }
            }
            else if (element is BidirectionalAssociation)
            {

            }
         }
         else if (store.ModelRoot().EntityFrameworkVersion == EFVersion.EF6)
         {
            if (element is UnidirectionalAssociation)
            {
               if (element.Target.IsDependentType)
               {
                  if (element.Target.AllIdentityAttributes.Any())
                  {
                     element.SourceMultiplicity = Multiplicity.One;
                     element.TargetMultiplicity = Multiplicity.ZeroOne;
                  }
                  else
                  {
                     element.SourceMultiplicity = Multiplicity.One;
                     element.TargetMultiplicity = Multiplicity.One;
                  }

                  element.SourceRole = EndpointRole.Principal;
                  element.TargetRole = EndpointRole.Dependent;
               }
            }
            else if (element is BidirectionalAssociation)
            {

            }
         }
         else
         {
               if (element is UnidirectionalAssociation)
               {

               }
               else if (element is BidirectionalAssociation)
               {

               }
         }

         if (element is UnidirectionalAssociation)
         {
            if (element.Target.IsDependentType)
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
               element.SourceRole = EndpointRole.NotSet;
               element.TargetRole = EndpointRole.NotSet;
            }
         }

         // add bidirectional
         //    neither can be dependent (connection builder handles this)

         if (string.IsNullOrEmpty(element.TargetPropertyName))
         {
            string rootName = element.TargetMultiplicity == Multiplicity.ZeroMany && pluralizationService?.IsSingular(element.Target.Name) == true
                                 ? pluralizationService.Pluralize(element.Target.Name)
                                 : element.Target.Name;

            string identifierName = rootName;
            int index = 0;

            while (element.Source.HasPropertyNamed(identifierName))
               identifierName = $"{rootName}_{++index}";

            element.TargetPropertyName = identifierName;
         }

         if (element is BidirectionalAssociation bidirectionalAssociation)
         {
            if (string.IsNullOrEmpty(bidirectionalAssociation.SourcePropertyName))
            {
               string rootName = element.SourceMultiplicity == Multiplicity.ZeroMany && pluralizationService?.IsSingular(element.Source.Name) == true
                                    ? pluralizationService.Pluralize(element.Source.Name)
                                    : element.Source.Name;

               string identifierName = rootName;
               int index = 0;

               while (element.Target.HasPropertyNamed(identifierName))
                  identifierName = $"{rootName}_{++index}";

               bidirectionalAssociation.SourcePropertyName = identifierName;
            }
         }

         AssociationChangedRules.SetEndpointRoles(element);
         PresentationHelper.UpdateAssociationDisplay(element);
      }
   }
}