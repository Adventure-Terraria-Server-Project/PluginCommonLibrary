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


    #region [Method: Constructor]
    static Terraria() {
      Terraria.tiles = new TerrariaTiles();
      Terraria.items = new TerrariaItems();
    }
    #endregion

    #region [Methods: MeasureSprite, IsLeftTreeBranch, IsRightTreeBranch, IsLeftCactusBranch, IsRightCactusBranch]
    // Note: A sprite is considered any tile type the player is not blocked from passing through plus 
    // Active Stone, Boulders, Wood Platforms and Dart Traps.
    // This function is currently unable to calculate the height of dynamic sprites.
    public static Terraria.SpriteMeasureData MeasureSprite(DPoint anyTileLocation) {
      Tile tile = Terraria.Tiles[anyTileLocation];
      if (!tile.active) {
        throw new ArgumentException(string.Format(
          "The tile at location {0} can not be measured because its not active", anyTileLocation
        ));
      }

      DPoint spriteSize = Terraria.GetSpriteSize((BlockType)tile.type);
      DPoint textureTileSize = new DPoint(Terraria.DefaultTextureTileSize, Terraria.DefaultTextureTileSize);
      int frameXOffsetAdd = 0;
      switch ((BlockType)tile.type) {
        // Dynamic sprites, special handling
        case BlockType.Tree:
          textureTileSize = new DPoint(22, 22);
          break;

        case BlockType.GrassPlants:
        case BlockType.CorruptionPlants:
        case BlockType.Candle:
        case BlockType.WaterCandle:
        case BlockType.JunglePlants:
        case BlockType.GlowingMushroom:
        case BlockType.PlantablePlantsGrowing:
        case BlockType.PlantablePlantsMature:
        case BlockType.PlantablePlantsBlooming:
        case BlockType.HallowedPlants:
          textureTileSize = new DPoint(18, 20);
          break;

        case BlockType.Coral:
          textureTileSize = new DPoint(26, 28);
          break;

        case BlockType.Torch:
          frameXOffsetAdd = Terraria.DefaultTextureTileSize * 3;
          textureTileSize = new DPoint(22, 22);
          break;

        case BlockType.PressurePlate:
          textureTileSize = new DPoint(16, 16);
          break;

        case BlockType.XMasLight:
          frameXOffsetAdd = Terraria.DefaultTextureTileSize * 2;
          break;

        case BlockType.TallGrassPlants:
        case BlockType.TallJunglePlants:
        case BlockType.TallHallowedPlants:
          textureTileSize = new DPoint(18, 16);
          break;
      }

      int originX, originY;
      switch ((BlockType)tile.type) {
        // Removed dynamic measuring support for Cactus due to Terraria bugs...
        case BlockType.Cactus:
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          break;
        case BlockType.Tree: 
        case BlockType.GiantGlowingMushroom: {
        //case Terraria.TileId_Cactus: {
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          if (tile.type == (int)BlockType.Tree) {
            if (Terraria.IsLeftTreeBranch(tile))
              originX++;
            else if (Terraria.IsRightTreeBranch(tile))
              originX--;
          } else if (tile.type == (int)BlockType.Cactus) {
            if (Terraria.IsLeftCactusBranch(tile))
              originX++;
            else if (Terraria.IsRightCactusBranch(tile))
              originX--;
          }

          while (true) {
            Tile tile2 = Terraria.Tiles[originX, originY + 1];

            if (tile2.type == tile.type)
              originY++;
            else 
              break;
          }

          spriteSize = new DPoint(3, 0);
          break;
        }
        case BlockType.Vine:
        case BlockType.JungleVine:
        case BlockType.HallowedVine: {
          originX = anyTileLocation.X;
          originY = anyTileLocation.Y;

          while (true) {
            Tile tile2 = Terraria.Tiles[originX, originY - 1];

            if (tile2.type == tile.type)
              originY--;
            else 
              break;
          }

          spriteSize = new DPoint(1, 0);
          break;
        }
        case BlockType.DoorOpened: {
          int tileIndexX = tile.frameX / textureTileSize.X;
          int frameIndexX = tileIndexX / spriteSize.X;

          // Is opened to the right side?
          if (frameIndexX == 0) {
            originX = anyTileLocation.X - tileIndexX;
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          } else {
            originX = (anyTileLocation.X - (tileIndexX - (frameIndexX * spriteSize.X))) + 1;
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          }

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

          break;
        }
      }

      return new Terraria.SpriteMeasureData(
        (BlockType)tile.type, new DPoint(originX, originY), spriteSize, textureTileSize, frameXOffsetAdd
      );
    }

    public static bool IsLeftTreeBranch(Tile tile) {
      if (tile.type != (int)BlockType.Tree)
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
      if (tile.type != (int)BlockType.Tree)
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
      if (tile.type != (int)BlockType.Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 54 || (frameX == 108 && frameY == 36));
    }

    public static bool IsRightCactusBranch(Tile tile) {
      if (tile.type != (int)BlockType.Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 36 || (frameX == 108 && frameY == 16));
    }
    #endregion

    #region [Methods: IsSpriteToggleable, HasSpriteActiveFrame, SetSpriteActiveFrame, IsSpriteWired]
    // Does not include doors or active stone!
    public static bool IsSpriteToggleable(BlockType spriteType) {
      return (
        spriteType == BlockType.Torch ||
        (spriteType >= BlockType.Candle && spriteType <= BlockType.GoldChandelier) ||
        spriteType == BlockType.ChainLantern ||
        spriteType == BlockType.LampPost ||
        spriteType == BlockType.TikiTorch ||
        spriteType == BlockType.ChineseLantern ||
        spriteType == BlockType.Candelabra ||
        spriteType == BlockType.DiscoBall ||
        spriteType == BlockType.Lever ||
        spriteType == BlockType.Switch ||
        spriteType == BlockType.XSecondTimer ||
        spriteType == BlockType.XMasLight
      );
    }

    public static bool HasSpriteActiveFrame(Terraria.SpriteMeasureData measureData) {
      Tile tile = Terraria.Tiles[measureData.OriginTileLocation];

      switch (measureData.SpriteType) {
        case BlockType.Switch:
          return (tile.frameY == 0);
        case BlockType.XSecondTimer:
          return (tile.frameY != 0);
        case BlockType.MusicBox:
          return (tile.frameX != 0);
        default:
          return (tile.frameX < measureData.FrameXOffsetAdd + 1);
      }
    }

    public static void SetSpriteActiveFrame(
      Terraria.SpriteMeasureData measureData, bool setActiveFrame, bool sendTileSquare = true
    ) {
      #if DEBUG
      if (Terraria.HasSpriteActiveFrame(measureData) == setActiveFrame) {
        throw new ArgumentException(string.Format(
          "The sprite \"{0}\" does already have the state \"{1}\".", Terraria.Tiles.GetBlockTypeName(measureData.SpriteType), setActiveFrame
        ));
      }
      #endif

      int originX = measureData.OriginTileLocation.X;
      int originY = measureData.OriginTileLocation.Y;
      int spriteWidth = measureData.Size.X;
      int spriteHeight = measureData.Size.Y;
      short newFrameXOffset = 0;
      short newFrameYOffset = 0;

      if (measureData.SpriteType != BlockType.Switch && measureData.SpriteType != BlockType.XSecondTimer) {
        int frameXOffset = (spriteWidth * measureData.TextureTileSize.X) + measureData.FrameXOffsetAdd;
        if (measureData.SpriteType == BlockType.MusicBox)
          setActiveFrame = !setActiveFrame;

        if (setActiveFrame)
          newFrameXOffset = (short)-frameXOffset;
        else
          newFrameXOffset = (short)frameXOffset;
      } else {
        int frameYOffset = (spriteHeight * measureData.TextureTileSize.Y);
        if (measureData.SpriteType == BlockType.XSecondTimer)
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

          Terraria.Tiles[absoluteX, absoluteY].frameX += newFrameXOffset;
          Terraria.Tiles[absoluteX, absoluteY].frameY += newFrameYOffset;
        }
      }
            
      if (sendTileSquare)
        TSPlayer.All.SendTileSquareEx(originX, originY, Math.Max(spriteWidth, spriteHeight));
    }

    public static bool IsSpriteWired(DPoint originTileLocation, DPoint size, out DPoint firstWirePosition) {
      for (int tx = 0; tx < size.X; tx++) {
        for (int ty = 0; ty < size.Y; ty++) {
          int ax = originTileLocation.X + tx;
          int ay = originTileLocation.Y + ty;

          if (Terraria.Tiles[ax, ay].wire) {
            firstWirePosition = new DPoint(ax, ay);
            return true;
          }
        }
      }

      firstWirePosition = DPoint.Empty;
      return false;
    }

    public static bool IsSpriteWired(DPoint originTileLocation, DPoint size) {
      DPoint dummy;
      return Terraria.IsSpriteWired(originTileLocation, size, out dummy);
    }

    public static bool IsSpriteWired(Terraria.SpriteMeasureData measureData) {
      DPoint dummy;
      return Terraria.IsSpriteWired(measureData.OriginTileLocation, measureData.Size, out dummy);
    }

    public static bool IsSpriteWired(Terraria.SpriteMeasureData measureData, out DPoint firstWirePosition) {
      return Terraria.IsSpriteWired(measureData.OriginTileLocation, measureData.Size, out firstWirePosition);
    }
    #endregion

    #region [Methods: GetStatueType, GetItemTypeFromStatueType]
    public static StatueType GetStatueType(DPoint anyStatueTileLocation) {
      Tile tile = Terraria.Tiles[anyStatueTileLocation];
      if (tile.type != (int)BlockType.Statue)
        throw new ArgumentException("The tile at the given location is not of type Statue.", "anyStatueTileLocation");

      return (StatueType)(tile.frameX / (Terraria.DefaultTextureTileSize * 2));
    }

    public static int GetItemTypeFromStatueType(StatueType statueType) {
      switch (statueType) {
        case StatueType.Armor:
          return 360;
        case StatueType.Angel:
          return 52;
        case StatueType.Star:
          return 438;
        case StatueType.Sword:
          return 439;
        case StatueType.Slime:
          return 440;
        case StatueType.Goblin:
          return 441;
        case StatueType.Shield:
          return 442;
        case StatueType.Bat:
          return 443;
        case StatueType.Fish:
          return 444;
        case StatueType.Bunny:
          return 445;
        case StatueType.Skeleton:
          return 446;
        case StatueType.Reaper:
          return 447;
        case StatueType.Woman:
          return 448;
        case StatueType.Imp:
          return 449;
        case StatueType.Gargoyle:
          return 450;
        case StatueType.Gloom:
          return 451;
        case StatueType.Hornet:
          return 452;
        case StatueType.Bomb:
          return 453;
        case StatueType.Crab:
          return 454;
        case StatueType.Hammer:
          return 455;
        case StatueType.Potion:
          return 456;
        case StatueType.Spear:
          return 457;
        case StatueType.Cross:
          return 458;
        case StatueType.Jellyfish:
          return 459;
        case StatueType.Bow:
          return 460;
        case StatueType.Boomerang:
          return 461;
        case StatueType.Boot:
          return 462;
        case StatueType.Chest:
          return 463;
        case StatueType.Bird:
          return 464;
        case StatueType.Axe:
          return 465;
        case StatueType.Corrupt:
          return 466;
        case StatueType.Tree:
          return 467;
        case StatueType.Anvil:
          return 468;
        case StatueType.Pickaxe:
          return 469;
        case StatueType.Mushroom:
          return 470;
        case StatueType.Eyeball:
          return 471;
        case StatueType.Pillar:
          return 472;
        case StatueType.Heart:
          return 473;
        case StatueType.Pot:
          return 474;
        case StatueType.Sunflower:
          return 475;
        case StatueType.King:
          return 476;
        case StatueType.Queen:
          return 477;
        case StatueType.Piranha:
          return 478;
        default:
          throw new ArgumentOutOfRangeException("statueType");
      }
    }
    #endregion

    #region [Methods: GetChestType, GetChestKind]
    public static ChestType GetChestType(DPoint anyChestTileLocation, out bool isLocked) {
      Tile tile = Terraria.Tiles[anyChestTileLocation];
      if (tile.type != (int)BlockType.Chest)
        throw new ArgumentException("The tile at the given location is not of type Chest.", "anyChestTileLocation");

      isLocked = false;
      if (tile.frameX <= 18) {
        return ChestType.WoodenChest;
      } else if (tile.frameX <= 54) {
        return ChestType.GoldChest;
      } else if (tile.frameX <= 90) {
        isLocked = true;
        return ChestType.GoldChest;
      } else if (tile.frameX <= 126) {
        return ChestType.ShadowChest;
      } else if (tile.frameX <= 162) {
        isLocked = true;
        return ChestType.ShadowChest;
      } else if (tile.frameX <= 198) {
        return ChestType.Barrel;
      }

      return ChestType.TrashCan;
    }

    public static ChestKind GetChestKind(DPoint anyChestTileLocation) {
      bool isLocked;
      ChestType chestType = Terraria.GetChestType(anyChestTileLocation, out isLocked);

      switch (chestType) {
        case ChestType.GoldChest:
          Tile chestTile = Terraria.Tiles[anyChestTileLocation];

          if (isLocked) {
            if (chestTile.wall < (int)WallType.DungeonBlueBrickWall || chestTile.wall > (int)WallType.DungeonPinkBrickWall)
              return ChestKind.SkyIslandChest;

            return ChestKind.DungeonChest;
          }

          if (chestTile.liquid < 255 || chestTile.lava)
            return ChestKind.Unknown;
          if (anyChestTileLocation.X > 250 && anyChestTileLocation.X > Main.maxTilesX - 250)
            return ChestKind.Unknown;
          if (anyChestTileLocation.Y < Main.worldSurface - 450 || anyChestTileLocation.Y > Main.worldSurface)
            return ChestKind.Unknown;

          return ChestKind.OceanChest;
        case ChestType.ShadowChest:
          return ChestKind.ShadowChest;
        default:
          return ChestKind.Unknown;
      }
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
          yield return Terraria.Tiles[x, y];
        }
      }
    }
    #endregion

    #region [Methods: GetSpriteSize, GetSpriteOrientation]
    private static DPoint[] spriteSizes;
    public static DPoint GetSpriteSize(BlockType spriteType) {
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

      if (spriteType < 0 || (int)spriteType >= Terraria.spriteSizes.Length)
        throw new ArgumentException(string.Format("The sprite type \"{0}\" is invalid.", spriteType), "spriteType");

      return Terraria.spriteSizes[(int)spriteType];
    }

    public static Direction GetSpriteOrientation(Tile anyTile) {
      if (!anyTile.active)
        return Direction.Unknown;

      switch ((BlockType)anyTile.type) {
        case BlockType.Torch:
          if (anyTile.frameX == 44 || anyTile.frameX == 110)
            return Direction.Left;

          if (anyTile.frameX == 22 || anyTile.frameX == 88)
            return Direction.Right;

          return Direction.Up;

        case BlockType.Sign:
          if (anyTile.frameX < 36)
            return Direction.Up;

          if (anyTile.frameX == 36 || anyTile.frameX == 54)
            return Direction.Down;

          if (anyTile.frameX == 72 || anyTile.frameX == 90)
            return Direction.Right;

          return Direction.Left;

        case BlockType.CrystalShard:
          if (anyTile.frameY == 0)
            return Direction.Up;

          if (anyTile.frameY == 18)
            return Direction.Down;

          if (anyTile.frameY == 36)
            return Direction.Left;

          return Direction.Right;

        case BlockType.Switch:
          if (anyTile.frameX == 0)
            return Direction.Up;

          if (anyTile.frameX == 18)
            return Direction.Right;

          return Direction.Left;

        default:
          return Direction.Unknown;
      }
    }
    #endregion

    #region [Methods: EnumerateNPCsInTileRange, CountItemsInTileRange]
    public static IEnumerable<NPC> EnumerateNPCsInRange(Vector2 location, float range) {
      float halfRange = range / 2;
      float areaL = location.X - halfRange;
      float areaT = location.Y - halfRange;
      float areaR = location.X + halfRange;
      float areaB = location.Y + halfRange;

      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (
          npc.active &&
            npc.position.X > areaL && npc.position.X < areaR &&
            npc.position.Y > areaT && npc.position.Y < areaB
          ) {
          yield return npc;
        }
      }
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

    public static List<int> GetShopNPCIndexes() {
      return Terraria.GetSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 38, 54, 107, 108, 124 });
    }
    #endregion
  }
}
