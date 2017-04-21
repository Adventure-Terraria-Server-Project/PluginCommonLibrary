using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using TShockAPI;

namespace Terraria.Plugins.Common.Collections {
  /// <summary>
  ///   Simulate inventory handling on an items collection. 
  ///   It will automatically stack items up and down, create new slots or remove them - all the basic inventory 
  ///   goodness.
  /// </summary>
  public class Inventory {
    private readonly bool specificPrefixes;
    public IList<ItemData> Items { get; }

    public Inventory(ItemsAdapter items, bool specificPrefixes = true): this(items as IList<ItemData>, specificPrefixes) {
      Contract.Requires<ArgumentNullException>(items != null);
    }

    /// <param name="specificPrefixes">if set to <c>true</c> items will considered equal if their prefixes match. set to <c>false</c> to ignore prefixes.</param>
    public Inventory(IList<ItemData> items, bool specificPrefixes = true) {
      Contract.Requires<ArgumentNullException>(items != null);

      this.Items = items;
      this.specificPrefixes = specificPrefixes;
    }

    public int Slots => this.Items.Count;

    /// <summary>
    ///   Removes one item and adds antoher one in a single transaction.
    /// </summary>
    /// <param name="itemToTake">The item to take.</param>
    /// <param name="itemToGive">The item to give.</param>
    public void Exchange(ItemData?[] updates, ItemData itemToTake, ItemData itemToGive) {
      Contract.Requires<ArgumentNullException>(updates != null);

      this.Remove(updates, (int)itemToTake.Type, itemToTake.StackSize, (int)itemToTake.Prefix);
      this.Add(updates, (int)itemToGive.Type, itemToGive.StackSize, (int)itemToGive.Prefix);
    }

    public ItemData?[] Add(ItemData item) { 
      var updates = new ItemData?[this.Items.Count];
      this.Add(updates, (int)item.Type, item.StackSize, (int)item.Prefix);

      return updates;
    }

    public void Add(ItemData?[] updates, ItemData item) => this.Add(updates, (int)item.Type, item.StackSize, (int)item.Prefix);

    public void Add(ItemData?[] updates, int itemType, int stack, int prefixType = -1) {
      Contract.Requires<ArgumentNullException>(updates != null);
      Contract.Requires<ArgumentOutOfRangeException>(updates.Length == this.Items.Count);
      Contract.Requires<ArgumentException>(stack > 0);

      Item itemInfo = new Item();
      itemInfo.netDefaults(itemType);

      if (stack > itemInfo.maxStack)
        throw new ArgumentException("Stack size can not be greater than the maximum.");

      // would never need more than one empty slot
      var emptySlotIndex = -1;
      bool isStackable = itemInfo.maxStack != 1;
      // except platinum coins because there is no special stacking with them
      bool isCoin = TerrariaUtils.Items.IsCoinType(itemType) && itemType != ItemID.PlatinumCoin;
      // this variable is just to measure whether the item fits at all
      int remainingStack = stack;
      for (int i = 0; i < this.Items.Count; i++) {
        ItemData invItem = updates[i] ?? this.Items[i];

        // the slot is either marked to be removed by the updates or is currently empty and not going to be filled by the updates
        if (invItem == ItemData.None) {
          bool isPreferred = false;
          var itemsAdapter = this.Items as ItemsAdapter;
          if (itemsAdapter != null)
            isPreferred = itemsAdapter.PreferredSlotIndexes(invItem.Type).Contains(i);

          emptySlotIndex = (isPreferred || emptySlotIndex == -1) ? i : emptySlotIndex;
          if (!isStackable)
            break;
        } else if (isStackable && (int)invItem.Type == itemType) {
          int newStack = Math.Min(invItem.StackSize + remainingStack, itemInfo.maxStack);
          int stackToAdd = newStack - invItem.StackSize;
          remainingStack -= stackToAdd;

          // a special case are coins if a stack is full
          if (isCoin && newStack == itemInfo.maxStack)
            updates[i] = new ItemData(TerrariaUtils.Items.GetHigherTierCoinType(itemType));
          else
            updates[i] = new ItemData(invItem.Type, newStack);

          Contract.Assert(remainingStack >= 0);
          if (remainingStack == 0 && !isCoin)
            return;
        }
      }

      if (remainingStack > 0) {
        if (emptySlotIndex == -1)
          throw new InvalidOperationException("Inventory is full.");

        updates[emptySlotIndex] = new ItemData(prefixType, itemType, remainingStack);
      }
    }

