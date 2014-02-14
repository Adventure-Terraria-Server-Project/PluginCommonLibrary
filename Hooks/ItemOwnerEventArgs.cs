using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ItemOwnerEventArgs: NetHookEventArgs {
    public int ItemIndex { get; private set; }
    public TSPlayer NewOwner { get; private set; }


    public ItemOwnerEventArgs(TSPlayer player, int itemIndex, TSPlayer newOwner): base(player) {
      this.ItemIndex = itemIndex;
      this.NewOwner = newOwner;
    }
  }
}
