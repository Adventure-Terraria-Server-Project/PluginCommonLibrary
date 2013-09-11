using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestOpenEventArgs: TileLocationEventArgs {
    public int ChestIndex { get; private set; }


    public ChestOpenEventArgs(TSPlayer player, int chestIndex, DPoint chestLocation): base(player, chestLocation) {
      this.ChestIndex = chestIndex;
    }
  }
}