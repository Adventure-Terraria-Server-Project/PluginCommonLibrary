using System;
using System.Diagnostics.Contracts;
using Terraria.DataStructures;
using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerDeathEventArgs: NetHookEventArgs {
    public PlayerDeathReason DeathReason { get; private set; }
    public int Direction { get; private set; }
    public int Damage { get; private set; }
    public bool Pvp { get; private set; }
    public string DeathText { get; private set; }

    public PlayerDeathEventArgs(TSPlayer player, PlayerDeathReason deathReason, int direction, int damage, bool pvp): base(player) {
      this.DeathReason = deathReason;
      this.Direction = direction;
      this.Damage = damage;
      this.Pvp = pvp;
    }
  }
}