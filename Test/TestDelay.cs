using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Test {
  public class TestDelay {
    public int FramesLeft { get; set; }
    public Action<TestContext> Action { get; private set; }


    public TestDelay(int delayInFrames, Action<TestContext> action) {
      this.FramesLeft = delayInFrames;
      this.Action = action;
    }
  }
}
