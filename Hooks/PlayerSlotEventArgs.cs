using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerSlotEventArgs: NetHookEventArgs {
    public int SlotIndex { get; private set; }


    public PlayerSlotEventArgs(TSPlayer player, int slotIndex): base(player) {
      this.SlotIndex = slotIndex;
    }
  }
}
