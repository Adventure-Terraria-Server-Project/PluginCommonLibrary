using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Terraria.Plugins.Common {
  public class TaskExceptionEventArgs: EventArgs {
    #region [Property: Task]
    private readonly Task task;

    public Task Task {
      get { return this.task; }
    }
    #endregion

    #region [Property: TaskState]
    private readonly object taskState;

    public object TaskState {
      get { return this.taskState; }
    }
    #endregion


    #region [Method: Constructor]
    public TaskExceptionEventArgs(Task task, object taskState = null) {
      this.task = task;
      this.taskState = taskState;
    }
    #endregion
  }
}