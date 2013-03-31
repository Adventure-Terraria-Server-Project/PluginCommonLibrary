using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class DoorUseEventArgs: TileLocationEventArgs {
    #region [Property: IsOpening]
    private readonly bool isOpening;

    public bool IsOpening {
      get { return this.isOpening; }
    }
    #endregion

    #region [Property: Direction]
    private readonly Direction direction;

    public Direction Direction {
      get { return this.direction; }
    }
    #endregion


    #region [Method: Constructor]
    public DoorUseEventArgs(TSPlayer player, DPoint location, bool isOpening, Direction direction): base(player, location) {
      this.isOpening = isOpening;
      this.direction = direction;
    }
    #endregion
  }
}