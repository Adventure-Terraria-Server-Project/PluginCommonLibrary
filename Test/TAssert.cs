// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow.Test {
  public static class TAssert {
    #region [Methods: IsSpriteActive, IsSpriteInactive]
    public static void IsSpriteActive(int x, int y) {
      TAssert.IsSpriteActive(x, y, true);
    }

    public static void IsSpriteInactive(int x, int y) {
      TAssert.IsSpriteActive(x, y, false);
    }

    public static void IsSpriteActive(int x, int y, bool expectedState) {
      Tile tile = Terraria.Tiles[x, y];
      if (!tile.active) {
        throw new AssertException(
          string.Format("Assert failed. There is no tile at [{0},{1}].", x, y)
        );
      }

      Terraria.SpriteMeasureData measureData = Terraria.MeasureSprite(new DPoint(x, y));
      bool isActive = Terraria.HasSpriteActiveFrame(measureData);
      if (isActive != expectedState) {
        string actualStateString;
        if (isActive)
          actualStateString = "active";
        else 
          actualStateString = "inactive";

        string expectedStateString;
        if (expectedState)
          expectedStateString = "Active";
        else
          expectedStateString = "Inactive";

        throw new AssertException(string.Format(
          "Assert failed. {0} frame for sprite \"{1}\" at [{2},{3}] was expected, but it is {4}.", 
          expectedStateString, Terraria.GetTileName(tile.type), x, y, actualStateString
        ));
      }
    }
    #endregion

    #region [Methods: IsTileId, HasLiquid, HasNoLiquid, HasFullLiquid, HasNotFullLiquid]
    public static void IsTileId(int x, int y, int expectedTileId) {
      Tile tile = Terraria.Tiles[x, y];
      
      if (!tile.active) {
        throw new AssertException(string.Format(
          "The tile id \"{0}\" was expected at [{1},{2}], but there is no tile at all.",
          Terraria.GetTileName(expectedTileId), x, y
        ));
      }

      if (tile.type != expectedTileId) {
        throw new AssertException(string.Format(
          "The tile id \"{0}\" was expected at [{1},{2}], but it is \"{3}\".",
          Terraria.GetTileName(expectedTileId), x, y, Terraria.GetTileName(tile.type)
        ));
      }
    }

    public static void HasLiquid(int x, int y) {
      Tile tile = Terraria.Tiles[x, y];
      
      if (tile.liquid <= 0) {
        throw new AssertException(string.Format(
          "The tile at [{0},{1}] was expected to have liquid, but it doesn't.", x, y
        ));
      }
    }

    public static void HasNoLiquid(int x, int y) {
      Tile tile = Terraria.Tiles[x, y];
      
      if (tile.liquid == 0) {
        throw new AssertException(string.Format(
          "The tile at [{0},{1}] was expected to have no liquid, but it has.", x, y
        ));
      }
    }

    public static void HasFullLiquid(int x, int y) {
      Tile tile = Terraria.Tiles[x, y];
      
      if (tile.liquid < 255) {
        throw new AssertException(string.Format(
          "The tile at [{0},{1}] was expected to have 255 liquid, but it has {2}.", x, y, tile.liquid
        ));
      }
    }

    public static void HasNotFullLiquid(int x, int y) {
      Tile tile = Terraria.Tiles[x, y];
      
      if (tile.liquid == 0) {
        throw new AssertException(string.Format(
          "The tile at [{0},{1}] was expected to have at least 1 liquid, but it has none.", x, y
        ));
      }

      if (tile.liquid == 255) {
        throw new AssertException(string.Format(
          "The tile at [{0},{1}] was expected to have less than 255 liquid, but it has {2}.", x, y, tile.liquid
        ));
      }
    }
    #endregion

    #region [Methods: AreItemsInBlockRect, AreNPCsInBlockRect]
    public static void AreItemsInBlockRect(
      int tileX, int tileY, int tileW, int tileH, int expectedItemId, int expectedCount, bool allowOtherNpcs = false
    ) {
      int x = tileX * Terraria.TileSize;
      int y = tileY * Terraria.TileSize;
      int r = x + (tileW * Terraria.TileSize);
      int b = y + (tileH * Terraria.TileSize);
      int count = 0;

      bool ofExpectedId = false;
      bool ofOtherIds = false;
      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (
          item.active && 
          item.position.X >= x && item.position.X <= r &&
          item.position.Y >= y && item.position.Y <= b
        ) {
          if (item.type == expectedItemId) {
            if (!ofExpectedId)
              ofExpectedId = true;

            count++;
          } else {
            if (!ofOtherIds)
              ofOtherIds = true;
          }
        }
      }

      if (ofExpectedId) {
        if (count != expectedCount) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] contains {4} of the items, {5} were expected though.",
            tileX, tileY, tileW, tileH, expectedCount, count
          ));
        }

        if (ofOtherIds && !allowOtherNpcs) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] contains {4} of the items, though it contains other items aswell.",
            tileX, tileY, tileW, tileH, expectedCount
          ));
        }
      } else {
        if (ofOtherIds) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] does not contain the expected item. There are items of other types though.",
            tileX, tileY, tileW, tileH
          ));
        } else {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] does not contain the expected item.",
            tileX, tileY, tileW, tileH
          ));
        }
      }
    }

    public static void AreNPCsInBlockRect(
      int tileX, int tileY, int tileW, int tileH, int expectedNPCId, int expectedCount, bool allowOtherItems = false
    ) {
      int x = tileX * Terraria.TileSize;
      int y = tileY * Terraria.TileSize;
      int r = x + (tileW * Terraria.TileSize);
      int b = y + (tileH * Terraria.TileSize);
      int count = 0;

      bool ofExpectedId = false;
      bool ofOtherIds = false;
      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (
          npc.active && 
          npc.position.X >= x && npc.position.X <= r &&
          npc.position.Y >= y && npc.position.Y <= b
        ) {
          if (npc.type == expectedNPCId) {
            if (!ofExpectedId)
              ofExpectedId = true;

            count++;
          } else {
            if (!ofOtherIds)
              ofOtherIds = true;
          }
        }
      }

      if (ofExpectedId) {
        if (count != expectedCount) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] contains {4} of the npcs, {5} were expected though.",
            tileX, tileY, tileW, tileH, expectedCount, count
          ));
        }

        if (ofOtherIds && !allowOtherItems) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] contains {4} of the npcs, though it contains other npcs aswell.",
            tileX, tileY, tileW, tileH, expectedCount
          ));
        }
      } else {
        if (ofOtherIds) {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] does not contain the expected npcs. There are npcs of other types though.",
            tileX, tileY, tileW, tileH
          ));
        } else {
          throw new AssertException(string.Format(
            "The block rectangle [{0},{1},{2},{3}] does not contain the expected npcs.",
            tileX, tileY, tileW, tileH
          ));
        }
      }
    }
    #endregion

    #region [Methods: ExpectException, Fail]
    public static bool ExpectException(Action action, Type expectedException) {
      try {
        action();
      } catch (Exception ex) {
        if (ex.GetType() == expectedException)
          return true;

        throw new AssertException(string.Format(
          "Asser failed. Expected exception \"{0}\" was not thrown, \"{1}\" was thrown instead.", 
          expectedException.Name, ex.GetType().Name
        ), ex);
      }

      throw new AssertException(string.Format("Asser failed. Expected exception \"{0}\" was not thrown.", expectedException.Name));
    }

    public static void Fail(string message, params object[] args) {
      throw new AssertException(string.Format(message, args));
    }
    #endregion
  }
}
