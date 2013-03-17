using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class BossSpawnEventArgs: NetHookEventArgs {
    #region [Property: BossType]
    private readonly BossType bossType;

    public BossType BossType {
      get { return this.bossType; }
    }
    #endregion


    #region [Method: Constructor]
    public BossSpawnEventArgs(TSPlayer player, BossType bossType): base(player) {
      this.bossType = bossType;
    }
    #endregion
  }
}
