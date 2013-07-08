using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerModifySlotEventArgs: NetHookEventArgs {
    #region [Property: SlotIndex]
    private readonly byte slotIndex;

    public byte SlotIndex {
      get { return this.slotIndex; }
    }
    #endregion

    #region [Property: NewItem]
    private readonly ItemData newItem;

    public ItemData NewItem {
      get { return this.newItem; }
    }
    #endregion


    #region [Method: Constructor]
    public PlayerModifySlotEventArgs(TSPlayer player, byte slotIndex, ItemData newItem): base(player) {
      this.slotIndex = slotIndex;
      this.newItem = newItem;
    }
    #endregion
  }
}
