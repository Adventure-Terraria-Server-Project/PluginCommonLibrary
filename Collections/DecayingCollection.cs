using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common.Collections {
  public class DecayingCollection<T>: ICollection<T>, ICollection {
    #region [Nested: DecayingItem struct]
    private struct DecayingItem {
      #region [Property: Value]
      private readonly T itemValue;

      public T ItemValue {
        get { return this.itemValue; }
      }
      #endregion

      #region [Property: TimeOfDeath]
      private DateTime timeOfDeath;

      public DateTime TimeOfDeath {
        get { return this.timeOfDeath; }
      }
      #endregion


      #region [Method: Constructor]
      public DecayingItem(T itemValue, TimeSpan lifeTime = default(TimeSpan)) {
        this.itemValue = itemValue;

        if (lifeTime == TimeSpan.Zero)
          this.timeOfDeath = DateTime.MaxValue;
        else
          this.timeOfDeath = DateTime.UtcNow + lifeTime;
      }
      #endregion

      #region [Methods: IsDeath, SetLifeTime]
      public bool IsDeath() {
        return (DateTime.UtcNow > this.timeOfDeath);
      }

      public void SetLifeTime(TimeSpan lifeTime) {
        this.timeOfDeath = DateTime.UtcNow + lifeTime;
      }
      #endregion

      #region [Methods: GetHashCode, Equals, ==, !=]
      public override int GetHashCode() {
        return this.itemValue.GetHashCode();
      }

      public bool Equals(DecayingItem other) {
        return this.itemValue.Equals(other.itemValue);
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
      #endregion
    }
    #endregion

    #region [Nested: Enumerator class]
    public class Enumerator: IEnumerator<T> {
      private readonly DecayingCollection<T> collection; 
      private readonly int originVersion;

      #region [Property: Current]
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
      #endregion

      #region [Method: Constructor]
      internal Enumerator(DecayingCollection<T> collection) {
        Contract.Requires<ArgumentNullException>(collection != null);

        this.collection = collection;
        this.originVersion = collection.version;
        (this as IEnumerator).Reset();
      }
      #endregion

      #region [Methods: MoveNext, Reset]
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
      #endregion

      public void Dispose() {}
    }
    #endregion

    private readonly LinkedList<DecayingItem> internalList;
    private int version;

    #region [Property: DefaultItemLifeTime]
    private TimeSpan defaultItemLifeTime;

    public TimeSpan DefaultItemLifeTime {
      get { return this.defaultItemLifeTime; }
      set { this.defaultItemLifeTime = value; }
    }
    #endregion

    #region [Properties: Count, IsReadOnly]
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
    #endregion

    #region [Properties: SyncRoot, IsSynchronized]
    public object SyncRoot {
      get { return (this.internalList as ICollection).SyncRoot; }
    }

    public bool IsSynchronized {
      get { return (this.internalList as ICollection).IsSynchronized; }
    }
    #endregion


    #region [Method: Constructor]
    public DecayingCollection(TimeSpan defaultItemLifeTime = default(TimeSpan)) {
      this.internalList = new LinkedList<DecayingItem>();
      this.defaultItemLifeTime = defaultItemLifeTime;
    }
    #endregion

    #region [Methods: Add, Remove, Contains, Clear]
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
    #endregion

    #region [Methods: ResetLifetime, ResetLifeTimeOrAddItem]
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
    #endregion

    #region [Methods: EnumerateAndDeleteDeathItems, Cleanup]
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
    #endregion

    #region [Methods: GetEnumerator, CopyTo]
    public IEnumerator<T> GetEnumerator() {
      return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public void CopyTo(T[] array, int arrayIndex) {
      throw new NotImplementedException();
    }
    #endregion

    #region [Method: ToString]
    public override string ToString() {
      return this.internalList.ToString();
    }
    #endregion
  }
}