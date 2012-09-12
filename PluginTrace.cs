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
  public class PluginTrace {
    #region [Constants]
    public const ConsoleColor ConsoleErrorColor = ConsoleColor.Red;
    public const ConsoleColor ConsoleWarningColor = ConsoleColor.Yellow;
    #endregion

    #region [Property: TracePrefix]
    private string tracePrefix;

    public string TracePrefix {
      get { return this.tracePrefix; }
      set { this.tracePrefix = value; }
    }
    #endregion


    #region [Method: Constructor]
    public PluginTrace(string tracePrefix) {
      this.tracePrefix = tracePrefix;
    }
    #endregion

    #region [Method: WriteLine]
    public void WriteLine(string message, TraceLevel level = TraceLevel.Info) {
      message = this.TracePrefix + message;

      ConsoleColor oldColor = Console.ForegroundColor;
      try {
        switch (level) {
          case TraceLevel.Error:
            Console.ForegroundColor = PluginTrace.ConsoleErrorColor;
            Log.Error(message);
            break;
          case TraceLevel.Warning:
            Console.ForegroundColor = PluginTrace.ConsoleWarningColor;
            Log.Warn(message);
            break;
        }

        Console.WriteLine(message);
        Log.Info(message);
      } finally {
        Console.ForegroundColor = oldColor;
      }
    }

    public void WriteLineInfo(string format, params object[] args) {
      this.WriteLine(string.Format(format, args));
    }

    public void WriteLineInfo(string format, object arg0, object arg1) {
      this.WriteLine(string.Format(format, arg0, arg1));
    }

    public void WriteLineInfo(string format, object arg0) {
      this.WriteLine(string.Format(format, arg0));
    }

    public void WriteLineWarning(string format, params object[] args) {
      this.WriteLine(string.Format(format, args), TraceLevel.Warning);
    }

    public void WriteLineWarning(string format, object arg0, object arg1) {
      this.WriteLine(string.Format(format, arg0, arg1), TraceLevel.Warning);
    }

    public void WriteLineWarning(string format, object arg0) {
      this.WriteLine(string.Format(format, arg0), TraceLevel.Warning);
    }

    public void WriteLineError(string format, params object[] args) {
      this.WriteLine(string.Format(format, args), TraceLevel.Error);
    }

    public void WriteLineError(string format, object arg0, object arg1) {
      this.WriteLine(string.Format(format, arg0, arg1), TraceLevel.Error);
    }

    public void WriteLineError(string format, object arg0) {
      this.WriteLine(string.Format(format, arg0), TraceLevel.Error);
    }

    [Conditional("Verbose")]
    public void WriteLineVerbose(string format, params object[] args) {
      this.WriteLine(string.Format(format, args), TraceLevel.Verbose);
    }

    [Conditional("Verbose")]
    public void WriteLineVerbose(string format, object arg0, object arg1) {
      this.WriteLine(string.Format(format, arg0, arg1), TraceLevel.Verbose);
    }

    [Conditional("Verbose")]
    public void WriteLineVerbose(string format, object arg0) {
      this.WriteLine(string.Format(format, arg0), TraceLevel.Verbose);
    }
    #endregion
  }
}
