using System.CodeDom.Compiler;
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
               string newNamespace = (string)e.NewValue;

               if (!string.IsNullOrEmpty(newNamespace) && !CodeGenerator.IsValidLanguageIndependentIdentifier(newNamespace))
                  errorMessage = "Namespace must be a valid .NET identifier";
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
               if (string.IsNullOrEmpty(element.DatabaseSchema))
                  element.DatabaseSchema = "dbo";
               break;

            case "FileNameMarker":
               if (!Regex.Match($"a.{element.FileNameMarker}.cs",
                                @"^(?!^(PRN|AUX|CLOCK\$|NUL|CON|COM\d|LPT\d|\..*)(\..+)?$)[^\x00-\x1f\\?*:\"";|/]+$").Success)
                  errorMessage = "Invalid value to make part of file name";
               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            MessageBox.Show(errorMessage);
         }
      }
   }
}
