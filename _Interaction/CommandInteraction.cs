using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class CommandInteraction {
    #region [Property: ForPlayer]
    private readonly TSPlayer forPlayer;

    public TSPlayer ForPlayer {
      get { return this.forPlayer; }
    }
    #endregion

    #region [Property: IsActive]
    private bool isActive;

    public bool IsActive {
      get { return this.isActive; }
      internal set { this.isActive = value; }
    }
    #endregion

    #region [Property: TimeoutMs]
    private int timeoutMs;

    public int TimeoutMs {
      get { return this.timeoutMs; }
      internal set { this.timeoutMs = value; }
    }
    #endregion

    #region [Property: TimeoutTimer]
    private System.Threading.Timer timeoutTimer;

    internal System.Threading.Timer TimeoutTimer {
      get { return this.timeoutTimer; }
      set { this.timeoutTimer = value; }
    }
    #endregion

    #region [Property: TileEditCallback]
    private Func<TSPlayer,TileEditType,BlockType,DPoint,int,CommandInteractionResult> tileEditCallback;

    public Func<TSPlayer,TileEditType,BlockType,DPoint,int,CommandInteractionResult> TileEditCallback {
      get { return this.tileEditCallback; }
      set { this.tileEditCallback = value; }
    }
    #endregion

    #region [Property: ChestOpenCallback]
    private Func<TSPlayer,DPoint,CommandInteractionResult> chestOpenCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> ChestOpenCallback {
      get { return this.chestOpenCallback; }
      set { this.chestOpenCallback = value; }
    }
    #endregion

    #region [Property: SignEditCallback]
    private Func<TSPlayer,int,DPoint,string,CommandInteractionResult> signEditCallback;

    public Func<TSPlayer,int,DPoint,string,CommandInteractionResult> SignEditCallback {
      get { return this.signEditCallback; }
      set { this.signEditCallback = value; }
    }
    #endregion

    #region [Property: SignReadCallback]
    private Func<TSPlayer, DPoint, CommandInteractionResult> signReadCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> SignReadCallback {
      get { return this.signReadCallback; }
      set { this.signReadCallback = value; }
    }
    #endregion

    #region [Property: HitSwitchCallback]
    private Func<TSPlayer,DPoint,CommandInteractionResult> hitSwitchCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> HitSwitchCallback {
      get { return this.hitSwitchCallback; }
      set { this.hitSwitchCallback = value; }
    }
    #endregion

    #region [Property: TimeExpiredCallback]
    private Action<TSPlayer> timeExpiredCallback;

    public Action<TSPlayer> TimeExpiredCallback {
      get { return this.timeExpiredCallback; }
      set { this.timeExpiredCallback = value; }
    }
    #endregion

    #region [Property: AbortedCallback]
    private Action<TSPlayer> abortedCallback;

    public Action<TSPlayer> AbortedCallback {
      get { return this.abortedCallback; }
      set { this.abortedCallback = value; }
    }
    #endregion

    #region [Property: InteractionData]
    private object interactionData;

    public object InteractionData {
      get { return this.interactionData; }
      set { this.interactionData = value; }
    }
    #endregion

    #region [Property: DoesNeverComplete]
    private bool doesNeverComplete;

    public bool DoesNeverComplete {
      get { return this.doesNeverComplete; }
      set { this.doesNeverComplete = value; }
    }
    #endregion


    #region [Method: Constructor]
    public CommandInteraction(TSPlayer forPlayer) {
      Contract.Requires<ArgumentNullException>(forPlayer != null);

      this.forPlayer = forPlayer;
    }
    #endregion

    #region [Method: ResetTimer]
    public void ResetTimer() {
      if (this.timeoutTimer != null)
        lock (this.timeoutTimer)
          this.timeoutTimer.Change(this.timeoutMs, Timeout.Infinite);
    }
    #endregion
  }
}
