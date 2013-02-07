// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class LiquidSetEventArgs: TileLocationEventArgs {
    #region [Property: LiquidAmount]
    private readonly byte liquidAmount;

    public byte LiquidAmount {
      get { return this.liquidAmount; }
    }
    #endregion

    #region [Property: IsLava]
    private readonly bool isLava;

    public bool IsLava {
      get { return this.isLava; }
    }
    #endregion


    #region [Method: Constructor]
    public LiquidSetEventArgs(TSPlayer player, DPoint location, byte liquidAmount, bool isLava): base(player, location) {
      this.liquidAmount = liquidAmount;
      this.isLava = isLava;
    }
    #endregion
  }
}

