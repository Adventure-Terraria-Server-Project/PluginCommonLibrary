using System;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public struct ObjectMeasureData {
    public static readonly ObjectMeasureData Invalid = default(ObjectMeasureData);

    public BlockType BlockType { get; private set; }
    public DPoint OriginTileLocation { get; private set; }
    public DPoint Size { get; private set; }
    public DPoint TextureTileSize { get; private set; }
    public DPoint TextureFrameLocation { get; private set; }


    public ObjectMeasureData(
      BlockType blockType, DPoint originTileLocation, DPoint size, DPoint textureTileSize, DPoint textureFrameLocation
    ): this() {
      this.BlockType = blockType;
      this.OriginTileLocation = originTileLocation;
      this.Size = size;
      this.TextureTileSize = textureTileSize;
      this.TextureFrameLocation = textureFrameLocation;
    }

    public override string ToString() {
      return this.BlockType.ToString();
    }
  }
}