using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class LiquidSetEventArgs: TileLocationEventArgs {
    public byte LiquidAmount { get; private set; }
    public LiquidKind LiquidKind { get; private set; }


    public LiquidSetEventArgs(TSPlayer player, DPoint location, byte liquidAmount, LiquidKind liquidKind): base(player, location) {
      this.LiquidAmount = liquidAmount;
      this.LiquidKind = liquidKind;
    }
  }
}

