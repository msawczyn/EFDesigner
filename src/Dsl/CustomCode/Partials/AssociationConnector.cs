using Microsoft.VisualStudio.Modeling.Diagrams;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract partial class AssociationConnector: IHasStore
   {
      /// <summary>
      /// Initializes style set resources for this shape type
      /// </summary>
      /// <param name="classStyleSet">The style set for this shape class</param>
      protected override void InitializeResources(StyleSet classStyleSet)
      {
         base.InitializeResources(classStyleSet);

         AssociateValueWith(Store, Association.SourceDeleteActionDomainPropertyId);
         AssociateValueWith(Store, Association.TargetDeleteActionDomainPropertyId);
         AssociateValueWith(Store, Association.SourceMultiplicityDomainPropertyId);
         AssociateValueWith(Store, Association.TargetMultiplicityDomainPropertyId);
         AssociateValueWith(Store, Association.NameDomainPropertyId);
         AssociateValueWith(Store, Association.FKPropertyNameDomainPropertyId);
         AssociateValueWith(Store, Association.PersistentDomainPropertyId);
      }

      public override bool HasToolTip => true;

      /// <summary>
      /// Gets the tooltip text for the PEL element under the cursor
      /// </summary>
      /// <param name="item">this contains the shape,field, and subfield under the cursor</param>
      /// <returns></returns>
      public override string GetToolTipText(DiagramItem item)
      {
         return item.Shape.ModelElement is Association association
                   ? association.GetDisplayText()
                   : string.Empty;
      }

      /// <summary>Called when a property changes.</summary>
      /// <param name="e">An EventArgs that contains the event data.</param>
      /// <remarks>
      /// This method will be called when the value changes for an
      /// IMS property that has been associated with a shape field.
      /// See ShapeField.AssociateValueWith for more detail.
      /// </remarks>
      /// <remarks>
      /// This method should be called from the setter of any CLR property
      /// (i.e., a non-IMS property on this shape) that has been associated
      /// with a shape field.
      /// See ShapeField.AssociateValueWith for more detail.
      /// </remarks>
      protected override void OnAssociatedPropertyChanged(PropertyChangedEventArgs e)
      {
         switch (e.PropertyName)
         {
            case "FKPropertyName":
               PresentationHelper.UpdateAssociationDisplay(this);
               break;

            case "Persistent":
               PresentationHelper.UpdateAssociationDisplay(this);
               break;

            case "SourceDeleteAction":
               // ReSharper disable once ArgumentsStyleOther
               PresentationHelper.UpdateAssociationDisplay(this, sourceDeleteAction: (DeleteAction)e.NewValue);
               break;

            case "TargetDeleteAction":
               PresentationHelper.UpdateAssociationDisplay(this, targetDeleteAction: (DeleteAction)e.NewValue);
               break;

            case "SourceMultiplicity":
               PresentationHelper.UpdateAssociationDisplay(this);
               break;

            case "TargetMultiplicity":
               PresentationHelper.UpdateAssociationDisplay(this);
               break;
         }

         base.OnAssociatedPropertyChanged(e);
      }

      /// <summary>
      /// Calculates highlight luminosity based on:
      /// 	if L &gt;= 160, then L = L * 0.9
      /// 	else, L += 40.
      /// </summary>
      /// <param name="currentLuminosity">Current luminosity</param>
      /// <param name="view">Design surface</param>
      /// <returns>New luminosity value.</returns>
      protected override int ModifyLuminosity(int currentLuminosity, DiagramClientView view)
      {
         if (!view.HighlightedShapes.Contains(new DiagramItem(this)))
            return currentLuminosity;

         int baseCalculation = base.ModifyLuminosity(currentLuminosity, view);

         // black (luminosity == 0) will be changed to luminosity 40, which doesn't show up.
         // so if it's black we're highlighting, return 130, since that looks ok.
         return baseCalculation == 40 ? 130 : baseCalculation;
      }
   }
}
