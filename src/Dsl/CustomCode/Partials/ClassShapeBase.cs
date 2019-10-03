using System.Drawing;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract partial class ClassShapeBase
   {
      private static string GetDisplayPropertyFromModelClassForAssociationsCompartment(ModelElement element)
      {
         Association association = (Association)element;
         ModelClass target = association.Target;
         
         if (!string.IsNullOrEmpty(association.TargetPropertyName))
            return $"{association.TargetPropertyName} : {target.Name}";

         return target.Name;
      }

      private static string GetDisplayPropertyFromModelClassForSourcesCompartment(ModelElement element)
      {
         BidirectionalAssociation association = (BidirectionalAssociation)element;
         ModelClass source = association.Source;

         if (!string.IsNullOrEmpty(association.SourcePropertyName))
            return $"{association.SourcePropertyName} : {source.Name}";

         return source.Name;
      }

      private static string GetDisplayPropertyFromModelClassForAttributesCompartment(ModelElement element)
      {
         ModelAttribute attribute = (ModelAttribute)element;

         string nullable = attribute.Required ? "" : "?";
         string name = attribute.Name;
         string type = attribute.Type;
         string initial = !string.IsNullOrEmpty(attribute.InitialValue) ? " = " + attribute.InitialValue : "";

         string lengthDisplay = "";

         if (attribute.MinLength > 0)
            lengthDisplay = $"[{attribute.MinLength}-{attribute.MaxLength}]";
         else if (attribute.MaxLength > 0)
            lengthDisplay = $"[{attribute.MaxLength}]";

         return $"{name} : {type}{nullable}{lengthDisplay}{initial}";
      }

      internal sealed partial class FillColorPropertyHandler
      {
         protected override void OnValueChanging(ClassShapeBase element, Color oldValue, Color newValue)
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
   }
}
