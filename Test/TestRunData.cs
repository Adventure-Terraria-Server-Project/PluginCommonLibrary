// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Test {
  public class TestRunData {
    #region [Property: TestAction]
    private readonly Action<TestContext> testAction;

    public Action<TestContext> TestAction {
      get { return this.testAction; }
    }
    #endregion

    #region [Property: Context]
    private readonly TestContext context;

    public TestContext Context {
      get { return this.context; }
    }
    #endregion

    #region [Property: FailException]
    private Exception failException;

    public Exception FailException {
      get { return this.failException; }
      set { this.failException = value; }
    }
    #endregion


    #region [Method: Constructor]
    public TestRunData(Action<TestContext> testAction) {
      this.testAction = testAction;
      this.context = new TestContext();
    }
    #endregion

    #region [Method: Reset]
    public void Reset() {
      this.Context.Reset();
      this.failException = null;
    }
    #endregion
  }
}
