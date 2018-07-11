using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Sawczyn.EFDesigner.EFModel.CustomCode.Rules;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelClass), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelClassChangeRules : ChangeRule
   {
      private string MakeDefaultName(string root)
      {
         return ModelRoot.PluralizationService?.IsSingular(root) == true
            ? ModelRoot.PluralizationService.Pluralize(root)
            : root;
      }

      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelClass element = (ModelClass)e.ModelElement;
         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();

         switch (e.DomainProperty.Name)
         {
            case "IsDependentType":
               bool newIsStruct = (bool)e.NewValue;

               if (newIsStruct)
               {
                  List<Association> associations = store.ElementDirectory
                                                        .AllElements
                                                        .OfType<Association>()
                                                        .Where(a => (a.Source == element && a.SourceMultiplicity == Multiplicity.ZeroMany) ||
                                                                    (a.Target == element && a.TargetMultiplicity == Multiplicity.ZeroMany))
                                                        .ToList();

                  if (associations.Any())
                  {
                     List<string> classNameList = associations.Select(a => a.Target.Name).ToList();
                     if (classNameList.Count > 1)
                        classNameList[classNameList.Count - 1] = "and " + classNameList[classNameList.Count - 1];
                     string classNames = string.Join(", ", classNameList);

                     errorMessages.Add($"Can't have a 0..* association to a dependent type. Found 0..* link(s) with {classNames}");

                     break;
                  }

                  foreach (ModelAttribute modelAttribute in element.AllAttributes.Where(a => a.IsIdentity))
                     modelAttribute.IsIdentity = false;
               }

               break;

            case "IsAbstract":
               bool newIsAbstract = (bool)e.NewValue;

               foreach (ClassShape classShape in PresentationViewsSubject.GetPresentation(element).OfType<ClassShape>())
               {
                  if (newIsAbstract)
                  {
                     classShape.OutlineColor = Color.OrangeRed;
                     classShape.OutlineThickness = 0.02f;
                     classShape.OutlineDashStyle = element.ImplementNotify ? DashStyle.Dot : DashStyle.Dash;
                  }
                  else if (element.ImplementNotify)
                  {
                     classShape.OutlineColor = Color.CornflowerBlue;
                     classShape.OutlineThickness = 0.02f;
                     classShape.OutlineDashStyle = DashStyle.Dot;
                  }
                  else
                  {
                     classShape.OutlineColor = Color.Black;
                     classShape.OutlineThickness = 0.01f;
                     classShape.OutlineDashStyle = DashStyle.Solid;
                  }
               }

               break;

            case "ImplementNotify":
               bool newImplementNotify = (bool)e.NewValue;

               if (!element.IsAbstract) // IsAbstract takes precedence
               {
                  foreach (ClassShape classShape in PresentationViewsSubject.GetPresentation(element).OfType<ClassShape>())
                  {
                     if (newImplementNotify)
                     {
                        classShape.OutlineColor = Color.CornflowerBlue;
                        classShape.OutlineThickness = 0.02f;
                        classShape.OutlineDashStyle = DashStyle.Dot;
                     }
                     else
                     {
                        classShape.OutlineColor = Color.Black;
                        classShape.OutlineThickness = 0.01f;
                        classShape.OutlineDashStyle = DashStyle.Solid;
                     }
                  }
               }

               if (element.ImplementNotify)
               {
                  foreach (ModelAttribute modelAttribute in element.Attributes.Where(x => x.AutoProperty))
                     WarningDisplay.Show($"{modelAttribute.Name} is an autoproperty, so will not participate in INotifyPropertyChanged messages");
               }

               break;

            case "TableName":
               string newTableName = (string)e.NewValue;

               if (string.IsNullOrEmpty(newTableName))
                  element.TableName = MakeDefaultName(element.Name);

               if (store.ElementDirectory
                        .AllElements
                        .OfType<ModelClass>()
                        .Except(new[] {element})
                        .Any(x => x.TableName == newTableName))
                  errorMessages.Add($"Table name '{newTableName}' already in use");
               break;

            case "DbSetName":
               string newDbSetName = (string)e.NewValue;

               if (string.IsNullOrEmpty(newDbSetName))
                  element.DbSetName = MakeDefaultName(element.Name);

               if (current.Name.ToLowerInvariant() != "paste" &&
                   (string.IsNullOrWhiteSpace(newDbSetName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newDbSetName)))
                  errorMessages.Add("DbSet name must be a valid .NET identifier");

               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Except(new[] {element})
                             .Any(x => x.DbSetName == newDbSetName))
                  errorMessages.Add($"DbSet name '{newDbSetName}' already in use");

               break;

            case "Name":
               string newName = (string)e.NewValue;

               if (current.Name.ToLowerInvariant() != "paste" &&
                   (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName)))
                  errorMessages.Add("Name must be a valid .NET identifier");
               
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Except(new[] {element})
                             .Any(x => x.Name == newName))
                  errorMessages.Add($"Class name '{newName}' already in use by another class");
               
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelEnum>()
                             .Any(x => x.Name == newName))
                  errorMessages.Add($"Class name '{newName}' already in use by an enum");
               
               else if (!string.IsNullOrEmpty((string)e.OldValue))
               {
                  string oldDefaultName = MakeDefaultName((string)e.OldValue);
                  string newDefaultName = MakeDefaultName(newName);

                  if (element.DbSetName == oldDefaultName)
                     element.DbSetName = newDefaultName;
                  if (element.TableName == oldDefaultName)
                     element.TableName = newDefaultName;
               }
               break;

            case "Namespace":
               string newNamespace = (string)e.NewValue;
               if (current.Name.ToLowerInvariant() != "paste")
                  errorMessages.Add(CommonRules.ValidateNamespace(newNamespace, CodeGenerator.IsValidLanguageIndependentIdentifier));
               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();
         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(string.Join("; ", errorMessages));
         }
      }
   }
}
