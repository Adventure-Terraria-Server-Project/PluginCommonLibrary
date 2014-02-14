using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestPlaceEventArgs: TileLocationEventArgs {
    public ChestStyle ChestStyle { get; private set; }


    public ChestPlaceEventArgs(TSPlayer player, DPoint chestLocation, ChestStyle chestStyle): base(player, chestLocation) {
      this.ChestStyle = chestStyle;
    }
  }
}
