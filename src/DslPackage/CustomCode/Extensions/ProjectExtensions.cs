using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ProjectExtensions
   {
      public static string TargetFrameworkVersion(this Project project)
      {
         if (project == null)
            throw new ArgumentNullException(nameof(project));

         return project.Properties.Item("TargetFramework")?.Value?.ToString();
      }
   }
}
