using System.Drawing;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class CommentBoxShapeBase
   {
      private bool GetVisibleValue()
      {
         return IsVisible;
      }

      private void SetVisibleValue(bool newValue)
      {
         if (newValue)
            Show();
         else
            Hide();
      }

      internal sealed partial class FillColorPropertyHandler
      {
         protected override void OnValueChanging(CommentBoxShapeBase element, Color oldValue, Color newValue)
         {
            base.OnValueChanging(element, oldValue, newValue);

            if (element.Store.InUndoRedoOrRollback || element.Store.InSerializationTransaction)
               return;

            // set text color to contrasting color based on new fill color, if it's currently black or white
            if (element.TextColor == Color.Black || element.TextColor == Color.White)
               element.TextColor = newValue.LegibleTextColor();
         }
      }
   }
}