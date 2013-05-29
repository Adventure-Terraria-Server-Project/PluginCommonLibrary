using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaNpcs {
    #region [Methods: EnumerateNPCsAroundPoint]
    public IEnumerable<NPC> EnumerateNPCsAroundPoint(DPoint location, float radius) {
      for (int i = 0; i < 200; i++) {
        NPC npc = Main.npc[i];

        if (
          npc.active && Math.Sqrt(Math.Pow(npc.position.X - location.X, 2) + Math.Pow(npc.position.Y - location.Y, 2)) <= radius
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

    public IEnumerable<int> EnumerateSpecificNPCIndexes(params int[] npcTypes) {
      return this.EnumerateSpecificNPCIndexes((IList<int>)npcTypes);
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

    #region [Methods: Move, MoveOrSpawnSpecificType]
    public void MoveOrSpawnSpecificType(int npcType, DPoint location) {
      Contract.Requires<ArgumentOutOfRangeException>(npcType >= TerrariaUtils.NpcType_Min && npcType <= TerrariaUtils.NpcType_Max);

      foreach (int npcIndex in Common.TerrariaUtils.Npcs.EnumerateSpecificNPCIndexes(npcType)) {
        this.Move(npcIndex, location);
        return;
      }

      // Spawn required.
      NPC.NewNPC(location.X, location.Y, npcType);
    }

    public void Move(int npcIndex, DPoint location) {
      Contract.Requires<ArgumentOutOfRangeException>(npcIndex >= 0 && npcIndex < Main.npc.Length);

      NPC npc = Main.npc[npcIndex];
      if (npc == null)
        throw new ArgumentException("Invalid npc index.", "npcIndex");

      npc.position.X = (location.X - (Main.npc[npcIndex].width / 2));
      npc.position.Y = (location.Y - (Main.npc[npcIndex].height - 1));
      TSPlayer.All.SendData(PacketTypes.NpcUpdate, string.Empty, npcIndex);
    }
    #endregion
  }
}
