using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaTiles {
    public Tile this[int x, int y] {
      get {
        #if DEBUG
        Contract.Assert(TerrariaUtils.Tiles.IsValidCoord(x, y), string.Format("{{{0}, {1}}}", x, y));
        #endif
        return Main.tile[x, y];
      }
    }

    public Tile this[DPoint point] {
      get {
        #if DEBUG
        Contract.Assert(TerrariaUtils.Tiles.IsValidCoord(point.X, point.Y), point.ToString());
        #endif
        return Main.tile[point.X, point.Y];
      }
    }


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
      return (blockType >= TerrariaUtils.BlockType_Min && blockType <= TerrariaUtils.BlockType_Max);
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

    public string GetBlockTypeName(BlockType blockType) {
      return blockType.ToString();
    }

    // Note: A block is considered any non-object, so any block type which blocks the player from passing through 
    // (cobwebs included).
    public bool IsSolidBlockType(
      BlockType blockType, bool considerWireableStoneAsSolid = false, bool considerWoodPlatformAsSolid = false, 
      bool considerBoulderAsSolid = false, bool considerDartTrapsAsSolid = false
    ) {
      if (blockType == BlockType.ActiveStone || blockType == BlockType.InactiveStone)
        return considerWireableStoneAsSolid;
      if (blockType == BlockType.WoodPlatform)
        return considerWoodPlatformAsSolid;
      if (blockType == BlockType.Boulder)
        return considerBoulderAsSolid;
      if (blockType == BlockType.DartTrap)
        return considerDartTrapsAsSolid;

      return Main.tileSolid[(int)blockType];
    }

    public void PlaceObject(
      DPoint originTileDestination, BlockType objectType, DPoint frameOffset = default(DPoint), bool localOnly = false
    ) {
      DPoint objectSize = this.GetObjectSize(objectType);
      DPoint textureTileSize = this.GetBlockTextureTileSize(objectType);

      for (int x = 0; x < objectSize.X; x++) {
        for (int y = 0; y < objectSize.Y; y++) {
          Tile objectTile;
          if (objectType == BlockType.DoorOpened && frameOffset.X >= 36)
            objectTile = TerrariaUtils.Tiles[originTileDestination.X + x - 1, originTileDestination.Y + y];
          else
            objectTile = TerrariaUtils.Tiles[originTileDestination.X + x, originTileDestination.Y + y];

          objectTile.active(true);
          objectTile.frameX = Convert.ToInt16(frameOffset.X + textureTileSize.X * x);
          objectTile.frameY = Convert.ToInt16(frameOffset.Y + textureTileSize.Y * y);

          WorldGen.SquareTileFrame(originTileDestination.X, originTileDestination.Y, true);
        }
      }

      if (!localOnly)
        TSPlayer.All.SendTileSquareEx(originTileDestination, Math.Max(objectSize.X, objectSize.Y));
    }

    public void PlantHerb(DPoint tileLocation, HerbStyle style, HerbGrowthState state = HerbGrowthState.Mature) {
      Tile tile = TerrariaUtils.Tiles[tileLocation];
      tile.active(true);
      switch (state) {
        case HerbGrowthState.Growing:
          tile.type = (int)BlockType.HerbGrowing;
          break;
        case HerbGrowthState.Mature:
          tile.type = (int)BlockType.HerbMature;
          break;
        case HerbGrowthState.Blooming:
          tile.type = (int)BlockType.HerbBlooming;
          break;
        default:
          throw new ArgumentException("state");
      }

      tile.frameX = Convert.ToInt16((int)style * TerrariaUtils.DefaultTextureTileSize);
      TSPlayer.All.SendTileSquare(tileLocation.X, tileLocation.Y, 3);
    }

    public void SetBlock(DPoint tileLocation, BlockType blockType, bool localOnly = false, bool squareFrame = true) {
      Tile tile = TerrariaUtils.Tiles[tileLocation];

      if (blockType != BlockType.Invalid) {
        tile.type = (byte)blockType;
        tile.active(true);
      } else {
        tile.type = 0;
        tile.active(false);
      }

      if (squareFrame)
        WorldGen.SquareTileFrame(tileLocation.X, tileLocation.Y, true);
      if (!localOnly)
        TSPlayer.All.SendTileSquare(tileLocation.X, tileLocation.Y, 1);
    }

    public void RemoveBlock(DPoint tileLocation, bool squareFrames = true, bool localOnly = false) {
      Tile tile = TerrariaUtils.Tiles[tileLocation];

      tile.type = 0;
      tile.active(false);
      tile.frameX = -1;
      tile.frameY = -1;
      tile.frameNumber(0);

      if (squareFrames)
        WorldGen.SquareTileFrame(tileLocation.X, tileLocation.Y);
      if (!localOnly)
        TSPlayer.All.SendTileSquare(tileLocation.X, tileLocation.Y, 1);
    }

    public void RemoveTile(DPoint tileLocation, bool squareFrames = true, bool localOnly = false) {
      Tile tile = TerrariaUtils.Tiles[tileLocation];

      tile.wall = 0;
      tile.liquid = 0;

      this.RemoveBlock(tileLocation, squareFrames, localOnly);
    }

    /// <remarks>
    ///   <p>
    ///     A object is considered any tile type the player is not blocked from passing through plus 
    ///     Active Stone, Boulders, Wood Platforms and Dart Traps. However, this method will still measure
    ///     any other block type too.
    ///   </p>
    ///   <p>
    ///     The origin tile of most objects is their most top left tile. Exceptions are trees and glowing mushrooms where 
    ///     the origin tile is their middle stump. Vines have their origin tile at the root on top. Doors have their origin
    ///     tile at their top bracing.
    ///   </p>
    /// </remarks>
    public ObjectMeasureData MeasureObject(DPoint anyTileLocation) {
      Tile tile = TerrariaUtils.Tiles[anyTileLocation];
      if (!tile.active()) {
        throw new ArgumentException(string.Format(
          "The tile at location {0} can not be measured because its not active", anyTileLocation
        ));
      }

      DPoint objectSize = this.GetObjectSize((BlockType)tile.type);
      DPoint textureTileSize = this.GetBlockTextureTileSize((BlockType)tile.type);
      DPoint textureFrameLocation = DPoint.Empty;

      switch ((BlockType)tile.type) {
        case BlockType.Undefined2:
          if (tile.frameY <= 72)
            objectSize = new DPoint(1, 2);

          break;
        case BlockType.Undefined15: {
          if (tile.frameY >= 36)
            objectSize = new DPoint(2, 2);

          break;
        }
      }

      DPoint originTileLocation;
      switch ((BlockType)tile.type) {
        case BlockType.Cactus: {
          // Removed dynamic measuring support for Cactus due to Terraria bugs...
          originTileLocation = anyTileLocation;
          textureFrameLocation = new DPoint(tile.frameX / textureTileSize.X, tile.frameY / textureTileSize.Y);
          break;
        }
        case BlockType.Tree: 
        case BlockType.GiantGlowingMushroom: {
        //case Terraria.TileId_Cactus: {
          // We consider the origin tile of Trees, Giant Glowing Mushrooms and Cacti their most bottom tile (stump) instead
          // of the most top left tile.
          DPoint anyTrunkTileLocation = anyTileLocation;
          if (tile.type == (int)BlockType.Tree) {
            if (this.IsLeftTreeBranch(tile))
              anyTrunkTileLocation = anyTileLocation.OffsetEx(1, 0);
            else if (this.IsRightTreeBranch(tile))
              anyTrunkTileLocation = anyTileLocation.OffsetEx(-1, 0);
          } else if (tile.type == (int)BlockType.Cactus) {
            if (this.IsLeftCactusBranch(tile))
              anyTrunkTileLocation = anyTileLocation.OffsetEx(1, 0);
            else if (this.IsRightCactusBranch(tile))
              anyTrunkTileLocation = anyTileLocation.OffsetEx(-1, 0);
          }

          // Go all the way down to the tree's stump.
          DPoint currentTileLocation = anyTrunkTileLocation;
          while (true) {
            Tile currentTile = TerrariaUtils.Tiles[currentTileLocation.OffsetEx(0, 1)];

            if (currentTile.active() && currentTile.type == tile.type)
              currentTileLocation.Y++;
            else 
              break;
          }
          DPoint treeStumpLocation = currentTileLocation;
          objectSize = new DPoint(3, (treeStumpLocation.Y - anyTrunkTileLocation.Y) + 1);

          // Now measure the tree's size by going it up.
          currentTileLocation = anyTrunkTileLocation;
          while (true) {
            Tile currentTile = TerrariaUtils.Tiles[currentTileLocation.OffsetEx(0, -1)];

            if (currentTile.active() && currentTile.type == tile.type) {
              currentTileLocation.Y--;
              objectSize.Y++;
            } else {
              break;
            }
          }

          originTileLocation = treeStumpLocation;
          textureFrameLocation = new DPoint(tile.frameX / textureTileSize.X, tile.frameY / textureTileSize.Y);
          break;
        }
        case BlockType.Vine:
        case BlockType.JungleVine:
        case BlockType.HallowedVine:
        case BlockType.Undefined14: {
          DPoint currentTileLocation = anyTileLocation;

          while (true) {
            Tile currentTile = TerrariaUtils.Tiles[currentTileLocation.OffsetEx(0, -1)];

            if (currentTile.type == tile.type) {
              currentTileLocation.Y--;
              objectSize.Y++;
            } else {
              break;
            }
          }
          originTileLocation = currentTileLocation;
          objectSize = new DPoint(1, (anyTileLocation.Y - originTileLocation.Y) + 1);

          // Now measure the vines's size by going it down.
          currentTileLocation = anyTileLocation;
          while (true) {
            Tile currentTile = TerrariaUtils.Tiles[currentTileLocation.OffsetEx(0, 1)];

            if (currentTile.type == tile.type) {
              currentTileLocation.Y++;
              objectSize.Y++;
            } else {
              break;
            }
          }

          textureFrameLocation = new DPoint(tile.frameX / textureTileSize.X, tile.frameY / textureTileSize.Y);
          break;
        }
        case BlockType.DoorOpened: 
        case BlockType.TrapdoorOpen: {
          int textureTileX = tile.frameX / textureTileSize.X;

          if (this.GetDoorDirection(anyTileLocation) == Direction.Right ||
            this.GetDoorDirection(anyTileLocation) == Direction.Up) {
            originTileLocation = anyTileLocation.OffsetEx(-textureTileX, -(tile.frameY / textureTileSize.Y));
          } else {
            originTileLocation = anyTileLocation.OffsetEx(
              -(textureTileX - objectSize.X) + 1, -(tile.frameY / textureTileSize.Y)
            );
            textureFrameLocation = new DPoint(1, 0);
          }

          break;
        }
        default: {
          if (objectSize.X == 1 && objectSize.Y == 1) {
            originTileLocation = anyTileLocation;
            textureFrameLocation = new DPoint(tile.frameX / textureTileSize.X, tile.frameY / textureTileSize.Y);
          } else {
            int textureTileX = tile.frameX / textureTileSize.X;
            int textureTileY = tile.frameY / textureTileSize.Y;
            int textureFrameX = textureTileX / objectSize.X;
            int textureFrameY = textureTileY / objectSize.Y;

            originTileLocation = anyTileLocation.OffsetEx(
              -(textureTileX - (textureFrameX * objectSize.X)),
              -(textureTileY - (textureFrameY * objectSize.Y))
            );

            textureFrameLocation = new DPoint(textureFrameX, textureFrameY);
          }

          break;
        }
      }

      return new ObjectMeasureData(
        (BlockType)tile.type, originTileLocation, objectSize, textureTileSize, textureFrameLocation
      );
    }

    private bool IsLeftTreeBranch(Tile tile) {
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

    private bool IsRightTreeBranch(Tile tile) {
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

    private bool IsLeftCactusBranch(Tile tile) {
      if (tile.type != (int)BlockType.Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 54 || (frameX == 108 && frameY == 36));
    }

    private bool IsRightCactusBranch(Tile tile) {
      if (tile.type != (int)BlockType.Cactus)
        return false;

      int frameX = tile.frameX;
      int frameY = tile.frameY;

      return (frameX == 36 || (frameX == 108 && frameY == 16));
    }

    /// <remarks>
    ///   Does not include doors or active stone.
    /// </remarks>
    public bool IsMultistateObject(BlockType objectType) {
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
        objectType == BlockType.MusicBox ||
        objectType == BlockType.XSecondTimer ||
        objectType == BlockType.XMasLight
      );
    }

    public bool ObjectHasActiveState(ObjectMeasureData measureData) {
      Tile tile = TerrariaUtils.Tiles[measureData.OriginTileLocation];

      switch (measureData.BlockType) {
        case BlockType.Switch:
          return (tile.frameY == 0);
        case BlockType.XSecondTimer:
        case BlockType.WaterFountain:
          return (tile.frameY != 0);
        case BlockType.MusicBox:
          return (tile.frameX != 0);
        case BlockType.Torch:
          return (tile.frameX < 66);
        case BlockType.XMasLight:
          return (tile.frameX < 54);
        default:
          return (tile.frameX == 0);
      }
    }

    public void SetObjectState(
      ObjectMeasureData measureData, bool activeState, bool sendTileSquare = true
    ) {
      #if DEBUG
      if (this.ObjectHasActiveState(measureData) == activeState) {
        throw new ArgumentException(string.Format(
          "The object \"{0}\" does already have the state \"{1}\".", TerrariaUtils.Tiles.GetBlockTypeName(measureData.BlockType), activeState
        ));
      }
      #endif

      int originX = measureData.OriginTileLocation.X;
      int originY = measureData.OriginTileLocation.Y;
      int objectWidth = measureData.Size.X;
      int objectHeight = measureData.Size.Y;
      int newFrameXOffset = 0;
      int newFrameYOffset = 0;
      if (measureData.BlockType == BlockType.Torch || measureData.BlockType == BlockType.XMasLight)
        newFrameXOffset = measureData.TextureTileSize.X * 2;

      if (
        measureData.BlockType != BlockType.Switch && 
        measureData.BlockType != BlockType.XSecondTimer &&
        measureData.BlockType != BlockType.WaterFountain
      ) {
        int frameXOffset = (objectWidth * measureData.TextureTileSize.X) + newFrameXOffset;
        if (measureData.BlockType == BlockType.MusicBox)
          frameXOffset = -frameXOffset;

        if (activeState)
          newFrameXOffset = (short)-frameXOffset;
        else
          newFrameXOffset = (short)frameXOffset;

        if (measureData.BlockType == BlockType.BubbleMachine && !activeState)
          newFrameYOffset = -TerrariaUtils.Tiles[measureData.OriginTileLocation].frameY;
      } else {
        int frameYOffset = (objectHeight * measureData.TextureTileSize.Y);
        if (measureData.BlockType == BlockType.WaterFountain && !activeState) {
          newFrameYOffset = -TerrariaUtils.Tiles[measureData.OriginTileLocation].frameY;
        } else {
          if (
            measureData.BlockType == BlockType.XSecondTimer ||
            measureData.BlockType == BlockType.WaterFountain
          )
            activeState = !activeState;

          if (activeState)
            newFrameYOffset = (short)-frameYOffset;
          else
            newFrameYOffset = (short)frameYOffset;
        }
      }
        
      for (int tx = 0; tx < objectWidth; tx++) {
        for (int ty = 0; ty < objectHeight; ty++) {
          int absoluteX = originX + tx;
          int absoluteY = originY + ty;

          TerrariaUtils.Tiles[absoluteX, absoluteY].frameX += (short)newFrameXOffset;
          TerrariaUtils.Tiles[absoluteX, absoluteY].frameY += (short)newFrameYOffset;
        }
      }
            
      if (sendTileSquare)
        TSPlayer.All.SendTileSquareEx(originX, originY, Math.Max(objectWidth, objectHeight));
    }

    public bool IsObjectWired(DPoint originTileLocation, DPoint size, out DPoint firstWirePosition) {
      for (int tx = 0; tx < size.X; tx++) {
        for (int ty = 0; ty < size.Y; ty++) {
          int ax = originTileLocation.X + tx;
          int ay = originTileLocation.Y + ty;

          Tile tile = TerrariaUtils.Tiles[ax, ay];
          if (tile.wire() || tile.wire2() || tile.wire3()) {
            firstWirePosition = new DPoint(ax, ay);
            return true;
          }
        }
      }

      firstWirePosition = DPoint.Empty;
      return false;
    }

    public bool IsObjectWired(ObjectMeasureData measureData, WireColor wireColor, out DPoint firstWireLocation) {
      foreach (DPoint tileLocation in this.EnumerateObjectTileLocations(measureData)) {
        Tile tile = TerrariaUtils.Tiles[tileLocation];
        if (tile.HasWire(wireColor)) {
          firstWireLocation = tileLocation;
          return true;
        }
      }

      firstWireLocation = DPoint.Empty;
      return false;
    }

    public bool IsObjectWired(DPoint originTileLocation, DPoint size, WireColor wireColor = WireColor.None) {
      DPoint dummy;
      return this.IsObjectWired(originTileLocation, size, out dummy);
    }

    public bool IsObjectWired(ObjectMeasureData measureData, WireColor wireColor = WireColor.None) {
      DPoint dummy;
      return this.IsObjectWired(measureData, wireColor, out dummy);
    }

    public TrapStyle GetTrapStyle(int objectStyle) {
      return (TrapStyle)objectStyle;
    }

    public ItemType GetItemTypeFromTrapStyle(TrapStyle trapStyle) {
      switch (trapStyle) {
        case TrapStyle.DartTrap:
          return ItemType.DartTrap;
        case TrapStyle.SuperDartTrap:
          return ItemType.SuperDartTrap;
        case TrapStyle.FlameTrap:
          return ItemType.FlameTrap;
        case TrapStyle.SpikyBallTrap:
          return ItemType.SpikyBallTrap;
        case TrapStyle.SpearTrap:
          return ItemType.SpearTrap;
        default:
          return ItemType.None;
      }
    }

    public StatueStyle GetStatueStyle(int objectStyle) {
      return (StatueStyle)(objectStyle + 1);
    }

    public StatueStyle GetStatueStyle(Tile tile) {
      return this.GetStatueStyle(tile.frameX / (TerrariaUtils.DefaultTextureTileSize * 2));
    }

    private static readonly Lazy<Dictionary<StatueStyle,ItemType>> statueStyleItemTypeLookup = new Lazy<Dictionary<StatueStyle, ItemType>>(() => {
      return new Dictionary<StatueStyle, ItemType>{
        [StatueStyle.Armor] = ItemType.Statue,
        [StatueStyle.Angel] = ItemType.AngelStatue,
        [StatueStyle.Star] = ItemType.StarStatue,
        [StatueStyle.Sword] = ItemType.SwordStatue,
        [StatueStyle.Slime] = ItemType.SlimeStatue,
        [StatueStyle.Goblin] = ItemType.GoblinStatue,
        [StatueStyle.Shield] = ItemType.ShieldStatue,
        [StatueStyle.Bat] = ItemType.BatStatue,
        [StatueStyle.Fish] = ItemType.FishStatue,
        [StatueStyle.Bunny] = ItemType.BunnyStatue,
        [StatueStyle.Skeleton] = ItemType.SkeletonStatue,
        [StatueStyle.Reaper] = ItemType.ReaperStatue,
        [StatueStyle.Woman] = ItemType.WomanStatue,
        [StatueStyle.Imp] = ItemType.ImpStatue,
        [StatueStyle.Gargoyle] = ItemType.GargoyleStatue,
        [StatueStyle.Gloom] = ItemType.GloomStatue,
        [StatueStyle.Hornet] = ItemType.HornetStatue,
        [StatueStyle.Bomb] = ItemType.BombStatue,
        [StatueStyle.Crab] = ItemType.CrabStatue,
        [StatueStyle.Hammer] = ItemType.HammerStatue,
        [StatueStyle.Potion] = ItemType.PotionStatue,
        [StatueStyle.Spear] = ItemType.SpearStatue,
        [StatueStyle.Cross] = ItemType.CrossStatue,
        [StatueStyle.Jellyfish] = ItemType.JellyfishStatue,
        [StatueStyle.Bow] = ItemType.BowStatue,
        [StatueStyle.Boomerang] = ItemType.BoomerangStatue,
        [StatueStyle.Boot] = ItemType.BootStatue,
        [StatueStyle.Chest] = ItemType.ChestStatue,
        [StatueStyle.Bird] = ItemType.BirdStatue,
        [StatueStyle.Axe] = ItemType.AxeStatue,
        [StatueStyle.Corrupt] = ItemType.CorruptStatue,
        [StatueStyle.Tree] = ItemType.TreeStatue,
        [StatueStyle.Anvil] = ItemType.AnvilStatue,
        [StatueStyle.Pickaxe] = ItemType.PickaxeStatue,
        [StatueStyle.Mushroom] = ItemType.MushroomStatue,
        [StatueStyle.Eyeball] = ItemType.EyeballStatue,
        [StatueStyle.Pillar] = ItemType.PillarStatue,
        [StatueStyle.Heart] = ItemType.HeartStatue,
        [StatueStyle.Pot] = ItemType.PotStatue,
        [StatueStyle.Sunflower] = ItemType.SunflowerStatue,
        [StatueStyle.King] = ItemType.KingStatue,
        [StatueStyle.Queen] = ItemType.QueenStatue,
        [StatueStyle.Piranha] = ItemType.PiranhaStatue,
        [StatueStyle.Lihzahrd] = ItemType.LihzahrdStatue,
        [StatueStyle.LihzahrdGuardian] = ItemType.LihzahrdGuardianStatue,
        [StatueStyle.LihzahrdWatcher] = ItemType.LihzahrdWatcherStatue,
        [StatueStyle.BlueDungeonVase] = ItemType.BlueDungeonVase,
        [StatueStyle.GreenDungeonVase] = ItemType.GreenDungeonVase,
        [StatueStyle.PinkDungeonVase] = ItemType.PinkDungeonVase,
        [StatueStyle.ObsidianVase] = ItemType.ObsidianVase,
        [StatueStyle.Shark] = ItemType.SharkStatue,
        // 1.3.1
        [StatueStyle.Squirrel] = ItemType.SquirrelStatue,
        [StatueStyle.Butterfly] = ItemType.ButterflyStatue,
        [StatueStyle.Worm] = ItemType.WormStatue,
        [StatueStyle.Firefly] = ItemType.FireflyStatue,
        [StatueStyle.Scorpion] = ItemType.ScorpionStatue,
        [StatueStyle.Snail] = ItemType.SnailStatue,
        [StatueStyle.Grasshopper] = ItemType.GrasshopperStatue,
        [StatueStyle.Mouse] = ItemType.MouseStatue,
        [StatueStyle.Duck] = ItemType.DuckStatue,
        [StatueStyle.Penguin] = ItemType.PenguinStatue,
        [StatueStyle.Frog] = ItemType.FrogStatue,
        [StatueStyle.Buggy] = ItemType.BuggyStatue,
        [StatueStyle.Unicorn] = ItemType.UnicornStatue,
        [StatueStyle.Drippler] = ItemType.DripplerStatue,
        [StatueStyle.Wraith] = ItemType.WraithStatue,
        [StatueStyle.BoneSkeleton] = ItemType.BoneSkeletonStatue,
        [StatueStyle.UndeadViking] = ItemType.UndeadVikingStatue,
        [StatueStyle.Medusa] = ItemType.MedusaStatue,
        [StatueStyle.Harpy] = ItemType.HarpyStatue,
        [StatueStyle.Pigron] = ItemType.PigronStatue,
        [StatueStyle.Hoplite] = ItemType.HopliteStatue,
        [StatueStyle.GraniteGolem] = ItemType.GraniteGolemStatue,
        [StatueStyle.ZombieArm] = ItemType.ZombieArmStatue,
        [StatueStyle.BloodZombie] = ItemType.BloodZombieStatue,
      };
    });
    public ItemType GetItemTypeFromStatueStyle(StatueStyle statueStyle) {
      ItemType itemType;
      if (!statueStyleItemTypeLookup.Value.TryGetValue(statueStyle, out itemType))
        throw new ArgumentException("StatueStyle");

      return itemType;
    }

    public PressurePlateKind GetPressurePlateKind(int objectStyle) {
      switch (objectStyle) {
        case 2:
        case 3:
        case 4:
        case 6:
          return PressurePlateKind.TriggeredByPlayers;
        case 0:
        case 1:
          return PressurePlateKind.TriggeredByPlayersNpcsEnemies;
        case 5:
          return PressurePlateKind.TriggeredByNpcsEnemies;
        default:
          return PressurePlateKind.Unknown;
      }
    }

    private static readonly Lazy<Tuple<ChestStyle, bool>[]> objectIdChestStyleLockedLookup = new Lazy<Tuple<ChestStyle, bool>[]>(() => {
      return new[] {
        new Tuple<ChestStyle, bool>(ChestStyle.WoodenChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.GoldChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.GoldChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.ShadowChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.ShadowChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.Barrel, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.TrashCan, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.EbonwoodChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.RichMahoganyChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.PearlwoodChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.IvyChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.IceChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.LivingWoodChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.SkywareChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.ShadewoodChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.WebCoveredChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.LihzahrdChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.WaterChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.JungleChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.CorruptionChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.CrimsonChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.HallowedChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.FrozenChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.JungleChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.CorruptionChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.CrimsonChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.HallowedChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.FrozenChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.CrimsonChest, true), 
        new Tuple<ChestStyle, bool>(ChestStyle.FrozenChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.DynastyChest, false), 
        new Tuple<ChestStyle, bool>(ChestStyle.HoneyChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.SteampunkChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.PalmWoodChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.MushroomChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.BorealWoodChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.SlimeChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.GreenDungeonChest, true),
        new Tuple<ChestStyle, bool>(ChestStyle.GreenDungeonChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.PinkDungeonChest, true),
        new Tuple<ChestStyle, bool>(ChestStyle.PinkDungeonChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.BlueDungeonChest, true),
        new Tuple<ChestStyle, bool>(ChestStyle.BlueDungeonChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.BoneChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.CactusChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.FleshChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.ObsidianChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.PumpkinChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.SpookyChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.GlassChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.MartianChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.MeteoriteChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.GraniteChest, false),
        new Tuple<ChestStyle, bool>(ChestStyle.MarbleChest, false),
      };
    });
    public ChestStyle GetChestStyle(int objectStyle, out bool isLocked) {
      if (objectStyle < 0 || objectStyle >= objectIdChestStyleLockedLookup.Value.Length)
        throw new ArgumentOutOfRangeException("objectStyle");

      Tuple<ChestStyle, bool> lookupItem = objectIdChestStyleLockedLookup.Value[objectStyle];
      isLocked = lookupItem.Item2;
      return lookupItem.Item1;
    }

    public ChestStyle GetChestStyle(Tile tile, out bool isLocked) {
      return this.GetChestStyle((tile.frameX / (TerrariaUtils.DefaultTextureTileSize * 2)), out isLocked);
    }

    public bool IsChestLocked(Tile tile) {
      bool isLocked;
      this.GetChestStyle((tile.frameX / (TerrariaUtils.DefaultTextureTileSize * 2)), out isLocked);
      return isLocked;
    }

    private static readonly Lazy<Dictionary<ChestStyle,ItemType>> chestStyleItemTypeLookup = new Lazy<Dictionary<ChestStyle, ItemType>>(() => {
      return new Dictionary<ChestStyle, ItemType>{
        [ChestStyle.WoodenChest] = ItemType.Chest,
        [ChestStyle.GoldChest] = ItemType.GoldChest,
        [ChestStyle.ShadowChest] = ItemType.ShadowChest,
        [ChestStyle.Barrel] = ItemType.Barrel,
        [ChestStyle.EbonwoodChest] = ItemType.EbonwoodChest,
        [ChestStyle.RichMahoganyChest] = ItemType.RichMahoganyChest,
        [ChestStyle.PearlwoodChest] = ItemType.PearlwoodChest,
        [ChestStyle.IvyChest] = ItemType.IvyChest,
        [ChestStyle.IceChest] = ItemType.IceChest,
        [ChestStyle.LivingWoodChest] = ItemType.LivingWoodChest,
        [ChestStyle.SkywareChest] = ItemType.SkywareChest,
        [ChestStyle.ShadewoodChest] = ItemType.ShadewoodChest,
        [ChestStyle.WebCoveredChest] = ItemType.WebCoveredChest,
        [ChestStyle.LihzahrdChest] = ItemType.LihzahrdChest,
        [ChestStyle.WaterChest] = ItemType.WaterChest,
        [ChestStyle.JungleChest] = ItemType.JungleChest,
        [ChestStyle.CorruptionChest] = ItemType.CorruptionChest,
        [ChestStyle.CrimsonChest] = ItemType.CrimsonChest,
        [ChestStyle.HallowedChest] = ItemType.HallowedChest,
        [ChestStyle.FrozenChest] = ItemType.FrozenChest,
        [ChestStyle.DynastyChest] = ItemType.DynastyChest,
        [ChestStyle.HoneyChest] = ItemType.HoneyChest,
        [ChestStyle.SteampunkChest] = ItemType.SteampunkChest,
        [ChestStyle.PalmWoodChest] = ItemType.PalmWoodChest,
        [ChestStyle.MushroomChest] = ItemType.MushroomChest,
        [ChestStyle.BorealWoodChest] = ItemType.BorealWoodChest,
        [ChestStyle.SlimeChest] = ItemType.SlimeChest,
        [ChestStyle.GreenDungeonChest] = ItemType.GreenDungeonChest,
        [ChestStyle.PinkDungeonChest] = ItemType.PinkDungeonChest,
        [ChestStyle.BlueDungeonChest] = ItemType.BlueDungeonChest,
        [ChestStyle.BoneChest] = ItemType.BoneChest,
        [ChestStyle.CactusChest] = ItemType.CactusChest,
        [ChestStyle.FleshChest] = ItemType.FleshChest,
        [ChestStyle.ObsidianChest] = ItemType.ObsidianChest,
        [ChestStyle.PumpkinChest] = ItemType.PumpkinChest,
        [ChestStyle.SpookyChest] = ItemType.SpookyChest,
        [ChestStyle.GlassChest] = ItemType.GlassChest,
        [ChestStyle.MartianChest] = ItemType.MartianChest,
        [ChestStyle.MeteoriteChest] = ItemType.MeteoriteChest,
        [ChestStyle.GraniteChest] = ItemType.GraniteChest,
        [ChestStyle.MarbleChest] = ItemType.MarbleChest,
      };
    });
    public ItemType GetItemTypeFromChestStyle(ChestStyle chestStyle) {
      ItemType itemType;
      if (!chestStyleItemTypeLookup.Value.TryGetValue(chestStyle, out itemType))
        throw new ArgumentException("ChestStyle");
      
      return itemType;
    }

    public void LockChest(DPoint anyChestTileLocation) {
      Tile chestTile = TerrariaUtils.Tiles[anyChestTileLocation];
      if (!chestTile.active() || chestTile.type != (int)BlockType.Chest)
        throw new ArgumentException("Tile is not a chest.", "anyChestTileLocation");

      bool isLocked;
      ChestStyle chestStyle = this.GetChestStyle(chestTile, out isLocked);
      if (isLocked)
        throw new InvalidChestStyleException("Chest is already locked.", chestStyle);

      if (
        chestStyle != ChestStyle.GoldChest &&
        chestStyle != ChestStyle.ShadowChest &&
        chestStyle != ChestStyle.JungleChest &&
        chestStyle != ChestStyle.CorruptionChest &&
        chestStyle != ChestStyle.CrimsonChest &&
        chestStyle != ChestStyle.HallowedChest &&
        chestStyle != ChestStyle.FrozenChest &&
        chestStyle != ChestStyle.BlueDungeonChest &&
        chestStyle != ChestStyle.GreenDungeonChest &&
        chestStyle != ChestStyle.PinkDungeonChest
      )
        throw new InvalidChestStyleException("Chest has to be a lockable chest.", chestStyle);

      ObjectMeasureData measureData = this.MeasureObject(anyChestTileLocation);
      if (
        chestStyle == ChestStyle.JungleChest ||
        chestStyle == ChestStyle.CorruptionChest ||
        chestStyle == ChestStyle.CrimsonChest ||
        chestStyle == ChestStyle.HallowedChest ||
        chestStyle == ChestStyle.FrozenChest)
      {
        foreach (Tile tile in this.EnumerateObjectTiles(measureData))
          tile.frameX += 180;
      }
      else
      {
        foreach (Tile tile in this.EnumerateObjectTiles(measureData))
          tile.frameX += 36;
      }
      
      TSPlayer.All.SendTileSquare(anyChestTileLocation, 4);
    }
 
    public ChestKind GuessChestKind(DPoint anyChestTileLocation) {
      Tile chestTile = TerrariaUtils.Tiles[anyChestTileLocation];
      if (!chestTile.active())
        throw new ArgumentException("The tile on the given location is not active.");

      bool isLocked;
      ChestStyle chestStyle = this.GetChestStyle(chestTile, out isLocked);
      switch (chestStyle) {
        case ChestStyle.GoldChest:
          if (
            chestTile.wall >= (int)WallType.DungeonBlueBrickWall && chestTile.wall <= (int)WallType.DungeonPinkBrickWall ||
            chestTile.wall >= (int)WallType.DungeonBlueSlabWall && chestTile.wall <= (int)WallType.DungeonGreenTiledWall
          )
            return ChestKind.DungeonChest;

          if (chestStyle == ChestStyle.GoldChest && chestTile.wall == (int)WallType.SandstoneBrickWall){
            if (anyChestTileLocation.Y < Main.worldSurface - 250 || anyChestTileLocation.Y > Main.worldSurface + 50)
              return ChestKind.Unknown;
            else
              return ChestKind.PyramidChest;
          }
          
          return ChestKind.Unknown;

        case ChestStyle.WaterChest:
          if (chestTile.liquid < 255 || chestTile.lava())
            return ChestKind.Unknown;
          if (anyChestTileLocation.X > 250 && anyChestTileLocation.X < Main.maxTilesX - 250)
            return ChestKind.Unknown;
          if (anyChestTileLocation.Y < Main.worldSurface - 200 || anyChestTileLocation.Y > Main.worldSurface + 50)
            return ChestKind.Unknown;

          return ChestKind.OceanChest;

        case ChestStyle.FrozenChest:
        case ChestStyle.HallowedChest:
        case ChestStyle.JungleChest:
        case ChestStyle.CrimsonChest:
        case ChestStyle.CorruptionChest:
          if (isLocked) {
            if (
              chestTile.wall >= (int)WallType.DungeonBlueBrickWall && chestTile.wall <= (int)WallType.DungeonPinkBrickWall ||
              chestTile.wall >= (int)WallType.DungeonBlueSlabWall && chestTile.wall <= (int)WallType.DungeonGreenTiledWall
            )
              return ChestKind.HardmodeDungeonChest;
          }
          
          return ChestKind.Unknown;

        case ChestStyle.ShadowChest:
          if (!isLocked)
            return ChestKind.Unknown;
          
          if (anyChestTileLocation.Y < Main.maxTilesY * (1.0 - 0.143))
            return ChestKind.Unknown;

          return ChestKind.HellShadowChest;

        //case ChestStyle.WoodenChest:
        //  if (chestTile.wall >= (int)WallType.DungeonBlueBrickWall && chestTile.wall <= (int)WallType.DungeonPinkBrickWall)
        //    return ChestKind.DungeonChest;

        //  return ChestKind.Unknown;

        case ChestStyle.SkywareChest:
          if (anyChestTileLocation.Y < Main.worldSurface)
            return ChestKind.SkyIslandChest;

          return ChestKind.Unknown;
        default:
          return ChestKind.Unknown;
      }
    }

    public Direction GetDoorDirection(DPoint anyDoorTileLocation) {
      Tile anyDoorTile = TerrariaUtils.Tiles[anyDoorTileLocation];
      if (!anyDoorTile.active())
        throw new ArgumentException("The tile is not active.");
      
      if (anyDoorTile.type == (int)BlockType.DoorClosed
        || anyDoorTile.type == (int)BlockType.TrapdoorClosed)
        return Direction.Unknown;

      if (anyDoorTile.frameX < 36)
        return anyDoorTile.type == (int)BlockType.DoorOpened ? Direction.Right : Direction.Up;
      else
        return anyDoorTile.type == (int)BlockType.DoorOpened ? Direction.Left : Direction.Down;
    }

    public HerbStyle GetHerbStyle(int objectStyle) {
      return (HerbStyle)(objectStyle + 1);
    }

    public HerbStyle GetHerbStyle(Tile tile) {
      return this.GetHerbStyle(tile.frameX / TerrariaUtils.DefaultTextureTileSize);
    }

    public IEnumerable<DPoint> EnumerateObjectTileLocations(ObjectMeasureData measureData) {
      DPoint origin = measureData.OriginTileLocation;

      if (
        (measureData.BlockType == BlockType.DoorOpened &&
        this.GetDoorDirection(measureData.OriginTileLocation) == Direction.Left) ||
        (measureData.BlockType == BlockType.TrapdoorOpen &&
        this.GetDoorDirection(measureData.OriginTileLocation) == Direction.Down )
      ) {
        origin.Offset(-1, 0);
      }

      for (int x = origin.X; x < origin.X + measureData.Size.X; x++)
        for (int y = origin.Y; y < origin.Y + measureData.Size.Y; y++)
          yield return new DPoint(x, y);
    }

    public IEnumerable<Tile> EnumerateObjectTiles(ObjectMeasureData measureData) {
      foreach (DPoint tileLocation in this.EnumerateObjectTileLocations(measureData))
        yield return TerrariaUtils.Tiles[tileLocation];
    }

    public IEnumerable<Tile> EnumerateTilesRectangularAroundPoint(DPoint tileLocation, int rectWidth, int rectHeight) {
      int halfWidth = (rectWidth / 2);
      int halfHeight = (rectHeight / 2);
      for (int x = tileLocation.X - halfWidth; x <= tileLocation.X + halfWidth; x++) {
        for (int y = tileLocation.Y - halfHeight; y <= tileLocation.Y + halfHeight; y++) {
          yield return TerrariaUtils.Tiles[x, y];
        }
      }
    }

    // TODO: update this on new versions
    private static readonly Lazy<DPoint[]> objectSizesLookup = new Lazy<DPoint[]>(() => {
      return new[] {
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
        new DPoint(1, 1), // Adamantite Beam
        new DPoint(1, 1), // Sandstone Brick
        new DPoint(1, 1), // Ebonstone Brick
        new DPoint(1, 1), // Red Stucco
        new DPoint(1, 1), // Blue Stucco
        new DPoint(1, 1), // Green Stucco
        new DPoint(1, 1), // Gray Stucco
        new DPoint(1, 1), // Ebonwood
        new DPoint(1, 1), // Rich Mahogany
        new DPoint(1, 1), // Pearlwood
        new DPoint(1, 1), // Rainbow Brick
        new DPoint(1, 1), // Ice Block
        new DPoint(1, 1), // 
        new DPoint(1, 1), // Purple Ice Block
        new DPoint(1, 1), // Pink Ice Block
        new DPoint(1, 1), // !Different sizes
        new DPoint(1, 1), // Tin
        new DPoint(1, 1), // Lead
        new DPoint(1, 1), // Tungsten
        new DPoint(1, 1), // Platinum
        new DPoint(3, 3), // Tin Chandelier
        new DPoint(3, 3), // Tungsten Chandelier
        new DPoint(3, 3), // Platinum Chandelier
        new DPoint(2, 2), // Candelabra
        new DPoint(1, 1), // Platinum Candle
        new DPoint(1, 1), // Tin Brick
        new DPoint(1, 1), // Tungsten Brick
        new DPoint(1, 1), // Platinum Brick
        new DPoint(1, 1), // Amber
        new DPoint(1, 1), // 
        new DPoint(1, 1), // 180
        new DPoint(1, 1), // 
        new DPoint(1, 1), // 
        new DPoint(1, 1), // 
        new DPoint(1, 1), // 
        new DPoint(1, 1), // 
        new DPoint(3, 2), // 
        new DPoint(3, 2), // 187
        new DPoint(1, 1), // Cactus
        new DPoint(1, 1), // Cloud
        new DPoint(1, 1), // Glowing Mushroom
        new DPoint(1, 1), // Living Wood Wand
        new DPoint(1, 1), // Leaf
        new DPoint(1, 1), // Slime Block
        new DPoint(1, 1), // Bone Wand
        new DPoint(1, 1), // Flesh Block
        new DPoint(1, 1), // Rain Cloud
        new DPoint(1, 1), // Frozen Slime Block
        new DPoint(1, 1), // Asphalt Block
        new DPoint(1, 1), // CrimsonGrass
        new DPoint(1, 1), // Red Ice Block
        new DPoint(1, 1), // 
        new DPoint(1, 1), // Sunplate Block
        new DPoint(1, 1), // Crimstone Block
        new DPoint(1, 1), // Crimtane Ore
        new DPoint(1, 1), // 
        new DPoint(1, 1), // Ice Brick
        new DPoint(2, 4), // Water Fountain
        new DPoint(1, 1), // Shadewood
        new DPoint(4, 3), // Cannon
        new DPoint(1, 1), // Land Mine
        new DPoint(1, 1), // Chlorophyte
        new DPoint(3, 3), // Turret
        new DPoint(1, 1), // Rope
        new DPoint(1, 1), // Chain
        new DPoint(3, 2), // Campfire
        new DPoint(1, 2), // Rocket
        new DPoint(3, 2), // Blend-O-Matic
        new DPoint(3, 2), // Meat Grinder
        new DPoint(3, 3), // Silt Extractinator
        new DPoint(3, 3), // Solidifier
        new DPoint(1, 1), // Palladium
        new DPoint(1, 1), // Orichalcum
        new DPoint(1, 1), // Titanium
        new DPoint(1, 1), // Slush Block
        new DPoint(1, 1), // Hive
        new DPoint(1, 1), // Lihzahrd Brick
        new DPoint(1, 1), // Orange Bloodroot
        new DPoint(3, 3), // Dye Vat
        new DPoint(1, 1), // Honey Block
        new DPoint(1, 1), // Crispy Honey Block
        new DPoint(3, 3), // Larva
        new DPoint(1, 1), // Wooden Spike
        new DPoint(3, 3), // !Different sizes!
        new DPoint(1, 1), // Crimsand Block
        new DPoint(3, 1), // Teleporter
        new DPoint(2, 2), // Life Fruit
        new DPoint(3, 2), // Lihzahrd Altar
        new DPoint(2, 2), // Plantera's Bulb
        new DPoint(1, 1), // Metal Bar
        new DPoint(3, 3), // Picture
        new DPoint(4, 3), // Picture
        new DPoint(6, 4), // Picture
        new DPoint(3, 3), // Imbuing Station
        new DPoint(3, 3), // Bubble Machine
        new DPoint(2, 3), // Picture
        new DPoint(3, 2), // Picture
        new DPoint(3, 3), // Autohammer
        new DPoint(1, 1), // Palladium Cloumn
        new DPoint(1, 1), // Bubblegum Block
        new DPoint(1, 1), // Titanstone Block
        new DPoint(1, 1), // PumpkinBlock
        new DPoint(1, 1), // Hay
        new DPoint(1, 1), // Spooky Wood
        new DPoint(2, 2), // Pumpkin
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // ?
        new DPoint(1, 1), // AmethystGemsparkBlock
        new DPoint(1, 1), // TopazGemsparkBlock
        new DPoint(1, 1), // SapphireGemsparkBlock
        new DPoint(1, 1), // EmeraldGemsparkBlock
        new DPoint(1, 1), // RubyGemsparkBlock
        new DPoint(1, 1), // DiamondGemsparkBlock
        new DPoint(1, 1), // AmberGemsparkBlock
        new DPoint(2, 3), // Womannequin
        new DPoint(1, 2), // FireflyinaBottle
        new DPoint(1, 2), // LightningBugInaBottle
        new DPoint(1, 1), // Cog
        new DPoint(1, 1), // StoneSlab
        new DPoint(1, 1), // SandstoneSlab
        new DPoint(6, 3), // BunnyCage
        new DPoint(6, 3), // SquirrelCage
        new DPoint(6, 3), // MallardDuckCage
        new DPoint(6, 3), // DuckCage
        new DPoint(6, 3), // BirdCage
        new DPoint(6, 3), // BlueJayCage
        new DPoint(6, 3), // CardinalCage
        new DPoint(2, 2), // FishBowl
        new DPoint(3, 3), // HeavyWorkBench
        new DPoint(1, 1), // CopperPlating
        new DPoint(3, 2), // SnailCage
        new DPoint(3, 2), // GlowingSnailCage
        new DPoint(2, 2), // AmmoBox
        new DPoint(2, 2), // MonarchButterflyJar
        new DPoint(2, 2), // PurpleEmperorButterflyJar
        new DPoint(2, 2), // RedAdmiralButterflyJar
        new DPoint(2, 2), // UlyssesButterflyJar
        new DPoint(2, 2), // SulphurButterflyJar
        new DPoint(2, 2), // TreeNymphButterflyJar
        new DPoint(2, 2), // ZebraSwallowtailButterflyJar
        new DPoint(2, 2), // JuliaButterflyJar
        new DPoint(6, 3), // ScorpionCage
        new DPoint(6, 3), // BlackScorpionCage
        new DPoint(3, 2), // FrogCage
        new DPoint(3, 2), // MouseCage
        new DPoint(3, 3), // BoneWelder
        new DPoint(3, 3), // FleshCloningVaat
        new DPoint(3, 3), // GlassKiln
        new DPoint(3, 3), // LihzahrdFurnace
        new DPoint(3, 3), // LivingLoom
        new DPoint(3, 3), // SkyMill
        new DPoint(3, 3), // IceMachine
        new DPoint(3, 3), // SteampunkBoiler
        new DPoint(3, 3), // HoneyDispenser
        new DPoint(6, 3), // PenguinCage
        new DPoint(3, 2), // WormCage
        new DPoint(1, 1), // DynastyWood
        new DPoint(1, 1), // RedDynastyShingles
        new DPoint(1, 1), // BlueDynastyShingles
        new DPoint(1, 1), // MinecartTrack
        new DPoint(1, 1), // CoralstoneBlock
        new DPoint(2, 2), // BlueJellyfishBowl
        new DPoint(2, 2), // GreenJellyfishBowl
        new DPoint(2, 2), // PinkJellyfishBowl
        new DPoint(2, 2), // ShipInABottle
        new DPoint(2, 3), // SeaweedPlanter
        new DPoint(1, 1), // BorealWood
        new DPoint(1, 1), // PalmWood
        new DPoint(1, 1), // PalmTree
        new DPoint(1, 1), // BeachPiles
        new DPoint(1, 1), // TinPlating
        new DPoint(1, 1), // Waterfall
        new DPoint(1, 1), // Lavafall
        new DPoint(1, 1), // Confetti
        new DPoint(1, 1), // ConfettiBlack
        new DPoint(1, 1), // CopperCoinPile
        new DPoint(1, 1), // SilverCoinPile
        new DPoint(1, 1), // GoldCoinPile
        new DPoint(1, 1), // PlatinumCoinPile
        new DPoint(3, 3), // WeaponsRack
        new DPoint(2, 2), // FireworksBox
        new DPoint(1, 1), // LivingFire
        new DPoint(2, 3), // AlphabetStatues
        new DPoint(1, 2), // FireworkFountain
        new DPoint(3, 2), // GrasshopperCage
        new DPoint(1, 1), // LivingCursedFire
        new DPoint(1, 1), // LivingDemonFire
        new DPoint(1, 1), // LivingFrostFire
        new DPoint(1, 1), // LivingIchor
        new DPoint(1, 1), // LivingUltrabrightFire
        new DPoint(1, 1), // Honeyfall
        new DPoint(1, 1), // ChlorophyteBrick
        new DPoint(1, 1), // CrimtaneBrick
        new DPoint(1, 1), // ShroomitePlating
        new DPoint(2, 3), // MushroomStatue
        new DPoint(1, 1), // MartianConduitPlating
        new DPoint(1, 1), // ChimneySmoke
        new DPoint(1, 1), // CrimtaneThorns
        new DPoint(1, 1), // VineRope
        new DPoint(3, 2), // BewitchingTable
        new DPoint(3, 2), // AlchemyTable
        new DPoint(2, 3), // Sundial
        new DPoint(1, 1), // MarbleBlock
        new DPoint(6, 3), // GoldBirdCage
        new DPoint(6, 3), // GoldBunnyCage
        new DPoint(2, 2), // GoldButterflyCage
        new DPoint(3, 2), // GoldFrogCage
        new DPoint(3, 2), // GoldGrasshopperCage
        new DPoint(3, 2), // GoldMouseCage
        new DPoint(3, 2), // GoldWormCage
        new DPoint(1, 1), // SilkRope
        new DPoint(1, 1), // WebRope
        new DPoint(1, 1), // Marble
        new DPoint(1, 1), // Granite
        new DPoint(1, 1), // GraniteBlock
        new DPoint(1, 1), // MeteoriteBrick
        new DPoint(1, 1), // PinkSlimeBlock
        new DPoint(1, 1), // PeaceCandle
        new DPoint(1, 1), // WaterDrip
        new DPoint(1, 1), // LavaDrip
        new DPoint(1, 1), // HoneyDrip
        new DPoint(2, 2), // FishingCrate
        new DPoint(3, 2), // SharpeningStation
        new DPoint(2, 3), // TargetDummy
        new DPoint(1, 1), // Bubble
        new DPoint(1, 1), // PlanterBox
        new DPoint(1, 1), // LavaMoss
        new DPoint(1, 1), // VineFlowers
        new DPoint(1, 1), // LivingMahogany
        new DPoint(1, 1), // LivingMahoganyLeaves
        new DPoint(1, 1), // CrystalBlock
        new DPoint(2, 2), // TrapdoorOpen
        new DPoint(2, 1), // TrapdoorClosed
        new DPoint(1, 5), // TallGateClosed
        new DPoint(1, 5), // TallGateOpen
        new DPoint(1, 2), // LavaLamp
        new DPoint(3, 2), // CageEnchantedNightcrawler
        new DPoint(3, 2), // CageBuggy
        new DPoint(3, 2), // CageGrubby
        new DPoint(3, 2), // CageSluggy
        new DPoint(2, 2), // ItemFrame
        new DPoint(1, 1), // Sandstone
        new DPoint(1, 1), // HardenedSand
        new DPoint(1, 1), // CorruptHardenedSand
        new DPoint(1, 1), // CrimsonHardenedSand
        new DPoint(1, 1), // CorruptSandstone
        new DPoint(1, 1), // CrimsonSandstone
        new DPoint(1, 1), // HallowHardenedSand
        new DPoint(1, 1), // HallowSandstone
        new DPoint(1, 1), // DesertFossil
        new DPoint(3, 2), // Fireplace
        new DPoint(3, 2), // Chimney
        new DPoint(1, 1), // FossilOre
        new DPoint(1, 1), // LunarOre
        new DPoint(1, 1), // LunarBrick
        new DPoint(2, 3), // LunarMonolith
        new DPoint(2, 2), // Detonator
        new DPoint(3, 3), // LunarCraftingStation
        new DPoint(6, 3), // SquirrelOrangeCage
        new DPoint(6, 3), // SquirrelGoldCage
        new DPoint(1, 1), // LunarBlockSolar
        new DPoint(1, 1), // LunarBlockVortex
        new DPoint(1, 1), // LunarBlockNebula
        new DPoint(1, 1), // LunarBlockStardust
        // 1.3.1
        new DPoint(1, 1), // LogicGateLamp = 419
        new DPoint(1, 1), // LogicGate = 420
        new DPoint(1, 1), // ConveyorBeltLeft = 421
        new DPoint(1, 1), // ConveyorBeltRight = 422
        new DPoint(1, 1), // LogicSensor = 423
        new DPoint(1, 1), // WirePipe = 424
        new DPoint(2, 2), // AnnouncementBox = 425
        new DPoint(1, 1), // TeamBlockRed = 426
        new DPoint(1, 1), // TeamBlockRedPlatform = 427
        new DPoint(1, 1), // WeightedPressurePlate = 428
        new DPoint(1, 1), // WireBulb = 429
        new DPoint(1, 1), // TeamBlockGreen = 430
        new DPoint(1, 1), // TeamBlockBlue = 431
        new DPoint(1, 1), // TeamBlockYellow = 432
        new DPoint(1, 1), // TeamBlockPink = 433
        new DPoint(1, 1), // TeamBlockWhite = 434
        new DPoint(1, 1), // TeamBlockGreenPlatform = 435
        new DPoint(1, 1), // TeamBlockBluePlatform = 436
        new DPoint(1, 1), // TeamBlockYellowPlatform = 437
        new DPoint(1, 1), // TeamBlockPinkPlatform = 438
        new DPoint(1, 1), // TeamBlockWhitePlatform = 439
        new DPoint(3, 3), // GemLocks = 440
        new DPoint(2, 2), // FakeContainers = 441
        new DPoint(1, 1), // ProjectilePressurePad = 442
        new DPoint(2, 3), // GeyserTrap = 443
        new DPoint(2, 2), // BeeHive = 444
        new DPoint(1, 1), // PixelBox = 445
        // 1.3.3
        new DPoint(1, 1), // SillyBalloonPink = 446
        new DPoint(1, 1), // SillyBalloonPurple = 447
        new DPoint(1, 1), // SillyBalloonGreen = 448
        new DPoint(1, 1), // SillyStreamerBlue = 449
        new DPoint(1, 1), // SillyStreamerGreen = 450
        new DPoint(1, 1), // SillyStreamerPink = 451
        new DPoint(1, 1), // SillyBalloonMachine = 452
        new DPoint(1, 1), // SillyBalloonTile = 453
        new DPoint(4, 3), // Pigronata = 454
        new DPoint(1, 1), // PartyMonolith = 455
        new DPoint(1, 1), // PartyBundleOfBalloonTile = 456
        new DPoint(1, 1), // PartyPresent = 457
        new DPoint(1, 1), // SandFallBlock = 458
        new DPoint(1, 1), // SnowFallBlock = 459
        new DPoint(1, 1), // SnowCloud = 460
        new DPoint(1, 1), // SandDrip = 461
        new DPoint(2, 1), // DjinnLamp = 462
        // 1.3.4
        new DPoint(3, 4), // DefendersForge = 463
        new DPoint(5, 4), // WarTable = 464
        new DPoint(2, 3), // WarTableBanner = 465
        new DPoint(5, 3)  // ElderCrystalStand = 466
      };
    });
    public DPoint GetObjectSize(BlockType objectType) {
      if (objectType < 0 || (int)objectType >= TerrariaTiles.objectSizesLookup.Value.Length)
        return new DPoint(1, 1);

      return TerrariaTiles.objectSizesLookup.Value[(int)objectType];
    }

    public DPoint GetBlockTextureTileSize(BlockType blockType) {
      switch (blockType) {
        case BlockType.Tree:
        case BlockType.Torch:
          return new DPoint(22, 22);
        case BlockType.GrassPlants:
        case BlockType.CorruptionPlants:
        case BlockType.Candle:
        case BlockType.WaterCandle:
        case BlockType.JunglePlants:
        case BlockType.GlowingMushroom:
        case BlockType.HerbGrowing:
        case BlockType.HerbMature:
        case BlockType.HerbBlooming:
        case BlockType.HallowedPlants:
        case BlockType.Undefined13:
          return new DPoint(18, 20);
        case BlockType.Coral:
          return new DPoint(26, 28);
        case BlockType.PressurePlate:
          return new DPoint(16, 16);
        case BlockType.TallGrassPlants:
        case BlockType.TallJunglePlants:
        case BlockType.TallHallowedPlants:
          return new DPoint(18, 16);
        case BlockType.Rocket:
          return new DPoint(19, 18);
        case BlockType.Undefined8:
          return new DPoint(22, 18);
        default:
          return new DPoint(TerrariaUtils.DefaultTextureTileSize, TerrariaUtils.DefaultTextureTileSize);
      }
    }

    public Direction GetObjectOrientation(Tile anyTile) {
      if (!anyTile.active())
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
        case BlockType.Undefined2:
          if (
            (anyTile.frameX >= 54 && anyTile.frameX <= 90) &&
            (anyTile.frameY == 36 || anyTile.frameY == 54 || anyTile.frameY == 90)
          ) {
            return Direction.Up;
          }
          if (anyTile.frameX >= 162 && anyTile.frameX <= 198 && anyTile.frameY == 90)
            return Direction.Up;

          return Direction.Down;
        case BlockType.Amber:
        case BlockType.Undefined8:
          if (anyTile.frameY >= 162)
            return Direction.Left;

          if (anyTile.frameY >= 108)
            return Direction.Right;

          if (anyTile.frameY >= 54)
            return Direction.Down;

          return Direction.Up;
        default:
          return Direction.Unknown;
      }
    }

    [Conditional("DEBUG")]
    public void DebugPrintCreatedTileIdList() {
      for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
        Item dummyItem = new Item();
        dummyItem.netDefaults(i);

        if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.createTile != -1) { 
          string itemName = dummyItem.name;
          itemName = itemName.Replace(" ", "");
          itemName = itemName.Replace("'", "");
          itemName = itemName.Replace("´", "");
          itemName = itemName.Replace("`", "");

          Debug.Print(itemName + " = " + dummyItem.createTile + ',');
        }
      }
    }

    [Conditional("DEBUG")]
    public void DebugPrintCreatedWallIdList() {
      for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
        Item dummyItem = new Item();
        dummyItem.netDefaults(i);

        if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.createWall != -1) { 
          string itemName = dummyItem.name;
          itemName = itemName.Replace(" ", "");
          itemName = itemName.Replace("'", "");
          itemName = itemName.Replace("´", "");
          itemName = itemName.Replace("`", "");

          Debug.Print(itemName + " = " + dummyItem.createWall + ',');
        }
      }
    }
  }
}
