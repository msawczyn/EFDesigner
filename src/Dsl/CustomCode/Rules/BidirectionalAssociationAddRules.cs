using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel {
   [RuleOn(typeof(BidirectionalAssociation), FireTime = TimeToFire.TopLevelCommit, Priority = DiagramFixupConstants.AddConnectionRulePriority + 1)]
   internal class BidirectionalAssociationAddRule : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         BidirectionalAssociation element = (BidirectionalAssociation)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         PresentationHelper.UpdateAssociationDisplay(element);
      }
   }
}