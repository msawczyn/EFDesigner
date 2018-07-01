using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   public static class Serialize
   {
      public static string ToJson(this NuGetPackages self)
      {
         return JsonConvert.SerializeObject(self, Converter.Settings);
      }
   }
}