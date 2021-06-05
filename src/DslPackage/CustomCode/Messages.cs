using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal static class Messages
   {
      private static readonly string MessagePaneTitle = "Entity Framework Designer";

      private static IVsOutputWindow _outputWindow;
      private static IVsOutputWindow OutputWindow
      {
         get
         {
            return _outputWindow ?? (_outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow);
         }
      }

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

      private static IVsStatusbar StatusBar
      {
         get
         {
            return _statusBar ?? (_statusBar = Package.GetGlobalService(typeof(SVsStatusbar)) as IVsStatusbar);
         }
      }

      private const string ERROR = "Error";
      private const string WARNING = "Warning";

      public static void AddError(string message)
      {
         AddMessage(message, ERROR);
      }

      public static void AddWarning(string message)
      {
         AddMessage(message, WARNING);
      }

      public static void AddMessage(string message, string prefix = null)
      {
         OutputWindowPane?.OutputString($"{(string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ": ")}{message}{(message.EndsWith("\n") ? string.Empty : "\n")}");
         if (prefix == ERROR || prefix == WARNING)
            OutputWindowPane?.Activate();
      }

      public static string LastStatusMessage
      {
         get;
         set;
      }

      public static void AddStatus(string message, Microsoft.VisualStudio.Shell.Interop.Constants? glyph = null)
      {
         StatusBar.IsFrozen(out int frozen);
         if (frozen == 0)
         {
            if (string.IsNullOrWhiteSpace(message))
            {
               StatusBar.SetText(string.Empty);
               return;
            }

            if (glyph != null && glyph.Value.ToString().StartsWith("SBAI_"))
            {
               object icon = (short)glyph.Value;
               StatusBar.Animation(1, ref icon);
            }
            StatusBar.SetText(message);
            LastStatusMessage = message;
         }
      }

      public static string GetChoice(string title, IEnumerable<string> choices)
      {
         ChooseForm form = new ChooseForm {Title = title};
         form.SetChoices(choices);

         return form.ShowDialog() == System.Windows.Forms.DialogResult.OK
                   ? form.Selection
                   : null;
      }
   }
}
