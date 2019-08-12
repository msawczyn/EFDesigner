using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Sawczyn.EFDesigner.EFModel.Extensions;

// ReSharper disable InconsistentNaming

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   ///    Double-derived class to allow easier code customization.
   /// </summary>
   internal partial class EFModelCommandSet
   {
      #region Identifiers

      // ReSharper disable once UnusedMember.Local
      private const int grpidEFDiagram = 0x01001;

      private const int cmdidFind = 0x0011;
      private const int cmdidLayoutDiagram = 0x0012;
      private const int cmdidHideShape = 0x0013;
      private const int cmdidShowShape = 0x0014;
      private const int cmdidGenerateCode = 0x0015;
      private const int cmdidAddCodeProperties = 0x0016;
      private const int cmdidSaveAsImage = 0x0017;
      private const int cmdidLoadNuGet = 0x0018;
      private const int cmdidAddCodeValues = 0x0019;
      private const int cmdidExpandSelected = 0x001A;
      private const int cmdidCollapseSelected = 0x001B;
      private const int cmdidMergeAssociations = 0x001C;
      private const int cmdidSplitAssociation = 0x001D;

      private const int cmdidSelectClasses = 0x0101;
      private const int cmdidSelectEnums = 0x0102;
      private const int cmdidSelectAssocs = 0x0103;
      private const int cmdidSelectUnidir = 0x0104;
      private const int cmdidSelectBidir = 0x0105;

      private readonly Guid guidEFDiagramMenuCmdSet = new Guid("31178ecb-5da7-46cc-bd4a-ce4e5420bd3e");

      #endregion Identifiers

      protected override IList<MenuCommand> GetMenuCommands()
      {
         IList<MenuCommand> commands = base.GetMenuCommands();

         DynamicStatusMenuCommand findCommand =
            new DynamicStatusMenuCommand(OnStatusFind, OnMenuFind, new CommandID(guidEFDiagramMenuCmdSet, cmdidFind));

         commands.Add(findCommand);

         DynamicStatusMenuCommand addAttributesCommand =
            new DynamicStatusMenuCommand(OnStatusAddProperties, OnMenuAddProperties, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddCodeProperties));

         commands.Add(addAttributesCommand);

         DynamicStatusMenuCommand addValuesCommand =
            new DynamicStatusMenuCommand(OnStatusAddValues, OnMenuAddValues, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddCodeValues));

         commands.Add(addValuesCommand);

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

         DynamicStatusMenuCommand saveAsImageCommand =
            new DynamicStatusMenuCommand(OnStatusSaveAsImage, OnMenuSaveAsImage, new CommandID(guidEFDiagramMenuCmdSet, cmdidSaveAsImage));

         commands.Add(saveAsImageCommand);

         DynamicStatusMenuCommand loadNuGetCommand =
            new DynamicStatusMenuCommand(OnStatusLoadNuGet, OnMenuLoadNuGet, new CommandID(guidEFDiagramMenuCmdSet, cmdidLoadNuGet));

         commands.Add(loadNuGetCommand);

         DynamicStatusMenuCommand selectClassesCommand =
            new DynamicStatusMenuCommand(OnStatusSelectClasses, OnMenuSelectClasses, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectClasses));

         commands.Add(selectClassesCommand);

         DynamicStatusMenuCommand selectEnumsCommand =
            new DynamicStatusMenuCommand(OnStatusSelectEnums, OnMenuSelectEnums, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectEnums));

         commands.Add(selectEnumsCommand);

         DynamicStatusMenuCommand selectAssocsCommand =
            new DynamicStatusMenuCommand(OnStatusSelectAssocs, OnMenuSelectAssocs, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectAssocs));

         commands.Add(selectAssocsCommand);

         DynamicStatusMenuCommand selectUnidirCommand =
            new DynamicStatusMenuCommand(OnStatusSelectUnidir, OnMenuSelectUnidir, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectUnidir));

         commands.Add(selectUnidirCommand);

         DynamicStatusMenuCommand selectBidirCommand =
            new DynamicStatusMenuCommand(OnStatusSelectBidir, OnMenuSelectBidir, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectBidir));

         commands.Add(selectBidirCommand);

         DynamicStatusMenuCommand expandSelectedCommand =
            new DynamicStatusMenuCommand(OnStatusExpandSelected, OnMenuExpandSelected, new CommandID(guidEFDiagramMenuCmdSet, cmdidExpandSelected));

         commands.Add(expandSelectedCommand);

         DynamicStatusMenuCommand collapseSelectedCommand =
            new DynamicStatusMenuCommand(OnStatusCollapseSelected, OnMenuCollapseSelected, new CommandID(guidEFDiagramMenuCmdSet, cmdidCollapseSelected));

         commands.Add(collapseSelectedCommand);

         DynamicStatusMenuCommand mergeAssociationsCommand =
            new DynamicStatusMenuCommand(OnStatusMergeAssociations, OnMenuMergeAssociations, new CommandID(guidEFDiagramMenuCmdSet, cmdidMergeAssociations));
         commands.Add(mergeAssociationsCommand);

         DynamicStatusMenuCommand splitAssociationCommand =
            new DynamicStatusMenuCommand(OnStatusSplitAssociation, OnMenuSplitAssociation, new CommandID(guidEFDiagramMenuCmdSet, cmdidSplitAssociation));
         commands.Add(splitAssociationCommand);

         // Additional commands go here.  
         return commands;
      }

      #region Find

      private void OnStatusFind(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            command.Enabled = true;

            // until we can figure out how we want to do this
            command.Visible = false;
            command.Enabled = false;
         }
      }

      private void OnMenuFind(object sender, EventArgs e)
      {
         // TODO: Implement OnMenuFind

         // find matching class name, property name, association endpoint name, enum name, or enum value name
         // output to tool window
         // bind data to each line of output so can highlight proper shape when entry is clicked (or double clicked)
      }

      #endregion Find

      #region Add Properties

      private void OnStatusAddProperties(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = CurrentSelection.OfType<ClassShape>().Any() && !CurrentSelection.OfType<EnumShape>().Any();
            command.Enabled = CurrentSelection.OfType<ClassShape>().Count() == 1;
         }
      }

      private void OnMenuAddProperties(object sender, EventArgs e)
      {
         NodeShape shapeElement = CurrentSelection.OfType<ClassShape>().FirstOrDefault();

         if (shapeElement?.ModelElement is ModelClass element)
         {
            AddCodeForm codeForm = new AddCodeForm(element);

            if (codeForm.ShowDialog() == DialogResult.OK)
            {
               using (Transaction tx = element.Store.TransactionManager.BeginTransaction("AddProperties"))
               {
                  element.Attributes.Clear();

                  foreach (string codeFormLine in codeForm.Lines)
                  {
                     try
                     {
                        ParseResult parseResult = ModelAttribute.Parse(element.ModelRoot, codeFormLine);

                        if (parseResult == null)
                        {
                           Messages.AddWarning($"Could not parse '{codeFormLine}'. The line will be discarded.");

                           continue;
                        }

                        string message = null;

                        if (string.IsNullOrEmpty(parseResult.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(parseResult.Name))
                           message = $"Could not add '{parseResult.Name}' to {element.Name}: '{parseResult.Name}' is not a valid .NET identifier";
                        else if (element.AllAttributes.Any(x => x.Name == parseResult.Name))
                           message = $"Could not add {parseResult.Name} to {element.Name}: {parseResult.Name} already in use";
                        else if (element.AllNavigationProperties().Any(p => p.PropertyName == parseResult.Name))
                           message = $"Could not add {parseResult.Name} to {element.Name}: {parseResult.Name} already in use";

                        if (message != null)
                        {
                           Messages.AddWarning(message);

                           continue;
                        }

                        ModelAttribute modelAttribute = new ModelAttribute(element.Store,
                                                                           new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name),
                                                                           new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String"),
                                                                           new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? true),
                                                                           new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength),
                                                                           new PropertyAssignment(ModelAttribute.MinLengthDomainPropertyId, parseResult.MinLength),
                                                                           new PropertyAssignment(ModelAttribute.InitialValueDomainPropertyId, parseResult.InitialValue),
                                                                           new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, parseResult.IsIdentity),
                                                                           new PropertyAssignment(ModelAttribute.SetterVisibilityDomainPropertyId, parseResult.SetterVisibility ?? SetterAccessModifier.Public));

                        element.Attributes.Add(modelAttribute);
                     }
                     catch (Exception exception)
                     {
                        Messages.AddWarning($"Could not parse '{codeFormLine}'. {exception.Message}. The line will be discarded.");
                     }
                  }

                  tx.Commit();
               }
            }
         }
      }

      #endregion Add Properties

      #region Add Values

      private void OnStatusAddValues(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = CurrentSelection.OfType<EnumShape>().Any() && !CurrentSelection.OfType<ClassShape>().Any();
            command.Enabled = CurrentSelection.OfType<EnumShape>().Count() == 1;
         }
      }

      private void OnMenuAddValues(object sender, EventArgs e)
      {
         NodeShape shapeElement = CurrentSelection.OfType<EnumShape>().FirstOrDefault();

         if (shapeElement?.ModelElement is ModelEnum element)
         {
            AddCodeForm codeForm = new AddCodeForm(element);

            if (codeForm.ShowDialog() == DialogResult.OK)
            {
               using (Transaction tx = element.Store.TransactionManager.BeginTransaction("AddValues"))
               {
                  element.Values.Clear();

                  foreach (string codeFormLine in codeForm.Lines)
                  {
                     try
                     {
                        string[] parts = codeFormLine.Replace(",", string.Empty)
                                                     .Replace(";", string.Empty)
                                                     .Split('=')
                                                     .Select(x => x.Trim())
                                                     .ToArray();

                        string message = null;

                        if (parts.Length > 0)
                        {
                           if (!CodeGenerator.IsValidLanguageIndependentIdentifier(parts[0]))
                              message = $"Could not add '{parts[0]}' to {element.Name}: '{parts[0]}' is not a valid .NET identifier";
                           else if (element.Values.Any(x => x.Name == parts[0]))
                              message = $"Could not add {parts[0]} to {element.Name}: {parts[0]} already in use";
                           else
                           {
                              switch (parts.Length)
                              {
                                 case 1:

                                    element.Values.Add(new ModelEnumValue(element.Store,
                                                                          new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, parts[0])));

                                    break;
                                 case 2:

                                    element.Values.Add(new ModelEnumValue(element.Store,
                                                                          new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, parts[0]),
                                                                          new PropertyAssignment(ModelEnumValue.ValueDomainPropertyId, parts[1])));

                                    break;
                                 default:
                                    message = $"Could not add '{codeFormLine}' to {element.Name}: The string was not in the proper format.";

                                    break;
                              }
                           }

                           if (message != null)
                              Messages.AddWarning(message);
                        }
                     }
                     catch (Exception exception)
                     {
                        Messages.AddWarning($"Could not parse '{codeFormLine}'. {exception.Message}. The line will be discarded.");
                     }
                  }

                  tx.Commit();
               }
            }
         }
      }

      #endregion Add Properties

      #region Generate Code

      private void OnStatusGenerateCode(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            command.Enabled = IsDiagramSelected() && !IsCurrentDiagramEmpty();
         }
      }

      private void OnMenuGenerateCode(object sender, EventArgs e)
      {
         EFModelDocData.GenerateCode();
      }

      #endregion Generate Code

      #region Show Shape

      private void OnStatusShowShape(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
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
         if (sender is MenuCommand command)
         {
            command.Visible = command.Enabled = CurrentSelection.OfType<ClassShape>().Any() || CurrentSelection.OfType<EnumShape>().Any();
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

      #region Expand Selected Shapes

      private void OnStatusExpandSelected(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            command.Enabled = CurrentSelection.OfType<ClassShape>().Any() || CurrentSelection.OfType<EnumShape>().Any();
         }
      }

      private void OnMenuExpandSelected(object sender, EventArgs e)
      {
         using (Transaction tx = CurrentSelection.OfType<NodeShape>().First().Store.TransactionManager.BeginTransaction("Expand Selected"))
         {
            foreach (ClassShape classShape in CurrentSelection.OfType<ClassShape>())
               classShape.ExpandShape();

            foreach (EnumShape enumShape in CurrentSelection.OfType<EnumShape>())
               enumShape.ExpandShape();

            tx.Commit();
         }
      }

      #endregion Expand Selected Shapes

      #region Collapse Selected Shapes

      private void OnStatusCollapseSelected(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            command.Enabled = CurrentSelection.OfType<ClassShape>().Any() || CurrentSelection.OfType<EnumShape>().Any();
         }
      }

      private void OnMenuCollapseSelected(object sender, EventArgs e)
      {
         {
            using (Transaction tx = CurrentSelection.OfType<NodeShape>().First().Store.TransactionManager.BeginTransaction("Collapse Selected"))
            {
               foreach (ClassShape classShape in CurrentSelection.OfType<ClassShape>())
                  classShape.CollapseShape();

               foreach (EnumShape enumShape in CurrentSelection.OfType<EnumShape>())
                  enumShape.CollapseShape();

               tx.Commit();
            }
         }
      }

      #endregion Collapse Selected Shapes

      #region Layout Diagram

      private void OnStatusLayoutDiagram(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = command.Enabled = IsDiagramSelected() && !IsCurrentDiagramEmpty();
         }
      }

      private void OnMenuLayoutDiagram(object sender, EventArgs e)
      {
         EFModelDiagram diagram = CurrentSelection.Cast<EFModelDiagram>().SingleOrDefault();

         if (diagram == null)
            return;

         Commands.LayoutDiagram(diagram);
      }

      #endregion Layout Diagram

      #region Save as Image

      private void OnStatusSaveAsImage(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            command.Enabled = IsDiagramSelected() && !IsCurrentDiagramEmpty();
         }
      }

      private void OnMenuSaveAsImage(object sender, EventArgs e)
      {
         Diagram currentDiagram = CurrentDocView?.CurrentDiagram;

         if (currentDiagram != null)
         {
            Bitmap bitmap = currentDiagram.CreateBitmap(currentDiagram.NestedChildShapes,
                                                        Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
               dlg.Filter = "BMP files (*.bmp)|*.bmp|GIF files (*.gif)|*.gif|JPG files (*.jpg)|*.jpg|PNG files (*.png)|*.png|TIFF files (*.tiff)|*.tiff|WMF files (*.wmf)|*.wmf";
               dlg.FilterIndex = 4;
               dlg.OverwritePrompt = true;
               dlg.AddExtension = true;
               dlg.CheckPathExists = true;
               dlg.DefaultExt = "png";

               if (dlg.ShowDialog() == DialogResult.OK)
               {
                  try
                  {
                     bitmap.Save(dlg.FileName, GetFormat(dlg.FileName));
                  }
                  catch (ArgumentException)
                  {
                     string errorMessage = $"Can't create a {Path.GetExtension(dlg.FileName)} image";
                     PackageUtility.ShowMessageBox(ServiceProvider, errorMessage, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_CRITICAL);
                  }
                  catch
                  {
                     string errorMessage = $"Error saving {dlg.FileName}";
                     PackageUtility.ShowMessageBox(ServiceProvider, errorMessage, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_CRITICAL);
                  }
               }
            }
         }
      }

      private ImageFormat GetFormat(string fileName)
      {
         switch (Path.GetExtension(fileName).ToLowerInvariant())
         {
            case ".bmp":

               return ImageFormat.Bmp;
            case ".gif":

               return ImageFormat.Gif;
            case ".jpg":

               return ImageFormat.Jpeg;
            case ".png":

               return ImageFormat.Png;
            case ".tiff":

               return ImageFormat.Tiff;
            case ".wmf":

               return ImageFormat.Wmf;
         }

         throw new ArgumentException();
      }

      #endregion

      #region Load NuGet

      private void OnStatusLoadNuGet(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            Store store = CurrentDocData.Store;
            ModelRoot modelRoot = store.ModelRoot();
            command.Visible = modelRoot != null && CurrentDocData is EFModelDocData && IsDiagramSelected();
            command.Enabled = IsDiagramSelected() && ModelRoot.CanLoadNugetPackages;
         }
      }

      private void OnMenuLoadNuGet(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         ModelRoot modelRoot = store.ModelRoot();

         ((EFModelDocData)CurrentDocData).EnsureCorrectNuGetPackages(modelRoot);
      }

      #endregion Load NuGet

      #region Merge Unidirectional Associations

      private void OnStatusMergeAssociations(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            Store store = CurrentDocData.Store;
            ModelRoot modelRoot = store.ModelRoot();
            command.Visible = true;

            UnidirectionalAssociation[] selected = CurrentSelection.OfType<UnidirectionalConnector>()
                                                                   .Select(connector => connector.ModelElement)
                                                                   .Cast<UnidirectionalAssociation>()
                                                                   .ToArray();

            command.Enabled = modelRoot != null &&
                              CurrentDocData is EFModelDocData &&
                              selected.Length == 2 &&
                              selected[0].Source == selected[1].Target &&
                              selected[0].Target == selected[1].Source;
         }
      }

      private void OnMenuMergeAssociations(object sender, EventArgs e)
      {
         UnidirectionalAssociation[] selected = CurrentSelection.OfType<UnidirectionalConnector>()
                                                                .Select(connector => connector.ModelElement)
                                                                .Cast<UnidirectionalAssociation>()
                                                                .ToArray();
         ((EFModelDocData)CurrentDocData).Merge(selected);
      }

      #endregion

      #region Split Bidirectional Association

      private void OnStatusSplitAssociation(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            Store store = CurrentDocData.Store;
            ModelRoot modelRoot = store.ModelRoot();
            command.Visible = true;

            BidirectionalAssociation[] selected = CurrentSelection.OfType<BidirectionalConnector>()
                                                                   .Select(connector => connector.ModelElement)
                                                                   .Cast<BidirectionalAssociation>()
                                                                   .ToArray();

            command.Enabled = modelRoot != null &&
                              CurrentDocData is EFModelDocData &&
                              selected.Length == 1;
         }
      }

      private void OnMenuSplitAssociation(object sender, EventArgs e)
      {
         BidirectionalAssociation selected = CurrentSelection.OfType<BidirectionalConnector>()
                                                             .Select(connector => connector.ModelElement)
                                                             .Cast<BidirectionalAssociation>()
                                                             .Single();

         ((EFModelDocData)CurrentDocData).Split(selected);
      }

      #endregion

      #region Select classes

      private void OnStatusSelectClasses(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<ClassShape>().Any();
         }
      }

      private void OnMenuSelectClasses(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (ClassShape shape in childShapes.OfType<ClassShape>().Where(x => x.CanSelect))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Select classes

      #region Select enums

      private void OnStatusSelectEnums(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<EnumShape>().Any();
         }
      }

      private void OnMenuSelectEnums(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (EnumShape shape in childShapes.OfType<EnumShape>().Where(x => x.CanSelect))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Select enums

      #region Select associations

      private void OnStatusSelectAssocs(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<AssociationConnector>().Any();
         }
      }

      private void OnMenuSelectAssocs(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (AssociationConnector shape in childShapes.OfType<AssociationConnector>().Where(x => x.CanSelect))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Select associations

      #region Select unidirectional associations

      private void OnStatusSelectUnidir(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<UnidirectionalConnector>().Any();
         }
      }

      private void OnMenuSelectUnidir(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (UnidirectionalConnector shape in childShapes.OfType<UnidirectionalConnector>().Where(x => x.CanSelect))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Find

      #region Select bidirectional associations

      private void OnStatusSelectBidir(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;
            command.Enabled = childShapes.OfType<BidirectionalConnector>().Any();
         }
      }

      private void OnMenuSelectBidir(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (BidirectionalConnector shape in childShapes.OfType<BidirectionalConnector>().Where(x => x.CanSelect))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Find
   }
}