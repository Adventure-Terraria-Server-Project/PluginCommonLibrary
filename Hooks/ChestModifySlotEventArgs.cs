using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestModifySlotEventArgs: NetHookEventArgs {
    public int ChestIndex { get; private set; }
    public int SlotIndex { get; private set; }
    public ItemData NewItem { get; private set; }


    public ChestModifySlotEventArgs(TSPlayer player, int chestIndex, int slotIndex, ItemData newItem): base(player) {
      this.ChestIndex = chestIndex;
      this.SlotIndex = slotIndex;
      this.NewItem = newItem;
    }
  }
}
