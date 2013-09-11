using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Collections {
  public class PriorityList<T>: Collection<T> {
    protected List<int> Priorities { get; private set; }


    public PriorityList() {
      this.Priorities = new List<int>();
    }

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
  }
}
