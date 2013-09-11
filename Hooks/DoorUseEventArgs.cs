using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class DoorUseEventArgs: TileLocationEventArgs {
    public bool IsOpening { get; private set; }
    public Direction Direction { get; private set; }


    public DoorUseEventArgs(TSPlayer player, DPoint location, bool isOpening, Direction direction): base(player, location) {
      this.IsOpening = isOpening;
      this.Direction = direction;
    }
  }
}