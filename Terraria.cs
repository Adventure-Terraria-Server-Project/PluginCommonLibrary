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

using TShockAPI;

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
        x >= 0 && x < Main.maxTilesX &&
        y >= 0 && y < Main.maxTilesY
      );
    }

    public static bool IsValidCoord(DPoint point) {
      return Terraria.IsValidCoord(point.X, point.Y);
    }
    #endregion

    #region [Methods: MeasureSprite, SetSpriteActiveFrame, IsSpriteWired, IsLeftTreeBranch, IsRightTreeBranch, IsLeftCactusBranch, IsRightCactusBranch]
    // Note: A sprite is considered any tile type the player is not blocked from passing through plus 
    // Active Stone, Boulders, Wood Platforms and Dart Traps.
    // This function is currently unable to calculate the height of dynamic sprites.
    public static Terraria.SpriteMeasureData MeasureSprite(DPoint anyTileLocation) {
      Tile tile = Main.tile[anyTileLocation.X, anyTileLocation.Y];
      if (!tile.active) {
        throw new ArgumentException(string.Format(
          "The tile at location {0} can not be measured because its not active", anyTileLocation
        ));
      }

      DPoint spriteSize = Terraria.GetSpriteSize(tile.type);
      DPoint textureTileSize = new DPoint(Terraria.DefaultTextureTileSize, Terraria.DefaultTextureTileSize);
      int frameXOffsetAdd = 0;
      switch (tile.type) {
        // Dynamic sprites, special handling
        case Terraria.TileId_Tree:
          textureTileSize = new DPoint(22, 22);
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

        case Terraria.TileId_TallGrassPlants:
        case Terraria.TileId_TallJunglePlants:
        case Terraria.TileId_TallHallowedPlants:
          textureTileSize = new DPoint(18, 16);
          break;
      }

      int originX, originY;
      bool hasActiveFrame;
      switch (tile.type) {
        // Removed dynamic measuring support for Cactus due to Terraria bugs...
        case Terraria.TileId_Cactus:
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          hasActiveFrame = false;
          break;
        case Terraria.TileId_Tree: 
        case Terraria.TileId_GiantGlowingMushroom: {
        //case Terraria.TileId_Cactus: {
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

            if (tile2.type == tile.type)
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

            if (tile2.type == tile.type)
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

      return new Terraria.SpriteMeasureData(
        tile.type, new DPoint(originX, originY), spriteSize, textureTileSize, hasActiveFrame, frameXOffsetAdd
      );
    }

    public static void SetSpriteActiveFrame(
      Terraria.SpriteMeasureData measureData, bool setActiveFrame, bool sendTileSquare = true
    ) {
      if (setActiveFrame == measureData.HasActiveFrame)
        return;

      int originX = measureData.OriginTileLocation.X;
      int originY = measureData.OriginTileLocation.Y;
      int frameXOffsetAdd = measureData.FrameXOffsetAdd;
      int spriteWidth = measureData.Size.X;
      int spriteHeight = measureData.Size.Y;
      short newFrameXOffset = 0;
      short newFrameYOffset = 0;

      if (measureData.SpriteType != Terraria.TileId_Switch && measureData.SpriteType != Terraria.TileId_XSecondTimer) {
        int frameXOffset = (spriteWidth * measureData.TextureTileSize.X) + frameXOffsetAdd;
        if (setActiveFrame)
          newFrameXOffset = (short)-frameXOffset;
        else
          newFrameXOffset = (short)frameXOffset;
      } else {
        int frameYOffset = (spriteHeight * measureData.TextureTileSize.Y);
        if (measureData.SpriteType == Terraria.TileId_XSecondTimer)
          setActiveFrame = !setActiveFrame;

        if (setActiveFrame)
          newFrameYOffset = (short)-frameYOffset;
        else
          newFrameYOffset = (short)frameYOffset;
      }
        
      for (int tx = 0; tx < spriteWidth; tx++) {
        for (int ty = 0; ty < spriteHeight; ty++) {
          int absoluteX = originX + tx;
          int absoluteY = originY + ty;

          Main.tile[absoluteX, absoluteY].frameX += newFrameXOffset;
          Main.tile[absoluteX, absoluteY].frameY += newFrameYOffset;
        }
      }
            
      if (sendTileSquare)
        TSPlayer.All.SendTileSquareEx(originX, originY, Math.Max(spriteWidth, spriteHeight));
    }

    public static bool IsSpriteWired(Terraria.SpriteMeasureData measureData, out DPoint firstWirePosition) {
      int originX = measureData.OriginTileLocation.X;
      int originY = measureData.OriginTileLocation.Y;
      int spriteWidth = measureData.Size.X;
      int spriteHeight = measureData.Size.Y;

      for (int tx = 0; tx < spriteWidth; tx++) {
        for (int ty = 0; ty < spriteHeight; ty++) {
          int ax = originX + tx;
          int ay = originY + ty;

          if (Main.tile[ax, ay].wire) {
            firstWirePosition = new DPoint(ax, ay);
            return true;
          }
        }
      }

      firstWirePosition = DPoint.Empty;
      return false;
    }

    public static bool IsSpriteWired(Terraria.SpriteMeasureData measureData) {
      DPoint dummy;
      return Terraria.IsSpriteWired(measureData, out dummy);
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
      if (tile.type != Terraria.TileId_Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 54 || (frameX == 108 && frameY == 36));
    }

    public static bool IsRightCactusBranch(Tile tile) {
      if (tile.type != Terraria.TileId_Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 36 || (frameX == 108 && frameY == 16));
    }
    #endregion

    #region [Methods: EnumerateSpriteTileLocations, EnumerateSpriteTiles]
    public static IEnumerable<DPoint> EnumerateSpriteTileLocations(Terraria.SpriteMeasureData measureData) {
      for (int x = measureData.OriginTileLocation.X; x < measureData.OriginTileLocation.X + measureData.Size.X; x++) {
        for (int y = measureData.OriginTileLocation.Y; y < measureData.OriginTileLocation.Y + measureData.Size.Y; y++) {
          yield return new DPoint(x, y);
        }
      }
    }

    public static IEnumerable<Tile> EnumerateSpriteTiles(Terraria.SpriteMeasureData measureData) {
      for (int x = measureData.OriginTileLocation.X; x < measureData.OriginTileLocation.X + measureData.Size.X; x++) {
        for (int y = measureData.OriginTileLocation.Y; y < measureData.OriginTileLocation.Y + measureData.Size.Y; y++) {
          yield return Main.tile[x, y];
        }
      }
    }
    #endregion

    #region [Methods: GetTileName, GetSpriteSize, IsBlockTile]
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
        throw new ArgumentException(string.Format("The tild id \"{0}\" is invalid.", tileId), "tileId");

      return Terraria.tileNames[tileId];
    }

    private static DPoint[] spriteSizes;
    public static DPoint GetSpriteSize(int tileId) {
      if (Terraria.spriteSizes == null) {
        Terraria.spriteSizes = new[] {
          new DPoint(1, 1), // Dirt
          new DPoint(1, 1), // Stone
          new DPoint(1, 1), // Grass
          new DPoint(1, 1), // Grass Plant
          new DPoint(1, 1), // Torch
          new DPoint(1, 1), // Tree (dynamic!)
          new DPoint(1, 1), // Iron Ore
          new DPoint(1, 1), // Copper Ore
          new DPoint(1, 1), // Gold Ore
          new DPoint(1, 1), // Silver Ore
          new DPoint(1, 3), // Door (Closed)
          new DPoint(2, 3), // Door (Opened)
          new DPoint(2, 2), // Crystal Heart
          new DPoint(1, 1), // Bottle
          new DPoint(3, 2), // Wooden Table
          new DPoint(1, 2), // Wooden Chair
          new DPoint(2, 1), // Iron Anvil
          new DPoint(3, 2), // Furnace
          new DPoint(2, 1), // Work Bench
          new DPoint(1, 1), // Wood Platform
          new DPoint(1, 2), // Sapling
          new DPoint(2, 2), // Chest
          new DPoint(1, 1), // Demonite Ore
          new DPoint(1, 1), // Corrupt Grass
          new DPoint(1, 1), // Corruption Plant
          new DPoint(1, 1), // Ebonstone Block
          new DPoint(3, 2), // Demon Altar
          new DPoint(2, 4), // Sunflower
          new DPoint(2, 2), // Pot
          new DPoint(2, 1), // Piggy Bank
          new DPoint(1, 1), // Wood
          new DPoint(2, 2), // Shadow Orb
          new DPoint(1, 1), // Corruption Thorny Bush
          new DPoint(1, 1), // Candle
          new DPoint(3, 3), // Copper Chandelier
          new DPoint(3, 3), // Silver Chandelier
          new DPoint(3, 3), // Gold Chandelier
          new DPoint(1, 1), // Meteorite
          new DPoint(1, 1), // Gray Brick
          new DPoint(1, 1), // Red Brick
          new DPoint(1, 1), // Clay
          new DPoint(1, 1), // Blue Brick
          new DPoint(1, 2), // Chain Lantern
          new DPoint(1, 1), // Green Brick
          new DPoint(1, 1), // Pink Brick
          new DPoint(1, 1), // Gold Brick
          new DPoint(1, 1), // Silver Brick
          new DPoint(1, 1), // Copper Brick
          new DPoint(1, 1), // Spike
          new DPoint(1, 1), // Water Candle
          new DPoint(1, 1), // Book
          new DPoint(1, 1), // Cobweb
          new DPoint(1, 1), // Vines (dynamic!)
          new DPoint(1, 1), // Sand
          new DPoint(1, 1), // Glass
          new DPoint(2, 2), // Sign
          new DPoint(1, 1), // Obsidian
          new DPoint(1, 1), // Ash
          new DPoint(1, 1), // Hellstone
          new DPoint(1, 1), // Mud
          new DPoint(1, 1), // Jungle Grass
          new DPoint(1, 1), // Jungle Plant
          new DPoint(1, 1), // Jungle Vine (dynamic!)
          new DPoint(1, 1), // Sapphire
          new DPoint(1, 1), // Ruby
          new DPoint(1, 1), // Emerald
          new DPoint(1, 1), // Topaz
          new DPoint(1, 1), // Amethyst
          new DPoint(1, 1), // Diamond
          new DPoint(1, 1), // Jungle Thorny Bush
          new DPoint(1, 1), // Mushroom Grass
          new DPoint(1, 1), // Glowing Mushroom
          new DPoint(1, 1), // Giant Glowing Mushroom (dynamic!)
          new DPoint(1, 2), // Tall Grass Plant
          new DPoint(1, 2), // Tall Jungle Plant
          new DPoint(1, 1), // Obsidian Brick
          new DPoint(1, 1), // Hellstone Brick
          new DPoint(3, 2), // Hellforge
          new DPoint(1, 1), // ClayPot
          new DPoint(4, 2), // Bed
          new DPoint(1, 1), // Cactus (dynamic!)
          new DPoint(1, 1), // Coral
          new DPoint(1, 1), // Plantable Plant (Growing)
          new DPoint(1, 1), // Plantable Plant (Mature)
          new DPoint(1, 1), // Plantable Plant (Blooming)
          new DPoint(2, 2), // Tombstone
          new DPoint(3, 2), // Loom
          new DPoint(3, 2), // Piano
          new DPoint(3, 2), // Dresser
          new DPoint(3, 2), // Bench
          new DPoint(4, 2), // Bathtub
          new DPoint(1, 3), // Banner
          new DPoint(1, 6), // LampPost
          new DPoint(1, 3), // Tiki Torch
          new DPoint(2, 2), // Keg
          new DPoint(2, 2), // Chinese Lantern
          new DPoint(2, 2), // Cooking Pot
          new DPoint(2, 2), // Safe
          new DPoint(2, 2), // Skull Lantern
          new DPoint(2, 2), // TashCan
          new DPoint(2, 2), // Candelabra
          new DPoint(3, 4), // Bookcase
          new DPoint(3, 4), // Throne
          new DPoint(2, 1), // Bowl
          new DPoint(2, 5), // Grandfather Clock
          new DPoint(2, 3), // Statue
          new DPoint(3, 3), // Sawmill
          new DPoint(1, 1), // Cobalt Ore
          new DPoint(1, 1), // Mythril Ore
          new DPoint(1, 1), // Hallowed Grass
          new DPoint(1, 1), // Hallowed Plant
          new DPoint(1, 1), // Adamantite Ore
          new DPoint(1, 1), // Ebonsand
          new DPoint(1, 2), // Tall Hallowed Plant
          new DPoint(3, 2), // Tinkerer's Workshop
          new DPoint(1, 1), // Hallowed Vine (dynamic!)
          new DPoint(1, 1), // Pearlsand
          new DPoint(1, 1), // Pearlstone
          new DPoint(1, 1), // Pearlstone Brick
          new DPoint(1, 1), // Iridescent Brick
          new DPoint(1, 1), // Mudstone
          new DPoint(1, 1), // Cobalt Brick
          new DPoint(1, 1), // Mythril Brick
          new DPoint(1, 1), // Silt
          new DPoint(1, 1), // Wooden Beam
          new DPoint(2, 2), // Crystal Ball
          new DPoint(2, 2), // Disco Ball
          new DPoint(1, 1), // Ice
          new DPoint(2, 3), // Mannequin
          new DPoint(1, 1), // Crystal Shard
          new DPoint(1, 1), // Active Stone
          new DPoint(1, 1), // Inactive Stone
          new DPoint(2, 2), // Lever
          new DPoint(3, 2), // Adamantite Forge
          new DPoint(2, 1), // Mythril Anvil
          new DPoint(1, 1), // Pressure Plate
          new DPoint(1, 1), // Switch
          new DPoint(1, 1), // Dart Trap
          new DPoint(2, 2), // Boulder
          new DPoint(2, 2), // Music Box
          new DPoint(1, 1), // Demonite Brick
          new DPoint(1, 1), // Explosives
          new DPoint(2, 2), // Inlet Pump
          new DPoint(2, 2), // Outlet Pump
          new DPoint(1, 1), // XSecond Timer
          new DPoint(1, 1), // Red Candy Cane
          new DPoint(1, 1), // Green Candy Cane
          new DPoint(1, 1), // Snow
          new DPoint(1, 1), // Snow Brick
          new DPoint(1, 1), // X-Mas Light
        };
      }

      if (tileId < 0 || tileId >= Terraria.spriteSizes.Length)
        throw new ArgumentException(string.Format("The tild id \"{0}\" is invalid.", tileId), "tileId");

      return Terraria.spriteSizes[tileId];
    }

    public static Orientation GetSpriteOrientation(Tile anyTile) {
      if (!anyTile.active)
        return Orientation.Unknown;

      switch (anyTile.type) {
        case Terraria.TileId_Torch:
          if (anyTile.frameX == 44 || anyTile.frameX == 110)
            return Orientation.Left;

          if (anyTile.frameX == 22 || anyTile.frameX == 88)
            return Orientation.Right;

          return Orientation.Up;

        case Terraria.TileId_Sign:
          if (anyTile.frameX < 36)
            return Orientation.Up;

          if (anyTile.frameX == 36 || anyTile.frameX == 54)
            return Orientation.Down;

          if (anyTile.frameX == 72 || anyTile.frameX == 90)
            return Orientation.Right;

          return Orientation.Left;

        case Terraria.TileId_CrystalShard:
          if (anyTile.frameY == 0)
            return Orientation.Up;

          if (anyTile.frameY == 18)
            return Orientation.Down;

          if (anyTile.frameY == 36)
            return Orientation.Left;

          return Orientation.Right;

        case Terraria.TileId_Switch:
          if (anyTile.frameX == 0)
            return Orientation.Up;

          if (anyTile.frameX == 18)
            return Orientation.Right;

          return Orientation.Left;

        default:
          return Orientation.Unknown;
      }
    }
    
    // Note: A block is considered any non-sprite, so any tile type which blocks the player from passing through 
    // (cobwebs inclusive).
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
        (tileId == Terraria.TileId_IceBlock) ||
        (tileId == Terraria.TileId_Cobweb)
      );
    }
    #endregion

    #region [Methods: CountNPCsInRange, CountItemsInRange]
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

          if (npcIndexes.Count == 10 || npcTypes.Count == 1)
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
