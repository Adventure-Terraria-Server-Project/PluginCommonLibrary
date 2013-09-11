using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public static partial class TerrariaUtils {
    public const int BlockType_Min = 0;
    public const int BlockType_Max = 149;
    public const int WallType_Min = 0;
    public const int WallType_Max = 31;
    public const int ItemType_Min = -24;
    public const int ItemType_Max = 603;
    public const int NpcType_Min = -17;
    public const int NpcType_Max = 145;
    public const int DefaultTextureTileSize = 18;
    public const int TileSize = 16;

    public static TerrariaTiles Tiles { get; private set; }
    public static TerrariaItems Items { get; private set; }
    public static TerrariaNpcs Npcs { get; private set; }

    public static InvasionType InvasionType {
      get { return (Common.InvasionType)Main.invasionType; }
    }


    static TerrariaUtils() {
      TerrariaUtils.Tiles = new TerrariaTiles();
      TerrariaUtils.Items = new TerrariaItems();
      TerrariaUtils.Npcs = new TerrariaNpcs();
    }
  }
}
