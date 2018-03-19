using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode;
using VSLangProj;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocData
   {
      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         if (!(RootElement is ModelRoot modelRoot)) return;

         // set to the project's namespace if no namespace set
         if (string.IsNullOrEmpty(modelRoot.Namespace))
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("SetDefaultNamespace"))
            {
               DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
               Project currentProject = GetActiveProject(dte);
               modelRoot.Namespace = currentProject.Properties.Item("DefaultNamespace")?.Value as string;
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
                                                                           .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible)
                                                                           .ToList();
         List<AssociationConnector> associationConnectors = modelRoot.Store
                                                                     .ElementDirectory
                                                                     .FindElements<AssociationConnector>()
                                                                     .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible)
                                                                     .ToList();

         if (generalizationConnectors.Any() || associationConnectors.Any())
         {
            using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("HideConnectors"))
            {
               // hide any connectors that may have been hidden due to hidden shapes
               foreach (GeneralizationConnector connector in generalizationConnectors) { connector.Hide(); }
               foreach (AssociationConnector connector in associationConnectors) { connector.Hide(); }
               tx.Commit();
            }
         }
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         ModelRoot modelRoot = RootElement as ModelRoot;
         if (modelRoot?.TransformOnSave != true) return;

         DocumentSavedEventArgs documentSavedEventArgs = (DocumentSavedEventArgs)e;
         GenerateCode(documentSavedEventArgs.NewFileName);
      }

      internal static void GenerateCode(string filepath = null)
      {
         DTE2 dte2 = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE)) as DTE2;

         string filename = Path.ChangeExtension(filepath ?? dte2.ActiveDocument.FullName, "tt");
         VSProjectItem item = dte2.Solution.FindProjectItem(filename)?.Object as VSProjectItem;

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

      internal static Project GetActiveProject(DTE dte)
      {
         Project activeProject = null;

         Array activeSolutionProjects = dte.ActiveSolutionProjects as Array;
         if (activeSolutionProjects != null && activeSolutionProjects.Length > 0)
            activeProject = activeSolutionProjects.GetValue(0) as Project;

         return activeProject;
      }
   }
}

