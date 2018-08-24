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
         Transaction currentTransaction = store.TransactionManager.CurrentTransaction;

         if (currentTransaction.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "Name":
               if (currentTransaction.Name.ToLowerInvariant() == "paste")
                  return;

               if (string.IsNullOrWhiteSpace(element.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(element.Name))
                  errorMessage = "Name must be a valid .NET identifier";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Any(x => x.Name == element.Name))
                  errorMessage = "Enum name already in use by a class";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelEnum>()
                             .Except(new[] {element})
                             .Any(x => x.Name == element.Name))
                  errorMessage = "Enum name already in use by another enum";
               else
               {
                  // rename type names for ModelAttributes that reference this enum
                  foreach (ModelAttribute modelAttribute in store.ElementDirectory.AllElements.OfType<ModelAttribute>().Where(a => a.Type == (string)e.OldValue))
                     modelAttribute.Type = element.Name;
               }
               break;

            case "Namespace":

               if (string.IsNullOrWhiteSpace(element.Namespace))
                  element.Namespace = element.ModelRoot.Namespace;

               if (currentTransaction.Name.ToLowerInvariant() != "paste")
                  errorMessage = CommonRules.ValidateNamespace(element.Namespace, CodeGenerator.IsValidLanguageIndependentIdentifier);
               break;

            case "IsFlags":
               element.SetFlagValues();

               break;
         }

         if (errorMessage != null)
         {
            currentTransaction.Rollback();
            ErrorDisplay.Show(errorMessage);
         }
      }
   }
}
