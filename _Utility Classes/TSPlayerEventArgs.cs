using System;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TSPlayerEventArgs: EventArgs {
    public TSPlayer TSPlayer { get; private set; }


    public TSPlayerEventArgs(TSPlayer tsPlayer) {
      if (tsPlayer == null) throw new ArgumentNullException();

      this.TSPlayer = tsPlayer;
    }
  }
}
