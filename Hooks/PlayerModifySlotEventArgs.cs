using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class PlayerModifySlotEventArgs: NetHookEventArgs {
    #region [Property: SlotId]
    private readonly byte slotId;

    public byte SlotId {
      get { return this.slotId; }
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
      TSPlayer player, byte slotId, byte itemStackSize, byte itemPrefix, short itemType
    ): base(player) {
      this.slotId = slotId;
      this.itemStackSize = itemStackSize;
      this.itemPrefix = itemPrefix;
      this.itemType = itemType;
    }
    #endregion
  }
}
