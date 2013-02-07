// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

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
