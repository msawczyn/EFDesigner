using System.Collections.Generic;

using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   public class PackageData
   {
      [JsonProperty("@id")]
      public string Id { get; set; }

      [JsonProperty("@type")]
      public TypeEnum Type { get; set; }

      [JsonProperty("registration")]
      public string Registration { get; set; }

      [JsonProperty("id")]
      public string DatumId { get; set; }

      [JsonProperty("version")]
      public string Version { get; set; }

      [JsonProperty("description")]
      public string Description { get; set; }

      [JsonProperty("summary")]
      public string Summary { get; set; }

      [JsonProperty("title")]
      public string Title { get; set; }

      [JsonProperty("iconUrl", NullValueHandling = NullValueHandling.Ignore)]
      public string IconUrl { get; set; }

      [JsonProperty("licenseUrl", NullValueHandling = NullValueHandling.Ignore)]
      public string LicenseUrl { get; set; }

      [JsonProperty("projectUrl", NullValueHandling = NullValueHandling.Ignore)]
      public string ProjectUrl { get; set; }

      [JsonProperty("tags")]
      public List<string> Tags { get; set; }

      [JsonProperty("authors")]
      public List<string> Authors { get; set; }

      [JsonProperty("totalDownloads")]
      public long TotalDownloads { get; set; }

      [JsonProperty("verified")]
      public bool Verified { get; set; }

      [JsonProperty("versions")]
      public List<Version> Versions { get; set; }
   }
}