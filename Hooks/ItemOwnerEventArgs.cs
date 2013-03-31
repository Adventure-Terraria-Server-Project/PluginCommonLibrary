using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
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
