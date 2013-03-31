using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public static partial class Terraria {
    #region [Constants]
    public const int BlockType_Min = 0;
    public const int BlockType_Max = 149;

    public const int WallType_Min = 0;
    public const int WallType_Max = 31;

    public const int ItemType_Min = 0;
    public const int ItemType_Max = 603;

    public const int DefaultTextureTileSize = 18;
    public const int TileSize = 16;
    #endregion

    #region [Property: Tiles]
    private static readonly TerrariaTiles tiles;

    public static TerrariaTiles Tiles {
      get { return Terraria.tiles; }
    }
    #endregion

    #region [Property: Items]
    private static readonly TerrariaItems items;

    public static TerrariaItems Items {
      get { return Terraria.items; }
    }
    #endregion

    #region [Property: Static Npcs]
    private static readonly TerrariaNpcs npcs;

    public static TerrariaNpcs Npcs {
      get { return Terraria.npcs; }
    }
    #endregion


    #region [Method: Constructor]
    static Terraria() {
      Terraria.tiles = new TerrariaTiles();
      Terraria.items = new TerrariaItems();
      Terraria.npcs = new TerrariaNpcs();
    }
    #endregion
  }
}
