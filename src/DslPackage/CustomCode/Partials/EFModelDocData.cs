﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using EnvDTE;

using EnvDTE80;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using Sawczyn.EFDesigner.EFModel.Extensions;

using VSLangProj;
// ReSharper disable MemberCanBePrivate.Global

namespace Sawczyn.EFDesigner.EFModel
{
   [SuppressMessage("ReSharper", "RemoveRedundantBraces")]
   internal partial class EFModelDocData
   {
      private static DTE _dte;
      private static DTE2 _dte2;
      private IComponentModel _componentModel;
      //private IVsOutputWindowPane _outputWindow;

      internal static DTE Dte => _dte ?? (_dte = Package.GetGlobalService(typeof(DTE)) as DTE);
      internal static DTE2 Dte2 => _dte2 ?? (_dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2);
      internal IComponentModel ComponentModel => _componentModel ?? (_componentModel = (IComponentModel)GetService(typeof(SComponentModel)));
      //internal IVsOutputWindowPane OutputWindow => _outputWindow ?? (_outputWindow = (IVsOutputWindowPane)GetService(typeof(SVsGeneralOutputWindowPane)));

      internal static Project ActiveProject =>
            Dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0
                  ? activeSolutionProjects.GetValue(0) as Project
                  : null;

      static EFModelDocData()
      {
         ModelDisplay.GetDiagramColors = GetDiagramColors;
      }

      internal static void GenerateCode()
      {
         GenerateCode(null);
      }

      internal static void GenerateCode(string filepath)
      {
         ProjectItem modelProjectItem = Dte2.Solution.FindProjectItem(filepath ?? Dte2.ActiveDocument.FullName);

         if (Guid.Parse(modelProjectItem.Kind) == VSConstants.GUID_ItemType_PhysicalFile)
            modelProjectItem.Save();

         string templateFilename = Path.ChangeExtension(filepath ?? Dte2.ActiveDocument.FullName, "tt");

         ProjectItem templateProjectItem = Dte2.Solution.FindProjectItem(templateFilename);
#pragma warning disable IDE0019 // Use pattern matching
         VSProjectItem templateVsProjectItem = templateProjectItem?.Object as VSProjectItem;
#pragma warning restore IDE0019 // Use pattern matching

         if (templateVsProjectItem == null)
            Messages.AddError($"Tried to generate code but couldn't find {templateFilename} in the solution.");
         else
         {

            try
            {
               Dte.StatusBar.Text = $"Generating code from {templateFilename}";
               templateVsProjectItem.RunCustomTool();
               Dte.StatusBar.Text = $"Finished generating code from {templateFilename}";
            }
            catch (COMException)
            {
               string message = $"Encountered an error generating code from {templateFilename}. Please transform T4 template manually.";
               Dte.StatusBar.Text = message;
               Messages.AddError(message);
            }
         }
      }

      /// <summary>
      /// Called before the document is initially loaded with data.
      /// </summary>
      protected override void OnDocumentLoading(EventArgs e)
      {
         base.OnDocumentLoading(e);
         ValidationController?.ClearMessages();
      }

      /// <summary>
      /// Called when user double clicks on a class shape
      /// </summary>
      /// <param name="modelClass"></param>
      internal static bool OpenFileFor(ModelClass modelClass)
      {
         try
         {
            Project activeProject = ActiveProject;

            if (activeProject != null)
            {
               string projectDirectory = Path.GetDirectoryName(activeProject.FullName);
               string filename = Path.Combine(projectDirectory, modelClass.GetRelativeFileName());

               if (File.Exists(filename))
               {
                  Dte.ItemOperations.OpenFile(filename);

                  return true;
               }
            }

            return false;
         }
         catch (Exception e)
         {
            Debug.WriteLine("Error opening file. " + e.Message);
            return false;
         }
      }

