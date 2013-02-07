// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Text;

namespace Terraria.Plugins.CoderCow {
  public static class TimeSpanEx {
    public static string ToLongString(this TimeSpan timeSpan) {
      StringBuilder result = new StringBuilder();
      int totalDays = (int)timeSpan.TotalDays;
      if (totalDays == 1) {
        result.Append("1 day");
      } else if (totalDays > 0) {
        result.Append(timeSpan.TotalDays);
        result.Append(" days");
      }

      if (timeSpan.Hours == 1) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append("1 hour");
      } else if (timeSpan.Hours > 0) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append(timeSpan.Hours);
        result.Append(" hours");
      }

      if (timeSpan.Minutes == 1) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append("1 minute");
      } else if (timeSpan.Minutes > 0) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append(timeSpan.Minutes);
        result.Append(" minutes");
      }

      return result.ToString();
    }
  }
}
