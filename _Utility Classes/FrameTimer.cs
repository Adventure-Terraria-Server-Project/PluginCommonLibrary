using System;

namespace Terraria.Plugins.Common {
  public class FrameTimer: TimerBase {
    public int FrameSpan { get; set; }
    protected int FramesLeft { get; set; }


    public FrameTimer(int frameSpan, object data, Func<TimerBase,bool> callback): base(data, callback) {
      this.FrameSpan = frameSpan;
      this.Reset();
    }

    public override bool IsExpired() {
      return (this.FramesLeft <= 0);
    }

    public override void Update(int framesPassed) {
      this.FramesLeft -= framesPassed;
    }

    public override void Reset() {
      this.FramesLeft = this.FrameSpan;
    }

    public override string ToString() {
      return this.FrameSpan.ToString();
    }
  }
}

