using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{

   //internal class ColorCache
   //{
   //   public Color FillColor { get; set; }
   //   public Color TextColor { get; set; }
   //}

   public partial class ClassShape : IHighlightFromModelExplorer, IMouseActionTarget
   {
      //internal ColorCache ColorCache = null;

      private static readonly Dictionary<bool, Dictionary<SetterAccessModifier, Bitmap>> AttributeGlyphs =
         new Dictionary<bool, Dictionary<SetterAccessModifier, Bitmap>>
         {
            {true, new Dictionary<SetterAccessModifier, Bitmap>
                   {
                      { SetterAccessModifier.Public, Resources.Public },
                      { SetterAccessModifier.Protected, Resources.Protected},
                      { SetterAccessModifier.Internal, Resources.Internal}
                   }},
            {false, new Dictionary<SetterAccessModifier, Bitmap>
                    {
                       { SetterAccessModifier.Public, Resources.Calculated },
                       { SetterAccessModifier.Protected, Resources.CalculatedProtected},
                       { SetterAccessModifier.Internal, Resources.CalculatedInternal}
                    }}
         };

      private static readonly Dictionary<Multiplicity, Dictionary<Multiplicity, Bitmap>> AssociationGlyphs =
         new Dictionary<Multiplicity, Dictionary<Multiplicity, Bitmap>>
         {
            {Multiplicity.ZeroOne, new Dictionary<Multiplicity, Bitmap>
                                   {
                                      {Multiplicity.ZeroOne, Resources.Cardinality_0_0},
                                      {Multiplicity.One, Resources.Cardinality_0_1},
                                      {Multiplicity.ZeroMany, Resources.Cardinality_0_many},
                                   }},
            {Multiplicity.One, new Dictionary<Multiplicity, Bitmap>
                               {
                                  {Multiplicity.ZeroOne, Resources.Cardinality_1_0},
                                  {Multiplicity.One, Resources.Cardinality_1_1},
                                  {Multiplicity.ZeroMany, Resources.Cardinality_1_many},
                               }},
            {Multiplicity.ZeroMany, new Dictionary<Multiplicity, Bitmap>
                                    {
                                       {Multiplicity.ZeroOne, Resources.Cardinality_many_0},
                                       {Multiplicity.One, Resources.Cardinality_many_1},
                                       {Multiplicity.ZeroMany, Resources.Cardinality_many_many},
                                    }},
         };

      /// <summary>
      /// Exposes NodeShape Collapse() function to DSL's context menu
      /// </summary>
      public void CollapseShape() => SetIsExpandedValue(false);

      /// <summary>
      /// Exposes NodeShape Expand() function to DSL's context menu
      /// </summary>
      public void ExpandShape() => SetIsExpandedValue(true);

      protected override CompartmentMapping[] GetCompartmentMappings(Type melType)
      {
         CompartmentMapping[] mappings = base.GetCompartmentMappings(melType);

         // Each item in the each compartment will call the appropriate method to determine its icon.
         // This happens any time the element's presentation element invalidates.
         foreach (ElementListCompartmentMapping mapping in mappings.OfType<ElementListCompartmentMapping>().Where(m => m.CompartmentId == "AttributesCompartment"))
            mapping.ImageGetter = GetAttributePropertyImage;
         foreach (ElementListCompartmentMapping mapping in mappings.OfType<ElementListCompartmentMapping>().Where(m => m.CompartmentId == "AssociationsCompartment"))
            mapping.ImageGetter = GetAssociationPropertyImage;
         foreach (ElementListCompartmentMapping mapping in mappings.OfType<ElementListCompartmentMapping>().Where(m => m.CompartmentId == "SourcesCompartment"))
            mapping.ImageGetter = GetSourcesPropertyImage;

         return mappings;
      }

      private Image GetAssociationPropertyImage(ModelElement element)
      {
         ModelRoot modelRoot = element.Store.ModelRoot();
         if (element is Association association)
         {
            if (modelRoot.ShowWarningsInDesigner && association.GetHasWarningValue())
               return Resources.Warning;

            return AssociationGlyphs[association.SourceMultiplicity][association.TargetMultiplicity];
         }

         return Resources.Spacer;
      }

      private Image GetSourcesPropertyImage(ModelElement element)
      {
         ModelRoot modelRoot = element.Store.ModelRoot();
         if (element is BidirectionalAssociation association)
         {
            if (modelRoot.ShowWarningsInDesigner && association.GetHasWarningValue())
               return Resources.Warning;

            return AssociationGlyphs[association.TargetMultiplicity][association.SourceMultiplicity];
         }

         return Resources.Spacer;
      }

      private Image GetAttributePropertyImage(ModelElement element)
      {
         ModelRoot modelRoot = element.Store.ModelRoot();
         if (element is ModelAttribute attribute)
         {
            if (modelRoot.ShowWarningsInDesigner && attribute.GetHasWarningValue())
               return Resources.Warning;

            // ReSharper disable once ConvertIfStatementToReturnStatement
            if (attribute.IsIdentity)
               return Resources.Identity;

            return AttributeGlyphs[attribute.Persistent][attribute.SetterVisibility];
         }

         return Resources.Spacer;
      }

      #region Drag/drop model attributes

      /// <summary>
      ///    Model element that is being dragged.
      /// </summary>
      private static ModelAttribute dragStartElement;

      /// <summary>
      ///    Absolute bounds of the compartment, used to set the cursor.
      /// </summary>
      private static RectangleD compartmentBounds;

      /// <summary>
      ///    Remember which item the mouse was dragged from.
      ///    We don't create an Action immediately, as this would inhibit the
      ///    inline text editing feature. Instead, we just remember the details
      ///    and will create an Action when/if the mouse moves off this list item.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Compartment_MouseDown(object sender, DiagramMouseEventArgs e)
      {
         dragStartElement = e.HitDiagramItem.RepresentedElements.OfType<ModelAttribute>().FirstOrDefault();
         compartmentBounds = e.HitDiagramItem.Shape.AbsoluteBoundingBox;
      }

      /// <summary>
      ///    When the mouse moves away from the initial list item, but still inside the compartment,
      ///    create an Action to supervise the cursor and handle subsequent mouse events.
      ///    Transfer the details of the initial mouse position to the Action.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Compartment_MouseMove(object sender, DiagramMouseEventArgs e)
      {
         if (dragStartElement != null && dragStartElement != e.HitDiagramItem.RepresentedElements.OfType<ModelAttribute>().FirstOrDefault())
         {
            e.DiagramClientView.ActiveMouseAction = new CompartmentDragMouseAction<ClassShape>(dragStartElement, this, compartmentBounds);
            dragStartElement = null;
         }
      }

      /// <summary>
      ///    User has released the mouse button.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void Compartment_MouseUp(object sender, DiagramMouseEventArgs e) => dragStartElement = null;

      /// <summary>
      ///    Called by the Action when the user releases the mouse.
      ///    If we are still on the same compartment but in a different list item,
      ///    move the starting item to the position of the current one.
      /// </summary>
      /// <param name="dragFrom"></param>
      /// <param name="e"></param>
      public void DoMouseUp(ModelElement dragFrom, DiagramMouseEventArgs e)
      {
         // Original or "from" item:
#pragma warning disable IDE0019 // Use pattern matching
         ModelAttribute dragFromElement = dragFrom as ModelAttribute;
#pragma warning restore IDE0019 // Use pattern matching

         // Current or "to" item:
         ModelAttribute dragToElement = e.HitDiagramItem.RepresentedElements.OfType<ModelAttribute>().FirstOrDefault();

         if (dragFromElement != null && dragToElement != null)
         {
            // Find the common parent model element, and the relationship links:
            ElementLink parentToLink = GetEmbeddingLink(dragToElement);
            ElementLink parentFromLink = GetEmbeddingLink(dragFromElement);

            if (parentToLink != parentFromLink && parentFromLink != null && parentToLink != null)
            {
               // Get the static relationship and role (= end of relationship):
               DomainRelationshipInfo relationshipFrom = parentFromLink.GetDomainRelationship();
               DomainRoleInfo parentFromRole = relationshipFrom.DomainRoles[0];

               // Get the node in which the element is embedded, usually the element displayed in the shape:
#pragma warning disable IDE0019 // Use pattern matching
               ModelClass parentFrom = parentFromLink.LinkedElements[0] as ModelClass;
#pragma warning restore IDE0019 // Use pattern matching

               // Same again for the target:
               DomainRelationshipInfo relationshipTo = parentToLink.GetDomainRelationship();
               DomainRoleInfo parentToRole = relationshipTo.DomainRoles[0];

               // Mouse went down and up in same parent and same compartment:
               if (parentFrom != null && parentToLink.LinkedElements[0] is ModelClass parentTo && parentTo == parentFrom && relationshipTo == relationshipFrom)
               {
                  // Find index of target position:
                  int newIndex = parentToRole.GetElementLinks(parentTo).IndexOf(parentToLink);

                  if (newIndex >= 0)
                  {
                     using (Transaction t = parentFrom.Store.TransactionManager.BeginTransaction("Move list item"))
                     {
                        parentFromLink.MoveToIndex(parentFromRole, newIndex);
                        // HACK
                        //parentTo.SetFlagValues();
                        t.Commit();
                     }
                  }
               }
            }
         }
      }

      /// <summary>
      ///    Attach mouse listeners to the compartments for the shape.
      ///    This is called once per compartment shape.
      ///    The base method creates the compartments for this shape.
      /// </summary>
      public override void EnsureCompartments()
      {
         base.EnsureCompartments();

         foreach (Compartment compartment in NestedChildShapes.OfType<Compartment>())
         {
            compartment.MouseDown += Compartment_MouseDown;
            compartment.MouseUp += Compartment_MouseUp;
            compartment.MouseMove += Compartment_MouseMove;
         }
      }

      /// <summary>
      ///    Get the embedding link to this element.
      ///    Assumes there is no inheritance between embedding relationships.
      ///    (If there is, you need to make sure you've got the relationship that is represented in the shape compartment.)
      /// </summary>
      /// <param name="child"></param>
      /// <returns></returns>
      private ElementLink GetEmbeddingLink(ModelAttribute child) => child.GetDomainClass()
                     .AllEmbeddedByDomainRoles
                     .SelectMany(role => role.OppositeDomainRole.GetElementLinks(child))
                     .FirstOrDefault();

      /// <summary>
      ///    Forget the source item if mouse up occurs outside the compartment.
      /// </summary>
      /// <param name="e"></param>
      public override void OnMouseUp(DiagramMouseEventArgs e)
      {
         base.OnMouseUp(e);
         dragStartElement = null;
      }

      #endregion

      /// <summary>
      /// Set when DocData is loaded. If non-null, calling this action will open the generated code file, if present
      /// </summary>
      public static Func<ModelClass, bool> OpenCodeFile { get; set; }

      /// <summary>
      /// If non-null, calling this method will execute code generation for the model
      /// </summary>
      public static Action ExecCodeGeneration;

      /// <summary>Called by the control's OnDoubleClick()</summary>
      /// <param name="e">A DiagramPointEventArgs that contains event data.</param>
      public override void OnDoubleClick(DiagramPointEventArgs e)
      {
         base.OnDoubleClick(e);

         if (OpenCodeFile != null)
         {
            ModelClass modelClass = (ModelClass)ModelElement;

            if (OpenCodeFile(modelClass))
               return;

            if (ExecCodeGeneration != null && QuestionDisplay.Show($"Can't open generated file for {modelClass.Name}. It may not have been generated yet. Do you want to generate the code now?") == true)
            {
               ExecCodeGeneration();

               if (OpenCodeFile(modelClass))
                  return;
            }

            ErrorDisplay.Show($"Can't open generated file for {modelClass.Name}");
         }
      }
   }
}
