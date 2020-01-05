using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class SendTileSquareEventArgs: TileLocationEventArgs {
    public int Size { get; private set; }


    public SendTileSquareEventArgs(TSPlayer player, DPoint tileLocation, int size): base(player, tileLocation) {
      this.Size = size;
    }
  }
}