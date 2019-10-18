using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract partial class AssociationConnector
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
         AssociateValueWith(Store, Association.PersistentDomainPropertyId);
         //AssociateValueWith(Store, Association.TargetPropertyNameDomainPropertyId);
      }

      /// <summary>
      /// Calculates highlight luminosity based on:
      /// 	if L &gt;= 160, then L = L * 0.9
      /// 	else, L += 40.
      /// </summary>
      /// <param name="currentLuminosity">Current luminosity</param>
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
         if (ModelElement is Association association)
         {
            switch (e.PropertyName)
            {
               case "TargetPropertyName":
                  PresentationHelper.UpdateAssociationDisplay(association);
                  break;
               case "SourceDeleteAction":
                  PresentationHelper.UpdateAssociationDisplay(association, sourceDeleteAction: (DeleteAction)e.NewValue);
                  break;
               case "TargetDeleteAction":
                  PresentationHelper.UpdateAssociationDisplay(association, targetDeleteAction: (DeleteAction)e.NewValue);
                  break;
               case "SourceMultiplicity":
                  PresentationHelper.UpdateAssociationDisplay(association, sourceMultiplicity: (Multiplicity)e.NewValue);
                  break;
               case "TargetMultiplicity":
                  PresentationHelper.UpdateAssociationDisplay(association, targetMultiplicity: (Multiplicity)e.NewValue);
                  break;
               case "Persistent":
                  PresentationHelper.UpdateAssociationDisplay(association);
                  break;
            }
         }

         base.OnAssociatedPropertyChanged(e);
      }

   }
}
