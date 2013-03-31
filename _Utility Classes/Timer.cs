using System;

namespace Terraria.Plugins.Common {
  public class Timer: TimerBase {
    #region [Property: TimeSpan]
    private TimeSpan timeSpan;

    public TimeSpan TimeSpan {
      get { return this.timeSpan; }
      set { this.timeSpan = value; }
    }
    #endregion


    #region [Method: Constructor]
    public Timer(TimeSpan timeSpan, object data, Func<TimerBase,bool> callback): base(data, callback) {
      this.timeSpan = timeSpan;
    }
    #endregion

    #region [Methods: IsExpired, Update, Reset]
    public override bool IsExpired() {
      return ((this.StartTime.Ticks + this.TimeSpan.Ticks) <= DateTime.Now.Ticks);
    }

    public override void Update(int framesPassed) {}

    public override void Reset() {
      this.StartTime = DateTime.Now;
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return this.TimeSpan.ToString();
    }
    #endregion
  }
}
