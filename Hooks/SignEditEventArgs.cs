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
