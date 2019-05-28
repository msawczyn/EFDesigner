namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelDocView 
   {
      public override void SetInfo()
      {
         base.SetInfo();
         Messages.AddStatus(Messages.LastStatusMessage);
      }

      protected EFModelExplorerToolWindow ModelExplorerWindow
      {
         get
         {
            return EFModelPackage.Instance?.GetToolWindow(typeof(EFModelExplorerToolWindow), true) as EFModelExplorerToolWindow;
         }
      }
   }
}
