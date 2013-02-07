// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class PlayerModifySlotEventArgs: NetHookEventArgs {
    #region [Property: SlotIndex]
    private readonly byte slotIndex;

    public byte SlotIndex {
      get { return this.slotIndex; }
    }
    #endregion

    #region [Property: ItemStackSize]
    private readonly byte itemStackSize;

    public byte ItemStackSize {
      get { return this.itemStackSize; }
    }
    #endregion

    #region [Property: ItemPrefix]
    private readonly byte itemPrefix;

    public byte ItemPrefix {
      get { return this.itemPrefix; }
    }
    #endregion

    #region [Property: ItemType]
    private readonly short itemType;

    public short ItemType {
      get { return this.itemType; }
    }
    #endregion


    #region [Method: Constructor]
    public PlayerModifySlotEventArgs(
      TSPlayer player, byte slotIndex, byte itemStackSize, byte itemPrefix, short itemType
    ): base(player) {
      this.slotIndex = slotIndex;
      this.itemStackSize = itemStackSize;
      this.itemPrefix = itemPrefix;
      this.itemType = itemType;
    }
    #endregion
  }
}
