using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   public class Context
   {
      [JsonProperty("@vocab")]
      public string Vocab { get; set; }

      [JsonProperty("@base")]
      public string Base { get; set; }
   }
}