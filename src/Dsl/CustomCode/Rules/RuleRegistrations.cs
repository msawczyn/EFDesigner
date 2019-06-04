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
         typeof(GeneralizationAddRules),
         typeof(GeneralizationChangeRules),
         typeof(GeneralizationDeleteRules),
         typeof(GeneralizationDeletingRules),
         typeof(ModelAttributeAddRules),
         typeof(ModelAttributeChangeRules),
         typeof(ModelClassAddRules),
         typeof(ModelClassChangeRules),
         typeof(ModelClassDeletingRules),
         typeof(ModelEnumChangeRules),
         typeof(ModelEnumValueAddRules),
         typeof(ModelEnumValueChangeRules),
         typeof(ModelRootChangeRules)
      };

      protected override Type[] GetCustomDomainModelTypes()
      {
         return base.GetCustomDomainModelTypes().Concat(RuleClasses).ToArray();
      }
   }
}
