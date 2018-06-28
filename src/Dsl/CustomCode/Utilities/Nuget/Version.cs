using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   public class Version
   {
      [JsonProperty("version")]
      public string VersionVersion { get; set; }

      [JsonProperty("downloads")]
      public long Downloads { get; set; }

      [JsonProperty("@id")]
      public string Id { get; set; }
   }
}