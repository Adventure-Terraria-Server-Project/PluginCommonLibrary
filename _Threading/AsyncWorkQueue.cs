using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.Plugins.CoderCow {
  using CompletedWorkItem = Tuple<WorkItem,object>;

  public class AsyncWorkQueue: IDisposable {
    #region [Property: IsBusy]
    public bool IsBusy {
      get {
        lock (this.currentWorkTaskLock) {
          return (this.currentWorkTask != null);
        }
      }
    }
    #endregion

    private List<WorkItem> enqueuedWorkItems;
    private readonly object enqueuedWorkItemsLock = new object();
    private Task currentWorkTask;
    private readonly object currentWorkTaskLock = new object();
    private List<CompletedWorkItem> nonAsyncCompletedWorkItems;
    private readonly object nonAsyncCompletedWorkItemsLock = new object();
    private CancellationTokenSource cancellationTokenSource;


    #region [Method: Constructor]
    public AsyncWorkQueue() {
      this.enqueuedWorkItems = new List<WorkItem>();
      this.nonAsyncCompletedWorkItems = new List<CompletedWorkItem>();
    }
    #endregion

    #region [Methods: EnqueueItem, FinishAll, CancelAll]
    public void EnqueueItem(WorkItem workItem) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);
      Contract.Requires<ArgumentNullException>(workItem != null);

      lock (this.enqueuedWorkItemsLock) {
        this.enqueuedWorkItems.Add(workItem);
      }

      if (!this.IsBusy) {
        Contract.Assert(this.currentWorkTask == null);
        this.cancellationTokenSource = new CancellationTokenSource();
        this.currentWorkTask = Task.Factory.StartNew(
          () => this.ProcessWorkItems(this.cancellationTokenSource.Token), this.cancellationTokenSource.Token
        );
      }
    }

    public void FinishAll() {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      lock (this.currentWorkTaskLock) {
        if (this.currentWorkTask != null) {
          this.currentWorkTask.Wait();

          if (this.currentWorkTask.Exception != null)
            throw this.currentWorkTask.Exception;
        }
      }
    }

    public void CancelAll() {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);

      this.cancellationTokenSource.Cancel();
      lock (this.currentWorkTaskLock) {
        if (this.currentWorkTask != null) {
          this.currentWorkTask.Wait();

          if (this.currentWorkTask.Exception != null)
            throw this.currentWorkTask.Exception;
        }
      }
      this.enqueuedWorkItems.Clear();
    }
    #endregion

    #region [Method: ProcessWorkItems]
    private void ProcessWorkItems(CancellationToken token) {
      List<WorkItem> workItems;
      
      lock (this.enqueuedWorkItemsLock) {
        if (this.enqueuedWorkItems.Count == 0) {
          lock (this.currentWorkTaskLock) {
            this.currentWorkTask = null;
            return;
          }
        }

        token.ThrowIfCancellationRequested();
        workItems = this.enqueuedWorkItems;
        this.enqueuedWorkItems = new List<WorkItem>();
      }

      foreach (WorkItem workItem in workItems) {
        token.ThrowIfCancellationRequested();

        object result;
        try {
          result = workItem.Execute();
        } catch (Exception ex) {
          throw new InvalidOperationException(
            "The method bound to a work item's execute delegate has thrown an exception. See inner exception for details.", ex
          );
        }

        token.ThrowIfCancellationRequested();
        if (workItem.Callback != null) {
          if (workItem.CallbackIsAsync) {
            try {
              workItem.Callback(result);
            } catch (Exception ex) {
              throw new InvalidOperationException(
                "The method bound to a work item's callback delegate has thrown an exception. See inner exception for details.", ex
              );
            }
          } else {
            lock (this.nonAsyncCompletedWorkItemsLock) {
              this.nonAsyncCompletedWorkItems.Add(new CompletedWorkItem(workItem, result));
            }
          }
        }
      }

      // New work items might already have been enqueued while we processed this set of items.
      this.ProcessWorkItems(token);
    }
    #endregion

    #region [Method: HandleGameUpdate]
    private int frameCounter;
    public void HandleGameUpdate() {
      if (this.frameCounter++ == 5) {
        this.frameCounter = 0;
        return;
      }

      List<CompletedWorkItem> workItems;
      lock (this.nonAsyncCompletedWorkItemsLock) {
        if (this.nonAsyncCompletedWorkItems.Count == 0)
          return;

        workItems = this.nonAsyncCompletedWorkItems;
        this.nonAsyncCompletedWorkItems = new List<CompletedWorkItem>();
      }

      foreach (CompletedWorkItem workItem in workItems) {
        try {
          workItem.Item1.Callback(workItem.Item2);
        } catch (Exception ex) {
          throw new InvalidOperationException(
            "The method bound to a work item's callback delegate has thrown an exception. See inner exception for details.", ex
          );
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
    
      if (isDisposing) {
        try {
          this.FinishAll();
        } catch {
          this.CancelAll();
        }
      }
    
      this.isDisposed = true;
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
