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
    public ChestModifySlotEventArgs(
      TSPlayer player, short chestIndex, byte slotIndex, byte itemStackSize, byte itemPrefix, short itemType
    ): base(player) {
      this.chestIndex = chestIndex;
      this.slotIndex = slotIndex;
      this.itemStackSize = itemStackSize;
      this.itemPrefix = itemPrefix;
      this.itemType = itemType;
    }
    #endregion
  }
}
