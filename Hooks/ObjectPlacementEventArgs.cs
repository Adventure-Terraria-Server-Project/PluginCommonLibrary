using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ObjectPlacementEventArgs: TileLocationEventArgs {
    public BlockType BlockType { get; private set; }
    public int ObjectStyle { get; private set; }
    public int Alternative { get; private set; }
    public int Random { get; private set; }
    public bool Direction { get; private set; }


    public ObjectPlacementEventArgs(
      TSPlayer player, DPoint location, BlockType blockType, int objectStyle, int alternative, int random, bool direction
    ): base(player, location) {
      this.BlockType = blockType;
      this.ObjectStyle = objectStyle;
      this.Alternative = alternative;
      this.Random = random;
      this.Direction = direction;
    }
  }
}
