// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

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
