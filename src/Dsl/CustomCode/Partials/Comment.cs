namespace Sawczyn.EFDesigner.EFModel
{
   public partial class Comment: IHasStore
   {
      private string GetShortTextValue()
      {
         return Text.Truncate(50);
      }

      private string GetNameValue()
      {
         return GetShortTextValue();
      }

   }
}
