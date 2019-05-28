using System;

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
