using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  /// <summary>
  ///   An asynchronous user interaction with a command, such as "put a wire somewhere to display the name of this region".
  /// </summary>
  public class CommandInteraction {
    public TSPlayer ForPlayer { get; private set; }
    public bool IsActive { get; internal set; }
    public bool DoesNeverComplete { get; set; }
    public int TimeoutMs { get; internal set; }
    internal System.Threading.Timer TimeoutTimer { get; set; }
    public Func<TSPlayer,TileEditType,int,DPoint,int,CommandInteractionResult> TileEditCallback { get; set; }
    public Func<TSPlayer,DPoint,CommandInteractionResult> ChestOpenCallback { get; set; }
    public Func<TSPlayer,int,DPoint,string,CommandInteractionResult> SignEditCallback { get; set; }
    public Func<TSPlayer,DPoint,CommandInteractionResult> SignReadCallback { get; set; }
    public Func<TSPlayer,DPoint,CommandInteractionResult> HitSwitchCallback { get; set; }
    public Action<TSPlayer> TimeExpiredCallback { get; set; }
    public Action<TSPlayer> AbortedCallback { get; set; }
    public object InteractionData { get; set; }


    public CommandInteraction(TSPlayer forPlayer) {
      Contract.Requires<ArgumentNullException>(forPlayer != null);

      this.ForPlayer = forPlayer;
    }

    public void ResetTimer() {
      if (this.TimeoutTimer != null)
        lock (this.TimeoutTimer)
          this.TimeoutTimer.Change(this.TimeoutMs, Timeout.Infinite);
    }
  }
}
