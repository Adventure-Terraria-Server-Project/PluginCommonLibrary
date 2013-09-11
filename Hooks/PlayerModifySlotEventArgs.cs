using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerModifySlotEventArgs: NetHookEventArgs {
    public byte SlotIndex { get; private set; }
    public ItemData NewItem { get; private set; }


    public PlayerModifySlotEventArgs(TSPlayer player, byte slotIndex, ItemData newItem): base(player) {
      this.SlotIndex = slotIndex;
      this.NewItem = newItem;
    }
  }
}
