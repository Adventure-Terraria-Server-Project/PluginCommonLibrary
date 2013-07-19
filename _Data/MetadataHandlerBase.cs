using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Terraria.Plugins.Common {
  public abstract class MetadataHandlerBase {
    #region [Property: MetadataFilePath]
    private readonly string metadataFilePath;

    protected string MetadataFilePath {
      get { return this.metadataFilePath; }
    }
    #endregion

    #region [Property: Metadata]
    private IMetadataFile metadata;

    public IMetadataFile Metadata {
      get { return this.metadata; }
    }
    #endregion

    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    protected PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion


    #region [Method: Constructor]
    protected MetadataHandlerBase(PluginTrace pluginTrace, string metadataFilePath) {
      this.pluginTrace = pluginTrace;
      this.metadataFilePath = metadataFilePath;
    }
    #endregion

    #region [Methods: RequiresMetadataInitialization, InitOrReadMetdata, InitMetadata, ReadMetadataFromFile, WriteMetadata, CreateMetadataSnapshot]
    public bool RequiresMetadataInitialization() {
      return !File.Exists(this.metadataFilePath);
    }

    public void InitOrReadMetdata() {
      if (!this.RequiresMetadataInitialization()) {
        try {
          this.metadata = this.ReadMetadataFromFile(this.metadataFilePath);
        } catch (Exception ex) {
          this.PluginTrace.WriteLineError(
            "Reading a metadata file failed. Exception details:\n{0}", ex
          );

          string backupFileName = Path.GetFileNameWithoutExtension(this.metadataFilePath) + ".bak";
          string backupFilePath = Path.Combine(Path.GetDirectoryName(this.metadataFilePath), backupFileName);

          this.metadata = this.ReadMetadataFromFile(backupFilePath);
          this.PluginTrace.WriteLine("Succeeded reading the metadata backup file.");
        }
      } else {
        this.metadata = this.InitMetadata();
      }
    }

    protected abstract IMetadataFile InitMetadata();
    protected abstract IMetadataFile ReadMetadataFromFile(string filePath);

    public virtual void WriteMetadata() {
      Contract.Requires<InvalidOperationException>(this.Metadata != null, "No present metadata.");

      // Make a backup of the old file if it exists.
      if (File.Exists(this.MetadataFilePath)) {
        string backupFileName = Path.GetFileNameWithoutExtension(this.MetadataFilePath) + ".bak";
        string backupFilePath = Path.Combine(Path.GetDirectoryName(this.MetadataFilePath), backupFileName);
        File.Copy(this.MetadataFilePath, backupFilePath, true);
      }

      this.Metadata.Write(this.MetadataFilePath);
    }

    public virtual void CreateMetadataSnapshot() {
      Contract.Requires<InvalidOperationException>(this.Metadata != null, "No present metadata.");

      if (!File.Exists(this.MetadataFilePath))
        throw new InvalidOperationException("Theres no actual metadata file, a snapshot can not be created.");

      DateTime actualMetadataTime = File.GetLastWriteTime(this.MetadataFilePath);
      string directoryPath = Path.GetDirectoryName(this.MetadataFilePath);
      string snapShotFileName = string.Concat(
        Path.GetFileNameWithoutExtension(this.MetadataFilePath), " snapshot ",
        actualMetadataTime.ToString("yyyy-MM-dd_HH-mm-ss"), ".json"
      );

      string snapShotPath = Path.Combine(directoryPath, snapShotFileName);
      if (File.Exists(snapShotPath))
        File.Delete(snapShotPath);

      File.Move(this.MetadataFilePath, snapShotPath);
      this.Metadata.Write(this.MetadataFilePath);
    }
    #endregion

    #region [Method: IsWorldOlderThanLastWrittenMetadata]
    public bool IsWorldOlderThanLastWrittenMetadata() {
      if (!File.Exists(this.MetadataFilePath))
        return false;

      return (File.GetLastWriteTime(Main.worldPathName) < File.GetLastWriteTime(this.MetadataFilePath));
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return Path.GetFileName(this.MetadataFilePath);
    }
    #endregion
  }
}
