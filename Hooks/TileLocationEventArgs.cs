using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class TileLocationEventArgs: NetHookEventArgs {
    public DPoint Location { get; private set; }


    public TileLocationEventArgs(TSPlayer player, DPoint location): base(player) {
      this.Location = location;
    }
  }
}
