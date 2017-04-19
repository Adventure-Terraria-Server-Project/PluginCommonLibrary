using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Terraria.Localization;
using TShockAPI;

namespace Terraria.Plugins.Common.Collections {
  public class PlayerItemsAdapter: ItemsAdapter {
    private static readonly HashSet<int> CoinSlots = new HashSet<int> {50, 51, 52, 53};
    private static readonly HashSet<int> AmmoSlots = new HashSet<int> {46, 47, 48, 49};
    private readonly int playerIndex;
    private readonly bool sendPacketOnChanges;

    /// <param name="sendPacketOnChanges">if set to <c>true</c> data packets about the inventory changes are sent to the player.</param>
    public PlayerItemsAdapter(int playerIndex, Item[] itemArray, int fromIndex = 0, int toIndex = 0, bool sendPacketOnChanges = true): base (itemArray, fromIndex, toIndex) {
      this.playerIndex = playerIndex;
      this.sendPacketOnChanges = sendPacketOnChanges;
    }

    public override ItemData this[int index] {
      get { return base[index]; }
      set {
        Item tItem = value.ToItem();
        this.itemArray[this.fromIndex + index] = tItem;
        
        if (this.sendPacketOnChanges)
          NetMessage.SendData((int)PacketTypes.PlayerSlot, this.playerIndex, -1, null, this.playerIndex, this.fromIndex + index, tItem.prefix);
      }
    }

    /// <inheritdoc />
    public override HashSet<int> PreferredSlotIndexes(ItemType itemType) {
      if (TerrariaUtils.Items.IsCoinType(itemType)) {
        return CoinSlots;
      } else if (TerrariaUtils.Items.IsAmmoType(itemType)) {
        return AmmoSlots;
      }
      
      return base.PreferredSlotIndexes(itemType);
    }
  }
}
