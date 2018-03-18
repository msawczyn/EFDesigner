using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   [RuleOn(typeof(ModelRoot), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelRootChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelRoot element = (ModelRoot)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "ConnectionString":
               if (e.NewValue != null)
                  element.ConnectionStringName = null;
               break;

            case "ConnectionStringName":
               if (e.NewValue != null)
                  element.ConnectionString = null;
               break;

            case "Namespace":
               errorMessage = CommonRules.ValidateNamespace((string)e.NewValue, CodeGenerator.IsValidLanguageIndependentIdentifier);
               break;

            case "EntityOutputDirectory":
               if (string.IsNullOrEmpty(element.EnumOutputDirectory))
                  element.EnumOutputDirectory = (string)e.NewValue;
               break;

            case "EnumOutputDirectory":
               if (string.IsNullOrEmpty((string)e.NewValue) && !string.IsNullOrEmpty(element.EntityOutputDirectory))
                  element.EnumOutputDirectory = element.EntityOutputDirectory;
               break;

            case "EntityFrameworkVersion":
               if ((EFVersion)e.NewValue == EFVersion.EFCore)
                  errorMessage = ImposeEFCoreRestrictions(element, store);
               break;

            case "DatabaseSchema":
               if (string.IsNullOrEmpty((string)e.NewValue))
                  element.DatabaseSchema = "dbo";
               break;

            case "FileNameMarker":
               string newFileNameMarker = (string)e.NewValue;
               if (!Regex.Match($"a.{newFileNameMarker}.cs",
                                @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$").Success)
                  errorMessage = "Invalid value to make part of file name";
               break;

            case "InheritanceStrategy":
               //TODO: EFCore limitation as of 2.0. Review for each new release.
               if (element.EntityFrameworkVersion == EFVersion.EFCore && (CodeStrategy)e.NewValue != CodeStrategy.TablePerHierarchy)
                  element.InheritanceStrategy = CodeStrategy.TablePerHierarchy; 
               break;

            case "ShowCascadeDeletes":
               // need these change rules to fire even though nothing in Association has changed
               // so we need to set this early -- requires guarding against recursion.
               bool newShowCascadeDeletes = (bool)e.NewValue;
               if (element.ShowCascadeDeletes != newShowCascadeDeletes)
               {
                  element.ShowCascadeDeletes = newShowCascadeDeletes;
                  foreach (Association association in store.ElementDirectory.FindElements<Association>())
                     AssociationChangeRules.UpdateDisplayForCascadeDelete(association);
               }
               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            MessageBox.Show(errorMessage);
         }
      }

      private string ImposeEFCoreRestrictions(ModelRoot modelRoot, Store store)
      {
         //TODO: EFCore limitations as of 2.0. Review for each new release.

         List<string> errors = new List<string>();

         if (modelRoot.InheritanceStrategy != CodeStrategy.TablePerHierarchy)
            errors.Add($"{modelRoot.InheritanceStrategy} inheritance strategy");

         List<Association> unsupportedAssociations = store.ElementDirectory
                                                          .AllElements
                                                          .OfType<Association>()
                                                          .Where(a => a.SourceMultiplicity == Multiplicity.ZeroMany && 
                                                                      a.TargetMultiplicity == Multiplicity.ZeroMany)
                                                          .ToList();

         if (unsupportedAssociations.Any())
         {
            List<string> classes = new List<string>();

            foreach (Association assoc in unsupportedAssociations)
               classes.Add($"{assoc.Source.Name} and {assoc.Target.Name}");
            
            errors.Add($"many-to-many associations between the following classes: {string.Join(", ", classes)}");
         }

         return errors.Any() 
                   ? $"Found model elements not (yet) supported in EFCore: {string.Join(", ", errors)}" 
                   : null;
      }
   }
}
