using System;
using System.Linq;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelDomainModel
   {
      private static readonly Type[] RuleClasses =
      {
         typeof(AssociationAddRules),
         typeof(AssociationChangeRules),
         typeof(AssociationDeletingRules),
         typeof(BidirectionalAssociationAddRule),
         typeof(ClassShapeAddRules),
         typeof(EnumShapeAddRules),
         typeof(GeneralizationAddRules),
         typeof(GeneralizationChangeRules),
         typeof(GeneralizationDeletingRules),
         typeof(ModelAttributeAddRules),
         typeof(ModelAttributeChangeRules),
         typeof(ModelClassAddRules),
         typeof(ModelClassChangeRules),
         typeof(ModelClassDeletingRules),
         typeof(ModelDiagramDataAddRules),
         typeof(ModelDiagramDataChangeRules),
         typeof(ModelDiagramDataDeleteRules),
         typeof(ModelEnumChangeRules),
         typeof(ModelEnumValueAddRules),
         typeof(ModelEnumValueChangeRules),
         typeof(ModelRootChangeRules),
         typeof(UnidirectionalAssociationAddRule)
      };

      protected override Type[] GetCustomDomainModelTypes()
      {
         return base.GetCustomDomainModelTypes().Concat(RuleClasses).ToArray();
      }
   }
}
