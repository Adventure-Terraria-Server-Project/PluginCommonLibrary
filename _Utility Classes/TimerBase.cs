using System;

namespace Terraria.Plugins.Common {
  public abstract class TimerBase {
    public DateTime StartTime { get; set; }
    public object Data { get; set; }
    public Func<TimerBase,bool> Callback { get; set; }


    protected TimerBase(object data, Func<TimerBase,bool> callback) {
      this.Data = data;
      this.Callback = callback;
    }

    public abstract void Update(int passedGameFrames);
    public abstract bool IsExpired();
    public abstract void Reset();
  }
}