      /// <summary>
      /// Called when user double clicks on a enum shape
      /// </summary>
      /// <param name="modelEnum"></param>
      internal static bool OpenFileFor(ModelEnum modelEnum)
      {
         try
         {
            Project activeProject = ActiveProject;

            if (activeProject != null)
            {
               string projectDirectory = Path.GetDirectoryName(activeProject.FullName);
               Debug.Assert(projectDirectory != null, nameof(projectDirectory) + " != null");
               string filename = Path.Combine(projectDirectory, modelEnum.GetRelativeFileName());

               if (File.Exists(filename))
               {
                  Dte.ItemOperations.OpenFile(filename);

                  return true;
               }
            }

            return false;
         }
         catch (Exception e)
         {
            Debug.WriteLine("Error opening file. " + e.Message);
            return false;
         }
      }

      private IMonitorSelectionService monitorSelection;

      protected IMonitorSelectionService MonitorSelection
      {
         get
         {
            return monitorSelection
                ?? (monitorSelection = (IMonitorSelectionService)GetService(typeof(IMonitorSelectionService)));
         }
      }

      protected ModelingDocView CurrentModelingDocView
      {
         get
         {
            return MonitorSelection?.CurrentDocumentView as ModelingDocView;
         }
      }

      /// <summary>Currently focused document view</summary>
      public DiagramDocView CurrentDocView
      {
         get
         {
            return CurrentModelingDocView as DiagramDocView;
         }
      }

      protected Diagram GetCurrentDiagram()
      {
         return CurrentDocView?.CurrentDiagram;
      }

      /// <summary>
      /// Called on both document load and reload.
      /// </summary>
      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();

         if (!(RootElement is ModelRoot modelRoot))
            return;

         // TODO: This is getting out of hand. Consolidate into an interface and load it up all at once

         ErrorDisplay.RegisterDisplayHandler(ShowError);
         WarningDisplay.RegisterDisplayHandler(ShowWarning);
         MessageDisplay.RegisterDisplayHandler(ShowMessage);
         BooleanQuestionDisplay.RegisterDisplayHandler(ShowBooleanQuestionBox);
         StatusDisplay.RegisterDisplayHandler(ShowStatus);
         ChoiceDisplay.RegisterDisplayHandler(GetChoice);
         ModelDisplay.RegisterLayoutDiagramAction(Commands.LayoutDiagram);

         ClassShape.OpenCodeFile = OpenFileFor;
         ClassShape.ExecCodeGeneration = GenerateCode;
         EnumShape.OpenCodeFile = OpenFileFor;
         EnumShape.ExecCodeGeneration = GenerateCode;
         ModelRoot.ExecuteValidator = ValidateAll;
         ModelRoot.GetCurrentDiagram = GetCurrentDiagram;
         ModelRoot.WriteDiagramAsBinary = () => EFModelPackage.Options.SaveDiagramsCompressed;
         ModelDiagramData.OpenDiagram = DisplayDiagram;
         ModelDiagramData.CloseDiagram = CloseDiagram;
         ModelDiagramData.RenameWindow = RenameWindow;

         VSColorTheme.ThemeChanged += VSColorTheme_OnThemeChanged;

         // set to the project's namespace if no namespace set
         if (string.IsNullOrEmpty(modelRoot.Namespace))
         {
            using (Transaction tx = Store.TransactionManager.BeginTransaction("SetDefaultNamespace"))
            {
               modelRoot.Namespace = ActiveProject.Properties.Item("DefaultNamespace")?.Value as string;
               tx.Commit();
            }
         }

         ReadOnlyCollection<Association> associations = Store.ElementDirectory.FindElements<Association>();

         if (associations.Any())
         {
            using (Transaction tx = Store.TransactionManager.BeginTransaction("StyleConnectors"))
            {
               // style association connectors if needed
               foreach (Association association in associations)
               {
                  PresentationHelper.UpdateAssociationDisplay(association);

                  // for older diagrams that didn't calculate this initially
                  AssociationChangedRules.SetEndpointRoles(association);
               }

               tx.Commit();
            }

            // in prior versions, we accidentally allowed EF6 1-1 associations to have defined foreign keys
            // but EF6 doesn't support that. Fix that now.
            if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
            {
               using (Transaction tx = Store.TransactionManager.BeginTransaction())
               {
                  Store.ElementDirectory
                       .AllElements
                       .OfType<Association>()
                       .Where(a => a.SourceMultiplicity != Multiplicity.ZeroMany
                                && a.TargetMultiplicity != Multiplicity.ZeroMany
                                && !string.IsNullOrEmpty(a.FKPropertyName))
                       .ToList()
                       .ForEach(a => a.FKPropertyName = null);

                  tx.Commit();
               }
            }


         }

