using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;
using EnvDTE80;

//using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

#if DO_NUGET
using System.Windows.Forms;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
#endif

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;

using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.VisualStudio;

using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;

using VSLangProj;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocData
   {
      private static DTE _dte;
      private static DTE2 _dte2;

      #if DO_NUGET
      private IVsPackageInstaller _nugetInstaller;
      private IVsPackageUninstaller _nugetUninstaller;
      private IVsPackageInstallerServices _nugetInstallerServices;
      
      private IVsPackageInstallerServices NuGetInstallerServices => _nugetInstallerServices ?? (_nugetInstallerServices = ((IComponentModel)GetService(typeof(SComponentModel)))?.GetService<IVsPackageInstallerServices>());
      private IVsPackageInstaller NuGetInstaller => _nugetInstaller ?? (_nugetInstaller = ((IComponentModel)GetService(typeof(SComponentModel))).GetService<IVsPackageInstaller>());
      private IVsPackageUninstaller NuGetUninstaller => _nugetUninstaller ?? (_nugetUninstaller = ((IComponentModel)GetService(typeof(SComponentModel))).GetService<IVsPackageUninstaller>());
      #endif

      private static DTE Dte => _dte ?? (_dte = Package.GetGlobalService(typeof(DTE)) as DTE);
      private static DTE2 Dte2 => _dte2 ?? (_dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2);

      private static Project ActiveProject => Dte.ActiveSolutionProjects is Array activeSolutionProjects && (activeSolutionProjects.Length > 0)
                                          ? activeSolutionProjects.GetValue(0) as Project
                                          : null;

      internal static void GenerateCode(string filepath = null)
      {
         string filename = Path.ChangeExtension(filepath ?? Dte2.ActiveDocument.FullName, "tt");
         VSProjectItem item = Dte2.Solution.FindProjectItem(filename)?.Object as VSProjectItem;

         if (item == null)
            Messages.AddError($"Tried to generate code but couldn't find {filename} in the solution.");
         else
         {
            try
            {
               item.RunCustomTool();
            }
            catch (COMException)
            {
               Messages.AddError($"Encountered an error generating code from {filename}. Please transform T4 template manually.");
            }
         }
      }

#if DO_NUGET

      internal bool GetInstalledEFNugetPackages(out string packageName, out string packageVersion)
      {
         packageName = null;
         packageVersion = null;

         if (NuGetInstallerServices == null) 
            return false;

         if (NuGetInstallerServices.IsPackageInstalled(ActiveProject, "EntityFramework"))
         {
            packageName = "EntityFramework";
            packageVersion = NuGetInstallerServices.GetInstalledPackages().FirstOrDefault(p => p.Title == "EntityFramework").VersionString;
         }

         if (NuGetInstallerServices.IsPackageInstalled(ActiveProject, "Microsoft.EntityFrameworkCore"))
         {
            packageName = "Microsoft.EntityFrameworkCore";
            packageVersion = NuGetInstallerServices.GetInstalledPackages().FirstOrDefault(p => p.Title == "Microsoft.EntityFrameworkCore").VersionString;
         }

         return true;
      }

      internal bool? HasCorrectNugetPackages(ModelRoot modelRoot)
      {
         if (NuGetInstallerServices != null)
         {
            string efPackageName = GetRequestedEFPackageName(modelRoot);
            string requestedVersion = modelRoot.EFVersionString.Split(' ').Last();

            return NuGetInstallerServices.IsPackageInstalled(ActiveProject, efPackageName) && 
                   (NuGetInstallerServices.GetInstalledPackages().FirstOrDefault(p => p.Title == efPackageName)?.VersionString?.StartsWith(requestedVersion) == true);
         }

         return null;
      }
#endif

      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         ErrorDisplay.RegisterDisplayHandler(ShowMessageBox);

         if (!(RootElement is ModelRoot modelRoot)) return;

#if DO_NUGET

         if (NuGetInstaller == null || NuGetUninstaller == null || NuGetInstallerServices == null)
            ModelRoot.CanLoadNugetPackages = false;
#endif

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

      private void ShowMessageBox(string message)
      {
         PackageUtility.ShowMessageBox(ServiceProvider, message, OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_CRITICAL);
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         ModelRoot modelRoot = RootElement as ModelRoot;

#if DO_NUGET
         if (modelRoot.InstallNugetPackages != AutomaticAction.False)
         {
            bool? hasCorrectNugetPackages = HasCorrectNugetPackages(modelRoot);

            if (hasCorrectNugetPackages == null)
            {
               string message = "Can't tell if Nuget packages are correct. References weren't updated.";
               Messages.AddWarning(message);
            }
            else // we know if the packages are correct or not
            {
               bool shouldLoadPackages = (modelRoot.InstallNugetPackages == AutomaticAction.True) || 
                                         (PackageUtility.ShowMessageBox(ServiceProvider, $"Referenced libraries don't support Entity Framework {modelRoot.EFVersionString}. Fix that now?", OLEMSGBUTTON.OLEMSGBUTTON_YESNO, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_SECOND, OLEMSGICON.OLEMSGICON_QUERY) == DialogResult.Yes);

               if (shouldLoadPackages)
                  AlignNugetPackages(modelRoot);
            }
         }
#endif

         if (modelRoot?.TransformOnSave == true)
         {
            DocumentSavedEventArgs documentSavedEventArgs = (DocumentSavedEventArgs)e;
            GenerateCode(documentSavedEventArgs.NewFileName);
         }
      }


      public static async void LoadNuGet(ModelRoot modelRoot)
      {
         if (modelRoot == null) return;


         string targetPackageId = modelRoot.EntityFrameworkVersion == EFVersion.EF6
                                 ? "EntityFramework"
                                 : "Microsoft.EntityFrameworkCore";

         string targetPackageVersion = modelRoot.NuGetPackageVersion.ActualPackageVersion;

         string targetTargetFramework = ActiveProject?.TargetFrameworkVersion();

         IEnumerable<IPackageSearchMetadata> efPackages = await FindNugetPackage(targetPackageId);
         
         NuGetFramework nuGetFramework = targetTargetFramework == null
                                              ? NuGetFramework.AnyFramework
                                              : NuGetFramework.ParseFrameworkName(targetTargetFramework, new DefaultFrameworkNameProvider());

         IPackageSearchMetadata efPackage = efPackages.FirstOrDefault(p => p.IsListed && p.Identity.Id == targetPackageId);
         IEnumerable<VersionInfo> versions = await efPackage.GetVersionsAsync();

         //var componentModel = (IComponentModel)GetService(typeof(SComponentModel));
         //IVsPackageInstallerServices installerServices =
         //   componentModel.GetService<IVsPackageInstallerServices>();

         //var installedPackages = installerServices.GetInstalledPackages();
      }

      //private bool InstallNuGetPackage(Project project, string package)
      //{
      //   bool installedPkg = true;
      //   var dte = Package.GetGlobalService(typeof(DTE)) as DTE;
      //   try
      //   {
      //      var componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
      //      IVsPackageInstallerServices installerServices = componentModel.GetService();
      //      if (!installerServices.IsPackageInstalled(project, package))
      //      {
      //         dte.StatusBar.Text = @"Installing " + package + " NuGet package, this may take a minute...";
      //         var installer = componentModel.GetService();
      //         installer.InstallPackage(null, project, package, (System.Version)null, false);                      dte.StatusBar.Text = @"Finished installing the " + package + " NuGet package";                }
      //   }
      //   catch (Exception ex)
      //   {
      //      installedPkg = false;
      //      dte.StatusBar.Text = @"Unable to install the  " + package + " NuGet package";
      //   }
      //   return installedPkg;
      //}
      
      private static async Task<IEnumerable<IPackageSearchMetadata>> FindNugetPackage(string packageId)
      {
         List<Lazy<INuGetResourceProvider>> providers = new List<Lazy<INuGetResourceProvider>>();
         providers.AddRange(Repository.Provider.GetCoreV3());  // Add v3 API support
         PackageSource packageSource = new PackageSource("https://api.nuget.org/v3/index.json");
         SourceRepository sourceRepository = new SourceRepository(packageSource, providers);
         PackageMetadataResource packageMetadataResource = await sourceRepository.GetResourceAsync<PackageMetadataResource>();
         IEnumerable<IPackageSearchMetadata> searchMetadata = await packageMetadataResource.GetMetadataAsync("Wyam.Core", true, true, null, CancellationToken.None);

         return searchMetadata;
      }
   }
}
