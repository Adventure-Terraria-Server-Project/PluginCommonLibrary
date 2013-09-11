using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class SendTileSquareEventArgs: TileLocationEventArgs {
    public short Size { get; private set; }


    public SendTileSquareEventArgs(TSPlayer player, DPoint tileLocation, short size): base(player, tileLocation) {
      this.Size = size;
    }
  }
}