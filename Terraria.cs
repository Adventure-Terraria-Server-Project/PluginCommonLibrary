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

    #region [Methods: MeasureObject, IsLeftTreeBranch, IsRightTreeBranch, IsLeftCactusBranch, IsRightCactusBranch]
    // Note: A object is considered any tile type the player is not blocked from passing through plus 
    // Active Stone, Boulders, Wood Platforms and Dart Traps.
    // This function is currently unable to calculate the height of objects of dynamic sizes.
    public static Terraria.ObjectMeasureData MeasureObject(DPoint anyTileLocation) {
      Tile tile = Terraria.Tiles[anyTileLocation];
      if (!tile.active) {
        throw new ArgumentException(string.Format(
          "The tile at location {0} can not be measured because its not active", anyTileLocation
        ));
      }

      DPoint objectSize = Terraria.GetObjectSize((BlockType)tile.type);
      DPoint textureTileSize = new DPoint(Terraria.DefaultTextureTileSize, Terraria.DefaultTextureTileSize);
      int frameXOffsetAdd = 0;
      switch ((BlockType)tile.type) {
        // Dynamic objects, special handling
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

          objectSize = new DPoint(3, 0);
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

          objectSize = new DPoint(1, 0);
          break;
        }
        case BlockType.DoorOpened: {
          int tileIndexX = tile.frameX / textureTileSize.X;
          int frameIndexX = tileIndexX / objectSize.X;

          // Is opened to the right side?
          if (frameIndexX == 0) {
            originX = anyTileLocation.X - tileIndexX;
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          } else {
            originX = (anyTileLocation.X - (tileIndexX - (frameIndexX * objectSize.X))) + 1;
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          }

          break;
        }
        default: {
          if (objectSize.X == 1 && objectSize.Y == 1) {
            originX = anyTileLocation.X;
            originY = anyTileLocation.Y;
          } else {
            int tileIndexX = tile.frameX / textureTileSize.X;
            int frameIndexX = tileIndexX / objectSize.X;

            originX = anyTileLocation.X - (tileIndexX - (frameIndexX * objectSize.X));
            originY = anyTileLocation.Y - (tile.frameY / textureTileSize.Y);
          }

          break;
        }
      }

      return new Terraria.ObjectMeasureData(
        (BlockType)tile.type, new DPoint(originX, originY), objectSize, textureTileSize, frameXOffsetAdd
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

    #region [Methods: IsMultistateObject, ObjectHasActiveState, SetObjectState, IsObjectWired]
    // Does not include doors or active stone!
    public static bool IsMultistateObject(BlockType objectType) {
      return (
        objectType == BlockType.Torch ||
        (objectType >= BlockType.Candle && objectType <= BlockType.GoldChandelier) ||
        objectType == BlockType.ChainLantern ||
        objectType == BlockType.LampPost ||
        objectType == BlockType.TikiTorch ||
        objectType == BlockType.ChineseLantern ||
        objectType == BlockType.Candelabra ||
        objectType == BlockType.DiscoBall ||
        objectType == BlockType.Lever ||
        objectType == BlockType.Switch ||
        objectType == BlockType.XSecondTimer ||
        objectType == BlockType.XMasLight
      );
    }

    public static bool ObjectHasActiveState(Terraria.ObjectMeasureData measureData) {
      Tile tile = Terraria.Tiles[measureData.OriginTileLocation];

      switch (measureData.BlockType) {
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

    public static void SetObjectState(
      Terraria.ObjectMeasureData measureData, bool activeState, bool sendTileSquare = true
    ) {
      #if DEBUG
      if (Terraria.ObjectHasActiveState(measureData) == activeState) {
        throw new ArgumentException(string.Format(
          "The object \"{0}\" does already have the state \"{1}\".", Terraria.Tiles.GetBlockTypeName(measureData.BlockType), activeState
        ));
      }
      #endif

      int originX = measureData.OriginTileLocation.X;
      int originY = measureData.OriginTileLocation.Y;
      int objectWidth = measureData.Size.X;
      int objectHeight = measureData.Size.Y;
      short newFrameXOffset = 0;
      short newFrameYOffset = 0;

      if (measureData.BlockType != BlockType.Switch && measureData.BlockType != BlockType.XSecondTimer) {
        int frameXOffset = (objectWidth * measureData.TextureTileSize.X) + measureData.FrameXOffsetAdd;
        if (measureData.BlockType == BlockType.MusicBox)
          activeState = !activeState;

        if (activeState)
          newFrameXOffset = (short)-frameXOffset;
        else
          newFrameXOffset = (short)frameXOffset;
      } else {
        int frameYOffset = (objectHeight * measureData.TextureTileSize.Y);
        if (measureData.BlockType == BlockType.XSecondTimer)
          activeState = !activeState;

        if (activeState)
          newFrameYOffset = (short)-frameYOffset;
        else
          newFrameYOffset = (short)frameYOffset;
      }
        
      for (int tx = 0; tx < objectWidth; tx++) {
        for (int ty = 0; ty < objectHeight; ty++) {
          int absoluteX = originX + tx;
          int absoluteY = originY + ty;

          Terraria.Tiles[absoluteX, absoluteY].frameX += newFrameXOffset;
          Terraria.Tiles[absoluteX, absoluteY].frameY += newFrameYOffset;
        }
      }
            
      if (sendTileSquare)
        TSPlayer.All.SendTileSquareEx(originX, originY, Math.Max(objectWidth, objectHeight));
    }

    public static bool IsObjectWired(DPoint originTileLocation, DPoint size, out DPoint firstWirePosition) {
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

    public static bool IsObjectWired(DPoint originTileLocation, DPoint size) {
      DPoint dummy;
      return Terraria.IsObjectWired(originTileLocation, size, out dummy);
    }

    public static bool IsObjectWired(Terraria.ObjectMeasureData measureData) {
      DPoint dummy;
      return Terraria.IsObjectWired(measureData.OriginTileLocation, measureData.Size, out dummy);
    }

    public static bool IsObjectWired(Terraria.ObjectMeasureData measureData, out DPoint firstWirePosition) {
      return Terraria.IsObjectWired(measureData.OriginTileLocation, measureData.Size, out firstWirePosition);
    }
    #endregion

    #region [Methods: GetStatueStyle, GetItemTypeFromStatueType, GetChestStyle, GetItemTypeFromChestType, LockChest]
    public static StatueStyle GetStatueStyle(Tile tile) {
      return Terraria.GetStatueStyle(tile.frameX / (Terraria.DefaultTextureTileSize * 2));
    }

    public static StatueStyle GetStatueStyle(int objectStyle) {
      return (StatueStyle)(objectStyle + 1);
    }

    public static ItemType GetItemTypeFromStatueType(StatueStyle statueStyle) {
      switch (statueStyle) {
        case StatueStyle.Armor:
          return ItemType.Statue;
        case StatueStyle.Angel:
          return ItemType.AngelStatue;
        case StatueStyle.Star:
          return ItemType.StarStatue;
        case StatueStyle.Sword:
          return ItemType.SwordStatue;
        case StatueStyle.Slime:
          return ItemType.SlimeStatue;
        case StatueStyle.Goblin:
          return ItemType.GoblinStatue;
        case StatueStyle.Shield:
          return ItemType.ShieldStatue;
        case StatueStyle.Bat:
          return ItemType.BatStatue;
        case StatueStyle.Fish:
          return ItemType.FishStatue;
        case StatueStyle.Bunny:
          return ItemType.BunnyStatue;
        case StatueStyle.Skeleton:
          return ItemType.SkeletonStatue;
        case StatueStyle.Reaper:
          return ItemType.ReaperStatue;
        case StatueStyle.Woman:
          return ItemType.WomanStatue;
        case StatueStyle.Imp:
          return ItemType.ImpStatue;
        case StatueStyle.Gargoyle:
          return ItemType.GargoyleStatue;
        case StatueStyle.Gloom:
          return ItemType.GloomStatue;
        case StatueStyle.Hornet:
          return ItemType.HornetStatue;
        case StatueStyle.Bomb:
          return ItemType.BombStatue;
        case StatueStyle.Crab:
          return ItemType.CrabStatue;
        case StatueStyle.Hammer:
          return ItemType.HammerStatue;
        case StatueStyle.Potion:
          return ItemType.PotionStatue;
        case StatueStyle.Spear:
          return ItemType.SpearStatue;
        case StatueStyle.Cross:
          return ItemType.CrossStatue;
        case StatueStyle.Jellyfish:
          return ItemType.JellyfishStatue;
        case StatueStyle.Bow:
          return ItemType.BowStatue;
        case StatueStyle.Boomerang:
          return ItemType.BoomerangStatue;
        case StatueStyle.Boot:
          return ItemType.BootStatue;
        case StatueStyle.Chest:
          return ItemType.ChestStatue;
        case StatueStyle.Bird:
          return ItemType.BirdStatue;
        case StatueStyle.Axe:
          return ItemType.AxeStatue;
        case StatueStyle.Corrupt:
          return ItemType.CorruptStatue;
        case StatueStyle.Tree:
          return ItemType.TreeStatue;
        case StatueStyle.Anvil:
          return ItemType.AnvilStatue;
        case StatueStyle.Pickaxe:
          return ItemType.PickaxeStatue;
        case StatueStyle.Mushroom:
          return ItemType.MushroomStatue;
        case StatueStyle.Eyeball:
          return ItemType.EyeballStatue;
        case StatueStyle.Pillar:
          return ItemType.PillarStatue;
        case StatueStyle.Heart:
          return ItemType.HeartStatue;
        case StatueStyle.Pot:
          return ItemType.PotStatue;
        case StatueStyle.Sunflower:
          return ItemType.SunflowerStatue;
        case StatueStyle.King:
          return ItemType.KingStatue;
        case StatueStyle.Queen:
          return ItemType.QueenStatue;
        case StatueStyle.Piranha:
          return ItemType.PiranhaStatue;
        default:
          throw new ArgumentException("StatueStyle");
      }
    }

    public static ChestStyle GetChestStyle(Tile tile, out bool isLocked) {
      return Terraria.GetChestStyle((tile.frameX / (Terraria.DefaultTextureTileSize * 2)), out isLocked);
    }

    public static ChestStyle GetChestStyle(int objectStyle, out bool isLocked) {
      isLocked = false;

      switch (objectStyle) {
        case 0:
          return ChestStyle.WoodenChest;
        case 1:
          return ChestStyle.GoldChest;
        case 2:
          isLocked = true;
          return ChestStyle.GoldChest;
        case 3:
          return ChestStyle.ShadowChest;
        case 4:
          isLocked = true;
          return ChestStyle.ShadowChest;
        case 5:
          return ChestStyle.Barrel;
        case 6:
          return ChestStyle.TrashCan;
        default:
          throw new ArgumentOutOfRangeException("objectStyle");
      }
    }

    public static ItemType GetItemTypeFromChestType(ChestStyle chestStyle) {
      switch (chestStyle) {
        case ChestStyle.WoodenChest:
          return ItemType.Chest;
        case ChestStyle.GoldChest:
          return ItemType.GoldChest;
        case ChestStyle.ShadowChest:
          return ItemType.ShadowChest;
        case ChestStyle.Barrel:
          return ItemType.Barrel;
        case ChestStyle.TrashCan:
          return ItemType.TrashCan;
        default:
          throw new ArgumentException("ChestStyle");
      }
    }
 
    public static ChestKind GetChestKind(DPoint anyChestTileLocation) {
      Tile chestTile = Terraria.Tiles[anyChestTileLocation];
      if (!chestTile.active)
        throw new ArgumentException("The tile on the given location is not active.");

      bool isLocked;
      ChestStyle chestStyle = Terraria.GetChestStyle(chestTile, out isLocked);
      switch (chestStyle) {
        case ChestStyle.GoldChest:
          if (isLocked) {
            if (chestTile.wall >= (int)WallType.DungeonBlueBrickWall && chestTile.wall <= (int)WallType.DungeonPinkBrickWall)
              return ChestKind.DungeonChest;

            if (anyChestTileLocation.Y < Main.worldSurface)
              return ChestKind.SkyIslandChest;

            return ChestKind.Unknown;
          }

          if (chestTile.liquid < 255 || chestTile.lava)
            return ChestKind.Unknown;
          if (anyChestTileLocation.X > 250 && anyChestTileLocation.X < Main.maxTilesX - 250)
            return ChestKind.Unknown;
          if (anyChestTileLocation.Y < Main.worldSurface - 200 || anyChestTileLocation.Y > Main.worldSurface + 50)
            return ChestKind.Unknown;

          return ChestKind.OceanChest;
        case ChestStyle.ShadowChest:
          if (!isLocked)
            return ChestKind.Unknown;
          
          if (anyChestTileLocation.Y < Main.maxTilesY * (1.0 - 0.143))
            return ChestKind.Unknown;

          return ChestKind.HellShadowChest;
        default:
          return ChestKind.Unknown;
      }
    }
    
    public static void LockChest(DPoint anyChestTileLocation) {
      Tile chestTile = Terraria.Tiles[anyChestTileLocation];
      if (!chestTile.active || chestTile.type != (int)BlockType.Chest)
        throw new ArgumentException("Tile is not a chest.", "anyChestTileLocation");

      bool isLocked;
      ChestStyle chestStyle = Terraria.GetChestStyle(chestTile, out isLocked);
      if (isLocked || (chestStyle != ChestStyle.GoldChest && chestStyle != ChestStyle.ShadowChest))
        throw new InvalidChestStyleException("Chest has to be a gold- or shadow chest.", chestStyle);

      Terraria.ObjectMeasureData measureData = Terraria.MeasureObject(anyChestTileLocation);
      foreach (Tile tile in Terraria.EnumerateObjectTiles(measureData))
        tile.frameX += 36;
      
      TSPlayer.All.SendTileSquare(anyChestTileLocation, 4);
    }
    #endregion

    #region [Methods: EnumerateObjectTileLocations, EnumerateObjectTiles]
    public static IEnumerable<DPoint> EnumerateObjectTileLocations(Terraria.ObjectMeasureData measureData) {
      for (int x = measureData.OriginTileLocation.X; x < measureData.OriginTileLocation.X + measureData.Size.X; x++) {
        for (int y = measureData.OriginTileLocation.Y; y < measureData.OriginTileLocation.Y + measureData.Size.Y; y++) {
          yield return new DPoint(x, y);
        }
      }
    }

    public static IEnumerable<Tile> EnumerateObjectTiles(Terraria.ObjectMeasureData measureData) {
      for (int x = measureData.OriginTileLocation.X; x < measureData.OriginTileLocation.X + measureData.Size.X; x++) {
        for (int y = measureData.OriginTileLocation.Y; y < measureData.OriginTileLocation.Y + measureData.Size.Y; y++) {
          yield return Terraria.Tiles[x, y];
        }
      }
    }
    #endregion

    #region [Methods: GetObjectSize, GetObjectOrientation]
    private static DPoint[] objectSizes;
    public static DPoint GetObjectSize(BlockType objectType) {
      if (Terraria.objectSizes == null) {
        Terraria.objectSizes = new[] {
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

      if (objectType < 0 || (int)objectType >= Terraria.objectSizes.Length)
        throw new ArgumentException(string.Format("The object type \"{0}\" is invalid.", objectType), "objectType");

      return Terraria.objectSizes[(int)objectType];
    }

    public static Direction GetObjectOrientation(Tile anyTile) {
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
