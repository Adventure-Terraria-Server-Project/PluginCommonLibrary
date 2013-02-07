// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class ItemOwnerEventArgs: NetHookEventArgs {
    #region [Property: ItemIndex]
    private readonly short itemIndex;

    public short ItemIndex {
      get { return this.itemIndex; }
    }
    #endregion

    #region [Property: NewOwner]
    private readonly TSPlayer newOwner;

    public TSPlayer NewOwner {
      get { return this.newOwner; }
    }
    #endregion


    #region [Method: Constructor]
    public ItemOwnerEventArgs(TSPlayer player, short itemIndex, TSPlayer newOwner): base(player) {
      this.itemIndex = itemIndex;
      this.newOwner = newOwner;
    }
    #endregion
  }
}
