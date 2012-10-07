using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  public class Tiles {
    #region [Indexers]
    public Tile this[int x, int y] {
      get {
        #if DEBUG
        Debug.Assert(Tiles.IsValidCoord(x, y));
        #endif
        return Main.tile[x, y];
      }
    }

    public Tile this[DPoint point] {
      get {
        #if DEBUG
        Debug.Assert(Tiles.IsValidCoord(point.X, point.Y));
        #endif
        return Main.tile[point.X, point.Y];
      }
    }
    #endregion


    #region [Method: Static IsValidCoord]
    public static bool IsValidCoord(int x, int y) {
      return (
        x >= 0 && x < Main.maxTilesX - 1 &&
        y >= 0 && y < Main.maxTilesY - 1
      );
    }

    public static bool IsValidCoord(DPoint point) {
      return Tiles.IsValidCoord(point.X, point.Y);
    }
    #endregion
  }
}
