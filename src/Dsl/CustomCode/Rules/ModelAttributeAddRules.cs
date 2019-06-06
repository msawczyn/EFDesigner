using System;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelAttribute), FireTime = TimeToFire.LocalCommit)]
   internal class ModelAttributeAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelAttribute element = (ModelAttribute)e.ModelElement;
         ModelClass modelClass = element.ModelClass;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (modelClass.IsReadOnly && !modelClass.ModelRoot.BypassReadOnlyChecks)
         {
            ErrorDisplay.Show($"{modelClass.Name} is read-only; can't add a property");
            current.Rollback();
            return;
         }

         // set a new default value if we want to implement notify, to reduce the chance of forgetting to change it
         if (modelClass?.ImplementNotify == true)
            element.AutoProperty = false;
      }
   }
}
