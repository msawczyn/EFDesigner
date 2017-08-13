using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Double-derived class to allow easier code customization.
   /// </summary>
   internal partial class EFModelCommandSet
   {
      private readonly Guid guidEFDiagramMenuCmdSet = new Guid("31178ecb-5da7-46cc-bd4a-ce4e5420bd3e");
      private const int grpidEFDiagram = 0x01001;
      private const int cmdidLayoutDiagram = 1;
      private const int cmdidHideShape = 2;
      private const int cmdidShowShape = 3;
      private const int cmdidGenerateCode = 4;
      private const int cmdidAddCodeProperties = 5;

      protected override IList<MenuCommand> GetMenuCommands()
      {
         IList<MenuCommand> commands = base.GetMenuCommands();

         DynamicStatusMenuCommand addAttributesCommand =
            new DynamicStatusMenuCommand(OnStatusAddProperties, OnMenuAddProperties, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddCodeProperties));
         commands.Add(addAttributesCommand);

         DynamicStatusMenuCommand layoutDiagramCommand =
            new DynamicStatusMenuCommand(OnStatusLayoutDiagram, OnMenuLayoutDiagram, new CommandID(guidEFDiagramMenuCmdSet, cmdidLayoutDiagram));
         commands.Add(layoutDiagramCommand);

         DynamicStatusMenuCommand hideShapeCommand =
            new DynamicStatusMenuCommand(OnStatusHideShape, OnMenuHideShape, new CommandID(guidEFDiagramMenuCmdSet, cmdidHideShape));
         commands.Add(hideShapeCommand);

         DynamicStatusMenuCommand showShapeCommand =
            new DynamicStatusMenuCommand(OnStatusShowShape, OnMenuShowShape, new CommandID(guidEFDiagramMenuCmdSet, cmdidShowShape));
         commands.Add(showShapeCommand);

         DynamicStatusMenuCommand generateCodeCommand =
            new DynamicStatusMenuCommand(OnStatusGenerateCode, OnMenuGenerateCode, new CommandID(guidEFDiagramMenuCmdSet, cmdidGenerateCode));
         commands.Add(generateCodeCommand);

         // Add more commands here.  
         return commands;
      }

      #region Add Properties

      private void OnStatusAddProperties(object sender, EventArgs e)
      {
         MenuCommand command = sender as MenuCommand;
         if (command != null)
         {
            command.Visible = true;
            command.Enabled = CurrentSelection.OfType<ClassShape>().Count() == 1;
         }
      }

      private void OnMenuAddProperties(object sender, EventArgs e)
      {
         NodeShape shapeElement = CurrentSelection.OfType<ClassShape>().FirstOrDefault();
         ModelClass element = shapeElement?.ModelElement as ModelClass;
         if (element != null)
         {
            AddCodeForm codeForm = new AddCodeForm(element);
            if (codeForm.ShowDialog() == DialogResult.OK)
            {
               using (Transaction tx = element.Store.TransactionManager.BeginTransaction("AddProperties"))
               {
                  element.Attributes.Clear();
                  IEnumerable<ModelAttribute> modelAttributes =
                     codeForm.Lines
                             .Select(s => ModelAttribute.Parse(element.ModelRoot, s))
                             .Where(attr => attr != null)
                             .Select(parseResult => new ModelAttribute(element.Store,
                                                                       new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name),
                                                                       new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String"),
                                                                       new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? false),
                                                                       new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength ?? 0),
                                                                       new PropertyAssignment(ModelAttribute.SetterVisibilityDomainPropertyId, parseResult.SetterVisibility ?? SetterAccessModifier.Public)
                                                                       ));
                  element.Attributes.AddRange(modelAttributes);
                  tx.Commit();
               }
            }
         }
      }

      #endregion OnMenuAddProperties
      #region Generate Code

      private void OnStatusGenerateCode(object sender, EventArgs e)
      {
         MenuCommand command = sender as MenuCommand;
         if (command != null)
         {
            // not yet. Need to work this some more.
            command.Visible = false; //true;
            command.Enabled = false; // IsDiagramSelected() && !IsCurrentDiagramEmpty();
         }
      }

      private void OnMenuGenerateCode(object sender, EventArgs e)
      {
         //DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
         //if (dte == null) return;

         //string activeDocumentFullName = dte.ActiveDocument.FullName;
         //string t4FullName = Path.ChangeExtension(activeDocumentFullName, "tt");

         //ITextTemplating t4 = ServiceProvider.GetService(typeof(STextTemplating)) as ITextTemplating;
         //string output = t4.ProcessTemplate("", File.ReadAllText(t4FullName));
      }

      #endregion Generate Code
      #region Show Shape

      private void OnStatusShowShape(object sender, EventArgs e)
      {
         MenuCommand command = sender as MenuCommand;
         if (command != null)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<ClassShape>().Any(s => !s.IsVisible) ||
                              childShapes.OfType<EnumShape>().Any(s => !s.IsVisible);
         }
      }

      private void OnMenuShowShape(object sender, EventArgs e)
      {
         NodeShape firstShape = CurrentSelection.OfType<NodeShape>().FirstOrDefault();
         if (firstShape != null)
         {
            using (Transaction tx = firstShape.Store.TransactionManager.BeginTransaction("HideShapes"))
            {
               LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
               foreach (ClassShape shape in childShapes.OfType<ClassShape>().Where(s => !s.IsVisible))
                  shape.Visible = true;
               foreach (EnumShape shape in childShapes.OfType<EnumShape>().Where(s => !s.IsVisible))
                  shape.Visible = true;
               foreach (ShapeElement shape in childShapes.Where(s => !s.IsVisible))
                  shape.Show();
               tx.Commit();
            }
         }
      }

      #endregion Show Shape
      #region Hide Shape

      private void OnStatusHideShape(object sender, EventArgs e)
      {
         MenuCommand command = sender as MenuCommand;
         if (command != null)
         {
            command.Visible = true;
            command.Enabled = CurrentSelection.OfType<ClassShape>().Any() || CurrentSelection.OfType<EnumShape>().Any();
         }
      }

      private void OnMenuHideShape(object sender, EventArgs e)
      {
         NodeShape firstShape = CurrentSelection.OfType<NodeShape>().FirstOrDefault();
         if (firstShape != null)
         {
            using (Transaction tx = firstShape.Store.TransactionManager.BeginTransaction("HideShapes"))
            {
               foreach (ClassShape shape in CurrentSelection.OfType<ClassShape>())
                  shape.Visible = false;
               foreach (EnumShape shape in CurrentSelection.OfType<EnumShape>())
                  shape.Visible = false;
               tx.Commit();
            }
         }
      }

      #endregion Hide Shape
      #region Layout Diagram

      private void OnStatusLayoutDiagram(object sender, EventArgs e)
      {
         MenuCommand command = sender as MenuCommand;
         if (command != null)
         {
            command.Visible = true;
            command.Enabled = IsDiagramSelected() && !IsCurrentDiagramEmpty();
         }
      }

      private void OnMenuLayoutDiagram(object sender, EventArgs e)
      {
         EFModelDiagram diagram = CurrentSelection.Cast<EFModelDiagram>().FirstOrDefault();
         if (diagram != null)
         {
            using (Transaction tx = diagram.Store.TransactionManager.BeginTransaction("ModelAutoLayout"))
            {
               diagram.AutoLayoutShapeElements(diagram.NestedChildShapes,
                  Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.VGRoutingStyle.VGRouteStraight,
                  Microsoft.VisualStudio.Modeling.Diagrams.GraphObject.PlacementValueStyle.VGPlaceSN,
                  false);
               tx.Commit();
            }
         }
      }

      #endregion Layout Diagram
   }
}
