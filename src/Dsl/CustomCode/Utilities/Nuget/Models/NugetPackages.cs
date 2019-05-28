using System;
using System.Collections.Generic;

using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.Nuget
{
   // ReSharper disable once PartialTypeWithSinglePart
   public partial class NuGetPackages
   {
      [JsonProperty("@context")]
      public Context Context { get; set; }

      [JsonProperty("totalHits")]
      public long TotalHits { get; set; }

      [JsonProperty("lastReopen")]
      public DateTimeOffset LastReopen { get; set; }

      [JsonProperty("index")]
      public string Index { get; set; }

      [JsonProperty("data")]
      public List<PackageData> Data { get; set; }

      public static NuGetPackages FromJson(string json)
      {
         return JsonConvert.DeserializeObject<NuGetPackages>(json, Converter.Settings);
      }
   }
}
