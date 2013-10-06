using System;

namespace Terraria.Plugins.Common {
  public class Timer: TimerBase {
    public TimeSpan TimeSpan { get; set; }


    public Timer(TimeSpan timeSpan, object data, Func<TimerBase,bool> callback): base(data, callback) {
      this.TimeSpan = timeSpan;
    }

    public override bool IsExpired() {
      return ((this.StartTime.Ticks + this.TimeSpan.Ticks) <= DateTime.Now.Ticks);
    }

    public override void Update(int framesPassed) {}

    public override void Reset() {
      this.StartTime = DateTime.Now;
    }

    public override string ToString() {
      return this.TimeSpan.ToString();
    }
  }
}
