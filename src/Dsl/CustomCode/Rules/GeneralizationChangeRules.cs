using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(Generalization), FireTime = TimeToFire.TopLevelCommit)]
   internal class GeneralizationChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         Generalization element = (Generalization)e.ModelElement;
         if (element.IsDeleted)
            return;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing || ModelRoot.BatchUpdating)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         switch (e.DomainProperty.Name)
         {
            case "Superclass":
            case "Subclass":

               if (!element.IsInCircularInheritance())
               {
                  ErrorDisplay.Show(store, $"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause a circular reference.");
                  current.Rollback();

                  return;
               }

               if (element.Superclass != null)
               {
                  List<string> superclassPropertyNames = element.Superclass
                                                                .AllAttributes
                                                                .Select(a => a.Name)
                                                                .Union(element.Superclass
                                                                              .AllNavigationProperties()
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

                  // the Add rule removes the subclass Id property if it's causing a name class, but we won't do that in the change rule

                  if (nameClashes.Any())
                  {
                     string nameClashList = string.Join("\n   ", nameClashes);
                     ErrorDisplay.Show(store, $"{element.Subclass.Name} -> {element.Superclass.Name}: That inheritance link would cause name clashes. Resolve the following before setting the inheritance:\n   " + nameClashList);
                     current.Rollback();
                  }
               }

               break;
         }
      }
   }
}
