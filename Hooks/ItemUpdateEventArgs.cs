using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class ItemUpdateEventArgs: NetHookEventArgs {
    #region [Property: ItemIndex]
    private readonly short itemIndex;

    public short ItemIndex {
      get { return this.itemIndex; }
    }
    #endregion

    #region [Property: Location]
    private readonly Vector2 location;

    public Vector2 Location {
      get { return this.location; }
    }
    #endregion

    #region [Property: Velocity]
    private readonly Vector2 velocity;

    public Vector2 Velocity {
      get { return this.velocity; }
    }
    #endregion

    #region [Property: Item]
    private readonly ItemMetadata item;

    public ItemMetadata Item {
      get { return this.item; }
    }
    #endregion


    #region [Method: Constructor]
    public ItemUpdateEventArgs(
      TSPlayer player, short itemIndex, Vector2 location, Vector2 velocity, ItemMetadata item
    ): base(player) {
      this.itemIndex = itemIndex;
      this.location = location;
      this.velocity = velocity;
      this.item = item;
    }
    #endregion
  }
}
