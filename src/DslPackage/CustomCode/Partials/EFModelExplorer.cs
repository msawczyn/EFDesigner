using System;

namespace Sawczyn.EFDesigner.EFModel
{
   internal partial class EFModelExplorer
   {
      /// <summary>Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick" /> event.</summary>
      /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
      protected override void OnDoubleClick(EventArgs e)
      {
         base.OnDoubleClick(e);

         // let's make this a switch so we can more easily extend it to different element types later
         switch (SelectedElement)
         {
            case ModelClass modelClass:
               EFModelDocData.OpenFileFor(modelClass);
               break;
            case ModelAttribute modelAttribute:
               EFModelDocData.OpenFileFor(modelAttribute.ModelClass);
               break;
            case ModelEnum modelEnum:
               EFModelDocData.OpenFileFor(modelEnum);
               break;
            case ModelEnumValue modelEnumValue:
               EFModelDocData.OpenFileFor(modelEnumValue.Enum);
               break;
         }
      }
   }
}
