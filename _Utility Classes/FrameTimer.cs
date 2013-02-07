// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;

namespace Terraria.Plugins.CoderCow {
  public class FrameTimer: TimerBase {
    #region [Property: FrameSpan]
    private int frameSpan;

    public int FrameSpan {
      get { return this.frameSpan; }
      set { this.frameSpan = value; }
    }
    #endregion

    #region [Property: FramesLeft]
    private int framesLeft;

    protected int FramesLeft {
      get { return this.framesLeft; }
      set { this.framesLeft = value; }
    }
    #endregion


    #region [Method: Constructor]
    public FrameTimer(int frameSpan, object data, Func<TimerBase,bool> callback): base(data, callback) {
      this.frameSpan = frameSpan;
      this.Reset();
    }
    #endregion

    #region [Methods: IsExpired, Update, Reset]
    public override bool IsExpired() {
      return (this.FramesLeft <= 0);
    }

    public override void Update(int framesPassed) {
      this.FramesLeft -= framesPassed;
    }

    public override void Reset() {
      this.FramesLeft = this.FrameSpan;
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return this.FrameSpan.ToString();
    }
    #endregion
  }
}

