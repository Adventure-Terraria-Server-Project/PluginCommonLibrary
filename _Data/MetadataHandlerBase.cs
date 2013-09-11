using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Terraria.Plugins.Common {
  public abstract class MetadataHandlerBase {
    protected PluginTrace PluginTrace { get; private set; }
    protected string MetadataFilePath { get; private set; }
    public IMetadataFile Metadata { get; private set; }

    protected MetadataHandlerBase(PluginTrace pluginTrace, string metadataFilePath) {
      this.PluginTrace = pluginTrace;
      this.MetadataFilePath = metadataFilePath;
    }

    public bool RequiresMetadataInitialization() {
      return !File.Exists(this.MetadataFilePath);
    }

    public void InitOrReadMetdata() {
      if (!this.RequiresMetadataInitialization()) {
        try {
          this.Metadata = this.ReadMetadataFromFile(this.MetadataFilePath);
        } catch (Exception ex) {
          this.PluginTrace.WriteLineError(
            "Reading a metadata file failed. Exception details:\n{0}", ex
          );

          string backupFileName = Path.GetFileNameWithoutExtension(this.MetadataFilePath) + ".bak";
          string backupFilePath = Path.Combine(Path.GetDirectoryName(this.MetadataFilePath), backupFileName);

          this.Metadata = this.ReadMetadataFromFile(backupFilePath);
          this.PluginTrace.WriteLine("Succeeded reading the metadata backup file.");
        }
      } else {
        this.Metadata = this.InitMetadata();
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

    public bool IsWorldOlderThanLastWrittenMetadata() {
      if (!File.Exists(this.MetadataFilePath))
        return false;

      return (File.GetLastWriteTime(Main.worldPathName) < File.GetLastWriteTime(this.MetadataFilePath) + TimeSpan.FromSeconds(30));
    }

    public override string ToString() {
      return Path.GetFileName(this.MetadataFilePath);
    }
  }
}
