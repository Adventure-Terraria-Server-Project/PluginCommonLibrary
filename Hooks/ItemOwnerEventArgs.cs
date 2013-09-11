using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ItemOwnerEventArgs: NetHookEventArgs {
    public short ItemIndex { get; private set; }
    public TSPlayer NewOwner { get; private set; }


    public ItemOwnerEventArgs(TSPlayer player, short itemIndex, TSPlayer newOwner): base(player) {
      this.ItemIndex = itemIndex;
      this.NewOwner = newOwner;
    }
  }
}
