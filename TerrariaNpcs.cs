using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaNpcs {
    #region [Methods: EnumerateNPCsInTileRange, CountItemsInTileRange]
    public IEnumerable<NPC> EnumerateNPCsInRange(Vector2 location, float range) {
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

    #region [Methods: EnumerateSpecificNPCIndexes, EnumerateFriendlyNPCIndexes, EnumerateFriendlyFemaleNPCIndexes, EnumerateFriendlyMaleNPCIndexes]
    public IEnumerable<int> EnumerateSpecificNPCIndexes(IList<int> npcTypes) {
      int foundNpcsCount = 0;
      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (!npc.active || !npc.friendly)
          continue;

        if (npcTypes.Contains(npc.type)) {
          yield return i;
          foundNpcsCount++;
          if (foundNpcsCount == 10 || npcTypes.Count == 1)
            yield break;
        }
      }
    }

    public IEnumerable<int> EnumerateFriendlyNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 22, 38, 54, 107, 108, 124 });
    }

    public IEnumerable<int> EnumerateFriendlyFemaleNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 18, 20, 124 });
    }

    public IEnumerable<int> EnumerateFriendlyMaleNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 19, 22, 38, 54, 107, 108 });
    }

    public IEnumerable<int> EnumerateShopNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 38, 54, 107, 108, 124 });
    }
    #endregion
  }
}
