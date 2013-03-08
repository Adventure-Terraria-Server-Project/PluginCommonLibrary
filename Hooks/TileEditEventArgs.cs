// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

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

    #region [Property: BlockType]
    private readonly BlockType blockType;

    public BlockType BlockType {
      get { return this.blockType; }
    }
    #endregion

    #region [Property: ObjectStyle]
    private readonly byte objectStyle;

    public byte ObjectStyle {
      get { return this.objectStyle; }
    }
    #endregion


    #region [Method: Constructor]
    public TileEditEventArgs(
      TSPlayer player, TileEditType editType, DPoint location, BlockType blockType, byte objectStyle
    ): base(player, location) {
      this.editType = editType;
      this.blockType = blockType;
      this.objectStyle = objectStyle;
    }
    #endregion
  }
}
