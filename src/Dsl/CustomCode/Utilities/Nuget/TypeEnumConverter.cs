using System;

using Newtonsoft.Json;

namespace Sawczyn.EFDesigner.EFModel.Nuget {
   internal class TypeEnumConverter : JsonConverter
   {
      public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();

      public override bool CanConvert(Type t)
      {
         return (t == typeof(TypeEnum)) || (t == typeof(TypeEnum?));
      }

      public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
      {
         if (reader.TokenType == JsonToken.Null) return null;

         string value = serializer.Deserialize<string>(reader);

         if (value == "Package")
            return TypeEnum.Package;

         throw new Exception("Cannot unmarshal type TypeEnum");
      }

      public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
      {
         if (untypedValue == null)
         {
            serializer.Serialize(writer, null);

            return;
         }

         TypeEnum value = (TypeEnum)untypedValue;

         if (value == TypeEnum.Package)
         {
            serializer.Serialize(writer, "Package");

            return;
         }

         throw new Exception("Cannot marshal type TypeEnum");
      }
   }
}