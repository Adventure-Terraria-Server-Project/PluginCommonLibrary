// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;

using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public struct ItemMetadata {
    #region [Property: Static None]
    private static readonly ItemMetadata none = default(ItemMetadata);

    public static ItemMetadata None {
      get { return ItemMetadata.none; }
    }
    #endregion

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
