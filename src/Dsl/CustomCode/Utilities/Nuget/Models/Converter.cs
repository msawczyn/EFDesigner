using System.Globalization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   internal static class Converter
   {
      public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
                                                               {
                                                                  MetadataPropertyHandling = MetadataPropertyHandling.Ignore
                                                                , DateParseHandling = DateParseHandling.None
                                                                , Converters =
                                                                  {
                                                                     TypeEnumConverter.Singleton
                                                                   , new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
                                                                  }
                                                               };
   }
}