using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Plugins.Common {
  public class AsyncWorkQueue: IDisposable {
    private struct WorkItem {
      public static readonly WorkItem Invalid = default(WorkItem);

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

      #region [Property: Function]
      private readonly Func<object,object> function;

      public Func<object,object> Function {
        get { return this.function; }
      }
      #endregion

      #region [Property: State]
      private readonly object state;

      public object State {
        get { return this.state; }
      }
      #endregion


      #region [Method: Constructor]
      public WorkItem(
        TaskCompletionSource<object> completionSource, CancellationToken? cancellationToken, Func<object,object> function,
        object state = null
      ) {
        Contract.Requires<ArgumentNullException>(completionSource != null);
        Contract.Requires<ArgumentNullException>(function != null);

        this.completionSource = completionSource;
        this.cancellationToken = cancellationToken;
        this.function = function;
        this.state = state;
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
    public Task<object> EnqueueTask(
      Func<object,object> function, object state = null, CancellationToken? cancellationToken = null
    ) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      var completionSource = new TaskCompletionSource<object>();
      this.queuedItems.Add(new WorkItem(completionSource, cancellationToken, function, state));
      
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
      try {
        WorkItem currentItem;
        while (this.queuedItems.TryTake(out currentItem, this.workerTimeoutMs, this.workerTokenSource.Token)) {
          if (currentItem.Equals(WorkItem.Invalid))
            throw new InvalidOperationException("Can not process an invalid work item.");

          if (currentItem.CancellationToken.HasValue && currentItem.CancellationToken.Value.IsCancellationRequested) {
            currentItem.CompletionSource.SetCanceled();
          } else {
            try {
              object result = currentItem.Function(currentItem.State);
              currentItem.CompletionSource.SetResult(result); // Signal completed
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
      } catch (OperationCanceledException) {
        // Thrown when "this.workerTokenSource.Token" token is set.
      } catch (Exception ex) {
        Debug.WriteLine("Async Work Queue worker has thrown an unexpected exception: \n" + ex);
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
