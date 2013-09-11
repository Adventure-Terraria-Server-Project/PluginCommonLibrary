using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestModifySlotEventArgs: NetHookEventArgs {
    public short ChestIndex { get; private set; }
    public byte SlotIndex { get; private set; }
    public ItemData NewItem { get; private set; }


    public ChestModifySlotEventArgs(TSPlayer player, short chestIndex, byte slotIndex, ItemData newItem): base(player) {
      this.ChestIndex = chestIndex;
      this.SlotIndex = slotIndex;
      this.NewItem = newItem;
    }
  }
}
