using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class LiquidSetEventArgs: TileLocationEventArgs {
    public int LiquidAmount { get; private set; }
    public LiquidKind LiquidKind { get; private set; }


    public LiquidSetEventArgs(TSPlayer player, DPoint location, int liquidAmount, LiquidKind liquidKind): base(player, location) {
      this.LiquidAmount = liquidAmount;
      this.LiquidKind = liquidKind;
    }
  }
}

