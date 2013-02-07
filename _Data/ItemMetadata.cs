// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public class ItemMetadata {
    #region [Property: Type]
    private int type;

    public int Type {
      get { return this.type; }
      set { this.type = value; }
    }
    #endregion

    #region [Property: Prefix]
    private byte prefix;

    public byte Prefix {
      get { return this.prefix; }
      set { this.prefix = value; }
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
    public ItemMetadata(int type, byte prefix, int stackSize) {
      this.type = type;
      this.prefix = prefix;
      this.stackSize = stackSize;
    }
    #endregion

    #region [Method: Static FromItem, FromNetItem, ToItem]
    public static ItemMetadata FromItem(Item item) {
      return new ItemMetadata(item.netID, (byte)item.stack, item.prefix);
    }

    public static ItemMetadata FromNetItem(NetItem netItem) {
      return new ItemMetadata(netItem.netID, (byte)netItem.prefix, netItem.stack);
    }

    public Item ToItem() {
      Item item = new Item();
      item.netDefaults(this.Type);
      item.Prefix(this.Prefix);
      item.stack = this.StackSize;

      return item;
    }
    #endregion
  }
}
