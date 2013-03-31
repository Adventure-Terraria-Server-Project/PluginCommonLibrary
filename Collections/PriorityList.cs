using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Collections {
  public class PriorityList<T>: Collection<T> {
    #region [Property: Priorities]
    private readonly List<int> priorities;

    protected List<int> Priorities {
      get { return this.priorities; }
    }
    #endregion


    #region [Method: Constructor]
    public PriorityList() {
      this.priorities = new List<int>();
    }
    #endregion

    #region [Methods: Add, InsertItem, GetPriority]
    public void Add(T item, int priority) {
      int index;
      for (index = 0; index < this.Count; index++) {
        if (this.Priorities[index] < priority)
          break;
      }

      this.Priorities.Insert(index, priority);
    }

    protected override void InsertItem(int index, T item) {
      if (index != this.Count)
        throw new InvalidOperationException("Item insertion is not supported by this collection.");
      
      this.Add(item, 0);
    }

    public int GetPriorityAt(int index) {
      return this.Priorities[index];
    }
    #endregion
  }
}
