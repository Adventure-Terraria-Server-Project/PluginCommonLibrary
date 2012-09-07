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
    public struct SpriteMeasureData {
      #region [Property: SpriteType]
      private readonly int spriteType;

      public int SpriteType {
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

      #region [Property: HasActiveFrame]
      private readonly bool hasActiveFrame;

      /// <summary>
      ///   Gets a bool indicating whether the sprite currently has its "active" frame. A torch is active when it's emitting light.
      /// </summary>
      public bool HasActiveFrame {
        get { return this.hasActiveFrame; }
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
        int spriteType, DPoint originTileLocation, DPoint size, DPoint textureTileSize, bool hasActiveFrame, 
        int frameXOffsetAdd
      ) {
        this.spriteType = spriteType;
        this.frameXOffsetAdd = frameXOffsetAdd;
        this.originTileLocation = originTileLocation;
        this.size = size;
        this.textureTileSize = textureTileSize;
        this.hasActiveFrame = hasActiveFrame;
        this.frameXOffsetAdd = frameXOffsetAdd;
      }
      #endregion
    }
  }
}
