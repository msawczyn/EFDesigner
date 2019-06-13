using System;
using System.Data.Entity.Design.PluralizationServices;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Association), FireTime = TimeToFire.LocalCommit)]
   internal class AssociationAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         Association element = (Association)e.ModelElement;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;
         PluralizationService pluralizationService = ModelRoot.PluralizationService;

         if (current.IsSerializing)
            return;

         IdentityHelper identityHelper = new IdentityHelper(store.ModelRoot());
         identityHelper.FixupIdentityAssociations();

         // add unidirectional
         //    source can't be dependent (connection builder handles this)
         // if target is dependent,
         //    source cardinality is 0..1
         //    target cardinality is 0..1 
         //    source is principal
         if (element is UnidirectionalAssociation && element.Target.IsDependentType)
         {
            element.TargetMultiplicity = Multiplicity.ZeroOne;
            element.SourceRole = EndpointRole.Principal;
            element.TargetRole = EndpointRole.Dependent;
         }

         // add bidirectional
         //    neither can be dependent (connection builder handles this)

         if (string.IsNullOrEmpty(element.TargetPropertyName))
         {
            string rootName = element.TargetMultiplicity == Multiplicity.ZeroMany &&
                              pluralizationService?.IsSingular(element.Target.Name) == true
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
               string rootName = element.SourceMultiplicity == Multiplicity.ZeroMany &&
                                 pluralizationService?.IsSingular(element.Source.Name) == true
                                    ? pluralizationService.Pluralize(element.Source.Name)
                                    : element.Source.Name;

               string identifierName = rootName;
               int index = 0;

               while (element.Target.HasPropertyNamed(identifierName))
                  identifierName = $"{rootName}_{++index}";

               bidirectionalAssociation.SourcePropertyName = identifierName;
            }
         }

         AssociationChangeRules.SetEndpointRoles(element);
      }
   }
}
