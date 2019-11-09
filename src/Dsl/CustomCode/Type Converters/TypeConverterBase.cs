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

         IHasStore currentElement = gridSelection is object[] objects && objects.Length > 0
                                       ? objects[0] as IHasStore
                                       : gridSelection as IHasStore;

         return currentElement?.Store;
      }

      protected IHasStore[] GetSelectedElements(object gridSelection)
        {
            object[] objects = gridSelection as object[];
            return objects?.Cast<IHasStore>().ToArray() ?? new[] { (IHasStore)gridSelection };
        }
    }
}
