using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Terraria.Plugins.Common {
  public class TaskExceptionEventArgs: EventArgs {
    public Task Task { get; private set; }
    public object TaskState { get; private set; }


    public TaskExceptionEventArgs(Task task, object taskState = null) {
      this.Task = task;
      this.TaskState = taskState;
    }
  }
}