using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public static class Vector2Ex {
    public const double Rad2Deg = 180.0 / Math.PI;
    public const double Deg2Rad = Math.PI / 180.0;

    public static Vector2 FromDPoint(DPoint point) {
      return new Vector2(point.X, point.Y);
    }

    public static Vector2 OffsetPolar(this Vector2 vector, float angle, float steps) {
      return Vector2.Add(vector, new Vector2((float)Math.Cos(angle) * steps, (float)(Math.Sin(angle) * steps)));
    }

    public static float DistanceBetween(this Vector2 vector1, Vector2 vector2) {
      return (float)Math.Sqrt(Math.Pow(vector1.X - vector2.X, 2) + Math.Pow(vector1.Y - vector2.Y, 2));
    }

    public static float AngleBetween(this Vector2 fromVector, Vector2 toVector) {
      return (float)(Math.Atan2(toVector.Y - fromVector.Y, toVector.X - fromVector.X));
    }
  }
}
