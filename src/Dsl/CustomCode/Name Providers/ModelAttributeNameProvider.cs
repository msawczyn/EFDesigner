using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public class ModelAttributeNameProvider : ElementNameProvider
   {
      public override void SetUniqueName(ModelElement element, ModelElement container, DomainRoleInfo embeddedDomainRole, string baseName)
      {
         base.SetUniqueName(element, container, embeddedDomainRole, "Property");
      }

      private IList<ModelElement> Siblings(ModelAttribute modelAttribute)
      {
         return modelAttribute.ModelClass.AllSuperclasses
                              .Union(modelAttribute.ModelClass.AllSubclasses)
                              .SelectMany(c => c.Attributes)
                              .Union(modelAttribute.ModelClass.AllAttributes)
                              .Except(new[] {modelAttribute})
                              .Distinct()
                              .Cast<ModelElement>()
                              .ToList();
      }

      // ReSharper disable once RedundantAssignment
      protected override void SetUniqueNameCore(ModelElement element, string baseName, IDictionary<string, ModelElement> siblingNames)
      {
         siblingNames = Siblings(element as ModelAttribute).GroupBy(x => ((ModelAttribute)x).Name)
                                                           .ToDictionary(g => g.Key, g => g.First());
         base.SetUniqueNameCore(element, baseName, siblingNames);
      }

      protected override void CustomSetUniqueNameCore(ModelElement element, string baseName, IList<ModelElement> siblings)
      {
         base.CustomSetUniqueNameCore(element, baseName, Siblings(element as ModelAttribute));
      }

   }
}
