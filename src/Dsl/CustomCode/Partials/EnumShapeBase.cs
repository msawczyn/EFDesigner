using System.Drawing;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EnumShapeBase
   {
      internal sealed partial class FillColorPropertyHandler
      {
         protected override void OnValueChanging(EnumShapeBase element, Color oldValue, Color newValue)
         {
            base.OnValueChanging(element, oldValue, newValue);

            if (element.Store.InUndoRedoOrRollback || element.Store.InSerializationTransaction)
               return;

            // set text color to contrasting color based on new fill color
            element.TextColor = newValue.LegibleTextColor();
         }
      }

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

      static string GetDisplayPropertyFromModelEnumForValuesCompartment(ModelElement element)
      {
         ModelEnumValue enumValue = element as ModelEnumValue;
         if (enumValue == null)
            return "?";

         return !string.IsNullOrWhiteSpace(enumValue.Value)
                   ? $"{enumValue.Name} = {enumValue.Value}"
                   : enumValue.Name;
      }
   }
}
