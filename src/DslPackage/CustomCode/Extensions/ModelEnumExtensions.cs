using System.IO;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class ModelEnumExtensions
   {
      public static string GetRelativeFileName(this ModelEnum modelEnum)
      {
         string outputDirectory = modelEnum.OutputDirectory ?? modelEnum.ModelRoot.EnumOutputDirectory;
         return Path.Combine(outputDirectory, $"{modelEnum.Name}.{modelEnum.ModelRoot.FileNameMarker}.cs");
      }
   }
}
