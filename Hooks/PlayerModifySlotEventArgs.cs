using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerModifySlotEventArgs: PlayerSlotEventArgs {
    public ItemData NewItem { get; private set; }


    public PlayerModifySlotEventArgs(TSPlayer player, int slotIndex, ItemData newItem): base(player, slotIndex) {
      this.NewItem = newItem;
    }
  }
}
