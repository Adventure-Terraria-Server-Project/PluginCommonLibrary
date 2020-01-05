using System;

namespace Terraria.Plugins.Common {
  public static class DateTimeEx {
    public static DateTime UnixTimeStamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static int ToUnixTime(this DateTime dateTime) {
      return (int)(dateTime - DateTimeEx.UnixTimeStamp).TotalSeconds;
    }
     
    public static DateTime FromUnixTime(double unixTime) {
      return DateTimeEx.UnixTimeStamp + TimeSpan.FromSeconds(unixTime);
    }
  }
}