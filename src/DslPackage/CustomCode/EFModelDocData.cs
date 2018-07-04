using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Documents;
using System.Windows.Forms;

using EnvDTE;
using EnvDTE80;

using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

using NuGet.VisualStudio;

using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;
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


      private static DTE Dte => _dte ?? (_dte = Package.GetGlobalService(typeof(DTE)) as DTE);
      private static DTE2 Dte2 => _dte2 ?? (_dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2);
      private IComponentModel ComponentModel => _componentModel ?? (_componentModel = (IComponentModel)GetService(typeof(SComponentModel)));
      private IVsOutputWindowPane OutputWindow => _outputWindow ?? (_outputWindow = (IVsOutputWindowPane)GetService(typeof(SVsGeneralOutputWindowPane)));
      private IVsPackageInstallerServices NuGetInstallerServices => _nugetInstallerServices ?? (_nugetInstallerServices = ComponentModel?.GetService<IVsPackageInstallerServices>());
      private IVsPackageInstaller NuGetInstaller => _nugetInstaller ?? (_nugetInstaller = ComponentModel.GetService<IVsPackageInstaller>());
      private IVsPackageUninstaller NuGetUninstaller => _nugetUninstaller ?? (_nugetUninstaller = ComponentModel.GetService<IVsPackageUninstaller>());

      private static Project ActiveProject => Dte.ActiveSolutionProjects is Array activeSolutionProjects && activeSolutionProjects.Length > 0
                                                 ? activeSolutionProjects.GetValue(0) as Project
                                                 : null;

      internal static void GenerateCode(string filepath = null)
      {
         string filename = Path.ChangeExtension(filepath ?? Dte2.ActiveDocument.FullName, "tt");

         if (!(Dte2.Solution.FindProjectItem(filename)?.Object is VSProjectItem item))
            Messages.AddError($"Tried to generate code but couldn't find {filename} in the solution.");
         else
         {
            try
            {
               Dte.StatusBar.Text = $"Generating code from {filename}";
               item.RunCustomTool();
               Dte.StatusBar.Text = $"Finished generating code from {filename}";
            }
            catch (COMException)
            {
               string message = $"Encountered an error generating code from {filename}. Please transform T4 template manually.";
               Dte.StatusBar.Text = message;
               Messages.AddError(message);
            }
         }
      }

      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         ErrorDisplay.RegisterDisplayHandler(ShowErrorBox);

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

         List<GeneralizationConnector> generalizationConnectors = modelRoot.Store.ElementDirectory.FindElements<GeneralizationConnector>().Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();
         List<AssociationConnector> associationConnectors = modelRoot.Store.ElementDirectory.FindElements<AssociationConnector>().Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible).ToList();

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
      }

      // ReSharper disable once UnusedMember.Local
      private void ShowMessageBox(string message)
      {
         PackageUtility.ShowMessageBox(ServiceProvider, message, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_INFO);
      }

      // ReSharper disable once UnusedMember.Local
      private void ShowErrorBox(string message)
      {
         PackageUtility.ShowMessageBox(ServiceProvider, message, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_CRITICAL);
      }

      // ReSharper disable once UnusedMember.Local
      private DialogResult ShowQuestionBox(string question)
      {
         return PackageUtility.ShowMessageBox(ServiceProvider, question, OLEMSGBUTTON.OLEMSGBUTTON_YESNO, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, OLEMSGICON.OLEMSGICON_QUERY);
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         if (RootElement is ModelRoot modelRoot)
         {
            // if false, don't even check
            if (modelRoot.InstallNuGetPackages != AutomaticAction.False)
               EnsureCorrectNuGetPackages(modelRoot);

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

      public void EnsureCorrectNuGetPackages(ModelRoot modelRoot)
      {
         EFVersionDetails versionInfo = GetEFVersionDetails(modelRoot);

         if (ShouldLoadPackages(modelRoot, versionInfo))
         {
            // first unload what's there, if anything
            if (versionInfo.CurrentPackageId != null)
            {
               // only remove dependencies if we're switching EF types
               Dte.StatusBar.Text = $"Uninstalling {versionInfo.CurrentPackageId} v{versionInfo.CurrentPackageVersion}";

               try
               {
                  //List<string> uninstallSequence = GetPackagesToUninstall(versionInfo.CurrentPackageId);
                  NuGetUninstaller.UninstallPackage(ActiveProject, versionInfo.CurrentPackageId, true /*versionInfo.TargetPackageId != versionInfo.CurrentPackageId*/);
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

      private List<string> GetPackagesToUninstall(string targetPackageId, IEnumerable<IVsPackageMetadata> installedPackages = null)
      {
         List<string> result = new List<string>();
         if (installedPackages == null)
            installedPackages = NuGetInstallerServices.GetInstalledPackages();

         IVsPackageMetadata target = installedPackages.FirstOrDefault(p => p.Id == targetPackageId);

         if (target != null)
         {
            // TODO
         }

         return result;
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

      private EFVersionDetails GetEFVersionDetails(ModelRoot modelRoot)
      {

         EFVersionDetails versionInfo = new EFVersionDetails
                                        {
                                           TargetPackageId = modelRoot.NuGetPackageVersion.PackageId
                                         , TargetPackageVersion = modelRoot.NuGetPackageVersion.ActualPackageVersion
                                         , CurrentPackageId = null
                                         , CurrentPackageVersion = null
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
