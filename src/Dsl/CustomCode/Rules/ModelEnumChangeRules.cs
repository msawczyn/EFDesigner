using System.CodeDom.Compiler;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Sawczyn.EFDesigner.EFModel.CustomCode.Rules;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelEnum), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelEnumChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelEnum element = (ModelEnum)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "Name":
               string newName = (string)e.NewValue;
               if (current.Name.ToLowerInvariant() == "paste")
                  return;

               if (current.Name.ToLowerInvariant() != "paste" && (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName)))
                  errorMessage = "Name must be a valid .NET identifier";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Any(x => x.Name == newName))
                  errorMessage = "Enum name already in use by a class";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelEnum>()
                             .Except(new[] {element})
                             .Any(x => x.Name == newName))
                  errorMessage = "Enum name already in use by another enum";

               break;

            case "Namespace":
               string newNamespace = (string)e.NewValue;
               if (current.Name.ToLowerInvariant() != "paste")
                  errorMessage = CommonRules.ValidateNamespace(newNamespace, CodeGenerator.IsValidLanguageIndependentIdentifier);
               break;

            case "IsFlags":
               element.SetFlagValues();

               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            ErrorDisplay.Show(errorMessage);
         }
      }
   }
}
