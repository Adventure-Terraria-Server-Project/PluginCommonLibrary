// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class SignEditEventArgs: TileLocationEventArgs {
    #region [Property: SignIndex]
    private readonly short signIndex;

    public short SignIndex {
      get { return this.signIndex; }
    }
    #endregion

    #region [Property: NewText]
    private readonly string newText;

    public string NewText {
      get { return this.newText; }
    }
    #endregion


    #region [Method: Constructor]
    public SignEditEventArgs(TSPlayer player, short signIndex, DPoint location, string newText): base(player, location) {
      this.signIndex = signIndex;
      this.newText = newText;
    }
    #endregion
  }
}
