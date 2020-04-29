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
               EFModelDiagram diagram = element.GetDiagram();

               if (diagram != null && string.IsNullOrWhiteSpace(diagram.Name))
               {
                  errorMessages.Add("Can't change default diagram name");
                  break;
               }

               if (string.IsNullOrWhiteSpace(element.Name))
               {
                  errorMessages.Add("Diagram must have a name");
                  break;
               }

               if (store.GetAll<ModelDiagramData>().Except(new[] {element}).Any(d => d.Name == element.Name))
               {
                  errorMessages.Add("Diagram must have a unique name");
                  break;
               }

               if (diagram != null)
               {
                  diagram.Name = element.Name;
                  ModelDiagramData.CloseDiagram(diagram);
               }

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            currentTransaction.Rollback();
            ErrorDisplay.Show(store, string.Join("\n", errorMessages));
         }
      }
   }
}
