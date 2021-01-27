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

      // Designer menu items

      // ReSharper disable once UnusedMember.Local
      private const int grpidEFDiagram         = 0x01001;

      private const int cmdidFind              = 0x0011;
      private const int cmdidLayoutDiagram     = 0x0012;
      private const int cmdidHideShape         = 0x0013;
      private const int cmdidShowShape         = 0x0014;
      private const int cmdidGenerateCode      = 0x0015;
      private const int cmdidAddCodeProperties = 0x0016;
      private const int cmdidSaveAsImage       = 0x0017;
      private const int cmdidLoadNuGet         = 0x0018;
      private const int cmdidAddCodeValues     = 0x0019;
      private const int cmdidExpandSelected    = 0x001A;
      private const int cmdidCollapseSelected  = 0x001B;
      private const int cmdidMergeAssociations = 0x001C;
      private const int cmdidSplitAssociation  = 0x001D;
      private const int cmdidRemoveShape       = 0x001E;
      private const int cmdidAddForeignKeys    = 0x001F;
      private const int cmdidDelForeignKeys    = 0x0020;

      private const int cmdidSelectClasses     = 0x0101;
      private const int cmdidSelectEnums       = 0x0102;
      private const int cmdidSelectAssocs      = 0x0103;
      private const int cmdidSelectUnidir      = 0x0104;
      private const int cmdidSelectBidir       = 0x0105;
      private const int cmdidAlignLeft         = 0x0106;
      private const int cmdidAlignRight        = 0x0107;
      private const int cmdidAlignTop          = 0x0108;
      private const int cmdidAlignBottom       = 0x0109;
      private const int cmdidAlignHCenter      = 0x010A;
      private const int cmdidAlignVCenter      = 0x010B;
      private const int cmdidResizeWidest      = 0x010C;
      private const int cmdidResizeNarrowest   = 0x010D;

      // Model Explorer menu items

      internal const int cmdidExpandAll        = 0x0201;
      internal const int cmdidCollapseAll      = 0x0202;
      internal const int cmdidGoToCode         = 0x0203;

      internal static readonly Guid guidEFDiagramMenuCmdSet = new Guid("31178ecb-5da7-46cc-bd4a-ce4e5420bd3e");
      internal static readonly Guid guidMenuExplorerCmdSet = new Guid("922EC20C-4054-4E96-8C10-2405A1F91486");

      #endregion Identifiers

      protected override IList<MenuCommand> GetMenuCommands()
      {
         IList<MenuCommand> commands = base.GetMenuCommands();

         #region findCommand

         DynamicStatusMenuCommand findCommand =
            new DynamicStatusMenuCommand(OnStatusFind, OnMenuFind, new CommandID(guidEFDiagramMenuCmdSet, cmdidFind));

         commands.Add(findCommand);

         #endregion

         #region addAttributesCommand

         DynamicStatusMenuCommand addAttributesCommand =
            new DynamicStatusMenuCommand(OnStatusAddProperties, OnMenuAddProperties, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddCodeProperties));

         commands.Add(addAttributesCommand);

         #endregion

         #region addValuesCommand

         DynamicStatusMenuCommand addValuesCommand =
            new DynamicStatusMenuCommand(OnStatusAddValues, OnMenuAddValues, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddCodeValues));

         commands.Add(addValuesCommand);

         #endregion

         #region layoutDiagramCommand

         DynamicStatusMenuCommand layoutDiagramCommand =
            new DynamicStatusMenuCommand(OnStatusLayoutDiagram, OnMenuLayoutDiagram, new CommandID(guidEFDiagramMenuCmdSet, cmdidLayoutDiagram));

         commands.Add(layoutDiagramCommand);

         #endregion

         #region hideShapeCommand

         DynamicStatusMenuCommand hideShapeCommand =
            new DynamicStatusMenuCommand(OnStatusHideShape, OnMenuHideShape, new CommandID(guidEFDiagramMenuCmdSet, cmdidHideShape));

         commands.Add(hideShapeCommand);

         #endregion

         #region showShapeCommand

         DynamicStatusMenuCommand showShapeCommand =
            new DynamicStatusMenuCommand(OnStatusShowShape, OnMenuShowShape, new CommandID(guidEFDiagramMenuCmdSet, cmdidShowShape));

         commands.Add(showShapeCommand);

         #endregion

         #region removeShapeCommand

         DynamicStatusMenuCommand removeShapeCommand =
            new DynamicStatusMenuCommand(OnStatusRemoveShape, OnMenuRemoveShape, new CommandID(guidEFDiagramMenuCmdSet, cmdidRemoveShape));

         commands.Add(removeShapeCommand);

         #endregion

         #region addForeignKeysCommand

         DynamicStatusMenuCommand addForeignKeysCommand =
            new DynamicStatusMenuCommand(OnStatusAddForeignKeys, OnMenuAddForeignKeys, new CommandID(guidEFDiagramMenuCmdSet, cmdidAddForeignKeys));

         commands.Add(addForeignKeysCommand);

         #endregion

         #region removeForeignKeysCommand

         DynamicStatusMenuCommand removeForeignKeysCommand =
            new DynamicStatusMenuCommand(OnStatusRemoveForeignKeys, OnMenuRemoveForeignKeys, new CommandID(guidEFDiagramMenuCmdSet, cmdidDelForeignKeys));

         commands.Add(removeForeignKeysCommand);

         #endregion

         #region generateCodeCommand

         DynamicStatusMenuCommand generateCodeCommand =
            new DynamicStatusMenuCommand(OnStatusGenerateCode, OnMenuGenerateCode, new CommandID(guidEFDiagramMenuCmdSet, cmdidGenerateCode));

         commands.Add(generateCodeCommand);

         #endregion

         #region saveAsImageCommand

         DynamicStatusMenuCommand saveAsImageCommand =
            new DynamicStatusMenuCommand(OnStatusSaveAsImage, OnMenuSaveAsImage, new CommandID(guidEFDiagramMenuCmdSet, cmdidSaveAsImage));

         commands.Add(saveAsImageCommand);

         #endregion

         #region loadNuGetCommand

         DynamicStatusMenuCommand loadNuGetCommand =
            new DynamicStatusMenuCommand(OnStatusLoadNuGet, OnMenuLoadNuGet, new CommandID(guidEFDiagramMenuCmdSet, cmdidLoadNuGet));

         commands.Add(loadNuGetCommand);

         #endregion

         #region selectClassesCommand

         DynamicStatusMenuCommand selectClassesCommand =
            new DynamicStatusMenuCommand(OnStatusSelectClasses, OnMenuSelectClasses, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectClasses));

         commands.Add(selectClassesCommand);

         #endregion

         #region selectEnumsCommand

         DynamicStatusMenuCommand selectEnumsCommand =
            new DynamicStatusMenuCommand(OnStatusSelectEnums, OnMenuSelectEnums, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectEnums));

         commands.Add(selectEnumsCommand);

         #endregion

         #region selectAssocsCommand

         DynamicStatusMenuCommand selectAssocsCommand =
            new DynamicStatusMenuCommand(OnStatusSelectAssocs, OnMenuSelectAssocs, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectAssocs));

         commands.Add(selectAssocsCommand);

         #endregion

         #region selectUnidirCommand

         DynamicStatusMenuCommand selectUnidirCommand =
            new DynamicStatusMenuCommand(OnStatusSelectUnidir, OnMenuSelectUnidir, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectUnidir));

         commands.Add(selectUnidirCommand);

         #endregion

         #region selectBidirCommand

         DynamicStatusMenuCommand selectBidirCommand =
            new DynamicStatusMenuCommand(OnStatusSelectBidir, OnMenuSelectBidir, new CommandID(guidEFDiagramMenuCmdSet, cmdidSelectBidir));

         commands.Add(selectBidirCommand);

         #endregion

         #region expandSelectedCommand

         DynamicStatusMenuCommand expandSelectedCommand =
            new DynamicStatusMenuCommand(OnStatusExpandSelected, OnMenuExpandSelected, new CommandID(guidEFDiagramMenuCmdSet, cmdidExpandSelected));

         commands.Add(expandSelectedCommand);

         #endregion

         #region collapseSelectedCommand

         DynamicStatusMenuCommand collapseSelectedCommand =
            new DynamicStatusMenuCommand(OnStatusCollapseSelected, OnMenuCollapseSelected, new CommandID(guidEFDiagramMenuCmdSet, cmdidCollapseSelected));

         commands.Add(collapseSelectedCommand);

         #endregion

         #region mergeAssociationsCommand

         DynamicStatusMenuCommand mergeAssociationsCommand =
            new DynamicStatusMenuCommand(OnStatusMergeAssociations, OnMenuMergeAssociations, new CommandID(guidEFDiagramMenuCmdSet, cmdidMergeAssociations));

         commands.Add(mergeAssociationsCommand);

         #endregion

         #region splitAssociationCommand

         DynamicStatusMenuCommand splitAssociationCommand =
            new DynamicStatusMenuCommand(OnStatusSplitAssociation, OnMenuSplitAssociation, new CommandID(guidEFDiagramMenuCmdSet, cmdidSplitAssociation));

         commands.Add(splitAssociationCommand);

         #endregion

         #region alignLeftCommand

         DynamicStatusMenuCommand alignLeftCommand =
            new DynamicStatusMenuCommand(OnStatusAlignLeft, OnMenuAlignLeft, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignLeft));

         commands.Add(alignLeftCommand);

         #endregion

         #region alignRightCommand

         DynamicStatusMenuCommand alignRightCommand =
            new DynamicStatusMenuCommand(OnStatusAlignRight, OnMenuAlignRight, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignRight));

         commands.Add(alignRightCommand);

         #endregion

         #region alignTopCommand

         DynamicStatusMenuCommand alignTopCommand =
            new DynamicStatusMenuCommand(OnStatusAlignTop, OnMenuAlignTop, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignTop));

         commands.Add(alignTopCommand);

         #endregion

         #region alignLeftCommand

         DynamicStatusMenuCommand alignBottomCommand =
            new DynamicStatusMenuCommand(OnStatusAlignBottom, OnMenuAlignBottom, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignBottom));

         commands.Add(alignBottomCommand);

         #endregion

         #region alignHCenterCommand

         DynamicStatusMenuCommand alignHCenterCommand =
            new DynamicStatusMenuCommand(OnStatusAlignHCenter, OnMenuAlignHCenter, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignHCenter));

         commands.Add(alignHCenterCommand);

         #endregion

         #region alignVCenterCommand

         DynamicStatusMenuCommand alignVCenterCommand =
            new DynamicStatusMenuCommand(OnStatusAlignVCenter, OnMenuAlignVCenter, new CommandID(guidEFDiagramMenuCmdSet, cmdidAlignVCenter));

         commands.Add(alignVCenterCommand);

         #endregion

         #region resizeWidestCommand

         DynamicStatusMenuCommand resizeWidestCommand =
            new DynamicStatusMenuCommand(OnStatusResizeWidest, OnMenuResizeWidest, new CommandID(guidEFDiagramMenuCmdSet, cmdidResizeWidest));

         commands.Add(resizeWidestCommand);

         #endregion

         #region resizeNarrowestCommand

         DynamicStatusMenuCommand resizeNarrowestCommand =
            new DynamicStatusMenuCommand(OnStatusResizeNarrowest, OnMenuResizeNarrowest, new CommandID(guidEFDiagramMenuCmdSet, cmdidResizeNarrowest));

         commands.Add(resizeNarrowestCommand);

         #endregion

         // Additional commands go here.  
         return commands;
      }

      /// <summary>Virtual method to process the menu Delete operation</summary>
      protected override void ProcessOnMenuDeleteCommand()
      {
         foreach (EnumShape enumShape in CurrentSelection.OfType<EnumShape>())
         {
            if (enumShape.ModelElement is ModelEnum modelEnum && ModelEnum.IsUsed(modelEnum))
            {
               string fullName = modelEnum.FullName.Split('.').Last();

               if (BooleanQuestionDisplay.Show(enumShape.Store, $"{fullName} is used as an entity property. Deleting the enumeration will remove those properties. Are you sure?") != true)
                  return;
            }
         }

         base.ProcessOnMenuDeleteCommand();
      }

      private NodeShape[] SelectedNodeShapes
      {
         get
         {
            return CurrentSelection == null
                      ? new NodeShape[0]
                      : CurrentSelection.OfType<ClassShape>().Cast<NodeShape>()
                                        .Union(CurrentSelection.OfType<EnumShape>())
                                        .Union(CurrentSelection.OfType<CommentBoxShape>())
                                        .ToArray();
         }
      }

      private void EnableCommandIfMultipleNodesSelected(MenuCommand command)
      {
         Store store = CurrentDocData.Store;
         ModelRoot modelRoot = store.ModelRoot();
         command.Visible = true;

         command.Enabled = modelRoot != null && CurrentDocData is EFModelDocData && SelectedNodeShapes.Length > 1;
      }

      #region Find

      private void OnStatusFind(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
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
            // don't make this always visible -- only when there are classes but no enums selected. See On...AddValues.
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
                  List<ModelAttribute> existingModelAttributes = element.Attributes.ToList();
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

                        ModelAttribute modelAttribute = existingModelAttributes.FirstOrDefault(a => a.Name == parseResult.Name);

                        if (modelAttribute == null)
                        {
                           modelAttribute = new ModelAttribute(element.Store
                                                             , new PropertyAssignment(ModelAttribute.NameDomainPropertyId, parseResult.Name)
                                                             , new PropertyAssignment(ModelAttribute.TypeDomainPropertyId, parseResult.Type ?? "String")
                                                             , new PropertyAssignment(ModelAttribute.RequiredDomainPropertyId, parseResult.Required ?? true)
                                                             , new PropertyAssignment(ModelAttribute.MaxLengthDomainPropertyId, parseResult.MaxLength ?? -1)
                                                             , new PropertyAssignment(ModelAttribute.MinLengthDomainPropertyId, parseResult.MinLength ?? 0)
                                                             , new PropertyAssignment(ModelAttribute.InitialValueDomainPropertyId, parseResult.InitialValue)
                                                             , new PropertyAssignment(ModelAttribute.IsIdentityDomainPropertyId, parseResult.IsIdentity)
                                                             , new PropertyAssignment(ModelAttribute.SetterVisibilityDomainPropertyId, parseResult.SetterVisibility ?? SetterAccessModifier.Public));
                        }
                        else
                        {
                           modelAttribute.Type = parseResult.Type ?? modelAttribute.Type;
                           modelAttribute.Required = parseResult.Required ?? modelAttribute.Required;
                           modelAttribute.MaxLength = parseResult.MaxLength ?? modelAttribute.MaxLength;
                           modelAttribute.MinLength = parseResult.MinLength ?? modelAttribute.MinLength;
                           modelAttribute.InitialValue = parseResult.InitialValue ?? modelAttribute.InitialValue;
                           modelAttribute.IsIdentity = parseResult.IsIdentity;
                           modelAttribute.SetterVisibility = parseResult.SetterVisibility ?? modelAttribute.SetterVisibility;
                        }

                        if (element.Attributes.All(a => a.Name != modelAttribute.Name))
                           element.Attributes.Add(modelAttribute);
                        else
                           Messages.AddWarning($"Tried to add multiple properties named '{modelAttribute.Name}'. Only the first will be added.");
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
            // don't make this always visible -- only when there are enums but no classes selected. See On...AddProperties.
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
                  List<ModelEnumValue> existingValues = element.Values.ToList();
                  element.Values.Clear();

                  foreach (string codeFormLine in codeForm.Lines)
                  {
                     try
                     {
                        string[] parts = codeFormLine.Replace(",", string.Empty)
                                                     .Replace(";", string.Empty)
                                                     .Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries)
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
                                    if (element.Values.Any(v => v.Name == parts[0]))
                                       Messages.AddWarning($"Tried to add multiple values named '{parts[0]}' to {element.Name}. Only the first will be added.");
                                    else
                                    {
                                       element.Values.Add(existingValues.FirstOrDefault(v => v.Name == parts[0])
                                                       ?? new ModelEnumValue(element.Store, new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, parts[0])));
                                    }

                                    break;
                                 case 2:
                                    if (element.Values.Any(v => v.Name == parts[0]))
                                       Messages.AddWarning($"Tried to add multiple values named '{parts[0]}' to {element.Name}. Only the first will be added.");
                                    else
                                    {
                                       ModelEnumValue existingValue = existingValues.FirstOrDefault(v => v.Name == parts[0]);

                                       if (existingValue != null)
                                       {
                                          existingValue.Value = parts[1];
                                          element.Values.Add(existingValue);
                                       }
                                       else
                                       {
                                          element.Values.Add(new ModelEnumValue(element.Store
                                                                              , new PropertyAssignment(ModelEnumValue.NameDomainPropertyId, parts[0])
                                                                              , new PropertyAssignment(ModelEnumValue.ValueDomainPropertyId, parts[1])));
                                       }
                                    }
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

      #region Add Foreign Keys

      private void OnStatusAddForeignKeys(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            Store store = CurrentEFModelDocData.Store;

            // TODO: Need to implement this
            command.Visible = false;

            IEnumerable<Association> associations = store.ElementDirectory.AllElements
                                                         .OfType<Association>()
                                                         .Where(a => a.Principal?.AllIdentityAttributes?.Any() == true
                                                                  && string.IsNullOrEmpty(a.FKPropertyName));

            command.Enabled = store != null && associations.Any();
         }
      }

      private void OnMenuAddForeignKeys(object sender, EventArgs e)
      {
         Store store = CurrentEFModelDocData.Store;
         List<Association> associations = store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => a.Principal?.AllIdentityAttributes?.Any() == true
                                                        && string.IsNullOrEmpty(a.FKPropertyName))
                                               .ToList();

         if (BooleanQuestionDisplay.Show(store, $"This will add defined foreign key properties to the {associations.Count} associations that don't have them. Are you sure?") == true)
         {
            using (Transaction tx = store.TransactionManager.BeginTransaction("Add Foreign Keys"))
            {

               tx.Commit();
            }
         }
      }

      #endregion Add Foreign Keys

      #region Remove Foreign Keys

      private void OnStatusRemoveForeignKeys(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;
            Store store = CurrentEFModelDocData.Store;

            IEnumerable<Association> associations = store.ElementDirectory.AllElements
                                                         .OfType<Association>()
                                                         .Where(a => a.Principal?.AllIdentityAttributes?.Any() == true
                                                                  && !string.IsNullOrEmpty(a.FKPropertyName));

            command.Enabled = store != null && associations.Any();
         }
      }

      private void OnMenuRemoveForeignKeys(object sender, EventArgs e)
      {
         Store store = CurrentEFModelDocData.Store;
         List<Association> associations = store.ElementDirectory.AllElements
                                               .OfType<Association>()
                                               .Where(a => !string.IsNullOrEmpty(a.FKPropertyName))
                                               .ToList();

         if (BooleanQuestionDisplay.Show(store, $"This will remove the {associations.Count} defined foreign key properties in the model. Are you sure?") == true)
         {
            using (Transaction tx = store.TransactionManager.BeginTransaction("Remove Foreign Keys"))
            {
               foreach (Association association in associations)
                  association.FKPropertyName = null;

               tx.Commit();
            }
         }
      }

      #endregion Remove Foreign Keys

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
         CurrentDocView.Frame.Show();
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
         Store store = CurrentSelection.OfType<NodeShape>().FirstOrDefault()?.Store;

         if (store != null)
         {
            using (Transaction tx = store.TransactionManager.BeginTransaction("HideShapes"))
            {
               foreach (ClassShape shape in CurrentSelection.OfType<ClassShape>())
                  shape.Visible = false;

               foreach (EnumShape shape in CurrentSelection.OfType<EnumShape>())
                  shape.Visible = false;

               foreach (CommentBoxShape shape in CurrentSelection.OfType<CommentBoxShape>())
                  shape.Visible = false;

               tx.Commit();
            }
         }
      }

      #endregion Hide Shape

      #region Remove Shape
      private void OnStatusRemoveShape(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            command.Visible = true;

            // we'll allow removal of class and enum nodes, plus association and inheritance lines
            command.Enabled = (CurrentSelection.Count == CurrentSelection.OfType<ClassShape>().Count()
                                                       + CurrentSelection.OfType<EnumShape>().Count()
                                                       + CurrentSelection.OfType<UnidirectionalConnector>().Count()
                                                       + CurrentSelection.OfType<BidirectionalConnector>().Count()
                                                       + CurrentSelection.OfType<GeneralizationConnector>().Count());
         }
      }

      private void OnMenuRemoveShape(object sender, EventArgs e)
      {
         Store store = CurrentSelection.OfType<NodeShape>().FirstOrDefault()?.Store;

         if (store != null)
         {
            using (Transaction tx = store.TransactionManager.BeginTransaction("HideShapes"))
            {
               // note that we're deleting the shape, not the represented model element
               foreach (ClassShape shape in CurrentSelection.OfType<ClassShape>().ToList())
                  shape.Delete();

               foreach (EnumShape shape in CurrentSelection.OfType<EnumShape>().ToList())
                  shape.Delete();

               foreach (UnidirectionalConnector shape in CurrentSelection.OfType<UnidirectionalConnector>().ToList())
                  shape.Delete();

               foreach (BidirectionalConnector shape in CurrentSelection.OfType<BidirectionalConnector>().ToList())
                  shape.Delete();

               foreach (GeneralizationConnector shape in CurrentSelection.OfType<GeneralizationConnector>().ToList())
                  shape.Delete();

               tx.Commit();
            }
         }
      }

      #endregion Remove Shape

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
            command.Visible = true;
            command.Enabled = IsDiagramSelected() && !IsCurrentDiagramEmpty();
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

         if (currentDiagram == null)
            return;

         //bool oldShowGrid = currentDiagram.ShowGrid;
         //currentDiagram.ShowGrid = false;
         //currentDiagram.Invalidate();

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

         //currentDiagram.ShowGrid = oldShowGrid;
         //currentDiagram.Invalidate();
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

      [Obsolete]
      private void OnStatusLoadNuGet(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
         {
            //Store store = CurrentDocData.Store;
            //ModelRoot modelRoot = store.ModelRoot();
            command.Visible = false; // modelRoot != null && CurrentDocData is EFModelDocData && IsDiagramSelected();
            command.Enabled = false; // IsDiagramSelected() && ModelRoot.CanLoadNugetPackages;
         }
      }

      [Obsolete]
      private void OnMenuLoadNuGet(object sender, EventArgs e)
      {
         //Store store = CurrentDocData.Store;
         //ModelRoot modelRoot = store.ModelRoot();

         //((EFModelDocData)CurrentDocData).EnsureCorrectNuGetPackages(modelRoot);
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
            command.Enabled = childShapes.OfType<ClassShape>().Any(x => x.IsVisible());
         }
      }

      private void OnMenuSelectClasses(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (ClassShape shape in childShapes.OfType<ClassShape>().Where(x => x.IsVisible()))
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
            command.Enabled = childShapes.OfType<EnumShape>().Any(x => x.IsVisible());
         }
      }

      private void OnMenuSelectEnums(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (EnumShape shape in childShapes.OfType<EnumShape>().Where(x => x.IsVisible()))
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
            command.Enabled = childShapes.OfType<AssociationConnector>().Any(x => x.IsVisible());
         }
      }

      private void OnMenuSelectAssocs(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (AssociationConnector shape in childShapes.OfType<AssociationConnector>().Where(x => x.IsVisible()))
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
            command.Enabled = childShapes.OfType<UnidirectionalConnector>().Any(x => x.IsVisible());
         }
      }

      private void OnMenuSelectUnidir(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (UnidirectionalConnector shape in childShapes.OfType<UnidirectionalConnector>().Where(x => x.IsVisible()))
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
            command.Enabled = childShapes.OfType<BidirectionalConnector>().Any(x => x.IsVisible());
         }
      }

      private void OnMenuSelectBidir(object sender, EventArgs e)
      {
         LinkedElementCollection<ShapeElement> childShapes = CurrentDocView.CurrentDiagram.NavigationRoot.NestedChildShapes;

         foreach (BidirectionalConnector shape in childShapes.OfType<BidirectionalConnector>().Where(x => x.IsVisible()))
            shape.Diagram.ActiveDiagramView.Selection.Add(new DiagramItem(shape));
      }

      #endregion Find

      #region Align Left

      private void OnStatusAlignLeft(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignLeft(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;
         double leftEdge = selected.Min(n => n.AbsoluteBounds.Left);

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignLeft"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               nodeShape.AbsoluteBounds = new RectangleD(leftEdge,
                                                         nodeShape.AbsoluteBounds.Y,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Align Right

      private void OnStatusAlignRight(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignRight(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;
         double rightEdge = selected.Max(n => n.AbsoluteBounds.Right);

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignRight"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               double delta = rightEdge - (nodeShape.AbsoluteBounds.X + nodeShape.AbsoluteBounds.Width);
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X + delta,
                                                         nodeShape.AbsoluteBounds.Y,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Align Top

      private void OnStatusAlignTop(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignTop(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;
         double topEdge = selected.Min(n => n.AbsoluteBounds.Top);

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignTop"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X,
                                                         topEdge,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Align Bottom

      private void OnStatusAlignBottom(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignBottom(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;
         double bottomEdge = selected.Max(n => n.AbsoluteBounds.Bottom);

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignBottom"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               double delta = bottomEdge - (nodeShape.AbsoluteBounds.Y + nodeShape.AbsoluteBounds.Height);
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X,
                                                         nodeShape.AbsoluteBounds.Y + delta,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Align Horiz Center

      private void OnStatusAlignHCenter(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignHCenter(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;

         if (!selected.Any())
            return;

         // use the middle hCenter
         var temp = selected.Select(n => new { Node = n, CenterX = n.AbsoluteBounds.X + n.AbsoluteBounds.Width / 2 }).OrderBy(x => x.CenterX).ToArray();
         double centerX = temp[(int)(temp.Length / 2.0 + .5)].CenterX;

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignHCenter"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               double delta = centerX - (nodeShape.AbsoluteBounds.X + nodeShape.AbsoluteBounds.Width / 2);
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X + delta,
                                                         nodeShape.AbsoluteBounds.Y,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Align Vert Center

      private void OnStatusAlignVCenter(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuAlignVCenter(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;

         if (!selected.Any())
            return;

         // use the middle vCenter
         var temp = selected.Select(n => new { Node = n, CenterY = n.AbsoluteBounds.Y + n.AbsoluteBounds.Height / 2 }).OrderBy(x => x.CenterY).ToArray();
         double centerY = temp[(int)(temp.Length / 2.0 + .5)].CenterY;

         using (Transaction tx = store.TransactionManager.BeginTransaction("AlignVCenter"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               double delta = centerY - (nodeShape.AbsoluteBounds.Y + nodeShape.AbsoluteBounds.Height / 2);
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X,
                                                         nodeShape.AbsoluteBounds.Y + delta,
                                                         nodeShape.AbsoluteBounds.Width,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Resize Widest

      private void OnStatusResizeWidest(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuResizeWidest(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;

         if (!selected.Any())
            return;

         double widest = selected.Max(n => n.AbsoluteBounds.Width);

         using (Transaction tx = store.TransactionManager.BeginTransaction("ResizeWidest"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X,
                                                         nodeShape.AbsoluteBounds.Y,
                                                         widest,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion

      #region Resize Narrowest

      private void OnStatusResizeNarrowest(object sender, EventArgs e)
      {
         if (sender is MenuCommand command)
            EnableCommandIfMultipleNodesSelected(command);
      }

      private void OnMenuResizeNarrowest(object sender, EventArgs e)
      {
         Store store = CurrentDocData.Store;
         NodeShape[] selected = SelectedNodeShapes;

         if (!selected.Any())
            return;

         double narrowest = selected.Min(n => n.AbsoluteBounds.Width);

         using (Transaction tx = store.TransactionManager.BeginTransaction("ResizeNarrowest"))
         {
            foreach (NodeShape nodeShape in selected)
            {
               nodeShape.AbsoluteBounds = new RectangleD(nodeShape.AbsoluteBounds.X,
                                                         nodeShape.AbsoluteBounds.Y,
                                                         narrowest,
                                                         nodeShape.AbsoluteBounds.Height);
               nodeShape.Invalidate();
            }

            tx.Commit();
         }

      }

      #endregion
   }
}