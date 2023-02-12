//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Windows.Forms;

//using Microsoft.VisualStudio.Modeling.Diagrams;

//namespace Sawczyn.EFDesigner.EFModel
//{
//   public class ClassDragMouseAction : MouseAction
//   {
//      private readonly ClassShape _classShape;
//      private readonly ModelClass _modelClass;

//      public ClassDragMouseAction(ClassShape classShape) : base(classShape.Diagram)
//      {
//         _classShape = classShape;
//         _modelClass = (ModelClass)classShape.ModelElement;
//      }

//      /// <summary>
//      /// Called when a MouseMove event has been dispatched to this MouseAction.
//      /// </summary>
//      /// <param name="e">A DiagramMouseEventArgs that contains event data.</param>
//      /// <remarks>To modify the cursor, override GetCursor.</remarks>
//      /// <remarks>
//      /// To draw feedback for this MouseAction, override DoPaintFeedback.
//      /// </remarks>
//      protected override void OnMouseMove(DiagramMouseEventArgs e)
//      {
//         Debug.WriteLine("OnMouseMove 0");
//         base.OnMouseMove(e);
//         Debug.WriteLine("OnMouseMove 1");
//      }

//      /// <summary>
//      /// Called when a MouseDown event has been dispatched to this MouseAction.
//      /// </summary>
//      /// <param name="e">A DiagramMouseEventArgs that contains event data.</param>
//      protected override void OnMouseDown(DiagramMouseEventArgs e)
//      {
//         Debug.WriteLine("OnMouseDown 0");
//         base.OnMouseDown(e);
//         Debug.WriteLine("OnMouseDown 1");
//      }

//      /// <summary>
//      /// Called when this MouseAction's drag operation has been canceled.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      /// <remarks>
//      /// Set e.ActionRequest to ActionRequest.CompleteAction to complete the
//      /// MouseAction and deactivate it.
//      /// </remarks>
//      /// <remarks>
//      /// Set e.ActionRequest to ActionRequest.CancelAction to cancel the
//      /// MouseAction and deactivate it.
//      /// </remarks>
//      /// <remarks>
//      /// Set e.ActionRequest to ActionRequest.ContinueAction to keep the
//      /// MouseAction active.  This will place the MouseAction in the
//      /// hovering state.
//      /// </remarks>
//      protected override void OnDragCanceled(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDragCanceled 0");
//         base.OnDragCanceled(e);
//         Debug.WriteLine("OnDragCanceled 1");
//      }

//      /// <summary>
//      /// Called when this MouseAction has entered the drag/click-pending state.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      /// <remarks>
//      /// The drag/click-pending state begins when a MouseDown event occurs while
//      /// the MouseAction is in a hovering state or while it is inactive.
//      /// </remarks>
//      protected override void OnDragPendingBegun(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDragPendingBegun 0");
//         base.OnDragPendingBegun(e);
//         Debug.WriteLine("OnDragPendingBegun 1");
//      }

//      /// <summary>
//      /// Called when this MouseAction has exited the drag/click-pending state.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      /// <remarks>
//      /// The drag/click-pending state ends when the criteria for dragging has been
//      /// satisfied, or when a MouseUp has been received before dragging could begin
//      /// (in which case the user has clicked), or when a Complete or Cancel event
//      /// has been received.
//      /// </remarks>
//      protected override void OnDragPendingEnded(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDragPendingEnded 0");
//         base.OnDragPendingEnded(e);
//         Debug.WriteLine("OnDragPendingEnded 1");
//      }

//      /// <summary>Called when this MouseAction has been activated.</summary>
//      /// <param name="e">A DiagramEventArgs that contains event data.</param>
//      protected override void OnMouseActionActivated(DiagramEventArgs e)
//      {
//         Debug.WriteLine("OnMouseActionActivated 0");
//         base.OnMouseActionActivated(e);
//         Debug.WriteLine("OnMouseActionActivated 1");
//      }

//      /// <summary>
//      /// Called when the MouseAction has been canceled and is ready to be deactivated.
//      /// </summary>
//      /// <param name="e">A DiagramEventArgs that contains event data.</param>
//      protected override void OnMouseActionCanceled(DiagramEventArgs e)
//      {
//         Debug.WriteLine("OnMouseActionCanceled 0");
//         base.OnMouseActionCanceled(e);
//         Debug.WriteLine("OnMouseActionCanceled 1");
//      }

