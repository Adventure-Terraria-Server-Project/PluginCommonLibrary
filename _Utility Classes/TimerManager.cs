using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public class TimerManager {
    #region [Constants]
    private const int FrameUpdateFreq = 10;
    #endregion

    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    public PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion

    #region [Property: RunningTimers]
    private readonly List<TimerBase> runningTimers;

    protected List<TimerBase> RunningTimers {
      get { return this.runningTimers; }
    }
    #endregion


    #region [Method: Constructor]
    public TimerManager(PluginTrace pluginTrace) {
      this.pluginTrace = pluginTrace;
      this.runningTimers = new List<TimerBase>();
    }
    #endregion

    #region [Methods: StartTimer, StartOrResetTimer, ContinueTimer, RemoveTimer, IsTimerRunning, HandleGameUpdate]
    public void StartTimer(TimerBase timer) {
      if (!this.IsTimerRunning(timer)) {
        this.RunningTimers.Add(timer);
        timer.Reset();
      }
    }

    public void StartOrResetTimer(TimerBase timer) {
      if (!this.IsTimerRunning(timer))
        this.RunningTimers.Add(timer);

      timer.Reset();
    }

    public void ContinueTimer(TimerBase timer) {
      if (!this.IsTimerRunning(timer))
        this.RunningTimers.Add(timer);
    }

    public void RemoveTimer(TimerBase timer, bool andInvokeCallback = false) {
      try {
        if (andInvokeCallback)
          this.InvokeTimerCallback(timer);
      } finally {
        this.RunningTimers.Remove(timer);
      }
    }

    public bool IsTimerRunning(TimerBase timer) {
      return this.RunningTimers.Contains(timer);
    }

    private int frameCounter;
    public void HandleGameUpdate() {
      if (this.frameCounter >= TimerManager.FrameUpdateFreq) {
        this.frameCounter = 0;

        for (int i = 0; i < this.RunningTimers.Count; i++) {
          TimerBase abstractTimer = this.RunningTimers[i];
          abstractTimer.Update(TimerManager.FrameUpdateFreq);

          if (abstractTimer.IsExpired()) {
            if (this.InvokeTimerCallback(abstractTimer))
              abstractTimer.Reset();
            else
              this.RunningTimers.RemoveAt(i--);
          }
        }
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
      return string.Format("{0} Timers running.", this.RunningTimers.Count);
    }
    #endregion
  }
}
