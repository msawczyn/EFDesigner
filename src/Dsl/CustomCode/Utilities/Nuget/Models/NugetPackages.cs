// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Sawczyn.EFDesigner.EFModel.Nuget;
//
//    var nugetPackages = NuGetPackages.FromJson(jsonString);

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.Nuget
{
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
