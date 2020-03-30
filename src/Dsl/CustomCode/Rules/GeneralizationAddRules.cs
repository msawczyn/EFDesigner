using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.TopLevelCommit)]
   internal class GeneralizationAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (element.IsInCircularInheritance())
         {
            ErrorDisplay.Show($"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause a circular reference.");
            current.Rollback();

            return;
         }

         List<string> superclassPropertyNames = element.Superclass.AllPropertyNames.ToList();

         List<string> nameClashes = element.Subclass
                                           .Attributes
                                           .Where(a => superclassPropertyNames.Contains(a.Name))
                                           .Select(a => a.Name)
                                           .Union(element.Subclass
                                                         .LocalNavigationProperties()
                                                         .Where(p => p.PropertyName != null && superclassPropertyNames.Contains(p.PropertyName))
                                                         .Select(p => p.PropertyName))
                                           .ToList();

         // remove attributes in subclass that are present in superclass IF they are completely identical (except for ModelClass, of course)
         for (int i = 0; i < nameClashes.Count; i++)
         {
            ModelAttribute subclassAttribute = element.Subclass.Attributes.First(a => a.Name == nameClashes[i]);
            ModelAttribute superclassAttribute = element.Superclass.AllAttributes.First(a => a.Name == nameClashes[i]);
            List<(string propertyName, object thisValue, object otherValue)> differences = superclassAttribute.GetDifferences(subclassAttribute);

            // ignore these differences if found
            differences.RemoveAll(x => x.propertyName == "ModelClass" || 
                                       x.propertyName == "Summary" || 
                                       x.propertyName == "Description");

            if (!differences.Any())
            {
               element.Subclass.Attributes.Remove(element.Subclass.Attributes.Single(a => a.Name == nameClashes[i]));
               nameClashes.RemoveAt(i--);
            }
         }

         // if any remain with the same name, it's an error
         if (nameClashes.Any())
         {
            string nameClashList = string.Join("\n   ", nameClashes);
            ErrorDisplay.Show($"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause name clashes. Resolve the following before setting the inheritance:\n   {nameClashList}");
            current.Rollback();
         }
      }
   }
}
