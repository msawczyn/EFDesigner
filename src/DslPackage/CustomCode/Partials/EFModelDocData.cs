﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using VSLangProj;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocData
   {
      private static DTE _dte;
      private static DTE2 _dte2;
      private IComponentModel _componentModel;
      private IVsOutputWindowPane _outputWindow;
      private IVsPackageInstaller _nugetInstaller;
      private IVsPackageUninstaller _nugetUninstaller;
      private IVsPackageInstallerServices _nugetInstallerServices;

      internal static DTE Dte => _dte ?? (_dte = Package.GetGlobalService(typeof(DTE)) as DTE);
      internal static DTE2 Dte2 => _dte2 ?? (_dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2);
      internal IComponentModel ComponentModel => _componentModel ?? (_componentModel = (IComponentModel)GetService(typeof(SComponentModel)));
      internal IVsOutputWindowPane OutputWindow => _outputWindow ?? (_outputWindow = (IVsOutputWindowPane)GetService(typeof(SVsGeneralOutputWindowPane)));
      internal IVsPackageInstallerServices NuGetInstallerServices => _nugetInstallerServices ?? (_nugetInstallerServices = ComponentModel?.GetService<IVsPackageInstallerServices>());
      internal IVsPackageInstaller NuGetInstaller => _nugetInstaller ?? (_nugetInstaller = ComponentModel.GetService<IVsPackageInstaller>());
      internal IVsPackageUninstaller NuGetUninstaller => _nugetUninstaller ?? (_nugetUninstaller = ComponentModel.GetService<IVsPackageUninstaller>());

      internal static Project ActiveProject => Dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0
                                                 ? activeSolutionProjects.GetValue(0) as Project
                                                 : null;

      internal static void GenerateCode()
      {
         GenerateCode(null);
      }

      internal static void GenerateCode(string filepath)
      {
         ProjectItem modelProjectItem = Dte2.Solution.FindProjectItem(filepath ?? Dte2.ActiveDocument.FullName);

         if (Guid.Parse(modelProjectItem.Kind) == VSConstants.GUID_ItemType_PhysicalFile)
            modelProjectItem?.Save();

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

      /// <summary>
      /// Called when user double clicks on a enum shape
      /// </summary>
      /// <param name="modelEnum"></param>
      internal static bool OpenFileFor(ModelEnum modelEnum)
      {
         Project activeProject = ActiveProject;

         if (activeProject != null)
         {
            string projectDirectory = Path.GetDirectoryName(activeProject.FullName);
            string filename = Path.Combine(projectDirectory, modelEnum.GetRelativeFileName());
            if (File.Exists(filename))
            {
               Dte.ItemOperations.OpenFile(filename);
               return true;
            }
         }

         return false;
      }

      /// <summary>
      /// Called on both document load and reload.
      /// </summary>
      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         ErrorDisplay.RegisterDisplayHandler(ShowError);
         WarningDisplay.RegisterDisplayHandler(ShowWarning);
         QuestionDisplay.RegisterDisplayHandler(ShowBooleanQuestionBox);
         StatusDisplay.RegisterDisplayHandler(ShowStatus);

         ClassShape.OpenCodeFile = OpenFileFor;
         ClassShape.ExecCodeGeneration = GenerateCode;
         EnumShape.OpenCodeFile = OpenFileFor;
         EnumShape.ExecCodeGeneration = GenerateCode;
         ModelRoot.ExecuteValidator = ValidateAll;

         if (!(RootElement is ModelRoot modelRoot)) return;

         if (NuGetInstaller == null || NuGetUninstaller == null || NuGetInstallerServices == null)
            ModelRoot.CanLoadNugetPackages = false;

         // set to the project's namespace if no namespace set
         if (string.IsNullOrEmpty(modelRoot.Namespace))
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("SetDefaultNamespace"))
            {
               modelRoot.Namespace = ActiveProject.Properties.Item("DefaultNamespace")?.Value as string;
               tx.Commit();
            }
         }

         ReadOnlyCollection<Association> associations = modelRoot.Store.ElementDirectory.FindElements<Association>();

         if (associations.Any())
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("StyleConnectors"))
            {
               // style association connectors if needed
               foreach (Association element in associations)
               {
                  AssociationChangeRules.UpdateDisplayForPersistence(element);
                  AssociationChangeRules.UpdateDisplayForCascadeDelete(element);

                  // for older diagrams that didn't calculate this initially
                     AssociationChangeRules.SetEndpointRoles(element);
               }

               tx.Commit();
            }
         }

         List<GeneralizationConnector> generalizationConnectors = modelRoot.Store
                                                                           .ElementDirectory
                                                                           .FindElements<GeneralizationConnector>()
                                                                           .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();
         List<AssociationConnector> associationConnectors = modelRoot.Store
                                                                     .ElementDirectory
                                                                     .FindElements<AssociationConnector>()
                                                                     .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();

         if (generalizationConnectors.Any() || associationConnectors.Any())
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("HideConnectors"))
            {
               // hide any connectors that may have been hidden due to hidden shapes
               foreach (GeneralizationConnector connector in generalizationConnectors)
                  connector.Hide();

               foreach (AssociationConnector connector in associationConnectors)
                  connector.Hide();

               tx.Commit();
            }
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("ColorShapeOutlines"))
         {
            foreach (ModelClass modelClass in modelRoot.Store.ElementDirectory.FindElements<ModelClass>())
               PresentationHelper.ColorShapeOutline(modelClass);
            tx.Commit();
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("ValidateOnChanges"))
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
               events.ElementPropertyChanged.Add(classInfo, new EventHandler<ElementPropertyChangedEventArgs>(ValidateModelElement));

            tx.Commit();
         }

         SetDocDataDirty(0);
      }

      private void ValidateAll()
      {
         ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
         Store.ElementDirectory.AllElements.OfType<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());
         ValidationController?.ClearMessages();
         ValidationController?.Validate(Store.ElementDirectory.AllElements, allCategories);
      }

      private void ValidateModelElement(object sender, ElementPropertyChangedEventArgs e)
      {
         ModelElement modelElement = e.ModelElement;

         if (modelElement is IDisplaysWarning displaysWarningElement)
         {
            displaysWarningElement.ResetWarning();

            ValidationCategories allCategories = ValidationCategories.Menu | ValidationCategories.Open | ValidationCategories.Save | ValidationCategories.Custom | ValidationCategories.Load;
            ValidationController.Validate(modelElement, allCategories);
            displaysWarningElement.RedrawItem();
         }
      }

      private DialogResult ShowQuestionBox(string question)
      {
         return PackageUtility.ShowMessageBox(ServiceProvider, 
                                              question, 
                                              OLEMSGBUTTON.OLEMSGBUTTON_YESNO, 
                                              OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, 
                                              OLEMSGICON.OLEMSGICON_QUERY);
      }

      private bool ShowBooleanQuestionBox(string question)
      {
         return ShowQuestionBox(question) == DialogResult.Yes;
      }

      // ReSharper disable once UnusedMember.Local
      private void ShowMessage(string message)
      {
         Messages.AddMessage(message);
      }

      private void ShowWarning(string message)
      {
         Messages.AddWarning(message);
      }

      private void ShowStatus(string message)
      {
         Messages.AddStatus(message);
      }

      public override IEnumerable<ModelElement> GetAllElementsForValidation()
      {
         List<ModelElement> elements = base.GetAllElementsForValidation().ToList();
         elements.OfType<IDisplaysWarning>().ToList().ForEach(e => e.ResetWarning());

         return elements;
      }

      private void ShowError(string message)
      {
         Messages.AddError(message);
         PackageUtility.ShowMessageBox(ServiceProvider, 
                                       message, 
                                       OLEMSGBUTTON.OLEMSGBUTTON_OK, 
                                       OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, 
                                       OLEMSGICON.OLEMSGICON_CRITICAL);
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
            // if false, don't even check
            if (modelRoot.InstallNuGetPackages != AutomaticAction.False)
               EnsureCorrectNuGetPackages(modelRoot, false);

            if (modelRoot.TransformOnSave)
               GenerateCode(((DocumentSavedEventArgs)e).NewFileName);
         }
      }

      private class EFVersionDetails
      {
         public string TargetPackageId { get; set; }
         public string TargetPackageVersion { get; set; }
         public string CurrentPackageId { get; set; }
         public string CurrentPackageVersion { get; set; }
      }

      public void EnsureCorrectNuGetPackages(ModelRoot modelRoot, bool force = true)
      {
         EFVersionDetails versionInfo = GetEFVersionDetails(modelRoot);

         if (force || ShouldLoadPackages(modelRoot, versionInfo))
         {
            // first unload what's there, if anything
            if (versionInfo.CurrentPackageId != null)
            {
               // only remove dependencies if we're switching EF types
               Dte.StatusBar.Text = $"Uninstalling {versionInfo.CurrentPackageId} v{versionInfo.CurrentPackageVersion}";

               try
               {
                  NuGetUninstaller.UninstallPackage(ActiveProject, versionInfo.CurrentPackageId, true);
                  Dte.StatusBar.Text = $"Finished uninstalling {versionInfo.CurrentPackageId} v{versionInfo.CurrentPackageVersion}";
               }
               catch (Exception ex)
               {
                  string message = $"Error uninstalling {versionInfo.CurrentPackageId} v{versionInfo.CurrentPackageVersion}";
                  Dte.StatusBar.Text = message;
                  OutputWindow.OutputString(message + "\n");
                  OutputWindow.OutputString(ex.Message + "\n");
                  OutputWindow.Activate();
                  return;
               }
            }

            Dte.StatusBar.Text = $"Installing {versionInfo.TargetPackageId} v{versionInfo.TargetPackageVersion}";

            try
            {
               NuGetInstaller.InstallPackage(null, ActiveProject, versionInfo.TargetPackageId, versionInfo.TargetPackageVersion, false);
               Dte.StatusBar.Text = $"Finished installing {versionInfo.TargetPackageId} v{versionInfo.TargetPackageVersion}";
            }
            catch (Exception ex)
            {
               string message = $"Error installing {versionInfo.TargetPackageId} v{versionInfo.TargetPackageVersion}";
               Dte.StatusBar.Text = message;
               OutputWindow.OutputString(message + "\n");
               OutputWindow.OutputString(ex.Message + "\n");
               OutputWindow.Activate();
            }
         }
         else if (versionInfo.CurrentPackageId == versionInfo.TargetPackageId && versionInfo.CurrentPackageVersion == versionInfo.TargetPackageVersion)
         {
            string message = $"{versionInfo.TargetPackageId} v{versionInfo.TargetPackageVersion} already installed";
            Dte.StatusBar.Text = message;
         }
      }

      private bool ShouldLoadPackages(ModelRoot modelRoot, EFVersionDetails versionInfo)
      {
         Version currentPackageVersion = new Version(versionInfo.CurrentPackageVersion);
         Version targetPackageVersion = new Version(versionInfo.TargetPackageVersion);

         return ModelRoot.CanLoadNugetPackages &&
                (versionInfo.CurrentPackageId != versionInfo.TargetPackageId || currentPackageVersion != targetPackageVersion) &&
                (modelRoot.InstallNuGetPackages == AutomaticAction.True ||
                 ShowQuestionBox($"Referenced libraries don't match Entity Framework {modelRoot.NuGetPackageVersion.ActualPackageVersion}. Fix that now?") == DialogResult.Yes);
      }

      private static EFVersionDetails GetEFVersionDetails(ModelRoot modelRoot)
      {

         EFVersionDetails versionInfo = new EFVersionDetails
         {
            TargetPackageId = modelRoot.NuGetPackageVersion.PackageId
                                         ,
            TargetPackageVersion = modelRoot.NuGetPackageVersion.ActualPackageVersion
                                         ,
            CurrentPackageId = null
                                         ,
            CurrentPackageVersion = null
         };

         References references = ((VSProject)ActiveProject.Object).References;

         foreach (Reference reference in references)
         {
            if (string.Compare(reference.Name, NuGetHelper.PACKAGEID_EF6, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                string.Compare(reference.Name, NuGetHelper.PACKAGEID_EFCORE, StringComparison.InvariantCultureIgnoreCase) == 0)
            {
               versionInfo.CurrentPackageId = reference.Name;
               versionInfo.CurrentPackageVersion = reference.Version;

               break;
            }
         }

         return versionInfo;
      }
   }
}
