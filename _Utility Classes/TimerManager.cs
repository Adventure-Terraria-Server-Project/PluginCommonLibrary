using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;

namespace Terraria.Plugins.Common {
	public class TimerManager {
		private const int FrameUpdateFreq = 10;

		public PluginTrace PluginTrace { get; }
		private List<TimerBase> runningTimers; // immutable
		private readonly object alterTimersLock;


		public TimerManager(PluginTrace pluginTrace) {
			this.PluginTrace = pluginTrace;

			this.runningTimers = new List<TimerBase>(10);
			this.alterTimersLock = new object();
		}

		public void StartTimer(TimerBase timer) {
			if (!this.IsTimerRunning(timer)) {
				timer.Reset();
				this.AddTimer(timer);
			}
		}

		public void StartOrResetTimer(TimerBase timer) {
			if (this.IsTimerRunning(timer))
				timer.Reset();
			else
				this.AddTimer(timer);
		}

		public void ContinueTimer(TimerBase timer) {
			if (!this.IsTimerRunning(timer))
				this.AddTimer(timer);
		}

		private void AddTimer(TimerBase timer) {
			lock (this.alterTimersLock) {
				List<TimerBase> newTimers = new List<TimerBase>(this.runningTimers.Count);
				newTimers.AddRange(this.runningTimers);

				newTimers.Add(timer);
				Interlocked.Exchange(ref this.runningTimers, newTimers);
			}
		}

		public void RemoveTimer(TimerBase timer, bool andInvokeCallback = false) {
			lock (this.alterTimersLock) {
				List<TimerBase> newTimers = new List<TimerBase>(this.runningTimers.Count);
				newTimers.AddRange(this.runningTimers);

				newTimers.Remove(timer);
				Interlocked.Exchange(ref this.runningTimers, newTimers);
			}

			if (andInvokeCallback)
				this.InvokeTimerCallback(timer);
		}

		public bool IsTimerRunning(TimerBase timer) {
			return this.runningTimers.Contains(timer);
		}

		private int frameCounter;
		public void HandleGameUpdate() {
			if (this.frameCounter >= TimerManager.FrameUpdateFreq) {
				this.frameCounter = 0;

				List<TimerBase> timersToRemove = null;
				foreach (TimerBase timer in this.runningTimers) {
					timer.Update(TimerManager.FrameUpdateFreq);

					if (timer.IsExpired()) {
						if (this.InvokeTimerCallback(timer)) {
							timer.Reset();
						} else {
							if (timersToRemove == null)
								timersToRemove = new List<TimerBase>();

							timersToRemove.Add(timer);
						}
					}
				}

				if (timersToRemove != null) {
					lock (this.alterTimersLock) {
						List<TimerBase> newTimers = new List<TimerBase>(this.runningTimers.Count);
						newTimers.AddRange(this.runningTimers.Where((t) => !timersToRemove.Contains(t)));

						Interlocked.Exchange(ref this.runningTimers, newTimers);
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
			return $"{this.runningTimers.Count} timers running.";
		}
	}
}
