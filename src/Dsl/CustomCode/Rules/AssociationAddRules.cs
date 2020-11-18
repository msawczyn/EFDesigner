using Microsoft.VisualStudio.Modeling;

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

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (e.ModelElement is UnidirectionalAssociation u)
            ConfigureNewAssociation(u);
         else if (e.ModelElement is BidirectionalAssociation b)
            ConfigureNewAssociation(b);

         AssociationChangedRules.SetEndpointRoles(element);
         PresentationHelper.UpdateAssociationDisplay(element);
      }

      private void ConfigureNewAssociation(BidirectionalAssociation element)
      {
         SetInitialMultiplicity(element);
         SetTargetPropertyName(element);
         SetSourcePropertyName(element);
      }

      private void ConfigureNewAssociation(UnidirectionalAssociation element)
      {
         SetInitialMultiplicity(element);
         SetTargetPropertyName(element);
      }

      private void SetInitialMultiplicity(BidirectionalAssociation element)
      {
         // valid bidirectional associations:
         // EF6 - entity to entity
         // EFCore - entity to entity, entity to dependent,  dependent to entity
         // EFCore5Plus - entity to entity, entity to dependent, dependent to entity

         ModelRoot modelRoot = element.Source.ModelRoot;
         EFVersion entityFrameworkVersion = modelRoot.EntityFrameworkVersion;

         if (entityFrameworkVersion == EFVersion.EF6)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }

            if (element.Source.IsEntity() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
            }

            if (element.Source.IsDependent() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
            }
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && modelRoot.IsEFCore5Plus)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }

            if (element.Source.IsEntity() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
            }

            if (element.Source.IsDependent() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
            }
         }
      }

      private void SetInitialMultiplicity(UnidirectionalAssociation element)
      {
         // valid unidirectional associations:
         // EF6 - entity to entity, entity to dependent
         // EFCore - entity to entity, entity to dependent
         // EFCore5Plus - entity to entity, entity to dependent, dependent to dependent, keyless to entity

         ModelRoot modelRoot = element.Source.ModelRoot;
         EFVersion entityFrameworkVersion = modelRoot.EntityFrameworkVersion;

         if (entityFrameworkVersion == EFVersion.EF6)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }

            if (element.Source.IsEntity() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.One;
            }
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }

            if (element.Source.IsEntity() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroOne;
            }
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && modelRoot.IsEFCore5Plus)
         {
            if (element.Source.IsEntity() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroMany;
            }

            if (element.Source.IsEntity() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroOne;
            }

            if (element.Source.IsDependent() && element.Target.IsDependent())
            {
               element.SourceMultiplicity = Multiplicity.One;
               element.TargetMultiplicity = Multiplicity.ZeroOne;
            }

            if (element.Source.IsKeyless() && element.Target.IsEntity())
            {
               element.SourceMultiplicity = Multiplicity.ZeroMany;
               element.TargetMultiplicity = Multiplicity.One;
            }
         }
      }

      private void SetSourcePropertyName(BidirectionalAssociation element)
      {
         if (string.IsNullOrEmpty(element.SourcePropertyName))
         {
            string rootName = element.SourceMultiplicity == Multiplicity.ZeroMany && ModelRoot.PluralizationService?.IsSingular(element.Source.Name) == true
                                 ? ModelRoot.PluralizationService.Pluralize(element.Source.Name)
                                 : element.Source.Name;

            string identifierName = rootName;
            int index = 0;

            while (element.Target.HasPropertyNamed(identifierName))
               identifierName = $"{rootName}_{++index}";

            element.SourcePropertyName = identifierName;
         }
      }

      private void SetTargetPropertyName(Association element)
      {
         if (string.IsNullOrEmpty(element.TargetPropertyName))
         {
            string rootName = element.TargetMultiplicity == Multiplicity.ZeroMany && ModelRoot.PluralizationService?.IsSingular(element.Target.Name) == true
                                 ? ModelRoot.PluralizationService.Pluralize(element.Target.Name)
                                 : element.Target.Name;

            string identifierName = rootName;
            int index = 0;

            while (element.Source.HasPropertyNamed(identifierName))
               identifierName = $"{rootName}_{++index}";

            element.TargetPropertyName = identifierName;
         }
      }
   }
}