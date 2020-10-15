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
                  if (element.IsDependentType)
                  {
                     errorMessages.Add($"Can't give {element.Name} a base class since it's a dependent type");

                     break;
                  }

                  break;
               }

            case "DbSetName":
               {
                  string newDbSetName = (string)e.NewValue;

                  if (element.IsDependentType)
                  {
                     if (!string.IsNullOrEmpty(newDbSetName))
                        element.DbSetName = string.Empty;
                  }
                  else
                  {
                     if (string.IsNullOrEmpty(newDbSetName))
                        element.DbSetName = MakeDefaultName(element.Name);

                     if (current.Name.ToLowerInvariant() != "paste" &&
                         (string.IsNullOrWhiteSpace(newDbSetName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newDbSetName)))
                        errorMessages.Add($"DbSet name '{newDbSetName}' isn't a valid .NET identifier.");
                     else if (store.GetAll<ModelClass>()
                                   .Except(new[] { element })
                                   .Any(x => x.DbSetName == newDbSetName))
                        errorMessages.Add($"DbSet name '{newDbSetName}' already in use");
                  }

                  break;
               }

            case "ImplementNotify":
               {
                  bool newImplementNotify = (bool)e.NewValue;

                  if (newImplementNotify)
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
                  bool newIsAbstract = (bool)e.NewValue;

                  if (newIsAbstract && element.IsDependentType)
                  {
                     errorMessages.Add($"Can't make {element.Name} abstract since it's a dependent type");

                     break;
                  }

                  PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "IsDependentType":
               {
                  bool newIsDependentType = (bool)e.NewValue;

                  if (newIsDependentType)
                  {
                     if (element.BaseClass != null)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it has a base class");

                        break;
                     }

                     if (element.IsAbstract)
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's abstract");

                        break;
                     }

                     // dependent type can't be source in an association
                     if (store.GetAll<Association>().Any(a => a.Source == element))
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it references other classes");

                        break;
                     }

                     if (store.GetAll<BidirectionalAssociation>().Any(a => a.Source == element || a.Target == element))
                     {
                        errorMessages.Add($"Can't make {element.Name} a dependent class since it's in a bidirectional association");

                        break;
                     }

                     if (element.ModelRoot.EntityFrameworkVersion == EFVersion.EF6 || element.ModelRoot.GetEntityFrameworkPackageVersionNum() < 2.2)
                     {
                        if (store.GetAll<Association>().Any(a => a.Target == element && a.TargetMultiplicity == Multiplicity.ZeroMany))
                        {
                           errorMessages.Add($"Can't make {element.Name} a dependent class since it's the target of a 0..* association");

                           break;
                        }

                        foreach (ModelAttribute modelAttribute in element.AllAttributes.Where(a => a.IsIdentity))
                           modelAttribute.IsIdentity = false;

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

                     element.DbSetName = string.Empty;
                  }
                  else
                  {
                     element.DbSetName = MakeDefaultName(element.Name);
                     element.TableName = MakeDefaultName(element.Name);
                  }

                  PresentationHelper.UpdateClassDisplay(element);

                  break;
               }

            case "IsMappedToSqlQuery":
               {
                  if ((bool)e.NewValue)
                  {
                     // Restrictions:
                     // =================================
                     // Cannot have a key defined.
                     if (element.Attributes.Any(x => x.IsIdentity))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has identity attributes. Set their 'Is Identity' property to false first.");

                     // Only support a subset of navigation mapping capabilities, specifically:
                     //    - They may never act as the principal end of a relationship.
                     foreach (Association association in store.ElementDirectory.AllElements
                                                              .OfType<Association>()
                                                              .Where(a => a.Principal == element))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it is the principal end of an association with {association.Dependent.Name} ({association.GetDisplayText()})");

                     //    - They may not have navigations to owned entities
                     foreach (Association association in store.ElementDirectory.AllElements
                                                                            .OfType<Association>()
                                                                            .Where(a => a.Source == element && a.Target.IsDependentType))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an association to the dependent type {association.Target.Name} ({association.GetDisplayText()})");

                     foreach (BidirectionalAssociation association in store.ElementDirectory.AllElements
                                                                            .OfType<BidirectionalAssociation>()
                                                                            .Where(a => a.Target == element && a.Source.IsDependentType))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an association to the dependent type {association.Source.Name} ({association.GetDisplayText()})");

                     //    - They can only contain reference navigation properties pointing to regular entities.
                     foreach (Association association in store.ElementDirectory.AllElements
                                                              .OfType<Association>()
                                                              .Where(a => a.TargetMultiplicity == Multiplicity.ZeroMany))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an zero-to-many association to {association.Target.Name} ({association.GetDisplayText()}). Only to-one or to-zero-or-one associations are allowed. ");

                     foreach (BidirectionalAssociation association in store.ElementDirectory.AllElements
                                                                           .OfType<BidirectionalAssociation>()
                                                                           .Where(a => a.SourceMultiplicity == Multiplicity.ZeroMany))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an zero-to-many association to {association.Source.Name} ({association.GetDisplayText()}). Only to-one or to-zero-or-one associations are allowed. ");

                     //    - Entities cannot contain navigation properties to keyless entity types.
                     foreach (Association association in store.ElementDirectory.AllElements
                                                              .OfType<Association>()
                                                              .Where(a => a.Source == element && a.Target.IsMappedToSqlQuery))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an association to the sql-mapped type {association.Target.Name} ({association.GetDisplayText()})");

                     foreach (BidirectionalAssociation association in store.ElementDirectory.AllElements
                                                                           .OfType<BidirectionalAssociation>()
                                                                           .Where(a => a.Target == element && a.Source.IsMappedToSqlQuery))
                        errorMessages.Add($"{element.Name} can't be mapped to a Sql query since it has an association to the sql-mapped type {association.Source.Name} ({association.GetDisplayText()})");

                     foreach (ModelAttribute attribute in element.Attributes)
                        attribute.ReadOnly = true;
                  }

                  break;
               }
            case "Name":
               {
                  string newName = (string)e.NewValue;

                  if (current.Name.ToLowerInvariant() != "paste" &&
                      (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName)))
                     errorMessages.Add($"Class name '{newName}' isn't a valid .NET identifier.");

                  else if (store.ElementDirectory
                                .AllElements
                                .OfType<ModelClass>()
                                .Except(new[] { element })
                                .Any(x => x.Name == newName))
                     errorMessages.Add($"Class name '{newName}' already in use by another class");

                  else if (store.ElementDirectory
                                .AllElements
                                .OfType<ModelEnum>()
                                .Any(x => x.Name == newName))
                     errorMessages.Add($"Class name '{newName}' already in use by an enum");

                  else if (!string.IsNullOrEmpty((string)e.OldValue))
                  {
                     string oldDefaultName = MakeDefaultName((string)e.OldValue);
                     string newDefaultName = MakeDefaultName(newName);

                     if (element.DbSetName == oldDefaultName)
                        element.DbSetName = newDefaultName;

                     if (element.TableName == oldDefaultName)
                        element.TableName = newDefaultName;
                  }

                  break;
               }

            case "Namespace":
               {
                  string newNamespace = (string)e.NewValue;

                  if (current.Name.ToLowerInvariant() != "paste")
                     errorMessages.Add(CommonRules.ValidateNamespace(newNamespace, CodeGenerator.IsValidLanguageIndependentIdentifier));

                  break;
               }

            case "TableName":
               {
                  string newTableName = (string)e.NewValue;

                  if (element.IsDependentType)
                  {
                     if (!string.IsNullOrEmpty(newTableName))
                        element.TableName = string.Empty;
                  }
                  else
                  {
                     if (string.IsNullOrEmpty(newTableName))
                        element.TableName = MakeDefaultName(element.Name);

                     if (store.GetAll<ModelClass>()
                              .Except(new[] { element })
                              .Any(x => x.TableName == newTableName))
                        errorMessages.Add($"Table name '{newTableName}' already in use");
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
      }

      private string MakeDefaultName(string root)
      {
         return ModelRoot.PluralizationService?.IsSingular(root) == true
                   ? ModelRoot.PluralizationService.Pluralize(root)
                   : root;
      }
   }
}
