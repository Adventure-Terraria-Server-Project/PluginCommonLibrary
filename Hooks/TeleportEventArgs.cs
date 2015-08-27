using System;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class TeleportEventArgs : NetHookEventArgs {
    public Vector2 ToLocation { get; private set; }
    public TeleportType TeleportType { get; private set; }

    public TeleportEventArgs(TSPlayer player, Vector2 toLocation, TeleportType teleportType): base(player) {
      this.ToLocation = toLocation;
      this.TeleportType = teleportType;
    }
  }
}