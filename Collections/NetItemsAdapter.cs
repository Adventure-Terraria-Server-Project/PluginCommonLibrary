using System;
using System.Collections;
using System.Collections.Generic;
using TShockAPI;

namespace Terraria.Plugins.Common.Collections {
  /// <summary>
  ///   TShockAPI.NetItem[] to IList<ItemData> adapter.
  /// </summary>
  public class NetItemsAdapter: IList<ItemData> {
    private static volatile Dictionary<int,int[]> PreferredSlots;
    protected readonly NetItem[] itemArray;
    protected readonly int fromIndex;
    protected readonly int toIndex;

    public NetItemsAdapter(NetItem[] itemArray, int fromIndex = 0, int toIndex = 0) {
      if (itemArray == null) throw new ArgumentNullException();
      if (!(itemArray.Length > 0)) throw new ArgumentException();
      if (!(fromIndex <= toIndex)) throw new ArgumentOutOfRangeException();

      this.itemArray = itemArray;
      this.fromIndex = fromIndex;
      this.toIndex = toIndex > 0 ? toIndex : itemArray.Length - 1;
    }

    /// <summary>
    ///   Gets the indexes of the slot preferred for the given item type.
    /// </summary>
    /// <param name="itemType">Type of the item.</param>
    public virtual HashSet<int> PreferredSlotIndexes(int itemType) {
      return new HashSet<int>();
    }

    #region IList<ItemData> Implementation
    public int Count => (this.toIndex - this.fromIndex) + 1;
    public bool IsReadOnly => false;

    public virtual ItemData this[int index] {
      get { return ItemData.FromNetItem(this.itemArray[this.fromIndex + index]); }
      set { this.itemArray[this.fromIndex + index] = new NetItem((int)value.Type, value.StackSize, (byte)value.Prefix); }
    }

    public int IndexOf(ItemData item) {
      for (int i = 0; i < this.itemArray.Length; i++) {
        NetItem tItem = this.itemArray[0];
        if (tItem.NetId == (int)item.Type && tItem.Stack == item.StackSize && tItem.PrefixId == (int)item.Prefix)
          return i;
      }

      return -1;
    }

    public void Insert(int index, ItemData item) {
      throw new NotImplementedException();
    }

    public void RemoveAt(int index) {
      throw new NotImplementedException();
    }

    public IEnumerator<ItemData> GetEnumerator() {
      foreach (NetItem item in this.itemArray)
        yield return ItemData.FromNetItem(item);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public void Add(ItemData item) {
      throw new NotImplementedException();
    }

    public void Clear() {
      throw new NotImplementedException();
    }

    public bool Contains(ItemData item) {
      return this.IndexOf(item) != -1;
    }

    public void CopyTo(ItemData[] array, int arrayIndex) {
      for (int i = arrayIndex; i < Math.Min(array.Length, this.itemArray.Length); i++)
        array[i] = ItemData.FromNetItem(this.itemArray[i]);
    }

    public bool Remove(ItemData item) {
      throw new NotImplementedException();
    }
    #endregion
  }
}
