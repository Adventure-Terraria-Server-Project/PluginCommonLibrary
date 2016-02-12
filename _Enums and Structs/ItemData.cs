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
          (ItemPrefix)int.Parse(rawData[0]),
          (ItemType)int.Parse(rawData[1]),
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

    public ItemPrefix Prefix { get; set; }
    public ItemType Type  { get; set; }
    public int StackSize { get; set; }


    public ItemData(ItemPrefix prefix, ItemType type, int stackSize = 1) {
      this.Prefix = prefix;
      this.Type = type;
      this.StackSize = stackSize;
    }

    public ItemData(ItemType type, int stackSize = 1) {
      this.Prefix = ItemPrefix.None;
      this.Type = type;
      this.StackSize = stackSize;
    }

    public static ItemData FromItem(Item item) {
      return new ItemData((ItemPrefix)item.prefix, (ItemType)item.netID, item.stack);
    }

    public static ItemData FromNetItem(NetItem netItem) {
      return new ItemData((ItemPrefix)netItem.PrefixId, (ItemType)netItem.NetId, netItem.Stack);
    }

    [Pure]
    public Item ToItem() {
      Item item = new Item();
      item.netDefaults((int)this.Type);
      item.Prefix((int)this.Prefix);
      item.stack = this.StackSize;

      return item;
    }

    public override int GetHashCode() {
      return (int)this.Prefix ^ (int)this.Type ^ this.StackSize;
    }

    public bool Equals(ItemData other) {
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

    #region [Method: ToString]
    public override string ToString() {
      string format;
      if (this.Prefix == ItemPrefix.None)
        format = "{1} ({2})";
      else
        format = "{0} {1} ({2})";

      return string.Format(format, this.Prefix, this.Type, this.StackSize);
    }
    #endregion
  }
}