    public ItemData?[] Remove(ItemData item) {
      var updates = new ItemData?[this.Items.Count];
      this.Remove(updates, (int)item.Type, item.StackSize, (int)item.Prefix);

      return updates;
    }

    public void Remove(ItemData?[] updates, ItemData item) => this.Remove(updates, (int)item.Type, item.StackSize, (int)item.Prefix);

    public void Remove(ItemData?[] updates, int itemType, int stack, int prefixType = 0) {
      Contract.Requires<ArgumentNullException>(updates != null);
      Contract.Requires<ArgumentOutOfRangeException>(updates.Length == this.Items.Count);
      Contract.Requires<ArgumentException>(stack > 0);

      Item itemInfo = new Item();
      itemInfo.netDefaults(itemType);

      // would never need more than one empty slot
      var emptySlotIndex = -1;
      int higherTierCoinSlotIndex = -1;
      // except platinum coins because it is already the highest tier
      bool isCoin = TerrariaUtils.Items.IsCoinType(itemType) && itemType != ItemID.PlatinumCoin;
      int higherTierCoinType = TerrariaUtils.Items.GetHigherTierCoinType(itemType);
      int remainingStack = stack;

      for (int i = 0; i < this.Items.Count; i++) {
        ItemData invItem = updates[i] ?? this.Items[i];

        // the slot is either marked to be removed by the updates or is currently empty and not going to be filled by the updates
        if (invItem == ItemData.None) {
          bool isPreferred = false;
          var itemsAdapter = this.Items as ItemsAdapter;
          if (itemsAdapter != null)
            isPreferred = itemsAdapter.PreferredSlotIndexes(invItem.Type).Contains(i);

          emptySlotIndex = (isPreferred || emptySlotIndex == -1) ? i : emptySlotIndex;
        // in case we are working with coins here, we might need one stack of the next higher tier coin type later
        } else if (isCoin && invItem.Type == higherTierCoinType) {
          higherTierCoinSlotIndex = higherTierCoinSlotIndex != -1 ? higherTierCoinSlotIndex : i;
        } else if (invItem.Type == itemType && (!specificPrefixes || invItem.Prefix == prefixType)) {
          int stackReduce = Math.Min(remainingStack, invItem.StackSize);
          if (stackReduce == invItem.StackSize)
            emptySlotIndex = i; // prefer the slots which would be emptied over already empty slots

          updates[i] = new ItemData(invItem.Prefix, invItem.Type, invItem.StackSize - stackReduce);
          remainingStack -= stackReduce;

          Contract.Assert(remainingStack >= 0);
          if (remainingStack == 0 && !isCoin)
            break;
        }
      }

      if (remainingStack > 0) {
        // special handling for coins, e.g. if there's 30 silver left after decrementing all existing silver stacks, then
        // try to decrement a gold coin stack by one and add the remaining silver as a new stack
        if (isCoin && higherTierCoinSlotIndex != -1 && emptySlotIndex != -1) {
          ItemData higherTierItem = updates[higherTierCoinSlotIndex] ?? this.Items[higherTierCoinSlotIndex];
          updates[higherTierCoinSlotIndex] = new ItemData(higherTierItem.Type, higherTierItem.StackSize - 1);
          updates[emptySlotIndex] = new ItemData(itemType, 100 - remainingStack);
          return;
        }

        throw new InvalidOperationException("The inventory doesn't contain enough items to remove.");
      }
    }

    public void ApplyUpdates(ItemData?[] updates) {
      Contract.Requires<ArgumentNullException>(updates != null);

      ApplyUpdatesTo(updates, this.Items);
    }

    public static void ApplyUpdatesTo(ItemData?[] updates, IList<ItemData> destinationInventory) {
      Contract.Requires<ArgumentNullException>(updates != null);

      // apply all the changes
      for (int i = 0; i < updates.Length; i++) {
        ItemData? updateItem = updates[i];
        if (updateItem != null) {
          if (updateItem.Value.StackSize > 0)
            destinationInventory[i] = updateItem.Value;
          else
            destinationInventory[i] = ItemData.None;
        }
      }
    }

    public int Amount(int itemId) => (from item in this.Items where (int)item.Type == itemId select item.StackSize).Sum();
    public int FilledSlotCount() => this.Items.Count((item) => item.StackSize > 0);
    public int EmptySlotCount() => this.Items.Count((item) => item.StackSize == 0);
  }
}
