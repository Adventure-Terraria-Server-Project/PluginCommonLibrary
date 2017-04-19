using System;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using TShockAPI;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common.Hooks {
  public class MassWireOperationEventArgs : NetHookEventArgs {
    public DPoint StartLocation { get; private set; }
    public DPoint EndLocation { get; private set; }
    public WiresUI.Settings.MultiToolMode ToolMode { get; private set; }

    public MassWireOperationEventArgs(TSPlayer player, DPoint startLocation, DPoint endLocation, WiresUI.Settings.MultiToolMode toolMode): base(player) {
      this.StartLocation = startLocation;
      this.EndLocation = endLocation;
      this.ToolMode = toolMode;
    }
  }
}