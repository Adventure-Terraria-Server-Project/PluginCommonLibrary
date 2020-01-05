using System;
using Microsoft.Xna.Framework;
using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChatTextEventArgs: NetHookEventArgs {
    public Color Color { get; private set; }
    public string Text { get; private set; }


    public ChatTextEventArgs(TSPlayer player, Color color, string text): base(player) {
      this.Color = color;
      this.Text = text;
    }
  }
}