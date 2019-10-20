using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel {
   [RuleOn(typeof(UnidirectionalAssociation), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority + 1)]
   internal class UnidirectionalAssociationAddRule : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         UnidirectionalAssociation element = (UnidirectionalAssociation)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         PresentationHelper.UpdateAssociationDisplay(element);
      }
   }
}