using System;
using System.ComponentModel;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class NetHookEventArgs: HandledEventArgs {
    #region [Property: Player]
    private readonly TSPlayer player;

    public TSPlayer Player {
      get { return this.player; }
    }
    #endregion


    #region [Method: Constructor]
    public NetHookEventArgs(TSPlayer player) {
      this.player = player;
    }
    #endregion
  }
}
