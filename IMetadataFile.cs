using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public interface IMetadataFile {
    void Write(string filePath);
  }
}
