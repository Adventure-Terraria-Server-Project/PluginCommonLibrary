using System;

namespace Terraria.Plugins.Common {
  public class PlayerEventArgs: EventArgs {
    public Player TPlayer { get; private set; }


    public PlayerEventArgs(Player tPlayer) {
      if (tPlayer == null) throw new ArgumentNullException();

      this.TPlayer = tPlayer;
    }
  }
}
