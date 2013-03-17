using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace Terraria.Plugins.CoderCow {
  public static class TimeSpanEx {
    #region [Method: Static TryParseShort]
    // Group identifiers: Days, Hours, Minutes, Seconds
    private static readonly Regex parseShortTimeSpanRegex = new Regex(
      @"^(\W*((?<Days>\d+)\W*d(ays)?)?\W*((?<Hours>\d+)\W*h(ours|rs)?)?\W*((?<Minutes>\d+)\W*m(inutes|ins)?)?\W*((?<Seconds>\d+)\W*s(econds|ecs)?)?)$",
      RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.IgnorePatternWhitespace
    );

    public static bool TryParseShort(string input, out TimeSpan result) {
      Contract.Requires<ArgumentNullException>(input != null);

      result = TimeSpan.Zero;
      if (string.IsNullOrWhiteSpace(input))
        return false;
      
      Match regexMatch = TimeSpanEx.parseShortTimeSpanRegex.Match(input);
      if (!regexMatch.Success)
        return false;

      Group daysGroup = regexMatch.Groups["Days"];
      if (daysGroup.Success) {
        int days;
        if (!int.TryParse(daysGroup.Value, out days))
          return false;

        result += TimeSpan.FromDays(days);
      }
      Group hoursGroup = regexMatch.Groups["Hours"];
      if (hoursGroup.Success) {
        int hours;
        if (!int.TryParse(hoursGroup.Value, out hours))
          return false;

        result += TimeSpan.FromHours(hours);
      }

      Group minutesGroup = regexMatch.Groups["Minutes"];
      if (minutesGroup.Success) {
        int minutes;
        if (!int.TryParse(minutesGroup.Value, out minutes))
          return false;

        result += TimeSpan.FromMinutes(minutes);
      }

      Group secondsGroup = regexMatch.Groups["Seconds"];
      if (secondsGroup.Success) {
        int seconds;
        if (!int.TryParse(secondsGroup.Value, out seconds))
          return false;

        result += TimeSpan.FromSeconds(seconds);
      }

      return true;
    }

    public static bool TryParseShort(string input, out TimeSpan? result) {
      TimeSpan realResult;

      if (TimeSpanEx.TryParseShort(input, out realResult)) {
        result = realResult;
        return true;
      } else {
        result = null;
        return false;
      }
    }
    #endregion

    #region [Method: Static ToLongString]
    public static string ToLongString(this TimeSpan timeSpan) {
      StringBuilder result = new StringBuilder();
      if (timeSpan.Days == 1) {
        result.Append("1 day");
      } else if (timeSpan.Days > 0) {
        result.Append(timeSpan.Days);
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

      if (timeSpan.Seconds == 1) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append("1 second");
      } else if (timeSpan.Seconds > 0) {
        if (result.Length > 0)
          result.Append(' ');

        result.Append(timeSpan.Seconds);
        result.Append(" seconds");
      }

      if (result.Length == 0) {
        if (timeSpan.Milliseconds > 0)
          result.Append("Less than a second.");
        else
          result.Append("Zero");
      }

      return result.ToString();
    }
    #endregion
  }
}
