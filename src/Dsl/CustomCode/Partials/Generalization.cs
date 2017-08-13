using System.Collections.Generic;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class Generalization
   {
      public bool IsInCircularInheritance()
      {
         List<ModelClass> classes = new List<ModelClass>();
         ModelClass modelClass = Subclass;

         while (modelClass != null)
         {
            if (classes.Contains(modelClass))
               return true;

            classes.Add(modelClass);
            modelClass = modelClass.Superclass;
         }

         return false;
      }
   }
}
