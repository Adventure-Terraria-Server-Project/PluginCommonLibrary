using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using TShockAPI;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  public class TerrariaTiles {
    #region [Indexers]
    public Tile this[int x, int y] {
      get {
        #if DEBUG
        Debug.Assert(Terraria.Tiles.IsValidCoord(x, y));
        #endif
        return Main.tile[x, y];
      }
    }

    public Tile this[DPoint point] {
      get {
        #if DEBUG
        Debug.Assert(Terraria.Tiles.IsValidCoord(point.X, point.Y));
        #endif
        return Main.tile[point.X, point.Y];
      }
    }
    #endregion


    #region [Method: IsValidCoord, IsValidTileId]
    public bool IsValidCoord(int x, int y) {
      return (
        x >= 0 && x < Main.maxTilesX - 1 &&
        y >= 0 && y < Main.maxTilesY - 1
      );
    }

    public bool IsValidCoord(DPoint point) {
      return this.IsValidCoord(point.X, point.Y);
    }

    public bool IsValidTileId(int blockId) {
      return (blockId >= Terraria.TileId_Min && blockId <= Terraria.TileId_Max);
    }
    #endregion

    #region [Methods: GetBlockName, IsSolidBlock]
    private static string[] tileNames;

    public string GetBlockName(int blockId) {
      if (TerrariaTiles.tileNames == null) {
        TerrariaTiles.tileNames = new[] {
          "Dirt",
          "Stone",
          "Grass",
          "Grass Plant",
          "Torch",
          "Tree",
          "Iron Ore",
          "Copper Ore",
          "Gold Ore",
          "Silver Ore",
          "Door (Closed)",
          "Door (Opened)",
          "Crystal Heart",
          "Bottle",
          "Wooden Table",
          "Wooden Chair",
          "Iron Anvil",
          "Furnace",
          "Work Bench",
          "Wood Platform",
          "Sapling",
          "Chest",
          "Demonite Ore",
          "Corrupt Grass",
          "Corruption Plant",
          "Ebonstone Block",
          "Demon Altar",
          "Sunflower",
          "Pot",
          "Piggy Bank",
          "Wood",
          "Shadow Orb",
          "Corruption Thorny Bush",
          "Candle",
          "Copper Chandelier",
          "Silver Chandelier",
          "Gold Chandelier",
          "Meteorite",
          "Gray Brick",
          "Red Brick",
          "Clay",
          "Blue Brick",
          "Chain Lantern",
          "Green Brick",
          "Pink Brick",
          "Gold Brick",
          "Silver Brick",
          "Copper Brick",
          "Spike",
          "Water Candle",
          "Book",
          "Cobweb",
          "Vines",
          "Sand",
          "Glass",
          "Sign",
          "Obsidian",
          "Ash",
          "Hellstone",
          "Mud",
          "Jungle Grass",
          "Jungle Plant",
          "Jungle Vine",
          "Sapphire",
          "Ruby",
          "Emerald",
          "Topaz",
          "Amethyst",
          "Diamond",
          "Jungle Thorny Bush",
          "Mushroom Grass",
          "Glowing Mushroom",
          "Giant Glowing Mushroom",
          "Tall Grass Plant",
          "Tall Jungle Plant",
          "Obsidian Brick",
          "Hellstone Brick",
          "Hellforge",
          "ClayPot",
          "Bed",
          "Cactus",
          "Coral",
          "Plantable Plant (Growing)",
          "Plantable Plant (Mature)",
          "Plantable Plant (Blooming)",
          "Tombstone",
          "Loom",
          "Piano",
          "Dresser",
          "Bench",
          "Bathtub",
          "Banner",
          "LampPost",
          "Tiki Torch",
          "Keg",
          "Chinese Lantern",
          "Cooking Pot",
          "Safe",
          "Skull Lantern",
          "TashCan",
          "Candelabra",
          "Bookcase",
          "Throne",
          "Bowl",
          "Grandfather Clock",
          "Statue",
          "Sawmill",
          "Cobalt Ore",
          "Mythril Ore",
          "Hallowed Grass",
          "Hallowed Plant",
          "Adamantite Ore",
          "Ebonsand",
          "Tall Hallowed Plant",
          "Tinkerer's Workshop",
          "Hallowed Vine",
          "Pearlsand",
          "Pearlstone",
          "Pearlstone Brick",
          "Iridescent Brick",
          "Mudstone",
          "Cobalt Brick",
          "Mythril Brick",
          "Silt",
          "Wooden Beam",
          "Crystal Ball",
          "Disco Ball",
          "Ice",
          "Mannequin",
          "Crystal Shard",
          "Active Stone",
          "Inactive Stone",
          "Lever",
          "Adamantite Forge",
          "Mythril Anvil",
          "Pressure Plate",
          "Switch",
          "Dart Trap",
          "Boulder",
          "Music Box",
          "Demonite Brick",
          "Explosives",
          "Inlet Pump",
          "Outlet Pump",
          "XSecond Timer",
          "Red Candy Cane",
          "Green Candy Cane",
          "Snow",
          "Snow Brick",
          "X-Mas Light",
        };
      }

      if (blockId < 0 || blockId >= TerrariaTiles.tileNames.Length)
        throw new ArgumentException(string.Format("The tild id \"{0}\" is invalid.", blockId), "blockId");

      return TerrariaTiles.tileNames[blockId];
    }

    // Note: A block is considered any non-sprite, so any tile type which blocks the player from passing through 
    // (cobwebs inclusive).
    public bool IsSolidBlock(
      int blockId, bool takeWireableStoneAsBlock = false, bool takeWoodPlatformAsBlock = false, 
      bool takeBouldersAsBlocks = false, bool takeDartTrapsAsBlocks = false
    ) {
      return (
        (blockId >= Terraria.TileId_DirtBlock && blockId <= Terraria.TileId_Grass) ||
        (blockId >= Terraria.TileId_SandBlock && blockId <= Terraria.TileId_JungleGrass && blockId != Terraria.TileId_Sign) ||
        (blockId >= Terraria.TileId_IronOre && blockId <= Terraria.TileId_SilverOre) ||
        (blockId >= Terraria.TileId_PearlsandBlock && blockId <= Terraria.TileId_SiltBlock) ||
        (takeWoodPlatformAsBlock && blockId == Terraria.TileId_WoodPlatform) ||
        (blockId >= Terraria.TileId_Meteorite && blockId <= Terraria.TileId_Spike && blockId != Terraria.TileId_ChainLantern) ||
        (blockId >= Terraria.TileId_ObsidianBrick && blockId <= Terraria.TileId_HellstoneBrick) ||
        (blockId >= Terraria.TileId_RedCandyCaneBlock && blockId <= Terraria.TileId_SnowBrick) ||
        (blockId >= Terraria.TileId_DemoniteOre && blockId <= Terraria.TileId_CorruptGrass) ||
        (blockId == Terraria.TileId_Wood) ||
        (blockId >= Terraria.TileId_SapphireBlock && blockId <= Terraria.TileId_DiamondBlock) ||
        (blockId >= Terraria.TileId_CobaltOre && blockId <= Terraria.TileId_EbonsandBlock && blockId != Terraria.TileId_HallowedPlants) ||
        (takeWireableStoneAsBlock && blockId >= Terraria.TileId_ActiveStone && blockId <= Terraria.TileId_InactiveStone) ||
        (blockId == Terraria.TileId_EbonstoneBlock) ||
        (takeBouldersAsBlocks && blockId == Terraria.TileId_Boulder) ||
        (blockId == Terraria.TileId_MushroomGrass) ||
        (blockId == Terraria.TileId_IceBlock) ||
        (blockId == Terraria.TileId_Cobweb)
      );
    }
    #endregion

    #region [Methods: SetBlock, RemoveBlock]
    public void SetBlock(DPoint tileLocation, byte blockId, bool localOnly = false) {
      Tile tile = Terraria.Tiles[tileLocation];

      tile.type = blockId;
      tile.active = true;

      WorldGen.SquareTileFrame(tileLocation.X, tileLocation.Y, true);
      if (!localOnly)
        TSPlayer.All.SendTileSquare(tileLocation.X, tileLocation.Y, 1);
    }

    public void RemoveBlock(DPoint tileLocation, bool squareFrames = true, bool localOnly = false) {
      Tile tile = Terraria.Tiles[tileLocation];

      tile.type = 0;
      tile.active = false;
      tile.frameX = -1;
      tile.frameY = -1;
      tile.frameNumber = 0;

      if (squareFrames)
        WorldGen.SquareTileFrame(tileLocation.X, tileLocation.Y, true);
      if (!localOnly)
        TSPlayer.All.SendTileSquare(tileLocation.X, tileLocation.Y, 1);
    }
    #endregion
  }
}
