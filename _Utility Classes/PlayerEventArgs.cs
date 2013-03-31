using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow {
  public class PlayerEventArgs: EventArgs {
    #region [Property: TPlayer]
    private readonly Player tPlayer;

    public Player TPlayer {
      get { return this.tPlayer; }
    }
    #endregion


    #region [Method: Constructor]
    public PlayerEventArgs(Player tPlayer) {
      Contract.Requires<ArgumentNullException>(tPlayer != null);

      this.tPlayer = tPlayer;
    }
    #endregion
  }
}
