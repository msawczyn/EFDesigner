using System;
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
using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;

namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Double-derived class to allow easier code customization.
   /// </summary>
   internal partial class EFModelCommandSet
   {
      // ReSharper disable once UnusedMember.Local
      private const int grpidEFDiagram = 0x01001;

      private readonly Guid guidEFDiagramMenuCmdSet = new Guid("31178ecb-5da7-46cc-bd4a-ce4e5420bd3e");
      private const int cmdidLayoutDiagram = 1;
      private const int cmdidHideShape = 2;
      private const int cmdidShowShape = 3;
      private const int cmdidGenerateCode = 4;
      private const int cmdidAddCodeProperties = 5;
      private const int cmdidSaveAsImage = 6;

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

         DynamicStatusMenuCommand saveAsImageCommand =
               new DynamicStatusMenuCommand(OnStatusSaveAsImage, OnMenuSaveAsImage, new CommandID(guidEFDiagramMenuCmdSet, cmdidSaveAsImage));
         commands.Add(saveAsImageCommand);

         // Add more commands here.  
         return commands;
      }


      #region Add Properties

      private void OnStatusAddProperties(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
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
                  IEnumerable<ModelAttribute> modelAttributes =
                     codeForm.Lines
                             .Select(s => ModelAttribute.Parse(element.ModelRoot, s))
                             .Where(attr => attr != null)
                             .Select(parseResult => new ModelAttribute(element.Store,
                                                                       new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name),
                                                                       new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String"),
                                                                       new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? true),
                                                                       new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength ?? 0),
                                                                       new PropertyAssignment(ModelAttribute.InitialValueDomainPropertyId, parseResult.InitialValue),
                                                                       new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, parseResult.IsIdentity),
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
         if (sender is MenuCommand command)
         {
            // not yet. Need to work this some more.
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
         if (sender is MenuCommand command)
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
                     MessageBox.Show($"Can't create a {Path.GetExtension(dlg.FileName)} image", "Bad image type", MessageBoxButtons.OK, MessageBoxIcon.Error);
                  }
                  catch
                  {
                     MessageBox.Show($"Error saving {dlg.FileName}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
   }
}
