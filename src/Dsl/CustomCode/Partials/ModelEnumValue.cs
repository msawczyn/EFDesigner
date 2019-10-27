using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Validation;
using Sawczyn.EFDesigner.EFModel.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class ModelEnumValue : IModelElementInCompartment, IDisplaysWarning
   {
      private ModelEnum cachedParent;

      public IModelElementWithCompartments ParentModelElement => Enum;
      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;

      #region Warning display

      // set as methods to avoid issues around serialization

      private bool hasWarning;

      public bool GetHasWarningValue() => hasWarning;

      public void ResetWarning() => hasWarning = false;

      public void RedrawItem()
      {
         List<ShapeElement> shapeElements = PresentationViewsSubject.GetPresentation(ParentModelElement as ModelElement).OfType<ShapeElement>().ToList();
         foreach (ShapeElement shapeElement in shapeElements)
            shapeElement.Invalidate();
      }

      #endregion

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         if (Enum?.ModelRoot == null) return;

         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (Enum != null && modelRoot?.WarnOnMissingDocumentation == true && string.IsNullOrWhiteSpace(Summary))
         {
            context.LogWarning($"{Enum.Name}.{Name}: Enum value should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
         }
      }

      /// <summary>Called by the model before the element is deleted.</summary>
      protected override void OnDeleting()
      {
         base.OnDeleting();

         cachedParent = Enum;
      }

      /// <summary>
      /// Called by the model after the element has been deleted.
      /// </summary>
      protected override void OnDeleted()
      {
         base.OnDeleted();

         cachedParent?.SetFlagValues();
      }

      /// <summary>Returns a string that represents the current object.</summary>
      /// <returns>A string that represents the current object.</returns>
      public override string ToString() => Name + (string.IsNullOrEmpty(Value) ? string.Empty : $" = {Value}");
   }
}
