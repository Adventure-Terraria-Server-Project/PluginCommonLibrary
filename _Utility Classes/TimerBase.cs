// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;

namespace Terraria.Plugins.CoderCow {
  public abstract class TimerBase {
    #region [Property: StartTime]
    private DateTime startTime;

    public DateTime StartTime {
      get { return this.startTime; }
      set { this.startTime = value; }
    }
    #endregion

    #region [Property: Data]
    private object data;

    public object Data {
      get { return this.data; }
      set { this.data = value; }
    }
    #endregion

    #region [Property: Callback]
    private Func<TimerBase,bool> callback;

    public Func<TimerBase,bool> Callback {
      get { return this.callback; }
      set { this.callback = value; }
    }
    #endregion


    #region [Method: Constructor]
    protected TimerBase(object data, Func<TimerBase,bool> callback) {
      this.data = data;
      this.callback = callback;
    }
    #endregion

    #region [Methods: Update, IsExpired, Reset]
    public abstract void Update(int passedGameFrames);
    public abstract bool IsExpired();
    public abstract void Reset();
    #endregion
  }
}
