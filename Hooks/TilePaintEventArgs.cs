using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class TilePaintEventArgs: TileLocationEventArgs {
    public PaintColor Color { get; private set; }


    public TilePaintEventArgs(TSPlayer player, DPoint location, PaintColor color): base(player, location) {
      this.Color = color;
    }
  }
}
