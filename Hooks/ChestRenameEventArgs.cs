using System;

using TShockAPI;

namespace Terraria.Plugins.Common.Hooks {
  public class ChestRenameEventArgs: NetHookEventArgs {
    public int ChestIndex { get; private set; }
    public string NewName { get; private set; }

    public ChestRenameEventArgs(TSPlayer player, int chestIndex, string newName): base(player) {
      this.ChestIndex = chestIndex;
      this.NewName = newName;
    }
  }
}
