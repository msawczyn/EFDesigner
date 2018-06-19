using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

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

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();

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
               errorMessages.Add(CommonRules.ValidateNamespace((string)e.NewValue, CodeGenerator.IsValidLanguageIndependentIdentifier));
               break;

            case "EntityOutputDirectory":
               if (string.IsNullOrEmpty(element.EnumOutputDirectory))
                  element.EnumOutputDirectory = (string)e.NewValue;
               break;

            case "EnumOutputDirectory":
               if (string.IsNullOrEmpty((string)e.NewValue) && !string.IsNullOrEmpty(element.EntityOutputDirectory))
                  element.EnumOutputDirectory = element.EntityOutputDirectory;
               break;

            case "DatabaseSchema":
               if (string.IsNullOrEmpty((string)e.NewValue))
                  element.DatabaseSchema = "dbo";
               break;

            case "FileNameMarker":
               string newFileNameMarker = (string)e.NewValue;
               if (!Regex.Match($"a.{newFileNameMarker}.cs",
                                @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$")
                         .Success)
                  errorMessages.Add("Invalid value to make part of file name");
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

            case "InheritanceStrategy":

               if ((element.EntityFrameworkVersion == EFVersion.EFCore) && (element.EntityFrameworkCoreVersion <= EFCoreVersion.EFCore21))
                  element.InheritanceStrategy = CodeStrategy.TablePerHierarchy;

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();
         if (errorMessages.Any())
         {
            current.Rollback();
            MessageBox.Show(string.Join("; ", errorMessages));
         }
      }
   }
}
