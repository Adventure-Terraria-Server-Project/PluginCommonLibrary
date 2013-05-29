using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestOpenEventArgs: TileLocationEventArgs {
    #region [Property: ChestIndex]
    private readonly int chestIndex;

    public int ChestIndex {
      get { return this.chestIndex; }
    }
    #endregion


    #region [Method: Constructor]
    public ChestOpenEventArgs(TSPlayer player, int chestIndex, DPoint chestLocation): base(player, chestLocation) {
      this.chestIndex = chestIndex;
    }
    #endregion
  }
}