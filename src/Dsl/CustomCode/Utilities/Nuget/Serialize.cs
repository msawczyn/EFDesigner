using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   public static class Serialize
   {
      public static string ToJson(this NugetPackages self)
      {
         return JsonConvert.SerializeObject(self, Converter.Settings);
      }
   }
}