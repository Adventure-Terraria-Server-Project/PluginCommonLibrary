using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;

namespace Terraria.Plugins.CoderCow {
  public abstract class WorldMetadataHandlerBase: MetadataHandlerBase {
    #region [Constants]
    private const string WorldMetadataFileNameFormat = @"{0}.json";
    #endregion


    #region [Method: Constructor]
    protected WorldMetadataHandlerBase(PluginTrace pluginTrace, string metadataDirectoryPath): base(
      pluginTrace, WorldMetadataHandlerBase.GetMetadataFilePath(metadataDirectoryPath)
    ) {}

    private static string GetMetadataFilePath(string metadataDirectoryPath) {
      if (!Directory.Exists(metadataDirectoryPath))
        Directory.CreateDirectory(metadataDirectoryPath);

      return Path.Combine(
        metadataDirectoryPath, string.Format(WorldMetadataHandlerBase.WorldMetadataFileNameFormat, Main.worldID)
      );
    }
    #endregion
  }
}
