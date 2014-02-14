using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  public static partial class TerrariaUtils {
    public static int BlockType_Min = 0;
    public static int BlockType_Max = Main.maxTileSets - 1;
    public static int WallType_Min = 0;
    public static int WallType_Max = Main.maxWallTypes - 1;
    public static int ItemType_Min = -48;
    public static int ItemType_Max = Main.maxItemTypes - 1;
    public static int NpcType_Min = -65;
    public static int NpcType_Max = Main.maxNPCTypes - 1;
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
