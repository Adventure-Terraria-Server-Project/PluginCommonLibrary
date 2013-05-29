using System;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public static class DateTimeEx {
    public static DateTime UnixTimeStamp = new DateTime(1970, 1, 1);
    public static int ToUnixTime(this DateTime dateTime) {
      return (int)(dateTime - DateTimeEx.UnixTimeStamp).TotalSeconds;
    }
     
    public static DateTime FromUnixTime(int unixTime) {
      return DateTimeEx.UnixTimeStamp + TimeSpan.FromSeconds(unixTime);
    }
  }
}