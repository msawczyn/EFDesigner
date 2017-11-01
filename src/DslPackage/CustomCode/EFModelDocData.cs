using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using DslEditorPowerToy.VisualStudio;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;

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

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("StyleConnectors"))
         {
            // style association connectors if needed
            foreach (Association element in modelRoot.Store.ElementDirectory.FindElements<Association>())
            {
               AssociationChangeRules.UpdateDisplayForPersistence(element);
               AssociationChangeRules.UpdateDisplayForCascadeDelete(element);
            }
            tx.Commit();
         }

         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("HideConnectors"))
         {
            // hide any connectors that may have been hidden due to hidden shapes
            foreach (GeneralizationConnector connector in modelRoot.Store
                                                                   .ElementDirectory
                                                                   .FindElements<GeneralizationConnector>()
                                                                   .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible))
            {
               connector.Hide();
            }


            foreach (AssociationConnector connector in modelRoot.Store
                                                                .ElementDirectory
                                                                .FindElements<AssociationConnector>()
                                                                .Where(x => !x.FromShape.IsVisible || !x.ToShape.IsVisible))
            {
               connector.Hide();
            }

            tx.Commit();
         }

         // for older diagrams that didn't calculate this initially
         using (Transaction tx = modelRoot.Store.TransactionManager.BeginTransaction("EnsureEndpointRoles"))
         {
            foreach (Association association in modelRoot.Store.ElementDirectory.FindElements<Association>())
               AssociationChangeRules.SetEndpointRoles(association);

            tx.Commit();
         }
      }

      protected override void OnDocumentSaved(EventArgs e)
      {
         base.OnDocumentSaved(e);

         ModelRoot modelRoot = RootElement as ModelRoot;
         if (modelRoot?.TransformOnSave != true) return;

         DTE2 dte2 = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE)) as DTE2;

         //DTE dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
         //Project currentProject = GetActiveProject(dte);

         string filename = dte2.ActiveDocument.Name.Split('.')[0] + ".tt";
         ProjectItem item = dte2.Solution.FindProjectItem(filename);

         if (item != null)
         {
            try
            {
               item.DTE.ExecuteCommand("Project.RunCustomTool");
            }
            catch (COMException)
            {
               MessageBox.Show($"Encountered an error generating code from {filename}. Please run custom tool manually.");
            }
         }
         else
         {
            MessageBox.Show($"Tried to generate code but couldn\'t find {filename} in the solution.");
         }

         //IVsCommandWindow commandWindow = (IVsCommandWindow)ServiceProvider.GetService(typeof(IVsCommandWindow));
         //commandWindow?.ExecuteCommand("TextTransformation.TransformAllTemplates");
      }

      //public static ProjectItem FindProjectItemInProject(Project project, string name, bool recursive)
      //{
      //   ProjectItem projectItem = null;

      //   if (project.Kind != Constants.vsProjectKindSolutionItems)
      //   {
      //      if (project.ProjectItems != null && project.ProjectItems.Count > 0)
      //      {
      //         projectItem = DteHelper.FindItemByName(project.ProjectItems, name, recursive);
      //      }
      //   }
      //   else
      //   {
      //      // if solution folder, one of its ProjectItems might be a real project
      //      foreach (ProjectItem item in project.ProjectItems)
      //      {
      //         Project realProject = item.Object as Project;

      //         if (realProject != null)
      //         {
      //            projectItem = FindProjectItemInProject(realProject, name, recursive);

      //            if (projectItem != null)
      //            {
      //               break;
      //            }
      //         }
      //      }
      //   }

      //   return projectItem;
      //}
      
      
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