//      /// <summary>
//      /// Called when the MouseAction has completed and is ready to be deactivated.
//      /// </summary>
//      /// <param name="e">A DiagramEventArgs that contains event data.</param>
//      protected override void OnMouseActionCompleted(DiagramEventArgs e)
//      {
//         Debug.WriteLine("OnMouseActionCompleted 0");
//         base.OnMouseActionCompleted(e);
//         Debug.WriteLine("OnMouseActionCompleted 1");
//      }

//      /// <summary>Called when this MouseAction has been deactivated.</summary>
//      /// <param name="e">A DiagramEventArgs that contains event data.</param>
//      protected override void OnMouseActionDeactivated(DiagramEventArgs e)
//      {
//         Debug.WriteLine("OnMouseActionDeactivated 0");
//         base.OnMouseActionDeactivated(e);
//         Debug.WriteLine("OnMouseActionDeactivated 1");
//      }

//      /// <summary>
//      /// Called when a MouseUp event has been dispatched to this MouseAction.
//      /// </summary>
//      /// <param name="e">A DiagramMouseEventArgs that contains event data.</param>
//      /// <remarks>
//      /// If your intent is to respond to a MouseDown + MouseUp
//      /// combination that does not include dragging, override the
//      /// OnClicked method instead.
//      /// (A MouseMove may occur between MouseDown and MouseUp that does
//      /// not exceed a drag delta and therefore does not start dragging.)
//      /// </remarks>
//      /// <remarks>
//      /// If your intent is to respond to a MouseDown + Drag + MouseUp
//      /// combination, override the OnDragCompleted method instead.
//      /// (Drag occurs when the MouseMove exceeds a drag delta.)
//      /// </remarks>
//      /// <remarks>
//      /// If your intent is to respond to a double-click event, override
//      /// the OnDoubleClick method instead.
//      /// </remarks>
//      /// <remarks>
//      /// If your intent is to respond to the right-click event (perhaps
//      /// to prevent the context menu from appearing), override the
//      /// OnContextMenuRequested method instead.
//      /// </remarks>
//      protected override void OnMouseUp(DiagramMouseEventArgs e)
//      {
//         Debug.WriteLine("OnMouseUp 0");
//         base.OnMouseUp(e);
//         Debug.WriteLine("OnMouseUp 1");
//         List<BidirectionalConnector> candidates = _classShape.GetBidirectionalConnectorsUnderShape();

//         if (candidates.Any())
//         {
//            Debug.WriteLine("OnMouseUp 2");

//            foreach (BidirectionalConnector candidate in candidates)
//            {
//               Debug.WriteLine("OnMouseUp 3");
//               BidirectionalAssociation association = ((BidirectionalAssociation)candidate.ModelElement);

//               if (BooleanQuestionDisplay.Show(_classShape.Store, $"Make {_modelClass.Name} an association class for {association.GetDisplayText()}?") == true)
//               {
//                  Debug.WriteLine("OnMouseUp 4");
//                  _classShape.AddAssociationClass(candidate);
//                  Complete(e.DiagramClientView);

//                  Debug.WriteLine("OnMouseUp 5");

//                  return;
//               }
//            }
//         }

//         Debug.WriteLine("OnMouseUp 6");
//         Cancel(e.DiagramClientView);
//      }

//      /// <summary>
//      /// Called when this MouseAction has entered the dragging state.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      /// <remarks>
//      /// The dragging state begins when the criteria for dragging has been satisfied.
//      /// Typically, the mouse cursor must move beyond a drag delta.
//      /// </remarks>
//      protected override void OnDraggingBegun(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDraggingBegun 0");
//         base.OnDraggingBegun(e);
//         Debug.WriteLine("OnDraggingBegun 1");
//      }

//      /// <summary>
//      /// Called when this MouseAction has exited the dragging state.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      /// <remarks>
//      /// The dragging state ends when a MouseUp event has been received or when a
//      /// Complete or Cancel event has been received.
//      /// </remarks>
//      protected override void OnDraggingEnded(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDraggingEnded 0");
//         base.OnDraggingEnded(e);
//         Debug.WriteLine("OnDraggingEnded 1");
//      }

