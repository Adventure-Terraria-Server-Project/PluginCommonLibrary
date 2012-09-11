// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  public static partial class Terraria {
    #region [Constants]
    public const int TileId_DirtBlock = 0;
    public const int TileId_StoneBlock = 1;
    public const int TileId_Grass = 2;
    public const int TileId_GrassPlants = 3;
    public const int TileId_Torch = 4;
    public const int TileId_Tree = 5;
    public const int TileId_IronOre = 6;
    public const int TileId_CopperOre = 7;
    public const int TileId_GoldOre = 8;
    public const int TileId_SilverOre = 9;
    public const int TileId_DoorClosed = 10;
    public const int TileId_DoorOpened = 11;
    public const int TileId_CrystalHeart = 12;
    public const int TileId_Bottle = 13;
    public const int TileId_WoodenTable = 14;
    public const int TileId_WoodenChair = 15;
    public const int TileId_IronAnvil = 16;
    public const int TileId_Furnace = 17;
    public const int TileId_WorkBench = 18;
    public const int TileId_WoodPlatform = 19;
    public const int TileId_Sapling = 20;
    public const int TileId_Chest = 21;
    public const int TileId_DemoniteOre = 22;
    public const int TileId_CorruptGrass = 23;
    public const int TileId_CorruptionPlants = 24;
    public const int TileId_EbonstoneBlock = 25;
    public const int TileId_DemonAltar = 26;
    public const int TileId_Sunflower = 27;
    public const int TileId_Pot = 28;
    public const int TileId_PiggyBank = 29;
    public const int TileId_Wood = 30;
    public const int TileId_ShadowOrb = 31;
    public const int TileId_CorruptionThornyBush = 32;
    public const int TileId_Candle = 33;
    public const int TileId_CopperChandelier = 34;
    public const int TileId_SilverChandelier = 35;
    public const int TileId_GoldChandelier = 36;
    public const int TileId_Meteorite = 37;
    public const int TileId_GrayBrick = 38;
    public const int TileId_RedBrick = 39;
    public const int TileId_Clay = 40;
    public const int TileId_BlueBrick = 41;
    public const int TileId_ChainLantern = 42;
    public const int TileId_GreenBrick = 43;
    public const int TileId_PinkBrick = 44;
    public const int TileId_GoldBrick = 45;
    public const int TileId_SilverBrick = 46;
    public const int TileId_CopperBrick = 47;
    public const int TileId_Spike = 48;
    public const int TileId_WaterCandle = 49;
    public const int TileId_Book = 50;
    public const int TileId_Cobweb = 51;
    public const int TileId_Vine = 52;
    public const int TileId_SandBlock = 53;
    public const int TileId_Glass = 54;
    public const int TileId_Sign = 55;
    public const int TileId_Obsidian = 56;
    public const int TileId_AshBlock = 57;
    public const int TileId_Hellstone = 58;
    public const int TileId_MudBlock = 59;
    public const int TileId_JungleGrass = 60;
    public const int TileId_JunglePlants = 61;
    public const int TileId_JungleVine = 62;
    public const int TileId_SapphireBlock = 63;
    public const int TileId_RubyBlock = 64;
    public const int TileId_EmeraldBlock = 65;
    public const int TileId_TopazBlock = 66;
    public const int TileId_AmethystBlock = 67;
    public const int TileId_DiamondBlock = 68;
    public const int TileId_JungleThornyBush = 69;
    public const int TileId_MushroomGrass = 70;
    public const int TileId_GlowingMushroom = 71;
    public const int TileId_GiantGlowingMushroom = 72;
    public const int TileId_TallGrassPlants = 73;
    public const int TileId_TallJunglePlants = 74;
    public const int TileId_ObsidianBrick = 75;
    public const int TileId_HellstoneBrick = 76;
    public const int TileId_Hellforge = 77;
    public const int TileId_ClayPot = 78;
    public const int TileId_Bed = 79;
    public const int TileId_Cactus = 80;
    public const int TileId_Coral = 81;
    public const int TileId_PlantablePlantsGrowing = 82;
    public const int TileId_PlantablePlantsMature = 83;
    public const int TileId_PlantablePlantsBlooming = 84;
    public const int TileId_Tombstone = 85;
    public const int TileId_Loom = 86;
    public const int TileId_Piano = 87;
    public const int TileId_Dresser = 88;
    public const int TileId_Bench = 89;
    public const int TileId_Bathtub = 90;
    public const int TileId_Banner = 91;
    public const int TileId_LampPost = 92;
    public const int TileId_TikiTorch = 93;
    public const int TileId_Keg = 94;
    public const int TileId_ChineseLantern = 95;
    public const int TileId_CookingPot = 96;
    public const int TileId_Safe = 97;
    public const int TileId_SkullLantern = 98;
    public const int TileId_TashCan_UNUSED = 99;
    public const int TileId_Candelabra = 100;
    public const int TileId_Bookcase = 101;
    public const int TileId_Throne = 102;
    public const int TileId_Bowl = 103;
    public const int TileId_GrandfatherClock = 104;
    public const int TileId_Statue = 105;
    public const int TileId_Sawmill = 106;
    public const int TileId_CobaltOre = 107;
    public const int TileId_MythrilOre = 108;
    public const int TileId_HallowedGrass = 109;
    public const int TileId_HallowedPlants = 110;
    public const int TileId_AdamantiteOre = 111;
    public const int TileId_EbonsandBlock = 112;
    public const int TileId_TallHallowedPlants = 113;
    public const int TileId_TinkerersWorkshop = 114;
    public const int TileId_HallowedVine = 115;
    public const int TileId_PearlsandBlock = 116;
    public const int TileId_PearlstoneBlock = 117;
    public const int TileId_PearlstoneBrick = 118;
    public const int TileId_IridescentBrick = 119;
    public const int TileId_MudstoneBlock = 120;
    public const int TileId_CobaltBrick = 121;
    public const int TileId_MythrilBrick = 122;
    public const int TileId_SiltBlock = 123;
    public const int TileId_WoodenBeam = 124;
    public const int TileId_CrystalBall = 125;
    public const int TileId_DiscoBall = 126;
    public const int TileId_IceBlock = 127;
    public const int TileId_Mannequin = 128;
    public const int TileId_CrystalShard = 129;
    public const int TileId_ActiveStone = 130;
    public const int TileId_InactiveStone = 131;
    public const int TileId_Lever = 132;
    public const int TileId_AdamantiteForge = 133;
    public const int TileId_MythrilAnvil = 134;
    public const int TileId_PressurePlate = 135;
    public const int TileId_Switch = 136;
    public const int TileId_DartTrap = 137;
    public const int TileId_Boulder = 138;
    public const int TileId_MusicBox = 139;
    public const int TileId_DemoniteBrick = 140;
    public const int TileId_Explosives = 141;
    public const int TileId_InletPump = 142;
    public const int TileId_OutletPump = 143;
    public const int TileId_XSecondTimer = 144;
    public const int TileId_RedCandyCaneBlock = 145;
    public const int TileId_GreenCandyCaneBlock = 146;
    public const int TileId_SnowBlock = 147;
    public const int TileId_SnowBrick = 148;
    public const int TileId_XMasLight = 149;

    public const int DefaultTextureTileSize = 18;
    public const int TileSize = 16;
    #endregion

    #region [Method: IsValidCoord]
    public static bool IsValidCoord(int x, int y) {
      return (
        x >= 0 && x <= Main.maxTilesX &&
        y >= 0 && y <= Main.maxTilesY
      );
    }

    public static bool IsValidCoord(DPoint point) {
      return Terraria.IsValidCoord(point.X, point.Y);
    }
    #endregion

    #region [Methods: MeasureSprite, IsLeftTreeBranch, IsRightTreeBranch, IsLeftCactusBranch, IsRightCactusBranch]
    // Note: A sprite is considered any tile type the player is not blocked from passing through plus 
    // Active Stone, Boulders, Wood Platforms and Dart Traps.
    public static bool MeasureSprite(DPoint anyTileLocation, out Terraria.SpriteMeasureData? data) {
      Tile tile = Main.tile[anyTileLocation.X, anyTileLocation.Y];

      DPoint spriteSize = new DPoint(1, 1);
      DPoint textureTileSize = new DPoint(Terraria.DefaultTextureTileSize, Terraria.DefaultTextureTileSize);
      int frameXOffsetAdd = 0;
      switch (tile.type) {
        // 1x1 sprites, normal handling
        case Terraria.TileId_Bottle:
        case Terraria.TileId_WoodPlatform:
        case Terraria.TileId_Book:
        case Terraria.TileId_WoodenBeam:
        case Terraria.TileId_CrystalShard:
        case Terraria.TileId_Switch:
        case Terraria.TileId_DartTrap:
        case Terraria.TileId_Explosives:
        case Terraria.TileId_XSecondTimer:
        case Terraria.TileId_ActiveStone:
        case Terraria.TileId_InactiveStone:
          break;

        // Dynamic sprites, special handling
        case Terraria.TileId_Tree:
          textureTileSize = new DPoint(22, 22);
          break;
        case Terraria.TileId_Vine:
        case Terraria.TileId_JungleVine:
        case Terraria.TileId_GiantGlowingMushroom:
        case Terraria.TileId_ClayPot:
        case Terraria.TileId_Cactus:
        case Terraria.TileId_HallowedVine:
          break;

        case Terraria.TileId_GrassPlants:
        case Terraria.TileId_CorruptionPlants:
        case Terraria.TileId_Candle:
        case Terraria.TileId_WaterCandle:
        case Terraria.TileId_JunglePlants:
        case Terraria.TileId_GlowingMushroom:
        case Terraria.TileId_PlantablePlantsGrowing:
        case Terraria.TileId_PlantablePlantsMature:
        case Terraria.TileId_PlantablePlantsBlooming:
        case Terraria.TileId_HallowedPlants:
          textureTileSize = new DPoint(18, 20);
          break;

        case Terraria.TileId_Coral:
          textureTileSize = new DPoint(26, 28);
          break;

        case Terraria.TileId_Torch:
          frameXOffsetAdd = Terraria.DefaultTextureTileSize * 3;
          textureTileSize = new DPoint(22, 22);
          break;

        case Terraria.TileId_PressurePlate:
          textureTileSize = new DPoint(16, 16);
          break;

        case Terraria.TileId_XMasLight:
          frameXOffsetAdd = Terraria.DefaultTextureTileSize * 2;
          break;

        case Terraria.TileId_IronAnvil:
        case Terraria.TileId_WorkBench:
        case Terraria.TileId_PiggyBank:
        case Terraria.TileId_Bowl:
        case Terraria.TileId_MythrilAnvil:
          spriteSize = new DPoint(2, 1);
          break;

        case Terraria.TileId_WoodenChair:
        case Terraria.TileId_Sapling:
        case Terraria.TileId_ChainLantern:
          spriteSize = new DPoint(1, 2);
          break;

        case Terraria.TileId_TallGrassPlants:
        case Terraria.TileId_TallJunglePlants:
        case Terraria.TileId_TallHallowedPlants:
          spriteSize = new DPoint(1, 2);
          textureTileSize = new DPoint(18, 16);
          break;

        case Terraria.TileId_CrystalHeart:
        case Terraria.TileId_Chest:
        case Terraria.TileId_Pot:
        case Terraria.TileId_ShadowOrb:
        case Terraria.TileId_Sign:
        case Terraria.TileId_Tombstone:
        case Terraria.TileId_Keg:
        case Terraria.TileId_ChineseLantern:
        case Terraria.TileId_CookingPot:
        case Terraria.TileId_Safe:
        case Terraria.TileId_SkullLantern:
        case Terraria.TileId_TashCan_UNUSED:
        case Terraria.TileId_Candelabra:
        case Terraria.TileId_CrystalBall:
        case Terraria.TileId_DiscoBall:
        case Terraria.TileId_Lever:
        case Terraria.TileId_Boulder:
        case Terraria.TileId_InletPump:
        case Terraria.TileId_OutletPump:
        case Terraria.TileId_MusicBox:
          spriteSize = new DPoint(2, 2);
          break;

        case Terraria.TileId_DoorClosed:
        case Terraria.TileId_Banner:
        case Terraria.TileId_TikiTorch:
          spriteSize = new DPoint(1, 3);
          break;

        case Terraria.TileId_DoorOpened:
        case Terraria.TileId_Statue:
        case Terraria.TileId_Mannequin:
          spriteSize = new DPoint(2, 3);
          break;

        case Terraria.TileId_Furnace:
        case Terraria.TileId_WoodenTable:
        case Terraria.TileId_DemonAltar:
        case Terraria.TileId_Hellforge:
        case Terraria.TileId_Loom:
        case Terraria.TileId_Piano:
        case Terraria.TileId_Dresser:
        case Terraria.TileId_Bench:
        case Terraria.TileId_TinkerersWorkshop:
        case Terraria.TileId_AdamantiteForge:
          spriteSize = new DPoint(3, 2);
          break;

        case Terraria.TileId_CopperChandelier:
        case Terraria.TileId_SilverChandelier:
        case Terraria.TileId_GoldChandelier:
        case Terraria.TileId_Sawmill:
          spriteSize = new DPoint(3, 3);
          break;

        case Terraria.TileId_Bed:
        case Terraria.TileId_Bathtub:
          spriteSize = new DPoint(4, 2);
          break;

        case Terraria.TileId_Bookcase:
        case Terraria.TileId_Throne:
          spriteSize = new DPoint(3, 4);
          break;

        case Terraria.TileId_LampPost:
          spriteSize = new DPoint(1, 6);
          break;

        case Terraria.TileId_Sunflower:
          spriteSize = new DPoint(2, 4);
          break;

        case Terraria.TileId_GrandfatherClock:
          spriteSize = new DPoint(2, 5);
          break;

        default:
          data = null;
          return false;
      }

      int originX, originY;
      bool hasActiveFrame;
      switch (tile.type) {
        case Terraria.TileId_Tree: 
        case Terraria.TileId_GiantGlowingMushroom:
        case Terraria.TileId_Cactus: {
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          if (tile.type == Terraria.TileId_Tree) {
            if (Terraria.IsLeftTreeBranch(tile))
              originX++;
            else if (Terraria.IsRightTreeBranch(tile))
              originX--;
          } else if (tile.type == Terraria.TileId_Cactus) {
            if (Terraria.IsLeftCactusBranch(tile))
              originX++;
            else if (Terraria.IsRightCactusBranch(tile))
              originX--;
          }

          while (true) {
            Tile tile2 = Main.tile[originX, originY + 1];

            if (tile2.type != tile.type)
              originY++;
            else 
              break;
          }

          spriteSize = new DPoint(3, 0);
          hasActiveFrame = false;
          break;
        }
        case Terraria.TileId_Vine:
        case Terraria.TileId_JungleVine:
        case Terraria.TileId_HallowedVine: {
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          while (true) {
            Tile tile2 = Main.tile[originX, originY - 1];

            if (tile2.type != tile.type)
              originY--;
            else 
              break;
          }

          spriteSize = new DPoint(1, 0);
          hasActiveFrame = false;
          break;
        }

        default: {
          if (spriteSize.X == 1 && spriteSize.Y == 1) {
            originX = anyTileLocation.X;
            originY = anyTileLocation.Y;
          } else {
            int tileIndexX = tile.frameX / textureTileSize.X;
            int frameIndexX = tileIndexX / spriteSize.X;

            originX = anyTileLocation.X - (tileIndexX - (frameIndexX * spriteSize.X));
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          }

          switch (tile.type) {
            case Terraria.TileId_Switch:
              hasActiveFrame = (Main.tile[originX, originY].frameY == 0);
              break;
            case Terraria.TileId_XSecondTimer:
              hasActiveFrame = (Main.tile[originX, originY].frameY != 0);
              break;
            default:
              hasActiveFrame = (Main.tile[originX, originY].frameX < frameXOffsetAdd + 1);

              if (tile.type == Terraria.TileId_MusicBox)
                hasActiveFrame = !hasActiveFrame;
              break;
          }
          break;
        }
      }

      data = new Terraria.SpriteMeasureData(
        tile.type, new DPoint(originX, originY), spriteSize, textureTileSize, hasActiveFrame, frameXOffsetAdd
      );
      return true;
    }

    public static bool IsLeftTreeBranch(Tile tile) {
      if (tile.type != Terraria.TileId_Tree)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      if (frameX == 44) {
        // Sub side or Green Branch
        return ((frameY >= 132 && frameY <= 176) || frameY >= 198);
      } else if (frameX == 66) {
        // Branch
        return (frameY >= 0 && frameY <= 44);
      } else {
        return false;
      }
    }

    public static bool IsRightTreeBranch(Tile tile) {
      if (tile.type != Terraria.TileId_Tree)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      if (frameX == 22) {
        // Sub side
        return (frameY >= 132 && frameY <= 176);
      } else if (frameX == 66) {
        // Green Branch
        return (frameY >= 198);
      } else if (frameX == 88) {
        // Branch
        return (frameY >= 66 && frameY <= 110);
      } else {
        return false;
      }
    }

    public static bool IsLeftCactusBranch(Tile tile) {
      if (tile.type != Terraria.TileId_Tree)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 54 || (frameX == 108 && frameY == 36));
    }

    public static bool IsRightCactusBranch(Tile tile) {
      if (tile.type != Terraria.TileId_Tree)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 34 || (frameX == 108 && frameY == 18));
    }
    #endregion

    #region [Method: GetTileName, IsBlockTile]
    private static string[] tileNames;

    public static string GetTileName(int tileId) {
      if (Terraria.tileNames == null) {
        Terraria.tileNames = new[] {
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

      if (tileId < 0 || tileId >= Terraria.tileNames.Length)
        return null;

      return Terraria.tileNames[tileId];
    }
    
    // Note: A block is considered any non-sprite, so any tile type which blocks the player from passing through.
    public static bool IsBlockTile(
      int tileId, bool takeWireableStoneAsBlock = false, bool takeWoodPlatformAsBlock = false, 
      bool takeBouldersAsBlocks = false, bool takeDartTrapsAsBlocks = false
    ) {
      return (
        (tileId >= Terraria.TileId_DirtBlock && tileId <= Terraria.TileId_Grass) ||
        (tileId >= Terraria.TileId_SandBlock && tileId <= Terraria.TileId_JungleGrass && tileId != Terraria.TileId_Sign) ||
        (tileId >= Terraria.TileId_IronOre && tileId <= Terraria.TileId_SilverOre) ||
        (tileId >= Terraria.TileId_PearlsandBlock && tileId <= Terraria.TileId_SiltBlock) ||
        (takeWoodPlatformAsBlock && tileId == Terraria.TileId_WoodPlatform) ||
        (tileId >= Terraria.TileId_Meteorite && tileId <= Terraria.TileId_Spike && tileId != Terraria.TileId_ChainLantern) ||
        (tileId >= Terraria.TileId_ObsidianBrick && tileId <= Terraria.TileId_HellstoneBrick) ||
        (tileId >= Terraria.TileId_RedCandyCaneBlock && tileId <= Terraria.TileId_SnowBrick) ||
        (tileId >= Terraria.TileId_DemoniteOre && tileId <= Terraria.TileId_CorruptGrass) ||
        (tileId == Terraria.TileId_Wood) ||
        (tileId >= Terraria.TileId_SapphireBlock && tileId <= Terraria.TileId_DiamondBlock) ||
        (tileId >= Terraria.TileId_CobaltOre && tileId <= Terraria.TileId_EbonsandBlock && tileId != Terraria.TileId_HallowedPlants) ||
        (takeWireableStoneAsBlock && tileId >= Terraria.TileId_ActiveStone && tileId <= Terraria.TileId_InactiveStone) ||
        (tileId == Terraria.TileId_EbonstoneBlock) ||
        (takeBouldersAsBlocks && tileId == Terraria.TileId_Boulder) ||
        (tileId == Terraria.TileId_MushroomGrass) ||
        (tileId == Terraria.TileId_IceBlock)
      );
    }
    #endregion

    #region [Method: CountNPCsInRange, CountItemsInRange]
    public static int CountNPCsInRange(int x, int y, int npcType, int blocksRange) {
      int halfAreaWidth = (blocksRange * Terraria.TileSize) / 2;
      int areaL = x - halfAreaWidth;
      int areaT = y - halfAreaWidth;
      int areaR = x + halfAreaWidth;
      int areaB = y + halfAreaWidth;
      int count = 0;

      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (
          npc.active && npc.type == npcType && 
          npc.position.X > areaL && npc.position.X < areaR &&
          npc.position.Y > areaT && npc.position.Y < areaB
        ) {
          count++;
        }
      }

      return count;
    }

    public static int CountItemsInRange(int x, int y, int itemType, int blocksRange) {
      int halfAreaWidth = (blocksRange * 16) / 2;
      int areaL = x - halfAreaWidth;
      int areaT = y - halfAreaWidth;
      int areaR = x + halfAreaWidth;
      int areaB = y + halfAreaWidth;
      int count = 0;

      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (
          item.active && item.type == itemType && 
          item.position.X > areaL && item.position.X < areaR &&
          item.position.Y > areaT && item.position.Y < areaB
        ) {
          count++;
        }
      }

      return count;
    }
    #endregion

    #region [Methods: GetSpecificNPCIndexes, GetFriendlyNPCIndexes, GetFriendlyFemaleNPCIndexes, GetFriendlyMaleNPCIndexes]
    public static List<int> GetSpecificNPCIndexes(IList<int> npcTypes) {
      List<int> npcIndexes = new List<int>();
      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (!npc.active || !npc.friendly)
          continue;

        if (npcTypes.Contains(npc.type)) {
          npcIndexes.Add(i);

          if (npcIndexes.Count == 10)
            break;
        }
      }

      return npcIndexes;
    }

    public static List<int> GetFriendlyNPCIndexes() {
      return Terraria.GetSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 22, 38, 54, 107, 108, 124 });
    }

    public static List<int> GetFriendlyFemaleNPCIndexes() {
      return Terraria.GetSpecificNPCIndexes(new List<int> { 18, 20, 124 });
    }

    public static List<int> GetFriendlyMaleNPCIndexes() {
      return Terraria.GetSpecificNPCIndexes(new List<int> { 17, 19, 22, 38, 54, 107, 108 });
    }
    #endregion
  }
}
