using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class SendTileSquareEventArgs: TileLocationEventArgs {
    #region [Property: Size]
    private readonly short size;

    public short Size {
      get { return this.size; }
    }
    #endregion


    #region [Method: Constructor]
    public SendTileSquareEventArgs(TSPlayer player, DPoint tileLocation, short size): base(player, tileLocation) {
      this.size = size;
    }
    #endregion
  }
}