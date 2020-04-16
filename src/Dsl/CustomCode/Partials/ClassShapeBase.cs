using System.Drawing;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract partial class ClassShapeBase: IHasStore
   {
      private static string GetDisplayPropertyFromModelClassForAssociationsCompartment(ModelElement element)
      {
         Association association = (Association)element;
         ModelClass target = association.Target;
         
         // ReSharper disable once ConvertIfStatementToReturnStatement
         if (!string.IsNullOrEmpty(association.TargetPropertyName))
            return $"{association.TargetPropertyName} : {target.Name}";

         return target.Name;
      }

      private static string GetDisplayPropertyFromModelClassForSourcesCompartment(ModelElement element)
      {
         BidirectionalAssociation association = (BidirectionalAssociation)element;
         ModelClass source = association.Source;

         // ReSharper disable once ConvertIfStatementToReturnStatement
         if (!string.IsNullOrEmpty(association.SourcePropertyName))
            return $"{association.SourcePropertyName} : {source.Name}";

         return source.Name;
      }

      private static string GetDisplayPropertyFromModelClassForAttributesCompartment(ModelElement element)
      {
         return ((ModelAttribute)element).ToDisplayString();
      }

      internal sealed partial class FillColorPropertyHandler
      {
         protected override void OnValueChanging(ClassShapeBase element, Color oldValue, Color newValue)
         {
            base.OnValueChanging(element, oldValue, newValue);

            if (element.Store.InUndoRedoOrRollback || element.Store.InSerializationTransaction)
               return;

            // set text color to contrasting color based on new fill color, if it's currently black or white
            if (element.TextColor == Color.Black || element.TextColor == Color.White)
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
   }
}
