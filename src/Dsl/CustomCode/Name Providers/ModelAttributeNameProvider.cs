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

      public static IList<ModelElement> Siblings(ModelAttribute modelAttribute)
      {
         List<ModelClass> inheritanceTree = modelAttribute.ModelClass.AllSuperclasses
                                                          .Union(modelAttribute.ModelClass.AllSubclasses)
                                                          .ToList();

         List<ModelAttribute> attributes = inheritanceTree.SelectMany(c => c.Attributes)
                                                          .Union(modelAttribute.ModelClass.AllAttributes)
                                                          .Except(new[] {modelAttribute})
                                                          .Distinct()
                                                          .ToList();

         List<Association> associations = modelAttribute.Store.GetAll<Association>()
                                                        .Where(association => inheritanceTree.Contains(association.Source))
                                                        .Distinct()
                                                        .ToList();

         return attributes.Cast<ModelElement>().Union(associations).ToList();
      }

      // ReSharper disable once RedundantAssignment
      protected override void SetUniqueNameCore(ModelElement element, string baseName, IDictionary<string, ModelElement> siblingNames)
      {

         siblingNames = Siblings(element as ModelAttribute).GroupBy(x => (x as ModelAttribute)?.Name ?? (x as Association)?.Name)
                                                           .ToDictionary(g => g.Key, g => g.First());
         base.SetUniqueNameCore(element, baseName, siblingNames);
      }

      protected override void CustomSetUniqueNameCore(ModelElement element, string baseName, IList<ModelElement> siblings)
      {
         base.CustomSetUniqueNameCore(element, baseName, Siblings(element as ModelAttribute));
      }

   }
}
