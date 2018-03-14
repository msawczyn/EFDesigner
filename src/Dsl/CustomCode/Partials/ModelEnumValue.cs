using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using Sawczyn.EFDesigner.EFModel.CustomCode.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class ModelEnumValue : IModelElementCompartmented
   {
      public IModelElementWithCompartments ParentModelElement => Enum;
      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;

      [ValidationMethod(ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Menu)]
      // ReSharper disable once UnusedMember.Local
      private void SummaryDescriptionIsEmpty(ValidationContext context)
      {
         ModelRoot modelRoot = Store.ElementDirectory.FindElements<ModelRoot>().FirstOrDefault();
         if (modelRoot.WarnOnMissingDocumentation)
         {
            if (string.IsNullOrWhiteSpace(Summary))
               context.LogWarning($"{Enum.Name}.{Name}: Enum value should be documented", "AWMissingSummary", this);
         }
      }
   }
}
