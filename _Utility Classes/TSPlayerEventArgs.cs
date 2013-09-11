using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TSPlayerEventArgs: EventArgs {
    public TSPlayer TSPlayer { get; private set; }


    public TSPlayerEventArgs(TSPlayer tsPlayer) {
      Contract.Requires<ArgumentNullException>(tsPlayer != null);

      this.TSPlayer = tsPlayer;
    }
  }
}
