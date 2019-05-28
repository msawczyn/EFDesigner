using System.Linq;

using Microsoft.VisualStudio.Modeling.Validation;

namespace Sawczyn.EFDesigner.EFModel
{
   [ValidationState(ValidationState.Enabled)]
   public partial class BidirectionalAssociation
   {
      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation && string.IsNullOrWhiteSpace(SourceSummary))
         {
            context.LogWarning($"{Target.Name}.{SourcePropertyName}: Association end should be documented", "AWMissingSummary", this);
            hasWarning = true;
            RedrawItem();
            Source.RedrawItem();
            Target.RedrawItem();
         }
      }
   }
}
