using System;
using System.Text;

namespace Terraria.Plugins.Common {
  public struct PluginInfo {
    public static PluginInfo Empty = default(PluginInfo);

    public string PluginName { get; private set; }
    public Version VersionNumber { get; private set; }
    public string VersionAppendix { get; private set; }
    public string Author { get; private set; }
    public string Description { get; private set; }


    public PluginInfo(
      string pluginName, Version versionNumber, string versionAppendix, string author, string description
    ): this() {
      this.PluginName = pluginName;
      this.VersionNumber = versionNumber;
      this.VersionAppendix = versionAppendix;
      this.Author = author;
      this.Description = description;
    }

    public override string ToString() {
      StringBuilder resultBuilder = new StringBuilder(this.PluginName);
      resultBuilder.Append(' ');
      resultBuilder.Append(this.VersionNumber.ToString(3));

      if (this.VersionAppendix != string.Empty) {
        resultBuilder.Append(' ');
        resultBuilder.Append(this.VersionAppendix);
      }

      resultBuilder.Append(" by ");
      resultBuilder.Append(this.Author);
      return resultBuilder.ToString();
    }
  }
}
