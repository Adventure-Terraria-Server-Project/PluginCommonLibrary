using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class SignEditEventArgs: TileLocationEventArgs {
    public int SignIndex { get; private set; }
    public string NewText { get; private set; }


    public SignEditEventArgs(TSPlayer player, int signIndex, DPoint location, string newText): base(player, location) {
      this.SignIndex = signIndex;
      this.NewText = newText;
    }
  }
}
