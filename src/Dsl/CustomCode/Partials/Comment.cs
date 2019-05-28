namespace Sawczyn.EFDesigner.EFModel
{
   public partial class Comment
   {
      private string GetShortTextValue()
      {
         return Text.Truncate(50);
      }
   }
}
