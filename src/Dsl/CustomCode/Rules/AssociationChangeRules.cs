using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <inheritdoc />
   [RuleOn(typeof(Association), FireTime = TimeToFire.TopLevelCommit)]
   public class AssociationChangedRules : ChangeRule
   {
      // matches [Display(Name="*")] and [System.ComponentModel.DataAnnotations.Display(Name="*")]
      private static readonly Regex DisplayAttributeRegex = new Regex("^(.*)\\[(System\\.ComponentModel\\.DataAnnotations\\.)?Display\\(Name=\"([^\"]+)\"\\)\\](.*)$", RegexOptions.Compiled);

      private static void CheckSourceForDisplayText(BidirectionalAssociation bidirectionalAssociation)
      {
         Match match = DisplayAttributeRegex.Match(bidirectionalAssociation.SourceCustomAttributes);

         // is there a custom attribute for [Display]?
         if (match != Match.Empty)
         {
            // if SourceDisplayText is empty, move the Name down to SourceDisplayText
            if (string.IsNullOrWhiteSpace(bidirectionalAssociation.SourceDisplayText))
               bidirectionalAssociation.SourceDisplayText = match.Groups[3].Value;

            // if custom attribute's Name matches SourceDisplayText, remove that attribute, leaving other custom attributes if present
            if (match.Groups[3].Value == bidirectionalAssociation.SourceDisplayText)
               bidirectionalAssociation.SourceCustomAttributes = match.Groups[1].Value + match.Groups[4].Value;
         }
      }

      private static void CheckTargetForDisplayText(Association association)
      {
         Match match = DisplayAttributeRegex.Match(association.TargetCustomAttributes);

         // is there a custom attribute for [Display]?
         if (match != Match.Empty)
         {
            // if TargetDisplayText is empty, move the Name down to TargetDisplayText
            if (string.IsNullOrWhiteSpace(association.TargetDisplayText))
               association.TargetDisplayText = match.Groups[3].Value;

            // if custom attribute's Name matches TargetDisplayText, remove that attribute, leaving other custom attributes if present
            if (match.Groups[3].Value == association.TargetDisplayText)
               association.TargetCustomAttributes = match.Groups[1].Value + match.Groups[4].Value;
         }
      }

      //internal static bool IsProcessingChange => ProcessingChangeCount > 0;

      ///// <summary>
      ///// Number of times the ElementPropertyChanged method has been recursed into
      ///// </summary>
      //private static int ProcessingChangeCount { get; set; }

      /// <inheritdoc />
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         Association element = (Association)e.ModelElement;

         if (element.IsDeleted)
            return;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();
         BidirectionalAssociation bidirectionalAssociation = element as BidirectionalAssociation;

         using (Transaction inner = store.TransactionManager.BeginTransaction("Association ElementPropertyChanged"))
         {
            bool doForeignKeyFixup = false;

            switch (e.DomainProperty.Name)
            {
               case "FKPropertyName":
               {
                  ValidateForeignKeyNames(element, errorMessages);

                  if (!errorMessages.Any())
                     doForeignKeyFixup = true;
               }

               break;

               case "SourceCustomAttributes":

                  if (bidirectionalAssociation != null && !string.IsNullOrWhiteSpace(bidirectionalAssociation.SourceCustomAttributes))
                  {
                     bidirectionalAssociation.SourceCustomAttributes = $"[{bidirectionalAssociation.SourceCustomAttributes.Trim('[', ']')}]";
                     CheckSourceForDisplayText(bidirectionalAssociation);
                  }

                  break;

               case "SourceDisplayText":

                  if (bidirectionalAssociation != null)
                     CheckSourceForDisplayText(bidirectionalAssociation);

                  break;

               case "SourceMultiplicity":
                  Multiplicity currentSourceMultiplicity = (Multiplicity)e.NewValue;
                  Multiplicity priorSourceMultiplicity = (Multiplicity)e.OldValue;

                  // change unidirectional source cardinality
                  // if target is dependent
                  //    source cardinality is 0..1 or 1
                  if (element.Target.IsDependentType && currentSourceMultiplicity == Multiplicity.ZeroMany)
                  {
                     errorMessages.Add($"Can't have a 0..* association from {element.Target.Name} to dependent type {element.Source.Name}");

                     break;
                  }

                  if ((currentSourceMultiplicity == Multiplicity.One && element.TargetMultiplicity == Multiplicity.One)
                   || (currentSourceMultiplicity == Multiplicity.ZeroOne && element.TargetMultiplicity == Multiplicity.ZeroOne))
                  {
                     if (element.SourceRole != EndpointRole.NotSet)
                        element.SourceRole = EndpointRole.NotSet;

                     if (element.TargetRole != EndpointRole.NotSet)
                        element.TargetRole = EndpointRole.NotSet;
                  }
                  else
                     SetEndpointRoles(element);

                  // cascade delete behavior could now be illegal. Reset to default
                  element.SourceDeleteAction = DeleteAction.Default;
                  element.TargetDeleteAction = DeleteAction.Default;

                  if (element.Dependent == null)
                     element.FKPropertyName = null;

                  if (((priorSourceMultiplicity == Multiplicity.ZeroOne || priorSourceMultiplicity == Multiplicity.ZeroMany) && currentSourceMultiplicity == Multiplicity.One) || 
                      ((currentSourceMultiplicity == Multiplicity.ZeroOne || currentSourceMultiplicity == Multiplicity.ZeroMany) && priorSourceMultiplicity == Multiplicity.One))
                     doForeignKeyFixup = true;

                  break;

               case "SourcePropertyName":
                  string sourcePropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Target, (string)e.NewValue);

                  if (EFModelDiagram.IsDropping && sourcePropertyNameErrorMessage != null)
                     element.Delete();
                  else
                     errorMessages.Add(sourcePropertyNameErrorMessage);

                  break;

               case "SourceRole":

                  if (element.Source.IsDependentType)
                  {
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;
                  }
                  else if (!SetEndpointRoles(element))
                  {
                     if (element.SourceRole == EndpointRole.Dependent && element.TargetRole != EndpointRole.Principal)
                        element.TargetRole = EndpointRole.Principal;
                     else if (element.SourceRole == EndpointRole.Principal && element.TargetRole != EndpointRole.Dependent)
                        element.TargetRole = EndpointRole.Dependent;
                  }

                  doForeignKeyFixup = true;

                  break;

               case "TargetCustomAttributes":

                  if (!string.IsNullOrWhiteSpace(element.TargetCustomAttributes))
                  {
                     element.TargetCustomAttributes = $"[{element.TargetCustomAttributes.Trim('[', ']')}]";
                     CheckTargetForDisplayText(element);
                  }

                  break;

               case "TargetDisplayText":

                  CheckTargetForDisplayText(element);

                  break;

               case "TargetMultiplicity":
                  Multiplicity currentTargetMultiplicity = (Multiplicity)e.NewValue;
                  Multiplicity priorTargetMultiplicity = (Multiplicity)e.OldValue;

                  // change unidirectional target cardinality
                  // if target is dependent
                  //    target cardinality must be 0..1 or 1
                  if (element.Target.IsDependentType && currentTargetMultiplicity == Multiplicity.ZeroMany)
                  {
                     errorMessages.Add($"Can't have a 0..* association from {element.Source.Name} to dependent type {element.Target.Name}");

                     break;
                  }

                  if ((element.SourceMultiplicity == Multiplicity.One && currentTargetMultiplicity == Multiplicity.One)
                   || (element.SourceMultiplicity == Multiplicity.ZeroOne && currentTargetMultiplicity == Multiplicity.ZeroOne))
                  {
                     if (element.SourceRole != EndpointRole.NotSet)
                        element.SourceRole = EndpointRole.NotSet;

                     if (element.TargetRole != EndpointRole.NotSet)
                        element.TargetRole = EndpointRole.NotSet;
                  }
                  else
                     SetEndpointRoles(element);

                  // cascade delete behavior could now be illegal. Reset to default
                  element.SourceDeleteAction = DeleteAction.Default;
                  element.TargetDeleteAction = DeleteAction.Default;

                  if (element.Dependent == null)
                     element.FKPropertyName = null;

                  if (((priorTargetMultiplicity == Multiplicity.ZeroOne || priorTargetMultiplicity == Multiplicity.ZeroMany) && currentTargetMultiplicity == Multiplicity.One) || 
                      ((currentTargetMultiplicity == Multiplicity.ZeroOne || currentTargetMultiplicity == Multiplicity.ZeroMany) && priorTargetMultiplicity == Multiplicity.One))
                     doForeignKeyFixup = true;

                  break;

               case "TargetPropertyName":

                  // if we're creating an association via drag/drop, it's possible the existing property name
                  // is the same as the default property name. The default doesn't get created until the transaction is 
                  // committed, so the drop's action will cause a name clash. Remove the clashing property, but
                  // only if drag/drop.

                  string targetPropertyNameErrorMessage = ValidateAssociationIdentifier(element, element.Source, (string)e.NewValue);

                  if (EFModelDiagram.IsDropping && targetPropertyNameErrorMessage != null)
                     element.Delete();
                  else
                     errorMessages.Add(targetPropertyNameErrorMessage);

                  break;

               case "TargetRole":

                  if (element.Target.IsDependentType && (element.SourceRole != EndpointRole.Principal || element.TargetRole != EndpointRole.Dependent))
                  {
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;
                     doForeignKeyFixup = true;
                  }
                  else if (!SetEndpointRoles(element))
                  {
                     if (element.TargetRole == EndpointRole.Dependent && element.SourceRole != EndpointRole.Principal)
                     {
                        element.SourceRole = EndpointRole.Principal;
                        doForeignKeyFixup = true;
                     }
                     else if (element.TargetRole == EndpointRole.Principal && element.SourceRole != EndpointRole.Dependent)
                     {
                        element.SourceRole = EndpointRole.Dependent;
                        doForeignKeyFixup = true;
                     }
                  }

                  break;
            }

            if (doForeignKeyFixup)
               FixupForeignKeys(element);

            inner.Commit();
            element.RedrawItem();
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(store, string.Join("\n", errorMessages));
         }
      }

      private static void ValidateForeignKeyNames(Association element, List<string> errorMessages)
      {
         if (!string.IsNullOrWhiteSpace(element.FKPropertyName))
         {
            string[] foreignKeyPropertyNames = element.GetForeignKeyPropertyNames();
            string tag = $"({element.Source.Name}:{element.Target.Name})";

            if (element.Dependent == null)
            {
               errorMessages.Add($"{tag} can't have foreign keys defined; no dependent role found");
               return;
            }

            int propertyCount = foreignKeyPropertyNames.Length;
            int identityCount = element.Principal.AllIdentityAttributes.Count();

            if (propertyCount != identityCount)
            {
               errorMessages.Add($"{tag} foreign key must have zero or {identityCount} {(identityCount == 1 ? "property" : "properties")} defined, "
                               + $"since {element.Principal.Name} has {identityCount} identity properties. Found {propertyCount} instead");
            }

            // validate names
            foreach (string propertyName in foreignKeyPropertyNames)
            {
               if (!CodeGenerator.IsValidLanguageIndependentIdentifier(propertyName))
                  errorMessages.Add($"{tag} FK property name '{propertyName}' isn't a valid .NET identifier");

               if (element.Dependent.AllAttributes.Except(element.Dependent.Attributes).Any(a => a.Name == propertyName))
                  errorMessages.Add($"{tag} FK property name '{propertyName}' is used in a base class of {element.Dependent.Name}");
            }

            // ensure no FKs are autogenerated identities
            errorMessages.AddRange(element.CheckFKAutoIdentityErrors()
                                          .Select(attribute => $"{attribute.Name} in {element.Dependent.FullName} is an auto-generated identity. Migration will fail."));
         }
      }

      public static void FixupForeignKeys(Association element)
      {
         List<ModelAttribute> fkProperties = element.Source.Attributes.Where(x => x.IsForeignKeyFor == element.Id)
                                                                      .Union(element.Target.Attributes.Where(x => x.IsForeignKeyFor == element.Id))
                                                                      .ToList();

         // if no FKs, remove all the attributes for this element
         if (string.IsNullOrEmpty(element.FKPropertyName) || element.Dependent == null)
         {
            WarningDisplay.Show($"Removing foreign key attribute(s) {string.Join(", ", fkProperties.Select(x => x.GetDisplayText()))}");
            foreach (ModelAttribute attribute in fkProperties)
            {
               attribute.ClearFKMods(string.Empty);
               attribute.Delete();
            }

            return;
         }

         // synchronize what's there to what should be there
         string[] currentForeignKeyPropertyNames = element.GetForeignKeyPropertyNames();

         (IEnumerable<string> add, IEnumerable<ModelAttribute> remove) = fkProperties.Synchronize(currentForeignKeyPropertyNames, (attribute, name) => attribute.Name == name);

         List<ModelAttribute> removeList = remove.ToList();
         fkProperties = fkProperties.Except(removeList).ToList();

         // remove extras
         if (removeList.Any())
            WarningDisplay.Show($"Removing unnecessary foreign key attribute(s) {string.Join(", ", removeList.Select(x => x.GetDisplayText()))}");

         for (int index = 0; index < removeList.Count; index++)
         {
            ModelAttribute attribute = removeList[index];
            attribute.ClearFKMods(string.Empty);
            attribute.Delete();
            removeList.RemoveAt(index--);
         }

         // reparent existing properties if needed
         foreach (ModelAttribute existing in fkProperties.Where(x => x.ModelClass != element.Dependent))
         {
            existing.ClearFKMods();
            existing.ModelClass.MoveAttribute(existing, element.Dependent);
            existing.SetFKMods(element);
         }

         // create new properties if they don't already exist
         foreach (string propertyName in add.Where(n => element.Dependent.Attributes.All(a => a.Name != n)))
            element.Dependent.Attributes.Add(new ModelAttribute(element.Store, new PropertyAssignment(ModelAttribute.NameDomainPropertyId, propertyName)));
         
         // make a pass through and fixup the types, summaries, etc. based on the principal's identity attributes
         ModelAttribute[] principalIdentityAttributes = element.Principal.AllIdentityAttributes.ToArray();
         string summaryBoilerplate = element.GetSummaryBoilerplate();

         for (int index = 0; index < currentForeignKeyPropertyNames.Length; index++)
         {
            ModelAttribute fkProperty = element.Dependent.Attributes.First(x=>x.Name == currentForeignKeyPropertyNames[index]);
            ModelAttribute idProperty = principalIdentityAttributes[index];

            bool required = element.Dependent == element.Source
                                    ? element.TargetMultiplicity == Multiplicity.One
                                    : element.SourceMultiplicity == Multiplicity.One;

            fkProperty.SetFKMods(element
                               , summaryBoilerplate
                               , required
                               , idProperty.Type);
         }
      }

      internal static bool SetEndpointRoles(Association element)
      {
         switch (element.TargetMultiplicity)
         {
            case Multiplicity.ZeroMany:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.NotApplicable;
                     element.TargetRole = EndpointRole.NotApplicable;

                     return true;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     return true;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     return true;
               }

               break;
            case Multiplicity.One:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     return true;
                  case Multiplicity.One:

                     return false;
                  case Multiplicity.ZeroOne:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     return true;
               }

               break;
            case Multiplicity.ZeroOne:

               switch (element.SourceMultiplicity)
               {
                  case Multiplicity.ZeroMany:
                     element.SourceRole = EndpointRole.Dependent;
                     element.TargetRole = EndpointRole.Principal;

                     return true;
                  case Multiplicity.One:
                     element.SourceRole = EndpointRole.Principal;
                     element.TargetRole = EndpointRole.Dependent;

                     return true;
                  case Multiplicity.ZeroOne:

                     return false;
               }

               break;
         }

         return false;
      }

      private static string ValidateAssociationIdentifier(Association association, ModelClass targetedClass, string identifier)
      {
         if (string.IsNullOrWhiteSpace(identifier) || !CodeGenerator.IsValidLanguageIndependentIdentifier(identifier))
            return $"{identifier} isn't a valid .NET identifier";

         ModelClass offendingModelClass = targetedClass.AllAttributes.FirstOrDefault(x => x.Name == identifier)?.ModelClass ??
                                          targetedClass.AllNavigationProperties(association).FirstOrDefault(x => x.PropertyName == identifier)?.ClassType;

         return offendingModelClass != null
                   ? $"Duplicate symbol {identifier} in {offendingModelClass.Name}"
                   : null;
      }
   }
}
