using System;

using Newtonsoft.Json;
using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  [JsonConverter(typeof(ItemMetadata.CJsonConverter))]
  public struct ItemMetadata {
    #region [Nested: CJsonConverter Class]
    public class CJsonConverter: JsonConverter {
      public override bool CanConvert(Type objectType) {
        return (objectType == typeof(ItemMetadata));
      }

      public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
        if (reader.TokenType != JsonToken.String)
          return ItemMetadata.None;

        string[] rawData = ((string)reader.Value).Split(',');
        return new ItemMetadata(
          (ItemPrefix)int.Parse(rawData[0]),
          (ItemType)int.Parse(rawData[1]),
          int.Parse(rawData[2])
        );
      }

      public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
        ItemMetadata itemMetadata = (ItemMetadata)value;
        writer.WriteValue(string.Format("{0}, {1}, {2}", (int)itemMetadata.Prefix, (int)itemMetadata.Type, itemMetadata.StackSize));
      }
    }
    #endregion

    public static readonly ItemMetadata None = default(ItemMetadata);

    #region [Property: Prefix]
    private readonly ItemPrefix prefix;

    public ItemPrefix Prefix {
      get { return this.prefix; }
    }
    #endregion

    #region [Property: Type]
    private readonly ItemType type;

    public ItemType Type {
      get { return this.type; }
    }
    #endregion

    #region [Property: StackSize]
    private readonly int stackSize;

    public int StackSize {
      get { return this.stackSize; }
    }
    #endregion


    #region [Method: Constructor]
    public ItemMetadata(ItemPrefix prefix, ItemType type, int stackSize) {
      this.prefix = prefix;
      this.type = type;
      this.stackSize = stackSize;
    }
    #endregion

    #region [Method: Static FromItem, FromNetItem, ToItem]
    public static ItemMetadata FromItem(Item item) {
      return new ItemMetadata((ItemPrefix)item.prefix, (ItemType)item.netID, (byte)item.stack);
    }

    public static ItemMetadata FromNetItem(NetItem netItem) {
      return new ItemMetadata((ItemPrefix)netItem.prefix, (ItemType)netItem.netID, netItem.stack);
    }

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

    public bool Equals(ItemMetadata other) {
      return (
        this.prefix == other.prefix &&
        this.type == other.type &&
        this.stackSize == other.stackSize
      );
    }

    public override bool Equals(object obj) {
      if (!(obj is ItemMetadata))
        return false;

      return this.Equals((ItemMetadata)obj);
    }

    public static bool operator ==(ItemMetadata a, ItemMetadata b) {
      return a.Equals(b);
    }

    public static bool operator !=(ItemMetadata a, ItemMetadata b) {
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
