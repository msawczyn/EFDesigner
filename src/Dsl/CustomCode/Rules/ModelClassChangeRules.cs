using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelClass), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelClassChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelClass element = (ModelClass)e.ModelElement;
         ModelRoot modelRoot = element.ModelRoot;

         if (element.IsDeleted)
            return;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = new List<string>();

         switch (e.DomainProperty.Name)
         {
            case "BaseClass":
               {
                  if (!string.IsNullOrEmpty(element.BaseClass))
                  {
                     if (element.IsDependentType)
                     {
                        errorMessages.Add($"Can't give {element.Name} a base class since it's a dependent type");

                        break;
                     }

                     if (element.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't give {element.Name} a base class since it's an association class");

                        break;
                     }

                     if (element.Superclass.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't give {element.Name} that base class since {element.Superclass.Name} is an association class");

                        break;
                     }
                  }

                  break;
               }

            case "CustomInterfaces":
               {
                  if (modelRoot.ShowInterfaceIndicators)
                     PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "DbSetName":
               {
                  if (element.IsDependentType)
                  {
                     if (!string.IsNullOrEmpty(element.DbSetName))
                        element.DbSetName = string.Empty;
                  }
                  else
                  {
                     if (string.IsNullOrEmpty(element.DbSetName))
                        element.DbSetName = MakeDefaultTableAndSetName(element.Name);

                     if (current.Name.ToLowerInvariant() != "paste" &&
                         (string.IsNullOrWhiteSpace(element.DbSetName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(element.DbSetName)))
                     {
                        errorMessages.Add($"DbSet name '{element.DbSetName}' isn't a valid .NET identifier.");

                        break;
                     }
                     
                     if (store.GetAll<ModelClass>()
                                   .Except(new[] { element })
                                   .Any(x => x.DbSetName == element.DbSetName))
                     {
                        errorMessages.Add($"DbSet name '{element.DbSetName}' already in use");

                        break;
                     }
                  }

                  break;
               }

            case "ImplementNotify":
               {
                  if (element.ImplementNotify)
                  {
                     List<string> nameList = element.Attributes.Where(x => x.AutoProperty).Select(x => x.Name).ToList();
                     if (nameList.Any())
                     {
                        string names = nameList.Count > 1
                                          ? string.Join(", ", nameList.Take(nameList.Count - 1)) + " and " + nameList.Last()
                                          : nameList.First();

                        string verb = nameList.Count > 1
                                         ? "is an autoproperty"
                                         : "are autoproperties";

                        WarningDisplay.Show($"{names} {verb}, so will not participate in INotifyPropertyChanged messages");
                     }
                  }

                  PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "IsAbstract":
               {
                  if (element.IsAbstract)
                  {
                     if (element.IsDependentType)
                     {
                        errorMessages.Add($"Can't make {element.Name} abstract since it's a dependent type");

                        break;
                     }

                     if (element.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't make {element.Name} abstract since it's an association class");

                        break;
                     }
                  }

                  PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "IsAssociationClass":
               {
                  PresentationHelper.UpdateClassDisplay(element);
                  break;
               }

            case "IsDatabaseView":
               {
                  if (element.IsDatabaseView)
                  {
                     if (element.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't base {element.Name} off a view since it's an association class");

                        break;
                     }

                     if (element.IsDependentType)
                     {
                        errorMessages.Add($"Can't base {element.Name} off a view since it's a dependent type");

                        break;
                     }

                     if (element.IsQueryType)
                     {
                        errorMessages.Add($"Can't base {element.Name} off a view since it's a query type");

                        break;
                     }

                     if (string.IsNullOrEmpty(element.ViewName))
                        element.ViewName = element.TableName;

                     if (string.IsNullOrEmpty(element.ViewName))
                        element.ViewName = MakeDefaultTableAndSetName(element.Name);

                     //if (modelRoot.IsEFCore5Plus)
                     //   VerifyKeylessTypeEFCore5();
                     //else
                     //   VerifyKeylessType();
                  }
                  break;
               }

            case "IsDependentType":
               {
                  if (element.IsDependentType)
                  {
                     if (element.IsDatabaseView)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's backed by a database view");

                        break;
                     }

                     if (element.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's an association class");

                        break;
                     }

                     if (element.IsQueryType)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's a query type");

                        break;
                     }

                     if (element.BaseClass != null)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it has a base class");

                        break;
                     }

                     string subclasses = string.Join(", ", store.GetAll<Generalization>().Where(g => g.Superclass == element).Select(g => g.Subclass.Name));
                     if (!string.IsNullOrEmpty(subclasses))
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it has subclass(es) {subclasses}");

                        break;
                     }

                     if (element.IsAbstract)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's abstract");

                        break;
                     }

                     List<Association> principalAssociations = store.GetAll<Association>().Where(a => a.Principal == element).ToList();

                     if (principalAssociations.Any())
                     {
                        string badAssociations = string.Join(", ", principalAssociations.Select(a => a.GetDisplayText()));
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's the principal end in: {badAssociations}");

                        break;
                     }

                     List<UnidirectionalAssociation> entityTargets = store.GetAll<UnidirectionalAssociation>().Where(a => a.Source == element && !a.Target.IsDependentType).ToList();

                     if (entityTargets.Any())
                     {
                        string badAssociations = string.Join(", ", entityTargets.Select(a => a.GetDisplayText()));
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it has unidirectional associations to entities in: {badAssociations}");

                        break;
                     }

                     List<BidirectionalAssociation> bidirectionalAssociations = store.GetAll<BidirectionalAssociation>().Where(a => a.Source == element || a.Target == element).ToList();
                     if (bidirectionalAssociations.Any())
                     {
                        if (!modelRoot.IsEFCore5Plus)
                        {
                           string badAssociations = string.Join(", ", entityTargets.Select(a => a.GetDisplayText()));
                           errorMessages.Add($"Can't make {element.Name} a dependent class since it has bidirectional associations in: {badAssociations}");

                           break;
                        }

                        bidirectionalAssociations = bidirectionalAssociations.Where(a => (a.Source == element && a.TargetMultiplicity != Multiplicity.One)
                                                                                      || (a.Target == element && a.SourceMultiplicity != Multiplicity.One))
                                                                             .ToList();

                        if (bidirectionalAssociations.Any())
                        {
                           string badAssociations = string.Join(", ", entityTargets.Select(a => a.GetDisplayText()));
                           errorMessages.Add($"Can't make {element.Name} a dependent class since it has bidirectional associations without 1 or 0/1 ownership multiplicity in: {badAssociations}. The other end must be a single, required reference.");

                           break;
                        }

                     }

                     if (element.ModelRoot.EntityFrameworkVersion == EFVersion.EF6 || element.ModelRoot.GetEntityFrameworkPackageVersionNum() < 2.2)
                     {
                        if (store.GetAll<Association>().Any(a => a.Target == element && a.TargetMultiplicity == Multiplicity.ZeroMany))
                        {
                           errorMessages.Add($"Can't make {element.Name} a dependent class since it's the target of a 0..* association");

                           break;
                        }

                        foreach (UnidirectionalAssociation association in Association.GetLinksToTargets(element).OfType<UnidirectionalAssociation>())
                        {
                           if (association.SourceMultiplicity == Multiplicity.ZeroMany)
                              association.SourceMultiplicity = Multiplicity.ZeroOne;

                           if (association.TargetMultiplicity == Multiplicity.ZeroMany)
                              association.TargetMultiplicity = Multiplicity.ZeroOne;

                           association.TargetRole = EndpointRole.Dependent;
                        }

                        element.TableName = string.Empty;
                     }

                     foreach (ModelAttribute modelAttribute in element.AllAttributes.Where(a => a.IsIdentity))
                        modelAttribute.IsIdentity = false;

                     element.DbSetName = string.Empty;
                  }
                  else
                  {
                     element.DbSetName = MakeDefaultTableAndSetName(element.Name);
                     element.TableName = MakeDefaultTableAndSetName(element.Name);
                  }

                  // Remove any foreign keys in any incoming or outgoing associations
                  foreach (Association association in Association.GetLinksToTargets(element).Union(Association.GetLinksToSources(element)).Distinct())
                     association.FKPropertyName = null;

                  PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "IsPropertyBag":
               {
                  if (element.IsPropertyBag && element.IsAssociationClass)
                  {
                     errorMessages.Add($"Can't make {element.Name} a property bag since it's an associatioin class");

                     break;
                  }

                  if (element.Superclass != null && !element.Superclass.IsPropertyBag)
                     element.Superclass.IsPropertyBag = true;

                  if (element.Subclasses.Any())
                  {
                     foreach (ModelClass subclass in element.Subclasses)
                        subclass.IsPropertyBag = true;
                  }

                  PresentationHelper.UpdateClassDisplay(element);
                  break;
               }

            case "IsQueryType":
               {
                  if (element.IsQueryType)
                  {
                     if (element.IsAssociationClass)
                     {
                        errorMessages.Add($"Can't make {element.Name} a query type since it's an association class");
                        break;
                     }

                     if (element.IsDependentType)
                     {
                        errorMessages.Add($"Can't make {element.Name} a query type since it's a dependent class");
                        break;
                     }

                     if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
                        element.IsQueryType = false;
                     else if (modelRoot.IsEFCore5Plus)
                        VerifyKeylessTypeEFCore5();
                     else
                        VerifyKeylessType();
                  }

                  break;
               }

            case "Name":
               {
                  if (current.Name.ToLowerInvariant() == "paste")
                     return;

                  if (string.IsNullOrWhiteSpace(element.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(element.Name))
                     errorMessages.Add("Name must be a valid .NET identifier");
                  else if (store.GetAll<ModelClass>().Except(new[] { element }).Any(x => x.FullName == element.FullName))
                     errorMessages.Add($"Class name '{element.FullName}' already in use by another class");
                  else if (store.GetAll<ModelEnum>().Any(x => x.FullName == element.FullName))
                     errorMessages.Add($"Class name '{element.FullName}' already in use by an enum");
                  else if (!string.IsNullOrEmpty((string)e.OldValue))
                  {
                     string oldDefaultName = MakeDefaultTableAndSetName((string)e.OldValue);
                     string newDefaultName = MakeDefaultTableAndSetName(element.Name);

                     if (element.DbSetName == oldDefaultName)
                        element.DbSetName = newDefaultName;

                     if (element.TableName == oldDefaultName)
                        element.TableName = newDefaultName;
                  }

                  break;
               }

            case "Namespace":
               {
                  if (current.Name.ToLowerInvariant() != "paste")
                     errorMessages.Add(CommonRules.ValidateNamespace(element.Namespace, CodeGenerator.IsValidLanguageIndependentIdentifier));

                  break;
               }

            case "TableName":
               {
                  if (!element.IsDatabaseView)
                  {
                     string newTableName = (string)e.NewValue;

                     if (element.IsDependentType)
                     {
                        if (!modelRoot.IsEFCore5Plus && !string.IsNullOrEmpty(newTableName))
                           element.TableName = string.Empty;
                     }
                     else
                     {
                        if (string.IsNullOrEmpty(newTableName))
                           element.TableName = MakeDefaultTableAndSetName(element.Name);

                        if (store.GetAll<ModelClass>()
                                 .Except(new[] { element })
                                 .Any(x => x.TableName == newTableName))
                           errorMessages.Add($"Table name '{newTableName}' already in use");
                     }
                  }

                  break;
               }

            case "ViewName":
               {
                  if (element.IsDatabaseView)
                  {
                     string newViewName = (string)e.NewValue;

                     if (element.IsDependentType)
                     {
                        if (!modelRoot.IsEFCore5Plus && !string.IsNullOrEmpty(newViewName))
                           element.TableName = string.Empty;
                     }
                     else
                     {
                        if (string.IsNullOrEmpty(newViewName))
                           element.TableName = MakeDefaultTableAndSetName(element.Name);

                        List<ModelClass> classesUsingTableName = store.GetAll<ModelClass>()
                              .Except(new[] { element })
                                                             .Where(x => x.TableName == newViewName)
                                                             .ToList();

                        if (classesUsingTableName.Any())
                           errorMessages.Add($"View name '{newViewName}' already in use in {classesUsingTableName.First().Name}");
                     }
                  }

                  break;
               }
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(store, string.Join("\n", errorMessages));
         }

         void VerifyKeylessTypeEFCore5()
         {
            // TODO: Find definitive documentation on query type restrictions in EFCore5+
            // Restrictions:
            // =================================
            // Cannot have a key defined.
            List<string> allIdentityAttributeNames = element.AllIdentityAttributeNames.ToList();

            if (allIdentityAttributeNames.Any())
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has identity attribute(s) {string.Join(", ", allIdentityAttributeNames)}. Set their 'Is Identity' property to false first.");

            // Only support a subset of navigation mapping capabilities, specifically:
            //    - They may never act as the principal end of a relationship.
            string badAssociations = string.Join(", "
                                               , store.ElementDirectory.AllElements
                                                      .OfType<Association>()
                                                      .Where(a => a.Principal == element)
                                                      .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it is the principal end of association(s) {badAssociations}.");

            //    - They may not have navigations to owned entities 
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.Target.IsDependentType) || (a is BidirectionalAssociation b && b.Source == element && b.Target.IsDependentType) || (a is BidirectionalAssociation c && c.Target == element && c.Source.IsDependentType))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has association(s) to dependent type(s) in {badAssociations}.");

            //    - Entities cannot contain navigation properties to query types.
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.Target.IsQueryType) || (a is BidirectionalAssociation b && b.Source == element && b.Target.IsQueryType) || (a is BidirectionalAssociation c && c.Target == element && c.Source.IsQueryType))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has association to sql-mapped type(s) in {badAssociations}.");

            //    - They can only contain reference navigation properties pointing to regular entities.
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.TargetMultiplicity == Multiplicity.ZeroMany) || (a is BidirectionalAssociation b && b.Source == element && b.TargetMultiplicity == Multiplicity.ZeroMany) || (a is BidirectionalAssociation c && c.Target == element && c.SourceMultiplicity == Multiplicity.ZeroMany))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has zero-to-many association(s) in {badAssociations}. Only to-one or to-zero-or-one associations are allowed. ");
         }

         void VerifyKeylessType()
         {
            // Restrictions:
            // =================================
            // Cannot have a key defined.
            List<string> allIdentityAttributeNames = element.AllIdentityAttributeNames.ToList();

            if (allIdentityAttributeNames.Any())
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has identity attribute(s) {string.Join(", ", allIdentityAttributeNames)}. Set their 'Is Identity' property to false first.");

            // Only support a subset of navigation mapping capabilities, specifically:
            //    - They may never act as the principal end of a relationship.
            string badAssociations = string.Join(", "
                                               , store.ElementDirectory.AllElements
                                                      .OfType<Association>()
                                                      .Where(a => a.Principal == element)
                                                      .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it is the principal end of association(s) {badAssociations}.");

            //    - They may not have navigations to owned entities 
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.Target.IsDependentType) || (a is BidirectionalAssociation b && b.Source == element && b.Target.IsDependentType) || (a is BidirectionalAssociation c && c.Target == element && c.Source.IsDependentType))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has association(s) to dependent type(s) in {badAssociations}.");

            //    - Entities cannot contain navigation properties to query types.
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.Target.IsQueryType) || (a is BidirectionalAssociation b && b.Source == element && b.Target.IsQueryType) || (a is BidirectionalAssociation c && c.Target == element && c.Source.IsQueryType))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has association to sql-mapped type(s) in {badAssociations}.");

            //    - They can only contain reference navigation properties pointing to regular entities.
            badAssociations = string.Join(", "
                                        , store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => (a is UnidirectionalAssociation && a.Source == element && a.TargetMultiplicity == Multiplicity.ZeroMany) || (a is BidirectionalAssociation b && b.Source == element && b.TargetMultiplicity == Multiplicity.ZeroMany) || (a is BidirectionalAssociation c && c.Target == element && c.SourceMultiplicity == Multiplicity.ZeroMany))
                                               .Select(a => a.GetDisplayText()));

            if (!string.IsNullOrEmpty(badAssociations))
               errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has zero-to-many association(s) in {badAssociations}. Only to-one or to-zero-or-one associations are allowed. ");
         }
      }

      private string MakeDefaultTableAndSetName(string root)
      {
         return ModelRoot.PluralizationService?.IsSingular(root) == true
                   ? ModelRoot.PluralizationService.Pluralize(root)
                   : root;
      }
   }
}
