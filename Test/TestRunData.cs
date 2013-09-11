using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Test {
  public class TestRunData {
    public Action<TestContext> TestAction { get; private set; }
    public TestContext Context { get; private set; }
    public Exception FailException { get; set; }


    public TestRunData(Action<TestContext> testAction) {
      this.TestAction = testAction;
      this.Context = new TestContext();
    }

    public void Reset() {
      this.Context.Reset();
      this.FailException = null;
    }
  }
}
