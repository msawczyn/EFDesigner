using System.CodeDom.Compiler;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

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
               break;

            case "DbSetName":
               if (string.IsNullOrEmpty(element.DbSetName))
                  element.DbSetName = MakeDefaultName(element.Name);
               break;

            case "Name":
               string newName = element.Name;
               string oldName = (string)e.OldValue;

               if (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName))
                  errorMessage = "Name must be a valid .NET identifier";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelClass>()
                             .Except(new[] { element })
                             .Any(x => x.Name == newName))
                  errorMessage = "Class name already in use";
               else if (store.ElementDirectory
                             .AllElements
                             .OfType<ModelEnum>()
                             .Any(x => x.Name == newName))
                  errorMessage = "Class name already in use";
               else if (!string.IsNullOrEmpty(oldName))
               {
                  string oldDefaultName = MakeDefaultName(oldName);
                  string newDefaultName = MakeDefaultName(newName);

                  if (element.DbSetName == oldDefaultName)
                     element.DbSetName = newDefaultName;
                  if (element.TableName == oldDefaultName)
                     element.TableName = newDefaultName;
               }
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
