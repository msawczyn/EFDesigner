using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal static class Messages
   {
      private static readonly string MessagePaneTitle = "Entity Framework Designer";

      private static IVsOutputWindow _outputWindow;
      private static IVsOutputWindow OutputWindow => _outputWindow ?? (_outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow);

      private static IVsOutputWindowPane _outputWindowPane;
      private static IVsOutputWindowPane OutputWindowPane
      {
         get
         {
            if (_outputWindowPane == null)
            {
               Guid paneGuid = new Guid(Constants.EFDesignerOutputPane);
               OutputWindow?.GetPane(ref paneGuid, out _outputWindowPane);

               if (_outputWindowPane == null)
               {
                  OutputWindow?.CreatePane(ref paneGuid, MessagePaneTitle, 1, 1);
                  OutputWindow?.GetPane(ref paneGuid, out _outputWindowPane);
               }
            }

            return _outputWindowPane;
         }
      }

      private static IVsStatusbar _statusBar;

      private static IVsStatusbar StatusBar => _statusBar ?? (_statusBar = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar);

      public static void AddError(string message)
      {
         AddMessage(message, "Error");
      }

      public static void AddWarning(string message)
      {
         AddMessage(message, "Warning");
      }

      public static void AddMessage(string message, string prefix = null)
      {
         OutputWindowPane?.OutputString($"{(string.IsNullOrWhiteSpace(prefix) ? "" : prefix + ": ")}{message}{(message.EndsWith("\n") ? "" : "\n")}");
         OutputWindowPane?.Activate();
      }

      public static void AddStatus(string message, Microsoft.VisualStudio.Shell.Interop.Constants? glyph = null)
      {
         StatusBar.IsFrozen(out int frozen);
         if (frozen == 0)
         {
            if (string.IsNullOrWhiteSpace(message))
            {
               ClearStatus();
               return;
            }

            if (glyph != null && glyph.Value.ToString().StartsWith("SBAI_"))
            {
               object icon = (short)glyph.Value;
               StatusBar.Animation(1, ref icon);
            }
            StatusBar.SetText(message);
         }
      }

      public static void ClearStatus()
      {
         StatusBar.IsFrozen(out int frozen);
         if (frozen == 0)
            StatusBar.Clear();
      }
   }
}