         List<GeneralizationConnector> generalizationConnectors = Store.ElementDirectory
                                                                       .FindElements<GeneralizationConnector>()
                                                                       .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();

         List<AssociationConnector> associationConnectors = Store.ElementDirectory
                                                                 .FindElements<AssociationConnector>()
                                                                 .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();

         if (generalizationConnectors.Any() || associationConnectors.Any())
         {
            using (Transaction tx = Store.TransactionManager.BeginTransaction("HideConnectors"))
            {
               // hide any connectors that shouldn't be visible because their nodes are hidden
               foreach (GeneralizationConnector connector in generalizationConnectors)
                  connector.Hide();

               foreach (AssociationConnector connector in associationConnectors)
                  connector.Hide();

               tx.Commit();
            }
         }

         using (Transaction tx = Store.TransactionManager.BeginTransaction("SetVisuals"))
         {
            foreach (ModelClass modelClass in Store.ElementDirectory.FindElements<ModelClass>())
               PresentationHelper.UpdateClassDisplay(modelClass);

            VSColorTheme_OnThemeChanged(null);
            tx.Commit();
         }

         using (Transaction tx = Store.TransactionManager.BeginTransaction("ValidateOnChanges"))
         {
            // validate classes that show warnings, so that we can change glyphs accordingly
            List<DomainClassInfo> classesWithWarnings = Store.ElementDirectory
                                                             .AllElements
                                                             .OfType<IDisplaysWarning>()
                                                             .OfType<ModelElement>()
                                                             .Select(e => e.GetDomainClass())
                                                             .Distinct()
                                                             .ToList();

            EventManagerDirectory events = Store.EventManagerDirectory;

            foreach (DomainClassInfo classInfo in classesWithWarnings)
            {
               events.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValidateModelElement));
               events.ElementAdded.Add(classInfo, new EventHandler<ElementAddedEventArgs>(ValidateModelElement));
            }

