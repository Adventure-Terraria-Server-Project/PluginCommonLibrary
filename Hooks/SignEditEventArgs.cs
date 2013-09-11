using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class SignEditEventArgs: TileLocationEventArgs {
    public short SignIndex { get; private set; }
    public string NewText { get; private set; }


    public SignEditEventArgs(TSPlayer player, short signIndex, DPoint location, string newText): base(player, location) {
      this.SignIndex = signIndex;
      this.NewText = newText;
    }
  }
}
