using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ItemUpdateEventArgs: NetHookEventArgs {
    public short ItemIndex { get; private set; }
    public Vector2 Location { get; private set; }
    public Vector2 Velocity { get; private set; }
    public bool NoDelay { get; private set; }
    public ItemData Item { get; private set; }


    public ItemUpdateEventArgs(
      TSPlayer player, short itemIndex, Vector2 location, Vector2 velocity, bool noDelay, ItemData item
    ): base(player) {
      this.ItemIndex = itemIndex;
      this.Location = location;
      this.Velocity = velocity;
      this.NoDelay = noDelay;
      this.Item = item;
    }
  }
}
