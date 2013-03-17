using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Test {
  public class TestDelay {
    #region [Property: FramesLeft]
    private int framesLeft;

    public int FramesLeft {
      get { return this.framesLeft; }
      set { this.framesLeft = value; }
    }
    #endregion

    #region [Property: Action]
    private readonly Action<TestContext> action;

    public Action<TestContext> Action {
      get { return this.action; }
    }
    #endregion


    #region [Method: Constructor]
    public TestDelay(int delayInFrames, Action<TestContext> action) {
      this.framesLeft = delayInFrames;
      this.action = action;
    }
    #endregion
  }
}
