using System;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestPlaceEventArgs: TileLocationEventArgs {
    public int StorageType { get; private set; }
    public int StorageStyle { get; private set; }


    public ChestPlaceEventArgs(TSPlayer player, DPoint storageLocation, int storageType, int storageStyle): base(player, storageLocation) {
      this.StorageType = storageType;
      this.StorageStyle = storageStyle;
    }
  }
}
