using System;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChatTextEventArgs: NetHookEventArgs {
    #region [Property: Color]
    private readonly Color color;

    public Color Color {
      get { return this.color; }
    }
    #endregion

    #region [Property: Text]
    private readonly string text;

    public string Text {
      get { return this.text; }
    }
    #endregion


    #region [Method: Constructor]
    public ChatTextEventArgs(TSPlayer player, Color color, string text): base(player) {
      this.color = color;
      this.text = text;
    }
    #endregion
  }
}