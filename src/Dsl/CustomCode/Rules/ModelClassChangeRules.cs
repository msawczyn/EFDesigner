using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
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
         return ModelRoot.PluralizationService.IsSingular(root)
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

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
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

               break;

            case "TableName":
               if (string.IsNullOrEmpty(element.TableName))
                  element.TableName = MakeDefaultName(element.Name);

               if (store.ElementDirectory
                        .AllElements
                        .OfType<ModelClass>()
                        .Except(new[] { element })
                        .Any(x => x.TableName == element.TableName))
                  errorMessage = $"Table name '{element.TableName}' already in use";
               break;

            case "DbSetName":
               if (string.IsNullOrEmpty(element.DbSetName))
                  element.DbSetName = MakeDefaultName(element.Name);

               if (current.Name.ToLowerInvariant() != "paste" &&
                   (string.IsNullOrWhiteSpace(element.DbSetName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(element.DbSetName)))
                  errorMessage = "DbSet name must be a valid .NET identifier";

               else if (store.ElementDirectory
                        .AllElements
                        .OfType<ModelClass>()
                        .Except(new[] { element })
                        .Any(x => x.DbSetName == element.DbSetName))
                  errorMessage = $"DbSet name '{element.DbSetName}' already in use";

               break;

            case "Name":
               if (current.Name.ToLowerInvariant() != "paste" && 
                   (string.IsNullOrWhiteSpace(element.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(element.Name)))
                  errorMessage = "Name must be a valid .NET identifier";
               
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Except(new[] { element })
                             .Any(x => x.Name == element.Name))
                  errorMessage = $"Class name '{element.Name}' already in use";
               
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelEnum>()
                             .Any(x => x.Name == element.Name))
                  errorMessage = $"Class name '{element.Name}' already in use";
               
               else if (!string.IsNullOrEmpty((string)e.OldValue))
               {
                  string oldDefaultName = MakeDefaultName((string)e.OldValue);
                  string newDefaultName = MakeDefaultName(element.Name);

                  if (element.DbSetName == oldDefaultName)
                     element.DbSetName = newDefaultName;
                  if (element.TableName == oldDefaultName)
                     element.TableName = newDefaultName;
               }
               break;

            case "Namespace":
               if (current.Name.ToLowerInvariant() != "paste")
                  errorMessage = CommonRules.ValidateNamespace((string)e.NewValue, CodeGenerator.IsValidLanguageIndependentIdentifier);
               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            MessageBox.Show(errorMessage);
         }
      }
   }
}
