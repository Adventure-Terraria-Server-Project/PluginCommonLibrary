using System;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public static class Check {
    public static void ValidItemType(int itemType) {
      if (itemType < TerrariaUtils.ItemType_Min || itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException($"The given item type {itemType} is invalid.", nameof(itemType));
    }

    public static void ValidBlockType(int blockType) {
      if (blockType < TerrariaUtils.BlockType_Min || blockType > TerrariaUtils.BlockType_Max)
        throw new ArgumentException($"The given block type {blockType} is invalid.", nameof(blockType));
    }

    public static void ValidWallType(int wallType) {
      if (wallType < TerrariaUtils.WallType_Min || wallType > TerrariaUtils.WallType_Max)
        throw new ArgumentException($"The given item type {wallType} is invalid.", nameof(wallType));
    }

    public static void ValidTileLocation(DPoint tileLocation) {
      Check.ValidTileLocation(tileLocation.X, tileLocation.Y);
    }

    public static void ValidTileLocation(int x, int y) {
      if (
        x < 0 || x >= Main.maxTilesX - 1 &&
        y < 0 || y >= Main.maxTilesY - 1
      )
        throw new ArgumentException("The given tile location is invalid (exceeds world boundaries).");
    }
  }
}
