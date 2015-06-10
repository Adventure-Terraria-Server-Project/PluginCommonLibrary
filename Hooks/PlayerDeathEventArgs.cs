using System;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class PlayerDeathEventArgs : NetHookEventArgs {
    public int direction { get; private set; }
    public int dmg { get; private set; }
    public bool pvp { get; private set; }
    public string deathText { get; private set; }

    public PlayerDeathEventArgs(TSPlayer player, int direction, int damage, bool pvp, string deathText)
      : base(player) {
        this.direction = direction;
        this.dmg = dmg;
        this.pvp = pvp;
        this.deathText = deathText;
    }
  }
}