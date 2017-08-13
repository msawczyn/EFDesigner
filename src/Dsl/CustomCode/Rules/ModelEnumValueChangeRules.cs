using System.CodeDom.Compiler;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   [RuleOn(typeof(ModelEnumValue), FireTime = TimeToFire.TopLevelCommit)]
   public class ModelEnumValueChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelEnumValue element = (ModelEnumValue)e.ModelElement;
         ModelEnum modelEnum = element.Enum;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "Name":
               string newName = (string)e.NewValue;

               if (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName))
                  errorMessage = "Name must be a valid .NET identifier";
               else if (modelEnum.Values.Except(new[] {element}).Any(v => v.Name == newName))
                  errorMessage = "Value name already in use";

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
