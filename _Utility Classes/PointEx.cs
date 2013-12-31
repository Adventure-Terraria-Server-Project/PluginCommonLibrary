using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public static class PointEx {
    public static string ToSimpleString(this DPoint point) {
      return string.Concat(point.X, ',', point.Y);
    }

    public static DPoint Parse(string pointData) {
      string[] locationCoords = pointData.Split(',');
      if (locationCoords.Length != 2)
        throw new ArgumentException();

      return new DPoint(int.Parse(locationCoords[0]), int.Parse(locationCoords[1]));
    }

    public static DPoint OffsetTowards(this DPoint point, Direction direction, int offset = 1) {
      switch (direction) {
        case Direction.Left:
          return new DPoint(point.X - offset, point.Y);
        case Direction.Up:
          return new DPoint(point.X, point.Y - offset);
        case Direction.Right:
          return new DPoint(point.X + offset, point.Y);
        case Direction.Down:
          return new DPoint(point.X, point.Y + offset);
        default:
          return point;
      }
    }

    public static DPoint OffsetEx(this DPoint point, int xOffset, int yOffset) {
      point.Offset(xOffset, yOffset);
      return point;
    }

    public static DPoint ToTileLocation(this DPoint point) {
      return new DPoint(point.X / TerrariaUtils.TileSize, point.Y / TerrariaUtils.TileSize);
    }

    public static DPoint ToFlatLocation(this DPoint point) {
      return new DPoint(point.X * TerrariaUtils.TileSize, point.Y * TerrariaUtils.TileSize);
    }

    public static Point ToXnaPoint(this DPoint point) {
      return new Point(point.X, point.Y);
    }

    public static Vector2 ToXnaVector2(this DPoint point) {
      return new Vector2(point.X, point.Y);
    }
  }
}
