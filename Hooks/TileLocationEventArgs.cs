using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class TileLocationEventArgs: NetHookEventArgs {
    #region [Property: Location]
    private readonly DPoint location;

    public DPoint Location {
      get { return this.location; }
    }
    #endregion


    #region [Method: Constructor]
    public TileLocationEventArgs(TSPlayer player, DPoint location): base(player) {
      this.location = location;
    }
    #endregion
  }
}
