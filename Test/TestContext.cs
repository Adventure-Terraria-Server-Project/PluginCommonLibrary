using System;
using System.Collections.ObjectModel;

namespace Terraria.Plugins.Common.Test {
  public class TestContext {
    public string Phase { get; set; }
    public Collection<TestDelay> DelayedActions { get; private set; }


    public TestContext() {
      this.DelayedActions = new Collection<TestDelay>();
    }

    public void Reset() {
      this.Phase = null;
      this.DelayedActions.Clear();
    }
  }
}
