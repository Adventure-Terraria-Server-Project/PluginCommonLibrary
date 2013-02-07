// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

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
