using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Plugins.Common {
  public class AsyncWorkQueue: IDisposable {
    private struct WorkItem {
      #region [Property: CompletionSource]
      private readonly TaskCompletionSource<object> completionSource;

      public TaskCompletionSource<object> CompletionSource {
        get { return this.completionSource; }
      }
      #endregion

      #region [Property: CancellationToken]
      private readonly CancellationToken? cancellationToken;

      public CancellationToken? CancellationToken {
        get { return this.cancellationToken; }
      }
      #endregion

      #region [Property: Action]
      private readonly Action action;

      public Action Action {
        get { return this.action; }
      }
      #endregion


      #region [Method: Constructor]
      public WorkItem(TaskCompletionSource<object> completionSource, CancellationToken? cancellationToken, Action action) {
        this.completionSource = completionSource;
        this.cancellationToken = cancellationToken;
        this.action = action;
      }
      #endregion
    }

    #region [Property: WorkerTimeoutMs]
    private readonly int workerTimeoutMs;

    public int WorkerTimeoutMs {
      get { return this.workerTimeoutMs; }
    }
    #endregion

    private Task worker;
    private CancellationTokenSource workerTokenSource;
    private readonly BlockingCollection<WorkItem> queuedItems;

    #region [Method: Constructor]
    public AsyncWorkQueue() {
      this.workerTimeoutMs = -1;
      this.queuedItems = new BlockingCollection<WorkItem>();

      this.StartWorker();
    }
    #endregion

    #region [Methods: EnqueueTask, AwaitAll, ProcessWorkItems]
    public Task EnqueueTask(Action action, object state = null, CancellationToken? cancellationToken = null) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      var completionSource = new TaskCompletionSource<object>(state);
      this.queuedItems.Add(new WorkItem(completionSource, cancellationToken, action));
      
      return completionSource.Task;
    }

    public void AwaitAll(TimeSpan timeout) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      this.AwaitAllInternal(timeout);
    }

    public void AwaitAll() {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      this.AwaitAllInternal(TimeSpan.FromMilliseconds(1)); // Infinite
    }

    private void AwaitAllInternal(TimeSpan timeout) {
      Task[] itemTasks = new Task[this.queuedItems.Count];
      int counter = 0;
      foreach (Task<object> itemTask in this.queuedItems.Select(item => item.CompletionSource.Task))
        itemTasks[counter++] = itemTask;

      Task.WaitAll(itemTasks, timeout);
    }

    private void StartWorker() {
      this.workerTokenSource = new CancellationTokenSource();
      this.worker = new Task(this.ProcessWorkItems, this.workerTokenSource.Token, TaskCreationOptions.LongRunning);
      this.worker.Start();
    }

    private void ProcessWorkItems() {
      WorkItem currentItem;
      while (this.queuedItems.TryTake(out currentItem, this.workerTimeoutMs, this.workerTokenSource.Token)) {
        if (currentItem.CancellationToken.HasValue && currentItem.CancellationToken.Value.IsCancellationRequested) {
          currentItem.CompletionSource.SetCanceled();
        } else {
          try {
            currentItem.Action();
            currentItem.CompletionSource.SetResult(null); // Completed
          } catch (OperationCanceledException ex) {
            if (ex.CancellationToken == currentItem.CancellationToken)
              currentItem.CompletionSource.SetCanceled();
            else
              currentItem.CompletionSource.SetException(ex);
          } catch (Exception ex) {
            currentItem.CompletionSource.SetException(ex);
          }
        }
      }
    }
    #endregion

    #region [IDisposable Implementation]
    private bool isDisposed;

    public bool IsDisposed {
      get { return this.isDisposed; } 
    }

    protected virtual void Dispose(bool isDisposing) {
      if (this.isDisposed)
        return;
    
      this.isDisposed = true;
      if (isDisposing) {
        if (this.workerTokenSource != null) {
          this.workerTokenSource.Cancel();

          try {
            this.worker.Wait(TimeSpan.FromSeconds(1));
          } catch (AggregateException) {}
        }
        
        if (this.workerTokenSource != null)
          this.workerTokenSource.Dispose();
        if (this.worker != null)
          this.worker.Dispose();
      }
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~AsyncWorkQueue() {
      this.Dispose(false);
    }
    #endregion
  }
}
