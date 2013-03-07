// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
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