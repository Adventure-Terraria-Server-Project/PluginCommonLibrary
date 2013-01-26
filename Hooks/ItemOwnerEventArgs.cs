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


    #region [Method: Constructor]
    public ItemOwnerEventArgs(TSPlayer player, short itemIndex): base(player) {
      this.itemIndex = itemIndex;
    }
    #endregion
  }
}
