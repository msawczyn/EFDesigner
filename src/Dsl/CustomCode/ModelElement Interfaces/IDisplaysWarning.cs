namespace Sawczyn.EFDesigner.EFModel
{
   public interface IDisplaysWarning
   {
      bool GetHasWarningValue();

      void ResetWarning();

      void RedrawItem();

   }
}
