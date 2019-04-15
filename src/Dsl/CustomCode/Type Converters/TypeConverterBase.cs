using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public class TypeConverterBase : TypeConverter
   {
      /// <summary>
      ///    Attempts to get to a store from the currently selected object(s) in the property grid.
      /// </summary>
      protected Store GetStore(object gridSelection)
      {
         // We assume that "instance" will either be a single model element, or   
         // an array of model elements (if multiple items are selected).  

         ModelElement currentElement = gridSelection is object[] objects && objects.Length > 0
                                          ? objects[0] as ModelElement
                                          : gridSelection as ModelElement;

         return currentElement?.Store;
      }

        protected ModelElement[] GetSelectedElements(object gridSelection)
        {
            object[] objects = gridSelection as object[];
            return objects?.Cast<ModelElement>().ToArray() ?? new[] { (ModelElement)gridSelection };
        }
    }
}
