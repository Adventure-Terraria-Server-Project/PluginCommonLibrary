using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Test {
  public class TestContext {
    #region [Property: Phase]
    private string phase;

    public string Phase {
      get { return this.phase; }
      set { this.phase = value; }
    }
    #endregion

    #region [Property: DelayedActions]
    private readonly Collection<TestDelay> delayedActions;

    public Collection<TestDelay> DelayedActions {
      get { return this.delayedActions; }
    }
    #endregion


    #region [Method: Constructor]
    public TestContext() {
      this.delayedActions = new Collection<TestDelay>();
    }
    #endregion

    #region [Method: Reset]
    public void Reset() {
      this.phase = null;
      this.delayedActions.Clear();
    }
    #endregion
  }
}
