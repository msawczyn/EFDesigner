using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

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

         if (current.IsSerializing)
            return;

         if (element.IsInCircularInheritance())
         {
            current.Rollback();
            MessageBox.Show("That inheritance link would cause a circular reference.");
            return;
         }

         List<string> superclassPropertyNames = element.Superclass
                                                       .AllAttributes
                                                       .Select(a => a.Name)
                                                       .Union(element.Superclass
                                                                     .AllNavigationProperties()
                                                                     .Where(x => x.PropertyName != null)
                                                                     .Select(a => a.PropertyName))
                                                       .ToList();

         List<string> nameClashes = element.Subclass
                                           .Attributes
                                           .Where(a => superclassPropertyNames.Contains(a.Name))
                                           .Select(a => a.Name)
                                           .Union(element.Subclass
                                                         .LocalNavigationProperties()
                                                         .Where(p => p.PropertyName != null && superclassPropertyNames.Contains(p.PropertyName))
                                                         .Select(p => p.PropertyName))
                                           .ToList();

         // since we created the Id property, we'll remove it. Any other clashes are up to the user to resolve.
         if (nameClashes.Contains("Id") &&
             element.Subclass.Attributes.FirstOrDefault(a => a.Name == "Id")?.IsIdentity == true &&
             element.Superclass.AllAttributes.FirstOrDefault(a => a.Name == "Id")?.IsIdentity == true)
         {
            element.Subclass.Attributes.Remove(element.Subclass.Attributes.Single(a => a.Name == "Id"));
            nameClashes.Remove("Id");
         }

         if (nameClashes.Any())
         {
            current.Rollback();
            string nameClashList = string.Join(", ", nameClashes);
            MessageBox.Show("That inheritance link would cause name clashes. Resolve the following before setting the inheritance: " + nameClashList);
         }
      }
   }
}
