using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   internal static class Messages
   {
      private static ErrorListProvider _errorListProvider;

      public static void Initialize(IServiceProvider serviceProvider)
      {
         _errorListProvider = new ErrorListProvider(serviceProvider);
      }

      public static void AddError(string message)
      {
         AddTask(message, TaskErrorCategory.Error);
      }

      public static void AddWarning(string message)
      {
         AddTask(message, TaskErrorCategory.Warning);
      }

      public static void AddMessage(string message)
      {
         AddTask(message, TaskErrorCategory.Message);
      }

      private static void AddTask(string message, TaskErrorCategory category)
      {
         _errorListProvider?.Tasks?.Add(new ErrorTask
                                        {
                                           Category = TaskCategory.User
                                         , ErrorCategory = category
                                         , Text = message
                                        });
      }
   }
}
