// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;

namespace Terraria.Plugins.CoderCow {
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
