using System;
using System.Diagnostics.Contracts;

using Newtonsoft.Json;

using TShockAPI;

namespace Terraria.Plugins.Common {
  [JsonConverter(typeof(ItemData.CJsonConverter))]
  public struct ItemData {
    #region [Nested: CJsonConverter Class]
    public class CJsonConverter: JsonConverter {
      public override bool CanConvert(Type objectType) {
        return (objectType == typeof(ItemData));
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType != JsonToken.String)
          return ItemData.None;

        string[] rawData = ((string)reader.Value).Split(',');
        return new ItemData(
          int.Parse(rawData[0]),
          int.Parse(rawData[1]),
          int.Parse(rawData[2])
        );
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        ItemData itemData = (ItemData)value;
        writer.WriteValue($"{(int)itemData.Prefix}, {(int)itemData.Type}, {itemData.StackSize}");
      }
    }
    #endregion

    public static readonly ItemData None = default(ItemData);

    public int Prefix { get; private set; }
    public int Type  { get; private set; }
    public int StackSize { get; private set; }


    public ItemData(int type, int stackSize = 1): this(0, type, stackSize) {}

    public ItemData(int prefix, int type, int stackSize = 1) {
      if (type != 0 && stackSize >= 1) {
        this.Prefix = prefix;
        this.Type = type;
        this.StackSize = stackSize;
      } else {
        // become ItemData.None
        this.Prefix = 0;
        this.Type = 0;
        this.StackSize = 0;
      }
    }

    public static ItemData FromItem(Item item) {
      if (item == null || item.type == 0 || item.stack <= 0)
        return ItemData.None;

      return new ItemData(item.prefix, item.netID, item.stack);
    }

    public static ItemData FromNetItem(NetItem netItem) {
      return new ItemData(netItem.PrefixId, netItem.NetId, netItem.Stack);
    }

    [Pure]
    public Item ToItem() {
      Item item = new Item();
      if (this.StackSize > 0) {
        item.netDefaults(this.Type);
        item.Prefix(this.Prefix);
        item.stack = this.StackSize;
      } else {
        item.netDefaults(0);
      }

      return item;
    }

    public override int GetHashCode() {
      return this.Prefix ^ this.Type ^ this.StackSize;
    }

    public bool Equals(ItemData other) {
      if (this.StackSize == 0 && other.StackSize == 0)
        return true;

      return (
        this.Prefix == other.Prefix &&
        this.Type == other.Type &&
        this.StackSize == other.StackSize
      );
    }

    public override bool Equals(object obj) {
      if (!(obj is ItemData))
        return false;

      return this.Equals((ItemData)obj);
    }

    public static bool operator ==(ItemData a, ItemData b) {
      return a.Equals(b);
    }

    public static bool operator !=(ItemData a, ItemData b) {
      return !a.Equals(b);
    }

    public override string ToString() {
      string format;
      if (this.Prefix == 0)
        format = "{1} ({2})";
      else
        format = "{0} {1} ({2})";

      return string.Format(format, this.Prefix, this.Type, this.StackSize);
    }
  }
}
