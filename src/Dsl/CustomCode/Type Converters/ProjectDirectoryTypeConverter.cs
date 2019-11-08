using System;
using System.Collections.Generic;
using System.ComponentModel;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public class ProjectDirectoryTypeConverter : TypeConverterBase
   {
      public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
      {
         Store store = GetStore(context.Instance);
         DTE dte = store?.GetService(typeof(DTE)) as DTE;
         Array projects = dte?.ActiveSolutionProjects as Array;

         if (projects?.GetValue(0) is Project currentProject)
         {
            List<string> result = new List<string> {string.Empty};
            result.AddRange(GetProjectDirectories(currentProject.ProjectItems, ""));

            return new StandardValuesCollection(result);
         }

         return base.GetStandardValues(context);
      }

      public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
      {
         return true;
      }

      public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
      {
         return true;
      }

      private List<string> GetProjectDirectories(ProjectItems projectItems, string root)
      {
         List<string> result = new List<string>();

         if (projectItems != null)
         {
            for (int index = 1; index <= projectItems.Count; index++)
            {
               ProjectItem item = projectItems.Item(index);

               if (item.Kind == Constants.vsProjectItemKindPhysicalFolder)
               {
                  string path = root.Length > 0 ? $"{root}\\{item.Name}" : item.Name;
                  result.Add(path);
                  result.AddRange(GetProjectDirectories(item.ProjectItems, path));
               }
            }
         }

         return result;
      }
   }
}
