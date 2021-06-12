using System.Linq;

using Microsoft.VisualStudio.Shell.Interop;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocView 
   {
      private static SearchForm searchForm;
      internal static SearchForm SearchForm
      {
         get 
         {
            return searchForm ?? (searchForm = new SearchForm());
         }
      }

      protected EFModelExplorerToolWindow ModelExplorerWindow
      {
         get
         {
            return EFModelPackage.Instance?.GetToolWindow(typeof(EFModelExplorerToolWindow), true) as EFModelExplorerToolWindow;
         }
      }

      /// <summary>
      ///    Called to initialize the view after the corresponding document has been loaded.
      /// </summary>
      protected override bool LoadView()
      {
         bool result = base.LoadView();

         if (result)
            Frame.SetProperty((int)__VSFPROPID.VSFPROPID_EditorCaption, $" [{Diagram.Name}]");

         return result;
      }

      /// <summary>
      ///    Called when window is closed. Overridden here to remove our objects from the selection context so that
      ///    the property browser doesn't call back on our objects after the window is closed.
      /// </summary>
      protected override void OnClose()
      {
         bool dirty = DocData.IsDirty(out int isDirty) == 0 && isDirty == 1;

         if (!DocData.DocViews.Except(new[] {this}).Any() && dirty && DocData.QuerySaveFile().CanSaveFile)
            DocData.Save(string.Empty, 1, 0);

         base.OnClose();
      }

      public override void SetInfo()
      {
         base.SetInfo();
         Messages.AddStatus(Messages.LastStatusMessage);
      }
   }
}