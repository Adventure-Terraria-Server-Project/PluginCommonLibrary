﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TShockAPI;

namespace Terraria.Plugins.Common {
  public class JsonTSPlayerConverter: JsonConverter {
    public override bool CanConvert(Type objectType) {
      return objectType == typeof(TSPlayer);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
      throw new NotImplementedException();
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
      TSPlayer tsPlayer = (TSPlayer)value;
      writer.WriteValue(tsPlayer.Name);
    }
  }
}
