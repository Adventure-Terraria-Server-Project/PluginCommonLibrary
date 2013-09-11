using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks{
  public class PlayerSpawnEventArgs: NetHookEventArgs {
    public DPoint SpawnTileLocation { get; private set; }

    
    public PlayerSpawnEventArgs(TSPlayer player, DPoint spawnTileLocation): base(player) {
      this.SpawnTileLocation = spawnTileLocation;
    }
  }
}