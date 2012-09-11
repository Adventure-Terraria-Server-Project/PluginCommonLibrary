// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public static class TShockEx {
    public static TSPlayer GetPlayerByName(
      string userAccountName, StringComparison stringComparison = StringComparison.InvariantCulture
    ) {
      foreach (TSPlayer tsPlayer in TShock.Players) {
        if (tsPlayer.UserAccountName.Equals(userAccountName, stringComparison))
          return tsPlayer;
      }

      return null;
    }

    public static void SendTileSquareEx(this TSPlayer player, int x, int y, int size = 10) {
      player.SendData(PacketTypes.TileSendSquare, string.Empty, size, x, y);
    }
  }
}
