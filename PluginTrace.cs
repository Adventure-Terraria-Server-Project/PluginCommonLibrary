using System;
using System.Diagnostics;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class PluginTrace {
    public const ConsoleColor ConsoleErrorColor = ConsoleColor.Red;
    public const ConsoleColor ConsoleWarningColor = ConsoleColor.Yellow;

    public string TracePrefix { get; set; }


    public PluginTrace(string tracePrefix) {
      this.TracePrefix = tracePrefix;
    }

    public void WriteLine(string message, TraceLevel level = TraceLevel.Info) {
      message = this.TracePrefix + message;

      ConsoleColor oldColor = Console.ForegroundColor;
      try {
        switch (level) {
          case TraceLevel.Error:
            Console.ForegroundColor = PluginTrace.ConsoleErrorColor;
            TShock.Log.Error(message);
            break;
          case TraceLevel.Warning:
            Console.ForegroundColor = PluginTrace.ConsoleWarningColor;
            TShock.Log.Warn(message);
            break;
          default:
            TShock.Log.Info(message);
            break;
        }

        Console.WriteLine(message);
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

    public void WriteLineInfo(string lineString) {
      this.WriteLine(lineString);
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

    public void WriteLineWarning(string lineString) {
      this.WriteLine(lineString, TraceLevel.Warning);
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

    public void WriteLineError(string lineString) {
      this.WriteLine(lineString, TraceLevel.Error);
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

    [Conditional("Verbose")]
    public void WriteLineVerbose(string lineString) {
      this.WriteLine(lineString, TraceLevel.Verbose);
    }
  }
}
