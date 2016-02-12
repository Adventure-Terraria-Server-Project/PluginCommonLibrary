using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public class TimerManager {
    private const int FrameUpdateFreq = 10;

    public PluginTrace PluginTrace { get; private set; }
    protected List<TimerBase> RunningTimers { get; private set; }


    public TimerManager(PluginTrace pluginTrace) {
      this.PluginTrace = pluginTrace;
      this.RunningTimers = new List<TimerBase>();
    }

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
        this.PluginTrace.WriteLineError("A timer's callback has thrown an exception. Excpetion details: " + ex);
        return false;
      }
    }

    public override string ToString() {
      return string.Format("{0} timers running.", this.RunningTimers.Count);
    }
  }
}