            tx.Commit();
         }

         EFModelDocView docView = (EFModelDocView)CurrentDocView;
         EFModelExplorer modelExplorer = (EFModelExplorer)docView?.ModelExplorerWindow?.TreeContainer;
         modelExplorer?.Show();

         SetDocDataDirty(0);
      }

      private static DiagramThemeColors GetDiagramColors()
      {
         return new DiagramThemeColors(VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundBrushKey));
      }

      private void VSColorTheme_OnThemeChanged(ThemeChangedEventArgs e)
      {
         DiagramThemeColors diagramColors = GetDiagramColors();

         EFModelDocView docView = (EFModelDocView)CurrentDocView;
         EFModelExplorer modelExplorer = (EFModelExplorer)docView?.ModelExplorerWindow.TreeContainer;
         modelExplorer?.SetThemeColors(diagramColors);
         modelExplorer?.Show();

         if (Store != null)
         {
            foreach (GeneralizationConnector connector in Store.ElementDirectory.AllElements.OfType<GeneralizationConnector>().ToArray())
               connector.SetThemeColors(diagramColors);

            foreach (AssociationConnector connector in Store.ElementDirectory.AllElements.OfType<AssociationConnector>().ToArray())
               connector.SetThemeColors(diagramColors);

            foreach (CommentConnector connector in Store.ElementDirectory.AllElements.OfType<CommentConnector>().ToArray())
               connector.SetThemeColors(diagramColors);

            foreach (ClassShape classShape in Store.ElementDirectory.AllElements.OfType<ClassShape>().ToArray())
               classShape.SetThemeColors(diagramColors);

            foreach (EnumShape enumShape in Store.ElementDirectory.AllElements.OfType<EnumShape>().ToArray())
               enumShape.SetThemeColors(diagramColors);

            foreach (CommentBoxShape commentShape in Store.ElementDirectory.AllElements.OfType<CommentBoxShape>().ToArray())
               commentShape.SetThemeColors(diagramColors);

            foreach (EFModelDiagram diagram in Store.ElementDirectory.AllElements.OfType<EFModelDiagram>().ToArray())
               diagram.SetThemeColors(diagramColors);
         }
      }

      private void DisplayDiagram(ModelDiagramData diagramData)
      {
         OpenView(Constants.LogicalView, new Mexedge.VisualStudio.Modeling.ViewContext(diagramData.Name, typeof(EFModelDiagram), RootElement));
      }

      private void CloseDiagram(EFModelDiagram diagram)
      {
         DocViews.OfType<EFModelDocView>().FirstOrDefault(d => d.Diagram == diagram)?.Frame?.CloseFrame((uint)__FRAMECLOSE.FRAMECLOSE_SaveIfDirty);
      }

      private void RenameWindow(EFModelDiagram diagram)
      {
         DocViews.OfType<EFModelDocView>().FirstOrDefault(d => d.Diagram == diagram)?.Frame
                ?.SetProperty((int)__VSFPROPID.VSFPROPID_EditorCaption, $" [{diagram.Name}]");
      }

      private void ValidateAll()
      {
         ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
         Store.GetAll<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());
         ValidationController?.ClearMessages();
         ValidationController?.Validate(Store.ElementDirectory.AllElements, allCategories);
      }

      private void ValidateModelElement(object sender, ElementPropertyChangedEventArgs e)
      {
         ModelElement modelElement = e.ModelElement;
         ValidateModelElement(modelElement);
      }

      private void ValidateModelElement(object sender, ElementAddedEventArgs e)
      {
         ModelElement modelElement = e.ModelElement;
         ValidateModelElement(modelElement);
      }

      private void ValidateModelElement(ModelElement modelElement)
      {
         if (!ModelRoot.BatchUpdating && modelElement is IDisplaysWarning displaysWarningElement)
         {
            displaysWarningElement.ResetWarning();

            ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
            ValidationController.Validate(modelElement, allCategories);
            
            displaysWarningElement.RedrawItem();
         }
      }

      internal static DialogResult ShowQuestionBox(IServiceProvider serviceProvider, string question)
      {
         return PackageUtility.ShowMessageBox(serviceProvider, 
                                              question, 
                                              OLEMSGBUTTON.OLEMSGBUTTON_YESNO, 
                                              OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, 
                                              OLEMSGICON.OLEMSGICON_QUERY);
      }

      internal static bool ShowBooleanQuestionBox(IServiceProvider serviceProvider, string question)
      {
         return ShowQuestionBox(serviceProvider, question) == DialogResult.Yes;
      }

      internal static void ShowMessage(string message)
      {
         Messages.AddMessage(message);
      }

      internal static void ShowWarning(string message)
      {
         Messages.AddWarning(message);
      }

      internal static void ShowStatus(string message)
      {
         Messages.AddStatus(message);
      }

      internal static void ShowError(IServiceProvider serviceProvider, string message)
      {
         Messages.AddError(message);
         PackageUtility.ShowError(serviceProvider, message);
      }

      internal static string GetChoice(string title, IEnumerable<string> choices)
      {
         return Messages.GetChoice(title, choices);
      }

      public override IEnumerable<ModelElement> GetAllElementsForValidation()
      {
         List<ModelElement> elements = base.GetAllElementsForValidation().ToList();
         elements.OfType<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());

         return elements;
      }

      /// <summary>
      /// Validate the model before the file is saved.
      /// </summary>
      protected override bool CanSave(bool allowUserInterface)
      {
         if (allowUserInterface)
            ValidationController?.ClearMessages();
         return base.CanSave(allowUserInterface);
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         if (RootElement is ModelRoot modelRoot)
         {
            if (modelRoot.TransformOnSave)
               GenerateCode(((DocumentSavedEventArgs)e).NewFileName);

            CurrentDocView?.CurrentDesigner?.Focus();

            // add other post-save processing as needed
         }
      }

      public void Merge(UnidirectionalAssociation[] selected)
      {
         if (!(RootElement is ModelRoot modelRoot)) return;

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("Merge associations"))
         {
            selected[0].Delete();
            selected[1].Delete();

            // ReSharper disable once UnusedVariable
            BidirectionalAssociation element = new BidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalSourceDomainRoleId, selected[0].Source),
                                                      new RoleAssignment(BidirectionalAssociation.BidirectionalTargetDomainRoleId, selected[1].Source)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(BidirectionalAssociation.SourcePropertyNameDomainPropertyId, selected[1].TargetPropertyName), 
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected[0].TargetPropertyName), 
                                                     
                                                      new PropertyAssignment(BidirectionalAssociation.SourceCustomAttributesDomainPropertyId, selected[1].TargetCustomAttributes), 
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected[0].TargetCustomAttributes), 

                                                      new PropertyAssignment(BidirectionalAssociation.SourceDisplayTextDomainPropertyId, selected[1].TargetDisplayText), 
                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected[0].TargetDisplayText), 

                                                      new PropertyAssignment(BidirectionalAssociation.SourceSummaryDomainPropertyId, selected[1].TargetSummary), 
                                                      new PropertyAssignment(BidirectionalAssociation.SourceDescriptionDomainPropertyId, selected[1].TargetDescription),

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected[0].TargetSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected[0].TargetDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected[1].TargetDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected[0].TargetDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected[1].TargetRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected[0].TargetRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected[1].TargetMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected[0].TargetMultiplicity), 
                                                   });
            tx.Commit();
         }
      }

      public void Split(BidirectionalAssociation selected)
      {
         if (!(RootElement is ModelRoot modelRoot)) return;

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("Split associations"))
         {
            selected.Delete();

            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation element1 = new UnidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(Association.SourceDomainRoleId, selected.Source),
                                                      new RoleAssignment(Association.TargetDomainRoleId, selected.Target)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected.TargetPropertyName), 
                                                     
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected.TargetCustomAttributes), 

                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected.TargetDisplayText), 

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected.TargetSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected.TargetDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected.SourceDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected.TargetDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected.SourceRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected.TargetRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected.SourceMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected.TargetMultiplicity), 
                                                   });

            // ReSharper disable once UnusedVariable
            UnidirectionalAssociation element2 = new UnidirectionalAssociation(Store,
                                                   new[]
                                                   {
                                                      new RoleAssignment(Association.SourceDomainRoleId, selected.Target),
                                                      new RoleAssignment(Association.TargetDomainRoleId, selected.Source)
                                                   },
                                                   new[]
                                                   {
                                                      new PropertyAssignment(Association.TargetPropertyNameDomainPropertyId, selected.SourcePropertyName), 
                                                     
                                                      new PropertyAssignment(Association.TargetCustomAttributesDomainPropertyId, selected.SourceCustomAttributes), 

                                                      new PropertyAssignment(Association.TargetDisplayTextDomainPropertyId, selected.SourceDisplayText), 

                                                      new PropertyAssignment(Association.TargetSummaryDomainPropertyId, selected.SourceSummary), 
                                                      new PropertyAssignment(Association.TargetDescriptionDomainPropertyId, selected.SourceDescription), 

                                                      new PropertyAssignment(Association.SourceDeleteActionDomainPropertyId, selected.TargetDeleteAction), 
                                                      new PropertyAssignment(Association.TargetDeleteActionDomainPropertyId, selected.SourceDeleteAction), 

                                                      new PropertyAssignment(Association.SourceRoleDomainPropertyId, selected.TargetRole), 
                                                      new PropertyAssignment(Association.TargetRoleDomainPropertyId, selected.SourceRole), 

                                                      new PropertyAssignment(Association.SourceMultiplicityDomainPropertyId, selected.TargetMultiplicity), 
                                                      new PropertyAssignment(Association.TargetMultiplicityDomainPropertyId, selected.SourceMultiplicity), 
                                                   });

            tx.Commit();
         }
      }

      protected override void CleanupOldDiagramFiles()
      {
         string diagramsFileName = FileName + this.DiagramExtension;
         if (diagramsFileName.EndsWith("x"))
         {
            string oldDiagramFileName = diagramsFileName.TrimEnd('x');

            if (File.Exists(oldDiagramFileName))
               Dte2.Solution.FindProjectItem(oldDiagramFileName)?.Delete();
         }
      }
   }
}
