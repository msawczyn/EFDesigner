using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ClassShape), FireTime = TimeToFire.TopLevelCommit)]
   internal class ClassShapeChangeRules : ChangeRule
   {
      /// <inheritdoc />
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ClassShape element = (ClassShape)e.ModelElement;

         if (element.IsDeleted)
            return;

         ModelClass modelClass = (ModelClass)element.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = new List<string>();

         switch (e.DomainProperty.Name)
         {
            case "AbsoluteBounds":
               {
                  List<Guid> linkedConnectionObjectIds = modelClass.Store.ElementDirectory.AllElements
                                                                   .OfType<ModelClass>()
                                                                   .Where(x => x.IsAssociationClass)
                                                                   .Select(x => x.DescribedAssociationElementId)
                                                                   .ToList();

                  List<BidirectionalConnector> candidates = store.ElementDirectory.AllElements
                                                                 .OfType<BidirectionalConnector>()
                                                                 .Where(c => ((BidirectionalAssociation)c.ModelElement).SourceMultiplicity == Multiplicity.ZeroMany
                                                                          && ((BidirectionalAssociation)c.ModelElement).TargetMultiplicity == Multiplicity.ZeroMany
                                                                          && !linkedConnectionObjectIds.Contains(((BidirectionalAssociation)c.ModelElement).Id)
                                                                          && c.AbsoluteBoundingBox.IntersectsWith(element.AbsoluteBoundingBox))
                                                                 .ToList();

                  if (modelClass.CanBecomeAssociationClass() && candidates.Any())
                  {
                     foreach (BidirectionalConnector candidate in candidates)
                     {
                        BidirectionalAssociation association = ((BidirectionalAssociation)candidate.ModelElement);

                        if (BooleanQuestionDisplay.Show(store, $"Make {modelClass.Name} an association class for {association.GetDisplayText()}?") == true)
                        {
                           ClassShape.AddAssociationClass(candidate, element);
                           break;
                        }
                     }
                  }
               }

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();

         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(store, string.Join("\n", errorMessages));
         }
      }
   }
}
