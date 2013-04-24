using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TSPlayerEventArgs: EventArgs {
    #region [Property: TPlayer]
    private readonly TSPlayer tsPlayer;

    public TSPlayer TSPlayer {
      get { return this.tsPlayer; }
    }
    #endregion


    #region [Method: Constructor]
    public TSPlayerEventArgs(TSPlayer tsPlayer) {
      Contract.Requires<ArgumentNullException>(tsPlayer != null);

      this.tsPlayer = tsPlayer;
    }
    #endregion
  }
}
