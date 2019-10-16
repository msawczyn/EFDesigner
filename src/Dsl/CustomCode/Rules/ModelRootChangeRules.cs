using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
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

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();
         bool redraw = false;

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

            case "DatabaseSchema":

               if (string.IsNullOrEmpty((string)e.NewValue))
                  element.DatabaseSchema = "dbo";

               break;

            case "EntityFrameworkVersion":
               element.EntityFrameworkPackageVersion = "Latest";

               if (element.EntityFrameworkVersion == EFVersion.EFCore)
                  element.InheritanceStrategy = CodeStrategy.TablePerHierarchy;

               break;

            case "EnumOutputDirectory":

               if (string.IsNullOrEmpty((string)e.NewValue) && !string.IsNullOrEmpty(element.EntityOutputDirectory))
                  element.EnumOutputDirectory = element.EntityOutputDirectory;

               break;

            case "StructOutputDirectory":

               if (string.IsNullOrEmpty((string)e.NewValue) && !string.IsNullOrEmpty(element.EntityOutputDirectory))
                  element.StructOutputDirectory = element.EntityOutputDirectory;

               break;

            case "EntityOutputDirectory":

               if (string.IsNullOrEmpty(element.EnumOutputDirectory) || element.EnumOutputDirectory == (string)e.OldValue)
                  element.EnumOutputDirectory = (string)e.NewValue;

               if (string.IsNullOrEmpty(element.StructOutputDirectory) || element.StructOutputDirectory == (string)e.OldValue)
                  element.StructOutputDirectory = (string)e.NewValue;

               break;

            case "FileNameMarker":
               string newFileNameMarker = (string)e.NewValue;

               if (!Regex.Match($"a.{newFileNameMarker}.cs",
                                @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$")
                         .Success)
                  errorMessages.Add("Invalid value to make part of file name");

               break;

            case "InheritanceStrategy":

               if ((element.EntityFrameworkVersion == EFVersion.EFCore) && (element.NuGetPackageVersion.MajorMinorVersionNum < 2.1))
                  element.InheritanceStrategy = CodeStrategy.TablePerHierarchy;

               break;

            case "LayoutAlgorithm":
               ModelDisplay.LayoutDiagram(element.Classes.FirstOrDefault()?.GetActiveDiagram() as EFModelDiagram);

               break;

            case "Namespace":
               errorMessages.Add(CommonRules.ValidateNamespace((string)e.NewValue, CodeGenerator.IsValidLanguageIndependentIdentifier));

               break;

            case "ShowCascadeDeletes":

               // need these change rules to fire even though nothing in Association has changed
               // so we need to set this early -- requires guarding against recursion.
               bool newShowCascadeDeletes = (bool)e.NewValue;

               //if (element.ShowCascadeDeletes != newShowCascadeDeletes)
               {
                  //element.ShowCascadeDeletes = newShowCascadeDeletes;

                  foreach (Association association in store.ElementDirectory.FindElements<Association>())
                     PresentationHelper.UpdateDisplayForCascadeDelete(association);
               }

               redraw = true;

               break;

            case "ShowWarningsInDesigner":
               redraw = true;

               break;

            case "WarnOnMissingDocumentation":

               if (element.ShowWarningsInDesigner)
                  redraw = true;

               ModelRoot.ExecuteValidator?.Invoke();

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(string.Join("\n", errorMessages));
         }

         if (redraw)
            element.InvalidateDiagrams();
      }
   }
}
