using System;
using System.IO;

namespace Terraria.Plugins.Common {
  public abstract class WorldMetadataHandlerBase: MetadataHandlerBase {
    private const string WorldMetadataFileNameFormat = @"{0}.json";

    public string MetadataDirectoryPath { get; private set; }


    protected WorldMetadataHandlerBase(PluginTrace pluginTrace, string metadataDirectoryPath): base(
      pluginTrace, Path.Combine(
        metadataDirectoryPath, string.Format(WorldMetadataHandlerBase.WorldMetadataFileNameFormat, Main.worldID)
      )
    ) {
      if (!Directory.Exists(metadataDirectoryPath))
        Directory.CreateDirectory(metadataDirectoryPath);

      this.MetadataDirectoryPath = metadataDirectoryPath;
    }
  }
}
