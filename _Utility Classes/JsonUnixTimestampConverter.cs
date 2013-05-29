using System;
using System.Diagnostics.Contracts;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Terraria.Plugins.Common {
  public class JsonUnixTimestampConverter: DateTimeConverterBase {
    public override bool CanConvert(Type objectType) {
      return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      bool isNullable = (objectType == typeof(DateTime?));

      if (reader.TokenType == JsonToken.Null) {
        if (isNullable)
          return null;
        else
          throw new FormatException("Can not convert null to DateTime.");
      } else if (reader.TokenType == JsonToken.Integer) {
        return DateTimeEx.FromUnixTime(Convert.ToInt32(reader.Value));
      } else {
        return DateTime.MinValue;
      }
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      if (!(value is DateTime))
        throw new FormatException("Expected DateTime value.");

      DateTime dateTime = (DateTime)value;
      writer.WriteValue(dateTime.ToUnixTime());
    }
  }
}