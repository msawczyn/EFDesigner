using System.IO;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ModelClassExtensions
   {
      public static string GetRelativeFileName(this ModelClass modelClass)
      {
         string outputDirectory = modelClass.OutputDirectory ?? modelClass.ModelRoot.EntityOutputDirectory;
         return Path.Combine(outputDirectory, $"{modelClass.Name}.{modelClass.ModelRoot.FileNameMarker}.cs");
      }
   }
}
