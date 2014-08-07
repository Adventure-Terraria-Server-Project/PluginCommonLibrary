﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaNpcs {
    public IEnumerable<NPC> EnumerateNPCsAroundPoint(DPoint location, float radius) {
      for (int i = 0; i < Main.npc.Length; i++) {
        NPC npc = Main.npc[i];

        if (
          npc.active && Math.Sqrt(Math.Pow(npc.position.X - location.X, 2) + Math.Pow(npc.position.Y - location.Y, 2)) <= radius
        ) {
          yield return npc;
        }
      }
    }

    public IEnumerable<int> EnumerateSpecificNPCIndexes(IList<int> npcTypes) {
      int foundNpcsCount = 0;
      for (int i = 0; i < Main.npc.Length; i++) {
        NPC npc = Main.npc[i];

        if (!npc.active || !npc.friendly)
          continue;

        if (npcTypes.Contains(npc.type)) {
          yield return i;
          foundNpcsCount++;
          if (foundNpcsCount == 18 || npcTypes.Count == 1)
            yield break;
        }
      }
    }

    public IEnumerable<int> EnumerateSpecificNPCIndexes(params int[] npcTypes) {
      return this.EnumerateSpecificNPCIndexes((IList<int>)npcTypes);
    }

    public IEnumerable<int> EnumerateFriendlyNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 22, 38, 54, 107, 108, 124, 160 ,178, 207, 208, 209, 227, 228, 229, 368, 369});
    }

    public IEnumerable<int> EnumerateFriendlyFemaleNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 18, 20, 124, 178, 208});
    }

    public IEnumerable<int> EnumerateFriendlyMaleNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 19, 22, 38, 54, 107, 108, 160, 207, 209, 227, 228, 229, 368, 369});
    }

    public IEnumerable<int> EnumerateShopNPCIndexes() {
      return this.EnumerateSpecificNPCIndexes(new List<int> { 17, 18, 19, 20, 38, 54, 107, 108, 124, 160, 178, 207, 208, 209, 227, 228, 229, 368});
    }

    public bool Spawn(int npcType, DPoint location, out int npcIndex, int lifeOverride = 0, int valueOverride = -1, bool noDrops = false) {
      // Thread static.
      if (Main.rand == null)
        Main.rand = new Random();

      npcIndex = NPC.NewNPC(location.X, location.Y, npcType);
      if (npcIndex == Main.npc.Length) {
        npcIndex = -1;
        return false;
      }

      NPC npc = Main.npc[npcIndex];
      if (npcType < 0)
        npc.netDefaults(npcType);

      if (lifeOverride > 0) {
          if (npc.lifeMax < 128)
              lifeOverride = 128;
          else if (lifeOverride > 32768)
              lifeOverride = 32768;
          
          npc.life = lifeOverride;
          npc.lifeMax = lifeOverride;
      }

      if (noDrops) {
          npc.value = 0f;
          npc.npcSlots = 0f;
      }

      if (valueOverride > -1)
          npc.value = valueOverride;

      npc.UpdateNPC(npcIndex);
      TSPlayer.All.SendData(PacketTypes.NpcUpdate, string.Empty, npcIndex);
      return true;
    }

    public bool Spawn(int npcType, DPoint location, int lifeOverride = 0, int valueOverride = -1, bool noDrops = false) {
      int npcIndex;
      return this.Spawn(npcType, location, out npcIndex, lifeOverride, valueOverride, noDrops);
    }

    public void MoveOrSpawnSpecificType(int npcType, DPoint location) {
      Contract.Requires<ArgumentOutOfRangeException>(npcType >= TerrariaUtils.NpcType_Min && npcType <= TerrariaUtils.NpcType_Max);

      foreach (int npcIndex in Common.TerrariaUtils.Npcs.EnumerateSpecificNPCIndexes(npcType)) {
        this.Move(npcIndex, location);
        return;
      }

      TerrariaUtils.Npcs.Spawn(npcType, location);
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
  }
}
