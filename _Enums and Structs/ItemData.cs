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
        writer.WriteValue(string.Format("{0}, {1}, {2}", (int)itemData.Prefix, (int)itemData.Type, itemData.StackSize));
      }
    }
    #endregion

    public static readonly ItemData None = default(ItemData);

    #region [Property: Prefix]
    private ItemPrefix prefix;

    public ItemPrefix Prefix {
      get { return this.prefix; }
      set { this.prefix = value; }
    }
    #endregion

    #region [Property: Type]
    private ItemType type;

    public ItemType Type {
      get { return this.type; }
      set { this.type = value; }
    }
    #endregion

    #region [Property: StackSize]
    private int stackSize;

    public int StackSize {
      get { return this.stackSize; }
      set { this.stackSize = value; }
    }
    #endregion


    #region [Method: Constructor]
    public ItemData(ItemPrefix prefix, ItemType type, int stackSize) {
      this.prefix = prefix;
      this.type = type;
      this.stackSize = stackSize;
    }
    #endregion

    #region [Method: Static FromItem, FromNetItem, ToItem]
    public static ItemData FromItem(Item item) {
      return new ItemData((ItemPrefix)item.prefix, (ItemType)item.netID, (byte)item.stack);
    }

    public static ItemData FromNetItem(NetItem netItem) {
      return new ItemData((ItemPrefix)netItem.prefix, (ItemType)netItem.netID, netItem.stack);
    }

    [Pure]
    public Item ToItem() {
      Item item = new Item();
      item.netDefaults((int)this.Type);
      item.Prefix((byte)this.Prefix);
      item.stack = this.StackSize;

      return item;
    }
    #endregion

    #region [Methods: GetHashCode, Equals, ==, !=]
    public override int GetHashCode() {
      return (int)this.Prefix ^ (int)this.Type ^ this.StackSize;
    }

    public bool Equals(ItemData other) {
      return (
        this.prefix == other.prefix &&
        this.type == other.type &&
        this.stackSize == other.stackSize
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
    #endregion

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
