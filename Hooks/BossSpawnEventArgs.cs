using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class BossSpawnEventArgs: NetHookEventArgs {
    public BossType BossType { get; private set; }


    public BossSpawnEventArgs(TSPlayer player, BossType bossType): base(player) {
      this.BossType = bossType;
    }
  }
}
