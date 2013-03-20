using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;

namespace Terraria.Plugins.CoderCow {
  public abstract class WorldMetadataHandlerBase: MetadataHandlerBase {
    #region [Constants]
    private const string WorldMetadataFileNameFormat = @"{0}.json";
    #endregion

    #region [Property: MetadataDirectoryPath]
    private readonly string metadataDirectoryPath;

    public string MetadataDirectoryPath {
      get { return this.metadataDirectoryPath; }
    }
    #endregion


    #region [Method: Constructor]
    protected WorldMetadataHandlerBase(PluginTrace pluginTrace, string metadataDirectoryPath): base(
      pluginTrace, Path.Combine(
        metadataDirectoryPath, string.Format(WorldMetadataHandlerBase.WorldMetadataFileNameFormat, Main.worldID)
      )
    ) {
      if (!Directory.Exists(metadataDirectoryPath))
        Directory.CreateDirectory(metadataDirectoryPath);

      this.metadataDirectoryPath = metadataDirectoryPath;
    }
    #endregion
  }
}
