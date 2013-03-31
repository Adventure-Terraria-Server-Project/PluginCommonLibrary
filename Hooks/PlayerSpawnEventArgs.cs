using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks{
  public class PlayerSpawnEventArgs: NetHookEventArgs {
    #region [Property: SpawnTileLocation]
    private readonly DPoint spawnTileLocation;

    public DPoint SpawnTileLocation {
      get { return this.spawnTileLocation; }
    }
    #endregion

    
    #region [Method: Constructor]
    public PlayerSpawnEventArgs(TSPlayer player, DPoint spawnTileLocation): base(player) {
      this.spawnTileLocation = spawnTileLocation;
    }
    #endregion
  }
}