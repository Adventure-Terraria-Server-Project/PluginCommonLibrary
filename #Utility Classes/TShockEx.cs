// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using TShockAPI.DB;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public static class TShockEx {
    #region [Method: Static GetUserAccountNameByPlayerName, GetUserIdByPlayerName, GetUserByPlayerName]
    public static bool GetUserAccountNameByPlayerName(string playerName, out string exactName, TSPlayer messagesReceiver = null) {
      exactName = null;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        List<TSPlayer> players = TShock.Utils.FindPlayer(playerName);
        if (players.Count == 0) {
          messagesReceiver.SendErrorMessage(string.Format("The player \"{0}\" does not exist.", playerName));
          return false;
        } if (players.Count > 1) {
          if (messagesReceiver != null) {
            string str = string.Empty;
            foreach (TSPlayer tsPlayer in players)
              str = str.Length == 0 ? str + tsPlayer.Name : str + ", " + tsPlayer.Name;

            messagesReceiver.SendErrorMessage("More than one player matched! Matches: " + str);
          }
          return false;
        }

        exactName = players[0].UserAccountName;
      } else {
        exactName = tsUser.Name;
      }

      return true;
    }

    public static bool GetUserIdByPlayerName(string playerName, out int userId, TSPlayer messagesReceiver = null) {
      userId = -1;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        List<TSPlayer> players = TShock.Utils.FindPlayer(playerName);
        if (players.Count == 0) {
          messagesReceiver.SendErrorMessage(string.Format("The player \"{0}\" does not exist.", playerName));
          return false;
        } if (players.Count > 1) {
          if (messagesReceiver != null) {
            string str = string.Empty;
            foreach (TSPlayer tsPlayer in players)
              str = str.Length == 0 ? str + tsPlayer.Name : str + ", " + tsPlayer.Name;

            messagesReceiver.SendErrorMessage("More than one player matched! Matches: " + str);
          }
          return false;
        }

        userId = players[0].UserID;
      } else {
        userId = tsUser.ID;
      }

      return true;
    }

    public static bool GetUserByPlayerName(string playerName, out TShockAPI.DB.User user, TSPlayer messagesReceiver = null) {
      user = null;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        List<TSPlayer> players = TShock.Utils.FindPlayer(playerName);
        if (players.Count == 0) {
          messagesReceiver.SendErrorMessage(string.Format("The player \"{0}\" does not exist.", playerName));
          return false;
        } if (players.Count > 1) {
          if (messagesReceiver != null) {
            string str = string.Empty;
            foreach (TSPlayer tsPlayer in players)
              str = str.Length == 0 ? str + tsPlayer.Name : str + ", " + tsPlayer.Name;

            messagesReceiver.SendErrorMessage("More than one player matched! Matches: " + str);
          }
          return false;
        }

        user = TShock.Users.GetUserByID(players[0].UserID);
      } else {
        user = tsUser;
      }

      return true;
    }
    #endregion

    #region [TSPlayer Extensions]
    public static TSPlayer GetPlayerByName(
      string name, StringComparison stringComparison = StringComparison.InvariantCulture
    ) {
      foreach (TSPlayer tsPlayer in TShock.Players) {
        if (tsPlayer == null)
          continue;

        if (tsPlayer.Name.Equals(name, stringComparison))
          return tsPlayer;
      }

      return null;
    }

    public static void SendTileSquare(this TSPlayer player, DPoint location, int size = 10) {
      player.SendTileSquare(location.X, location.Y, size);
    }

    public static void SendTileSquareEx(this TSPlayer player, int x, int y, int size = 10) {
      player.SendData(PacketTypes.TileSendSquare, string.Empty, size, x, y);
    }

    public static void SendTileSquareEx(this TSPlayer player, DPoint location, int size = 10) {
      TShockEx.SendTileSquareEx(player, location.X, location.Y, size);
    }
    #endregion

    #region [CommandArgs Extensions]
    public static string ParamsToSingleString(this CommandArgs args, int fromIndex = 0) {
      StringBuilder builder = new StringBuilder();
      for (int i = fromIndex; i < args.Parameters.Count; i++) {
        if (i > fromIndex)
          builder.Append(' ');

        builder.Append(args.Parameters[i]);
      }

      return builder.ToString();
    }
    #endregion
  }
}
