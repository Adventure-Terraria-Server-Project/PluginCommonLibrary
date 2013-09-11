using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class TileEditEventArgs: TileLocationEventArgs {
    public TileEditType EditType { get; private set; }
    public BlockType BlockType { get; private set; }
    public byte ObjectStyle { get; private set; }


    public TileEditEventArgs(
      TSPlayer player, TileEditType editType, DPoint location, BlockType blockType, byte objectStyle
    ): base(player, location) {
      this.EditType = editType;
      this.BlockType = blockType;
      this.ObjectStyle = objectStyle;
    }
  }
}
