// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;

using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public class ItemMetadata {
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
