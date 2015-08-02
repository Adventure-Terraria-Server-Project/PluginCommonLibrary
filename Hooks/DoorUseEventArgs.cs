using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class DoorUseEventArgs: TileLocationEventArgs {
    public DoorAction action { get; private set; }
    public Direction Direction { get; private set; }


    public DoorUseEventArgs(TSPlayer player, DPoint location, DoorAction action, Direction direction)
      : base(player, location) {
      this.action = action;
      this.Direction = direction;
    }
  }
}