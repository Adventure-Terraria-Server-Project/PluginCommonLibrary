using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class TileEditEventArgs: TileLocationEventArgs {
    #region [Property: EditType]
    private readonly TileEditType editType;

    public TileEditType EditType {
      get { return this.editType; }
    }
    #endregion

    #region [Property: BlockId]
    private readonly byte blockId;

    public byte BlockId {
      get { return this.blockId; }
    }
    #endregion

    #region [Property: SpriteStyle]
    private readonly byte spriteStyle;

    public byte SpriteStyle {
      get { return this.spriteStyle; }
    }
    #endregion


    #region [Method: Constructor]
    public TileEditEventArgs(
      TSPlayer player, TileEditType editType, DPoint location, byte blockId, byte spriteStyle
    ): base(player, location) {
      this.editType = editType;
      this.blockId = blockId;
      this.spriteStyle = spriteStyle;
    }
    #endregion
  }
}
