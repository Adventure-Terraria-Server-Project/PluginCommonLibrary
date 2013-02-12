using System;
using System.Data;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow {
  public class WorkItem {
    #region [Property: Execute]
    private readonly Func<object> execute;

    public Func<object> Execute {
      get { return this.execute; }
    }
    #endregion

    #region [Property: CallbackIsAsync]
    private readonly bool callbackIsAsync;

    public bool CallbackIsAsync {
      get { return this.callbackIsAsync; }
    }
    #endregion

    #region [Property: Callback]
    private readonly Action<object> callback;

    public Action<object> Callback {
      get { return this.callback; }
    }
    #endregion


    #region [Method: Constructor]
    public WorkItem(Func<object> execute, Action<object> callback = null, bool callbackIsAsync = true) {
      Contract.Requires<ArgumentNullException>(execute != null);

      this.execute = execute;
      this.callback = callback;
      this.callbackIsAsync = callbackIsAsync;
    }
    #endregion
  }
}
