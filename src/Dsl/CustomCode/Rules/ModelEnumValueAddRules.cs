using System.Linq;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelEnumValue), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelEnumValueAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelEnumValue element = (ModelEnumValue)e.ModelElement;
         ModelEnum enumElement = element.Enum;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         enumElement.SetFlagValue(element);

         if (!enumElement.IsFlags)
         {
            bool hasDuplicates = enumElement.Values.Any(x => x != element && x.Value == element.Value);

            if (hasDuplicates)
               element.Value = null;
         }
      }
   }
}
