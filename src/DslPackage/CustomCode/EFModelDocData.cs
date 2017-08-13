using System;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocData
   {
      protected override void OnDocumentLoaded()
      {
         base.OnDocumentLoaded();
         ModelRoot modelRoot = RootElement as ModelRoot;
         if (modelRoot == null) return;


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
            foreach (Association element in modelRoot.Store
                                                     .ElementDirectory
                                                     .FindElements<Association>())
            {
               AssociationChangeRules.UpdateDisplayForPersistence(element, element.Persistent);
               AssociationChangeRules.UpdateDisplayForCascadeDelete(element, element.SourceMultiplicity == Multiplicity.One ||
                                                                             element.TargetMultiplicity == Multiplicity.One ||
                                                                             element.SourceDeleteAction == DeleteAction.Cascade ||
                                                                             element.TargetDeleteAction == DeleteAction.Cascade);
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

         IVsCommandWindow commandWindow = (IVsCommandWindow)ServiceProvider.GetService(typeof(IVsCommandWindow));
         commandWindow?.ExecuteCommand("TextTransformation.TransformAllTemplates");
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
