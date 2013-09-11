using System;
using System.ComponentModel;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class NetHookEventArgs: HandledEventArgs {
    public TSPlayer Player { get; private set; }


    public NetHookEventArgs(TSPlayer player) {
      this.Player = player;
    }
  }
}
