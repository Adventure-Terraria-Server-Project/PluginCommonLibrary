using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terraria.Plugins.CoderCow {
  public abstract class TimerBase {
    #region [Property: StartTime]
    private DateTime startTime;

    public DateTime StartTime {
      get { return this.startTime; }
      internal set { this.startTime = value; }
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
    private readonly Func<TimerBase,bool> callback;

    internal Func<TimerBase,bool> Callback {
      get { return this.callback; }
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
