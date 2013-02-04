using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Hooks;

namespace Terraria.Plugins.CoderCow {
  public class TimerManager: IDisposable {
    #region [Constants]
    private const int FrameUpdateFreq = 10;
    #endregion

    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    public PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion

    #region [Property: Timers]
    private readonly List<TimerBase> timers;

    protected List<TimerBase> Timers {
      get { return this.timers; }
    }
    #endregion


    #region [Method: Constructor]
    public TimerManager(PluginTrace pluginTrace) {
      this.pluginTrace = pluginTrace;
      this.timers = new List<TimerBase>();

      GameHooks.Update += this.Game_Update;
    }
    #endregion

    #region [Methods: StartTimer, StartOrResetTimer, RemoveTimer, HandleGameUpdate]
    public void StartTimer(TimerBase timer) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      if (!this.Timers.Contains(timer))
        this.Timers.Add(timer);
    }

    public void StartOrResetTimer(TimerBase timer) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      int timerIndex = this.Timers.IndexOf(timer);
      if (timerIndex == -1)
        this.Timers.Add(timer);
      else
        this.Timers[timerIndex].Reset();
    }

    public void RemoveTimer(TimerBase timer, bool andInvokeCallback = false) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      try {
        if (andInvokeCallback)
          this.InvokeTimerCallback(timer);
      } finally {
        this.Timers.Remove(timer);
      }
    }

    private int frameCounter;
    public void Game_Update() {
      if (this.frameCounter >= TimerManager.FrameUpdateFreq) {
        if (this.Timers.Count > 0) {
          for (int i = 0; i < this.Timers.Count; i++) {
            TimerBase abstractTimer = this.Timers[i];
            abstractTimer.Update(TimerManager.FrameUpdateFreq);

            if (abstractTimer.IsExpired()) {
              if (this.InvokeTimerCallback(abstractTimer))
                abstractTimer.Reset();
              else
                this.Timers.RemoveAt(i--);
            }
          }
        }

        this.frameCounter = 0;
      }

      this.frameCounter++;
    }

    private bool InvokeTimerCallback(TimerBase timer) {
      try {
        return timer.Callback(timer);
      } catch (Exception ex) {
        this.PluginTrace.WriteLineError("A Timer's callback has thrown an exception. Excpetion details: " + ex);
        return false;
      }
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return string.Format("{0} Timers running.", this.Timers.Count);
    }
    #endregion

    #region [IDisposable Implementation]
    private bool isDisposed;

    public bool IsDisposed {
      get { return this.isDisposed; } 
    }

    protected virtual void Dispose(bool isDisposing) {
      if (this.isDisposed)
        return;
    
      if (isDisposing)
        GameHooks.Update -= this.Game_Update;
    
      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~TimerManager() {
      this.Dispose(false);
    }
    #endregion
  }
}
