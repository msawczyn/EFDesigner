using System.Collections.Generic;
using System.Security.AccessControl;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace Sawczyn.EFDesigner.EFModel
{
   public class WizardImplementation : IWizard
   {
      private static string modelPath;
      private static string diagramPath;
      //private static string xsdPath;
      private static string ttPath;
      private static DTE dte;

      public void RunStarted(object automationObject,
                             Dictionary<string, string> replacementsDictionary,
                             WizardRunKind runKind,
                             object[] customParams)
      {
      }

      public void ProjectFinishedGenerating(Project project)
      {
      }

      public void ProjectItemFinishedGenerating(ProjectItem projectItem)
      {
         dte = dte ?? projectItem.DTE;
         string path = projectItem.FileNames[0];

         //if (path.EndsWith("EFModel.xsd"))
         //   xsdPath = path;
         //else 
         if (path.EndsWith(".efmodel"))
            modelPath = path;
         else if (path.EndsWith(".diagram"))
            diagramPath = path;
         else if (path.EndsWith(".tt"))
            ttPath = path;
      }

      
      public bool ShouldAddProjectItem(string filePath)
      {
         return true;
      }

      public void BeforeOpeningFile(ProjectItem projectItem)
      {
      }

      public void RunFinished()
      {
         if (modelPath != null && dte != null)
         {
            ProjectItem modelItem = dte.Solution.FindProjectItem(modelPath);
            if (modelItem != null)
            {
               //if (xsdPath != null)
               //{
               //   ProjectItem xsdItem = dte.Solution.FindProjectItem(xsdPath);
               //   if (xsdItem != null)
               //   {
               //      xsdItem.Remove();
               //      modelItem.ProjectItems.AddFromFile(xsdPath);
               //   }
               //}

               if (diagramPath != null)
               {
                  ProjectItem diagramItem = dte.Solution.FindProjectItem(diagramPath);
                  if (diagramItem != null)
                  {
                     diagramItem.Remove();
                     modelItem.ProjectItems.AddFromFile(diagramPath);
                  }
               }

               if (ttPath != null)
               {
                  ProjectItem ttItem = dte.Solution.FindProjectItem(ttPath);
                  if (ttItem != null)
                  {
                     ttItem.Remove();
                     modelItem.ProjectItems.AddFromFile(ttPath);
                  }
               }
            }
         }

         //xsdPath = null;
         diagramPath = null;
         modelPath = null;
         ttPath = null;
         dte = null;
      }
   }
}