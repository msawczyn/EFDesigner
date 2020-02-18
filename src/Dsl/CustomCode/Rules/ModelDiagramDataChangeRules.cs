using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelDiagramData), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelDiagramDataChangeRules : ChangeRule
   {
      
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelDiagramData element = (ModelDiagramData)e.ModelElement;
         if (element.IsDeleted) return;

         Store store = element.Store;
         Transaction currentTransaction = store.TransactionManager.CurrentTransaction;

         if (currentTransaction.IsSerializing) 
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = new List<string>();

         switch (e.DomainProperty.Name)
         {
            case "Name":
               if (string.IsNullOrWhiteSpace(e.NewValue?.ToString()))
               {
                  errorMessages.Add("Diagram must have a name");
                  break;
               }

               if (store.ElementDirectory.AllElements.OfType<ModelDiagramData>().Where(d => d != element).Any(d => d.Name == e.NewValue.ToString()))
               {
                  errorMessages.Add("Diagram must have a unique name");
                  break;
               }

               ModelDiagramData.CloseDiagram(element.GetDiagram());
               element.GetDiagram().Name = element.Name = e.NewValue.ToString();
               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            currentTransaction.Rollback();
            ErrorDisplay.Show(string.Join("\n", errorMessages));
         }
      }
   }
}