//      /// <summary>
//      /// Called when this MouseAction's drag operation has completed.
//      /// </summary>
//      /// <param name="e">A MouseActionEventArgs that contains event data.</param>
//      protected override void OnDragCompleted(MouseActionEventArgs e)
//      {
//         Debug.WriteLine("OnDragCompleted 0");
//         base.OnDragCompleted(e);
//         Debug.WriteLine("OnDragCompleted 1");

//         List<BidirectionalConnector> candidates = _classShape.GetBidirectionalConnectorsUnderShape();

//         if (candidates.Any())
//         {
//            Debug.WriteLine("OnDragCompleted 2");

//            foreach (BidirectionalConnector candidate in candidates)
//            {
//               Debug.WriteLine("OnDragCompleted 3");
//               BidirectionalAssociation association = ((BidirectionalAssociation)candidate.ModelElement);

//               if (BooleanQuestionDisplay.Show(_classShape.Store, $"Make {_modelClass.Name} an association class for {association.GetDisplayText()}?") == true)
//               {
//                  Debug.WriteLine("OnDragCompleted 4");
//                  _classShape.AddAssociationClass(candidate);
//                  e.ActionRequest = ActionRequest.CompleteAction;

//                  Debug.WriteLine("OnDragCompleted 5");

//                  return;
//               }
//            }
//         }

//         Debug.WriteLine("OnDragCompleted 6");
//         e.ActionRequest = ActionRequest.CancelAction;
//      }

//      /// <summary>
//      /// Gets the cursor to display at the specified mouse position.
//      /// </summary>
//      /// <param name="currentCursor">The existing cursor.</param>
//      /// <param name="diagramClientView">The DiagramClientView requesting the cursor.</param>
//      /// <param name="mousePosition">The cursor position in world units relative to the top-left of the diagram.</param>
//      /// <returns>The cursor to display at the specified mouse position.</returns>
//      /// <remarks>
//      /// This method is called by the DiagramClientView if this MouseAction is active or
//      /// if it is the potential MouseAction.
//      /// </remarks>
//      /// <remarks>By default, this method returns the currentCursor.</remarks>
//      public override Cursor GetCursor(Cursor currentCursor, DiagramClientView diagramClientView, PointD mousePosition)
//      {
//         return Cursors.SizeAll;
//      }

//      /// <summary>
//      /// Called by the DiagramClientView to paint the feedback for the MouseAction.
//      /// </summary>
//      /// <param name="e">A DiagramPaintEventArgs that contains event data.</param>
//      public override void DoPaintFeedback(DiagramPaintEventArgs e)
//      {
//         Debug.WriteLine("DoPaintFeedback 0");
//         base.DoPaintFeedback(e);
//         Debug.WriteLine("DoPaintFeedback 1");

//         if (_modelClass != null && _modelClass.CanBecomeAssociationClass())
//         {
//            Debug.WriteLine("DoPaintFeedback 2");
//            List<BidirectionalConnector> connectors = _classShape.GetBidirectionalConnectorsUnderShape();
//            HighlightedShapesCollection highlightedShapes = e.View.HighlightedShapes;

//            if (connectors.Any())
//            {
//               Debug.WriteLine("DoPaintFeedback 3");

//               if (!highlightedShapes.Contains(new DiagramItem(_classShape)))
//               {
//                  Debug.WriteLine("DoPaintFeedback 4");
//                  highlightedShapes.Add(new DiagramItem(_classShape));
//                  _classShape.Invalidate();
//               }

//               foreach (BidirectionalConnector connector in connectors.Where(c => !highlightedShapes.Contains(new DiagramItem(c))))
//               {
//                  Debug.WriteLine("DoPaintFeedback 5");
//                  highlightedShapes.Add(new DiagramItem(connector));
//                  connector.Invalidate();
//               }
//            }
//            else
//            {
//               Debug.WriteLine("DoPaintFeedback 6");
//               highlightedShapes.Remove(new DiagramItem(_classShape));
//               _classShape.Invalidate();

//               foreach (BidirectionalConnector connector in connectors)
//               {
//                  Debug.WriteLine("DoPaintFeedback 7");
//                  highlightedShapes.Remove(new DiagramItem(connector));
//                  connector.Invalidate();
//               }
//            }
//         }
//      }
//   }
//}
