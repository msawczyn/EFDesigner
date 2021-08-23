using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;
using VSLangProj;

namespace Sawczyn.EFDesigner.EFModel
{
   // NOTE: This could go into the existing partial file if desired.  Might be more tidy and keep it all in one place.
   // Uncomment the following line if the extension is converted to an async package.
   //[ProvideAutoLoad(EFModelCommandSet.guidEFModelUIContextGuidString, PackageAutoLoadFlags.BackgroundLoad)]
   [ProvideUIContextRule(EFModelCommandSet.guidEFModelUIContextGuidString,
                         name : "EFModel auto load",
                         expression : "EFModel",
                         termNames : new[] {"EFModel"},
                         termValues : new[] {"HierSingleSelectionName:.efmodel$"})]
   internal sealed partial class EFModelPackage { }

   public static class CommandHelper
   {
      public static string EFModelerFileNameExtension = ".efmodel";
      private const string TextTransformationFileExtension = ".tt";

		public static string GetSingleFileSelectedPath()
		{
			if (!IsSingleProjectItemSelection(out IVsHierarchy hierarchy, out uint itemid)) return null;

			// Get the file path
         // ReSharper disable once SuspiciousTypeConversion.Global
         ((IVsProject)hierarchy).GetMkDocument(itemid, out string itemFullPath);
			return itemFullPath;
		}

		private static bool IsSingleProjectItemSelection(out IVsHierarchy hierarchy, out uint itemid)
		{
			hierarchy = null;
			itemid = VSConstants.VSITEMID_NIL;

         IVsMonitorSelection monitorSelection = Package.GetGlobalService(typeof(SVsShellMonitorSelection)) as IVsMonitorSelection;
         IVsSolution solution = Package.GetGlobalService(typeof(SVsSolution)) as IVsSolution;
			if (monitorSelection == null || solution == null)
			{
				return false;
			}

			IntPtr hierarchyPtr = IntPtr.Zero;
			IntPtr selectionContainerPtr = IntPtr.Zero;

			try
			{
				int hr = monitorSelection.GetCurrentSelection(out hierarchyPtr, out itemid, out IVsMultiItemSelect multiItemSelect, out selectionContainerPtr);

				if (ErrorHandler.Failed(hr) || hierarchyPtr == IntPtr.Zero || itemid == VSConstants.VSITEMID_NIL)
				{
					// there is no selection
					return false;
				}

				// multiple items are selected
				if (multiItemSelect != null) return false;

				// there is a hierarchy root node selected, thus it is not a single item inside a project

				if (itemid == VSConstants.VSITEMID_ROOT) return false;

				hierarchy = Marshal.GetObjectForIUnknown(hierarchyPtr) as IVsHierarchy;
				if (hierarchy == null) return false;

				if (ErrorHandler.Failed(solution.GetGuidOfProject(hierarchy, out Guid _)))
				{
					return false; // hierarchy is not a project inside the Solution if it does not have a ProjectID Guid
				}

				// if we got this far then there is a single project item selected
				return true;
			}
			finally
			{
				if (selectionContainerPtr != IntPtr.Zero)
				{
					Marshal.Release(selectionContainerPtr);
				}

				if (hierarchyPtr != IntPtr.Zero)
				{
					Marshal.Release(hierarchyPtr);
				}
			}
		}

		public static void VisibleAndEnabled(this MenuCommand menuCommand, bool visibleAndEnabled)
		{
			menuCommand.Visible = visibleAndEnabled;
			menuCommand.Enabled = visibleAndEnabled;
		}

		public static bool HasExtension(this FileInfo fileInfo, string extension)
		{
			return string.Compare(fileInfo.Extension, extension, StringComparison.OrdinalIgnoreCase) == 0;
		}

      // parameter must be EntityContainer file path
      public static void GenerateCode(string entityContainerFilepath)
      {
         DTE Dte = Package.GetGlobalService(typeof(DTE)) as DTE;
         DTE2 Dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2;

         ThreadHelper.ThrowIfNotOnUIThread();
         if (entityContainerFilepath == null)
         {
            throw new ArgumentNullException(nameof(entityContainerFilepath));
         }

         if (!entityContainerFilepath.EndsWith(EFModelerFileNameExtension))
         {
            throw new ArgumentOutOfRangeException(nameof(entityContainerFilepath));
         }

         ProjectItem modelProjectItem = Dte2.Solution.FindProjectItem(entityContainerFilepath);

         // Save file
         if (Guid.Parse(modelProjectItem.Kind) == VSConstants.GUID_ItemType_PhysicalFile && !modelProjectItem.Saved)
         {
            try
            {
               modelProjectItem.Save();
            }
            catch (Exception)
            {
               throw new Exception($"Save failed for {modelProjectItem.Name}");
            }
         }

         // TODO - clean up constant string
         string templateFilename = Path.ChangeExtension(entityContainerFilepath, TextTransformationFileExtension);

         ProjectItem templateProjectItem = Dte2.Solution.FindProjectItem(templateFilename);

         VSProjectItem templateVsProjectItem = templateProjectItem?.Object as VSProjectItem;

         if (templateVsProjectItem == null)
         {
            // TODO - place messages in output window or on PCML UI
            throw new Exception($"Tried to generate code but couldn't find {templateFilename} in the solution.");
         }

         try
         {
            Dte.StatusBar.Text = $"Generating code from {templateFilename}";
            templateVsProjectItem.RunCustomTool();
            Dte.StatusBar.Text = $"Finished generating code from {templateFilename}";
         }
         catch (COMException)
         {
            string message = $"Encountered an error generating code from {templateFilename}. Please transform T4 template manually.";
            Dte.StatusBar.Text = message;
            // TODO - place messages in output window or on PCML UI
            throw new Exception(message);
         }
      }
   }
}
