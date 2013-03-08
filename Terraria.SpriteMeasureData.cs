// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  public static partial class Terraria {
    public struct ObjectMeasureData {
      #region [Property: Static Invalid]
      public static ObjectMeasureData Invalid {
        get { return default(ObjectMeasureData); }
      }
      #endregion

      #region [Property: BlockType]
      private readonly BlockType blockType;

      public BlockType BlockType {
        get { return this.blockType; }
      }
      #endregion

      #region [Property: OriginTileLocation]
      private readonly DPoint originTileLocation;

      public DPoint OriginTileLocation {
        get { return this.originTileLocation; }
      }
      #endregion

      #region [Property: Size]
      private readonly DPoint size;

      public DPoint Size {
        get { return this.size; }
      }
      #endregion

      #region [Property: TextureTileSize]
      private readonly DPoint textureTileSize;

      public DPoint TextureTileSize {
        get { return this.textureTileSize; }
      }
      #endregion

      #region [Property: FrameXOffsetAdd]
      private readonly int frameXOffsetAdd;

      public int FrameXOffsetAdd {
        get { return this.frameXOffsetAdd; }
      }
      #endregion


      #region [Method: Constructor]
      public ObjectMeasureData(
        BlockType blockType, DPoint originTileLocation, DPoint size, DPoint textureTileSize, int frameXOffsetAdd
      ) {
        this.blockType = blockType;
        this.frameXOffsetAdd = frameXOffsetAdd;
        this.originTileLocation = originTileLocation;
        this.size = size;
        this.textureTileSize = textureTileSize;
        this.frameXOffsetAdd = frameXOffsetAdd;
      }
      #endregion

      #region [Method: ToString]
      public override string ToString() {
        return this.BlockType.ToString();
      }
      #endregion
    }
  }
}
