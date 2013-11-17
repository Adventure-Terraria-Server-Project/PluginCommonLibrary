using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class UnlockEventArgs: TileLocationEventArgs {
    public UnlockType UnlockType { get; private set; }


    public UnlockEventArgs(TSPlayer player, DPoint location, UnlockType unlockType): base(player, location) {
      this.UnlockType = unlockType;
    }
  }
}
