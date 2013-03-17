using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow {
  public struct PluginInfo {
    public static PluginInfo Empty = default(PluginInfo);

    #region [Property: PluginName]
    private readonly string pluginName;

    public string PluginName {
      get { return this.pluginName; }
    }
    #endregion

    #region [Property: VersionNumber]
    private readonly Version versionNumber;

    public Version VersionNumber {
      get { return this.versionNumber; }
    }
    #endregion

    #region [Property: VersionAppendix]
    private readonly string versionAppendix;

    public string VersionAppendix {
      get { return this.versionAppendix; }
    }
    #endregion

    #region [Property: Author]
    private readonly string author;

    public string Author {
      get { return this.author; }
    }
    #endregion

    #region [Property: Description]
    private readonly string description;

    public string Description {
      get { return this.description; }
    }
    #endregion


    #region [Method: Constructor]
    public PluginInfo(string pluginName, Version versionNumber, string versionAppendix, string author, string description) {
      this.pluginName = pluginName;
      this.versionNumber = versionNumber;
      this.versionAppendix = versionAppendix;
      this.author = author;
      this.description = description;
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return string.Format(
        "{0} {1} {2} by {3}", this.PluginName, this.VersionNumber.ToString(3), this.VersionAppendix, this.Author
      );
    }
    #endregion
  }
}
