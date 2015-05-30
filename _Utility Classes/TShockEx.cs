using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Text;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public static class TShockEx {
    public static bool MatchUserAccountNameByPlayerName(string playerName, out string exactName, TSPlayer messagesReceiver = null) {
      exactName = null;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        TSPlayer player;
        if (!TShockEx.MatchPlayerByName(playerName, out player, messagesReceiver))
          return false;

        exactName = player.User.Name;
      } else {
        exactName = tsUser.Name;
      }

      return true;
    }

    public static bool MatchUserIdByPlayerName(string playerName, out int userId, TSPlayer messagesReceiver = null) {
      userId = -1;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        TSPlayer player;
        if (!TShockEx.MatchPlayerByName(playerName, out player, messagesReceiver))
          return false;

        userId = player.User.ID;
      } else {
        userId = tsUser.ID;
      }

      return true;
    }

    public static bool MatchUserByPlayerName(string playerName, out TShockAPI.DB.User user, TSPlayer messagesReceiver = null) {
      user = null;
      TShockAPI.DB.User tsUser = TShock.Users.GetUserByName(playerName);
      if (tsUser == null) {
        TSPlayer player;
        if (!TShockEx.MatchPlayerByName(playerName, out player, messagesReceiver))
          return false;

        user = TShock.Users.GetUserByID(player.User.ID);
      } else {
        user = tsUser;
      }

      return true;
    }

    public static bool MatchPlayerByName(
      string name, out TSPlayer matchedPlayer, TSPlayer messagesReceiver = null
    ) {
      matchedPlayer = null;
      List<TSPlayer> matchedPlayers = TShock.Utils.FindPlayer(name);
      if (matchedPlayers.Count == 0) {
        if (messagesReceiver != null)
          messagesReceiver.SendErrorMessage(string.Format("Could not match any players for \"{0}\".", name));

        return false;
      } if (matchedPlayers.Count > 1) {
        if (messagesReceiver != null) {
          messagesReceiver.SendErrorMessage(
            "More than one player matched! Matches: " + string.Join(", ", matchedPlayers.Select(p => p.Name))
          );
        }
        return false;
      }

      matchedPlayer = matchedPlayers[0];
      return true;
    }

    public static TSPlayer GetPlayerByName(string name, StringComparison stringComparison = StringComparison.InvariantCulture) {
      foreach (TSPlayer tsPlayer in TShock.Players) {
        if (tsPlayer == null)
          continue;

        if (tsPlayer.Name.Equals(name, stringComparison))
          return tsPlayer;
      }

      return null;
    }

    public static TSPlayer GetPlayerByIp(string ip) {
      foreach (TSPlayer tsPlayer in TShock.Players) {
        if (tsPlayer == null)
          continue;

        if (tsPlayer.IP == ip)
          return tsPlayer;
      }

      return null;
    }

    public static TSPlayer GetPlayerByTPlayer(Player tPlayer) {
      foreach (TSPlayer tsPlayer in TShock.Players) {
        if (tsPlayer == null)
          continue;

        if (tsPlayer.TPlayer == tPlayer)
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

    public static DPoint ToLocation(this TSPlayer player) {
      return new DPoint((int)player.X, (int)player.Y);
    }

    public static DPoint ToTileLocation(this TSPlayer player) {
      return new DPoint(player.TileX, player.TileY);
    }

    public static string ParamsToSingleString(this CommandArgs args, int fromIndex = 0, int paramsToTrimFromEnd = 0) {
      StringBuilder builder = new StringBuilder();
      for (int i = fromIndex; i < args.Parameters.Count - paramsToTrimFromEnd; i++) {
        if (i > fromIndex)
          builder.Append(' ');

        builder.Append(args.Parameters[i]);
      }

      return builder.ToString();
    }

    public static bool ContainsParameter(this CommandArgs args, string parameter, StringComparison comparison) {
      foreach (string param in args.Parameters) {
        if (param.Equals(parameter, comparison))
          return true;
      }

      return false;
    }
  }
}
