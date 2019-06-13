using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.LocalCommit)]
   internal class GeneralizationAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         Generalization element = (Generalization)e.ModelElement;
         Store store = element.Store;
         ModelRoot modelRoot = store.ModelRoot();
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (element.Subclass.IsReadOnly && !modelRoot.BypassReadOnlyChecks)
         {
            ErrorDisplay.Show($"{element.Subclass.Name} is read-only; can't change its base class.");
            current.Rollback();

            return;
         }

         if (element.IsInCircularInheritance())
         {
            ErrorDisplay.Show($"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause a circular reference.");
            current.Rollback();

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
            string nameClashList = string.Join("\n   ", nameClashes);
            ErrorDisplay.Show($"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause name clashes. Resolve the following before setting the inheritance:\n   {nameClashList}");
            current.Rollback();

            return;
         }

         // make sure identity associations are correct (if necessary)
         IdentityHelper identityHelper = new IdentityHelper(store.ModelRoot());
         identityHelper.FixupIdentityAssociations();
      }
   }
}
