// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow {
  public static class BoolEx {
    public static bool ParseEx(string input) {
      input = input.Trim();
      if (input.Equals("1", StringComparison.OrdinalIgnoreCase))
        return true;
      if (input.Equals("0", StringComparison.OrdinalIgnoreCase))
        return false;
      if (input.Equals("true", StringComparison.OrdinalIgnoreCase))
        return true;
      if (input.Equals("false", StringComparison.OrdinalIgnoreCase))
        return false;

      throw new FormatException(string.Format("The value \"{0}\" can not be parsed as a valid boolean.", input));
    }
  }
}
