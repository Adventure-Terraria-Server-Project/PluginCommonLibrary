using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public static partial class TerrariaUtils {
    #region [Constants]
    public const int BlockType_Min = 0;
    public const int BlockType_Max = 149;

    public const int WallType_Min = 0;
    public const int WallType_Max = 31;

    public const int ItemType_Min = 0;
    public const int ItemType_Max = 603;

    public const int NpcType_Min = -17;
    public const int NpcType_Max = 145;

    public const int DefaultTextureTileSize = 18;
    public const int TileSize = 16;
    #endregion

    #region [Property: Tiles]
    private static readonly TerrariaTiles tiles;

    public static TerrariaTiles Tiles {
      get { return TerrariaUtils.tiles; }
    }
    #endregion

    #region [Property: Items]
    private static readonly TerrariaItems items;

    public static TerrariaItems Items {
      get { return TerrariaUtils.items; }
    }
    #endregion

    #region [Property: Npcs]
    private static readonly TerrariaNpcs npcs;

    public static TerrariaNpcs Npcs {
      get { return TerrariaUtils.npcs; }
    }
    #endregion

    #region [Property: InvasionType]
    public static InvasionType InvasionType {
      get { return (Common.InvasionType)Main.invasionType; }
    }
    #endregion


    #region [Method: Constructor]
    static TerrariaUtils() {
      TerrariaUtils.tiles = new TerrariaTiles();
      TerrariaUtils.items = new TerrariaItems();
      TerrariaUtils.npcs = new TerrariaNpcs();
    }
    #endregion
  }
}
