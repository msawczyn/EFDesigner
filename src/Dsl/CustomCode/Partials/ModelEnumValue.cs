using Sawczyn.EFDesigner.EFModel.CustomCode.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class ModelEnumValue : IModelElementCompartmented
   {
      public IModelElementWithCompartments ParentModelElement => Enum;
      public string CompartmentName => this.GetFirstShapeElement().AccessibleName;
   }
}
