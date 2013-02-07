// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  public static partial class Terraria {
    public struct SpriteMeasureData {
      #region [Property: Static Invalid]
      public static SpriteMeasureData Invalid {
        get { return default(SpriteMeasureData); }
      }
      #endregion

      #region [Property: SpriteType]
      private readonly BlockType spriteType;

      public BlockType SpriteType {
        get { return this.spriteType; }
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
      public SpriteMeasureData(
        BlockType spriteType, DPoint originTileLocation, DPoint size, DPoint textureTileSize, int frameXOffsetAdd
      ) {
        this.spriteType = spriteType;
        this.frameXOffsetAdd = frameXOffsetAdd;
        this.originTileLocation = originTileLocation;
        this.size = size;
        this.textureTileSize = textureTileSize;
        this.frameXOffsetAdd = frameXOffsetAdd;
      }
      #endregion

      #region [Method: ToString]
      public override string ToString() {
        return this.SpriteType.ToString();
      }
      #endregion
    }
  }
}
