using System.IO;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ModelClassExtensions
   {
      public static string GetRelativeFileName(this ModelClass modelClass)
      {
         return Path.Combine(modelClass.OutputDirectory, modelClass.Name + ".cs");
      }
   }
}
