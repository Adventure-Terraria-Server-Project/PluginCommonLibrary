using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Collections {
  public class DecayingCollection<T>: ICollection<T>, ICollection {
    #region [Nested: DecayingItem]
    private struct DecayingItem {
      public T ItemValue { get; private set; }
      public DateTime TimeOfDeath { get; private set; }


      public DecayingItem(T itemValue, TimeSpan lifeTime = default(TimeSpan)): this() {
        this.ItemValue = itemValue;

        if (lifeTime == TimeSpan.Zero)
          this.TimeOfDeath = DateTime.MaxValue;
        else
          this.TimeOfDeath = DateTime.UtcNow + lifeTime;
      }

      public bool IsDeath() {
        return (DateTime.UtcNow > this.TimeOfDeath);
      }

      public void SetLifeTime(TimeSpan lifeTime) {
        this.TimeOfDeath = DateTime.UtcNow + lifeTime;
      }

      public override int GetHashCode() {
        return this.ItemValue.GetHashCode();
      }

      public bool Equals(DecayingItem other) {
        return this.ItemValue.Equals(other.ItemValue);
      }

      public override bool Equals(object obj) {
        if (!(obj is DecayingItem))
          return false;

        return this.Equals((DecayingItem)obj);
      }

      public static bool operator ==(DecayingItem a, DecayingItem b) {
        return a.Equals(b);
      }

      public static bool operator !=(DecayingItem a, DecayingItem b) {
        return !a.Equals(b);
      }
    }
    #endregion

    #region [Nested: Enumerator]
    public class Enumerator: IEnumerator<T> {
      private readonly DecayingCollection<T> collection; 
      private readonly int originVersion;

      private LinkedListNode<DecayingItem> currentNode;

      public T Current {
        get {
          if (this.currentNode == null)
            return default(T);

          return this.currentNode.Value.ItemValue;
        }
      }

      object IEnumerator.Current {
        get { return this.Current; }
      }



      internal Enumerator(DecayingCollection<T> collection) {
        Contract.Requires<ArgumentNullException>(collection != null);

        this.collection = collection;
        this.originVersion = collection.version;
        (this as IEnumerator).Reset();
      }

      public bool MoveNext() {
        if (this.originVersion != this.collection.version)
          throw new InvalidOperationException("Collection was changed during the enumeration.");

        if (this.currentNode == null) {
          if (this.collection.internalList.Count == 0)
            return false;

          this.currentNode = this.collection.internalList.First;
          if (!this.currentNode.Value.IsDeath())
            return true;
        }

        while (true) {
          this.currentNode = this.currentNode.Next;
          if (this.currentNode == null)
            return false;

          if (this.currentNode.Value.IsDeath())
            continue;

          return true;
        }
      }

      void IEnumerator.Reset() {
        if (this.originVersion != this.collection.version)
          throw new InvalidOperationException("Collection was changed during the enumeration.");

        this.currentNode = null;
      }

      public void Dispose() {}
    }
    #endregion

    private readonly LinkedList<DecayingItem> internalList;
    private int version;
    public TimeSpan DefaultItemLifeTime { get; private set; }


    public DecayingCollection(TimeSpan defaultItemLifeTime = default(TimeSpan)) {
      this.internalList = new LinkedList<DecayingItem>();
      this.DefaultItemLifeTime = defaultItemLifeTime;
    }

    public void Add(T item, TimeSpan lifeTime) {
      this.internalList.AddLast(new LinkedListNode<DecayingItem>(new DecayingItem(item, lifeTime)));
      this.version++;
    }

    public void Add(T item) {
      this.Add(item, this.DefaultItemLifeTime);
    }

    private bool Remove(DecayingItem decayingItem) {
      bool result = this.internalList.Remove(decayingItem);
      this.version++;

      return result;
    }

    public bool Remove(T item) {
      return this.Remove(new DecayingItem(item, TimeSpan.Zero));
    }

    public bool Contains(T item) {
      foreach (T collectionItem in this)
        if (collectionItem.Equals(item))
          return true;

      return false;
    }

    public void Clear() {
      this.internalList.Clear();
      this.version++;
    }

    private bool ResetLifetimeInternal(T item, TimeSpan lifeTime) {
      LinkedListNode<DecayingItem> currentNode = this.internalList.First;
      while (currentNode != null) {
        if (!currentNode.Value.IsDeath() && currentNode.Value.ItemValue.Equals(item)) {
          DecayingItem decayingItem = currentNode.Value;
          decayingItem.SetLifeTime(lifeTime);
          currentNode.Value = decayingItem;
          return true;
        }

        currentNode = currentNode.Next;
      }

      return false;
    }

    public void ResetLifetime(T item, TimeSpan lifeTime) {
      if (!this.ResetLifetimeInternal(item, lifeTime))
        throw new ArgumentException("The item does not exist.", "item");
    }

    public void ResetLifetime(T item) {
      this.ResetLifetime(item, this.DefaultItemLifeTime);
    }

    public void ResetLifeTimeOrAddItem(T item, TimeSpan lifeTime) {
      if (!this.ResetLifetimeInternal(item, lifeTime))
        this.Add(item, lifeTime);
    }

    public void ResetLifeTimeOrAddItem(T item) {
      this.ResetLifeTimeOrAddItem(item, this.DefaultItemLifeTime);
    }

    public IEnumerable<T> EnumerateAndDeleteDeathItems() {
      LinkedListNode<DecayingItem> currentNode = this.internalList.First;
      while (currentNode != null) {
        if (currentNode.Value.IsDeath())
          this.Remove(currentNode.Value);
        else
          yield return currentNode.Value.ItemValue;

        currentNode = currentNode.Next;
      }
    }

    public void Cleanup() {
      if (this.internalList.Count == 0)
        return;

      foreach (T item in this.EnumerateAndDeleteDeathItems()) {}
    }

    public IEnumerator<T> GetEnumerator() {
      return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public void CopyTo(T[] array, int arrayIndex) {
      throw new NotImplementedException();
    }

    public void CopyTo(Array array, int index) {
      throw new NotImplementedException();
    }

    public int Count {
      get {
        int count = 0;

        LinkedListNode<DecayingItem> currentNode = this.internalList.First;
        while (currentNode != null) {
          if (!currentNode.Value.IsDeath())
            count++;

          currentNode = currentNode.Next;
        }

        return count;
      }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public object SyncRoot {
      get { return (this.internalList as ICollection).SyncRoot; }
    }

    public bool IsSynchronized {
      get { return (this.internalList as ICollection).IsSynchronized; }
    }

    public override string ToString() {
      return this.internalList.ToString();
    }
  }
}