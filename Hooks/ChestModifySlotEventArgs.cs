// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class ChestModifySlotEventArgs: NetHookEventArgs {
    #region [Property: ChestIndex]
    private readonly short chestIndex;

    public short ChestIndex {
      get { return this.chestIndex; }
    }
    #endregion

    #region [Property: SlotIndex]
    private readonly byte slotIndex;

    public byte SlotIndex {
      get { return this.slotIndex; }
    }
    #endregion

    #region [Property: ItemPrefix]
    private readonly ItemPrefix itemPrefix;

    public ItemPrefix ItemPrefix {
      get { return this.itemPrefix; }
    }
    #endregion

    #region [Property: ItemType]
    private readonly ItemType itemType;

    public ItemType ItemType {
      get { return this.itemType; }
    }
    #endregion

    #region [Property: ItemStackSize]
    private readonly byte itemStackSize;

    public byte ItemStackSize {
      get { return this.itemStackSize; }
    }
    #endregion


    #region [Method: Constructor]
    public ChestModifySlotEventArgs(
      TSPlayer player, short chestIndex, byte slotIndex, ItemPrefix itemPrefix, ItemType itemType, byte itemStackSize
    ): base(player) {
      this.chestIndex = chestIndex;
      this.slotIndex = slotIndex;
      this.itemPrefix = itemPrefix;
      this.itemType = itemType;
      this.itemStackSize = itemStackSize;
    }
    #endregion
  }
}
