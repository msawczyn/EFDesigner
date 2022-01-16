//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Windows.Forms;

//using Microsoft.VisualStudio.Modeling;
//using Microsoft.VisualStudio.Modeling.Diagrams;

//namespace Sawczyn.EFDesigner.EFModel
//{
//   [RuleOn(typeof(ClassShape), FireTime = TimeToFire.TopLevelCommit)]
//   internal class ClassShapeChangeRules : ChangeRule
//   {
//      /// <inheritdoc />
//      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
//      {
//         base.ElementPropertyChanged(e);

//         ClassShape element = (ClassShape)e.ModelElement;

//         if (element.IsDeleted)
//            return;

//         ModelClass modelClass = (ModelClass)element.ModelElement;
//         Store store = element.Store;
//         Transaction current = store.TransactionManager.CurrentTransaction;

//         if (current.IsSerializing || ModelRoot.BatchUpdating)
//            return;

//         if (Equals(e.NewValue, e.OldValue))
//            return;

//         List<string> errorMessages = new List<string>();

//         switch (e.DomainProperty.Name)
//         {
//            case "AbsoluteBounds":
//               {
//                  if (modelClass.CanBecomeAssociationClass())
//                  {
//                     RectangleD oldBounds = (RectangleD)e.OldValue;
//                     RectangleD newBounds = element.AbsoluteBoundingBox;
//                     double dw = newBounds.Width - oldBounds.Width;
//                     double dh = newBounds.Height - oldBounds.Height;
//                     double dx = newBounds.X - oldBounds.X;
//                     double dy = newBounds.Y - oldBounds.Y;

//                     // Moved or resized? If moving, height and width don't change.
//                     if (dw != 0.0 && dh != 0.0)
//                        break;

//                     List<BidirectionalConnector> candidates = GetBidirectionalConnectorsUnderShape(modelClass, store, element);

//                     if (candidates.Any())
//                     {
//                        foreach (BidirectionalConnector candidate in candidates)
//                        {
//                           BidirectionalAssociation association = ((BidirectionalAssociation)candidate.ModelElement);

//                           if (BooleanQuestionDisplay.Show(store, $"Make {modelClass.Name} an association class for {association.GetDisplayText()}?") == true)
//                           {
//                              ClassShape.AddAssociationClass(candidate, element);
//                              break;
//                           }
//                        }
//                     }
//                  }
//               }

//               break;
//         }

//         errorMessages = errorMessages.Where(m => m != null).ToList();

//         if (errorMessages.Any())
//         {
//            current.Rollback();
//            ErrorDisplay.Show(store, string.Join("\n", errorMessages));
//         }
//      }

//      private static List<BidirectionalConnector> GetBidirectionalConnectorsUnderShape(ModelClass modelClass, Store store, ClassShape element)
//      {
//         List<Guid> linkedConnectionObjectIds = modelClass.Store.ElementDirectory.AllElements
//                                                          .OfType<ModelClass>()
//                                                          .Where(x => x.IsAssociationClass)
//                                                          .Select(x => x.DescribedAssociationElementId)
//                                                          .ToList();

//         List<BidirectionalConnector> candidates = store.ElementDirectory.AllElements
//                                                        .OfType<BidirectionalConnector>()
//                                                        .Where(c => ((BidirectionalAssociation)c.ModelElement).SourceMultiplicity == Multiplicity.ZeroMany
//                                                                 && ((BidirectionalAssociation)c.ModelElement).TargetMultiplicity == Multiplicity.ZeroMany
//                                                                 && !linkedConnectionObjectIds.Contains(((BidirectionalAssociation)c.ModelElement).Id)
//                                                                 && c.Diagram.Id == element.Diagram.Id
//                                                                 && c.AbsoluteBoundingBox.IntersectsWith(element.AbsoluteBoundingBox))
//                                                        .ToList();

//         return candidates;
//      }

//      private static Cursor _moveCursor;

//      private static Cursor MoveCursor
//      {
//         get
//         {
//            if (_moveCursor == null)
//            {
//               using (MemoryStream stream = new MemoryStream(Resources.MoveCursor))
//               {
//                  _moveCursor = new Cursor(stream);
//               }
//            }

//            return _moveCursor;
//         }
//      }
//   }
//}
