using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelDiagram: IHasStore
   {
      public override void OnInitialize()
      {
         base.OnInitialize();
         ModelRoot modelRoot = Store.ModelRoot();

         // because we can hide elements, line routing looks odd when it thinks it's jumping over lines
         // that really aren't visible. Since replacing the routing algorithm is too hard (impossible?)
         // let's just stop it from showing jumps at all. A change to the highlighting on mouseover
         // makes it easier to see which lines are which in complex diagrams, so this doesn't hurt anything.
         RouteJumpType = VGPageLineJumpCode.NoJumps;

         ShowGrid = modelRoot?.ShowGrid ?? true;
         GridColor = modelRoot?.GridColor ?? Color.Gainsboro;
         SnapToGrid = modelRoot?.SnapToGrid ?? true;
         GridSize = modelRoot?.GridSize ?? 0.125;

      }

      /// <summary>
      /// Called during view fixup to ask the parent whether a shape should be created for the given child element.
      /// </summary>
      /// <remarks>
      /// Always return true, since we assume there is only one diagram per model file for DSL scenarios.
      /// </remarks>
      protected override bool ShouldAddShapeForElement(ModelElement element)
      {
         ModelElement parent = null;

         // for attributes and enum values, the we should add the shape if its parent is on the diagram
         if (element is ModelAttribute modelAttribute)
            parent = modelAttribute.ModelClass;

         if (element is ModelEnumValue enumValue)
            parent = enumValue.Enum;

         bool result =
            ForceAddShape // we've made the decision somewhere else that this shape should be added 
         || IsDropping // we're dropping a file from Solution Explorer or File Explorer
         || base.ShouldAddShapeForElement(element) // the built-in rules say to do this
         || DisplayedElements.Contains(element) // the serialized diagram has this element present (other rules should prevent duplication)
         || (parent != null && DisplayedElements.Contains(parent)) // the element's parent is on this diagram
         || (element is ElementLink link && link.LinkedElements.All(linkedElement => DisplayedElements.Contains(linkedElement))); // adding a link and both of the linkk's end nodes are in this diagram

         return result;
      }

      public static bool IsDropping { get; private set; }
      public bool ForceAddShape { get; set; }

      public override void OnDragOver(DiagramDragEventArgs diagramDragEventArgs)
      {
         base.OnDragOver(diagramDragEventArgs);

         if (diagramDragEventArgs.Handled)
            return;

         if (diagramDragEventArgs.Data.GetData("Sawczyn.EFDesigner.EFModel.ModelEnum") is ModelEnum
          || diagramDragEventArgs.Data.GetData("Sawczyn.EFDesigner.EFModel.ModelClass") is ModelClass
          || IsAcceptableDropItem(diagramDragEventArgs))
            diagramDragEventArgs.Effect = DragDropEffects.Copy;
         else
            diagramDragEventArgs.Effect = DragDropEffects.None;
      }

      /// <summary>
      /// Used for dropping data originating outside of VStudio
      /// </summary>
      /// <param name="diagramDragEventArgs"></param>
      /// <returns></returns>
      private bool IsAcceptableDropItem(DiagramDragEventArgs diagramDragEventArgs)
      {
         IsDropping = (diagramDragEventArgs.Data.GetData("Text") is string filenames1
                    && filenames1.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries) is string[] filenames2
                    && filenames2.All(File.Exists))
                   || (diagramDragEventArgs.Data.GetData("FileDrop") is string[] filenames3 && filenames3.All(File.Exists));

         return IsDropping;
      }

      private List<ModelElement> DisplayedElements => NestedChildShapes.Select(nestedShape => nestedShape.ModelElement).ToList();

      public static ShapeElement AddExistingModelElement(EFModelDiagram diagram, ModelElement element)
      {
         if (diagram == null)
            return null;

         if (diagram.NestedChildShapes.All(s => s.ModelElement != element))
         {
            using (Transaction t = element.Store.TransactionManager.BeginTransaction())
            {
               try
               {
                  diagram.ForceAddShape = true;
                  FixUpAllDiagrams.FixUp(diagram, element);

                  // find all element links that are attached to our element where the ends are in the diagram but the link isn't already in the diagram
                  List<ElementLink> elementLinks = element.Store.GetAll<ElementLink>()
                                                          .Where(link => link.LinkedElements.Contains(element)
                                                                      && link.LinkedElements.All(linkedElement => diagram.DisplayedElements.Contains(linkedElement))
                                                                      && !diagram.DisplayedElements.Contains(link))
                                                          .ToList();

                  foreach (ElementLink elementLink in elementLinks)
                     FixUpAllDiagrams.FixUp(diagram, elementLink);

                  t.Commit();
               }
               finally
               {
                  diagram.ForceAddShape = false;
               }
            }
         }
         
         return diagram.NestedChildShapes.FirstOrDefault(s => s.ModelElement == element);
      }

      public override void OnDragDrop(DiagramDragEventArgs diagramDragEventArgs)
      {
         // came from model explorer?
         ModelElement element = (diagramDragEventArgs.Data.GetData("Sawczyn.EFDesigner.EFModel.ModelClass") as ModelElement)
                             ?? (diagramDragEventArgs.Data.GetData("Sawczyn.EFDesigner.EFModel.ModelEnum") as ModelElement);

         if (element != null)
         {
               ShapeElement newShape = AddExistingModelElement(this, element);

               using (Transaction t = element.Store.TransactionManager.BeginTransaction("Moving pasted shapes"))
               {
                  if (newShape is NodeShape nodeShape)
                     nodeShape.Location = diagramDragEventArgs.MousePosition;

                  t.Commit();
               }
         }
         else
         {
            if (IsDropping)
            {
               string[] filenames;

               if (diagramDragEventArgs.Data.GetData("Text") is string filename)
                  filenames = filename.Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
               else if (diagramDragEventArgs.Data.GetData("FileDrop") is string[] filenames1)
                  filenames = filenames1;
               else
               {
                  ErrorDisplay.Show($"Unexpected error dropping files. Please create an issue in Github.");
                  IsDropping = false;

                  return;
               }

               string[] existingFiles = filenames.Where(File.Exists).ToArray();
               FileDropHelper.HandleMultiDrop(Store, existingFiles);
               string[] missingFiles = filenames.Except(existingFiles).ToArray();

               if (missingFiles.Any())
               {
                  if (missingFiles.Length > 1)
                     missingFiles[missingFiles.Length - 1] = "and " + missingFiles[missingFiles.Length - 1];

                  ErrorDisplay.Show($"Can't find files {string.Join(", ", missingFiles)}");
               }

               IsDropping = false;
            }
            else
            {
               try
               {
                  base.OnDragDrop(diagramDragEventArgs);
               }
               catch (ArgumentException)
               {
                  // ignore. byproduct of multiple diagrams
               }
            }
         }
      }

      /// <summary>Called by the control's OnMouseUp().</summary>
      /// <param name="e">A DiagramMouseEventArgs that contains event data.</param>
      public override void OnMouseUp(DiagramMouseEventArgs e)
      {
         IsDropping = false;
         base.OnMouseUp(e);
      }
   }
}
