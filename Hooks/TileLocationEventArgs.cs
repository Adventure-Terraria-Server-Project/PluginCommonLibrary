// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

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
