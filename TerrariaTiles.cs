using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
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


    #region [Methods: IsValidCoord, IsValidBlockType, IsSwitchableBlockType]
    public bool IsValidCoord(int x, int y) {
      return (
        x >= 0 && x < Main.maxTilesX - 1 &&
        y >= 0 && y < Main.maxTilesY - 1
      );
    }

    public bool IsValidCoord(DPoint point) {
      return this.IsValidCoord(point.X, point.Y);
    }

    public bool IsValidBlockType(int blockType) {
      return (blockType >= Terraria.BlockType_Min && blockType <= Terraria.BlockType_Max);
    }

    public bool IsSwitchableBlockType(BlockType blockType) {
      return (
        blockType == BlockType.Switch ||
        blockType == BlockType.Lever ||
        blockType == BlockType.PressurePlate ||
        blockType == BlockType.XSecondTimer ||
        blockType == BlockType.MusicBox
      );
    }
    #endregion

    #region [Methods: GetBlockTypeName, IsSolidBlockType]
    private static string[] tileNames;

    public string GetBlockTypeName(BlockType blockType) {
      if (tileNames == null) {
        tileNames = new[] {
          "Dirt Block",
          "Stone Block",
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
          "Clay Block",
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
          "Sand Block",
          "Glass",
          "Sign",
          "Obsidian",
          "Ash Block",
          "Hellstone",
          "Mud Block",
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
          "Clay Pot",
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
          "Ebonsand Block",
          "Tall Hallowed Plant",
          "Tinkerer's Workshop",
          "Hallowed Vine",
          "Pearlsand Block",
          "Pearlstone Block",
          "Pearlstone Brick",
          "Iridescent Brick",
          "Mudstone Block",
          "Cobalt Brick",
          "Mythril Brick",
          "Silt Block",
          "Wooden Beam",
          "Crystal Ball",
          "Disco Ball",
          "Ice",
          "Mannequin",
          "Crystal Shard",
          "Active Stone Block",
          "Inactive Stone Block",
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
          "Candy Cane Block",
          "Green Candy Cane Block",
          "Snow Block",
          "Snow Brick",
          "X-Mas Light",
        };
      }

      if (blockType < 0 || (int)blockType >= tileNames.Length)
        throw new ArgumentException(string.Format("The block type \"{0}\" is invalid.", blockType), "blockType");

      return tileNames[(int)blockType];
    }

    // Note: A block is considered any non-object, so any block type which blocks the player from passing through 
    // (cobwebs inclusive).
    public bool IsSolidBlockType(
      BlockType blockType, bool takeWireableStoneAsSolid = false, bool takeWoodPlatformAsSolid = false, 
      bool takeBouldersAsSolid = false, bool takeDartTrapsAsSolid = false
    ) {
      return (
        (blockType >= BlockType.DirtBlock && blockType <= BlockType.Grass) ||
        (blockType >= BlockType.SandBlock && blockType <= BlockType.JungleGrass && blockType != BlockType.Sign) ||
        (blockType >= BlockType.IronOre && blockType <= BlockType.SilverOre) ||
        (blockType >= BlockType.PearlsandBlock && blockType <= BlockType.SiltBlock) ||
        (takeWoodPlatformAsSolid && blockType == BlockType.WoodPlatform) ||
        (blockType >= BlockType.Meteorite && blockType <= BlockType.Spike && blockType != BlockType.ChainLantern) ||
        (blockType >= BlockType.ObsidianBrick && blockType <= BlockType.HellstoneBrick) ||
        (blockType >= BlockType.RedCandyCaneBlock && blockType <= BlockType.SnowBrick) ||
        (blockType >= BlockType.DemoniteOre && blockType <= BlockType.CorruptGrass) ||
        (blockType == BlockType.Wood) ||
        (blockType >= BlockType.SapphireBlock && blockType <= BlockType.DiamondBlock) ||
        (blockType >= BlockType.CobaltOre && blockType <= BlockType.EbonsandBlock && blockType != BlockType.HallowedPlants) ||
        (takeWireableStoneAsSolid && blockType >= BlockType.ActiveStone && blockType <= BlockType.InactiveStone) ||
        (blockType == BlockType.EbonstoneBlock) ||
        (takeBouldersAsSolid && blockType == BlockType.Boulder) ||
        (blockType == BlockType.MushroomGrass) ||
        (blockType == BlockType.IceBlock) ||
        (blockType == BlockType.Cobweb)
      );
    }
    #endregion

    #region [Methods: SetBlock, RemoveBlock]
    public void SetBlock(DPoint tileLocation, BlockType blockType, bool localOnly = false) {
      Tile tile = Terraria.Tiles[tileLocation];

      tile.type = (byte)blockType;
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
