using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Terraria.ID;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public class TerrariaItems {
    public bool IsValidType(int itemType) {
      return (itemType >= TerrariaUtils.ItemType_Min && itemType <= TerrariaUtils.ItemType_Max);
    }

    private static bool[] craftableItemTypes;
    public bool IsCraftableType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");

      if (TerrariaItems.craftableItemTypes == null) {
        TerrariaItems.craftableItemTypes = new bool[TerrariaUtils.ItemType_Max + 1 + Math.Abs(TerrariaUtils.ItemType_Min)];

        for (int i = 0; i < Main.recipe.Length; i++) {
          Recipe recipe = Main.recipe[i];
          if (recipe == null)
            continue;

          int index = recipe.createItem.netID;
          if (index < 0)
            index += TerrariaUtils.ItemType_Max;

          TerrariaItems.craftableItemTypes[index] = true;
        }
      }
      
      {
        int index = (int)itemType;
        if (index < 0)
          index += TerrariaUtils.ItemType_Max;

        return TerrariaItems.craftableItemTypes[index];
      }
    }

    private static bool[] equipableItemTypes;
    public bool IsEquipableType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.equipableItemTypes == null) {
        TerrariaItems.equipableItemTypes = new bool[TerrariaUtils.ItemType_Max + 1 + Math.Abs(TerrariaUtils.ItemType_Min)];

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (
            !string.IsNullOrEmpty(dummyItem.name) && !dummyItem.vanity && (
              dummyItem.headSlot != -1 || dummyItem.bodySlot != -1 || dummyItem.legSlot != -1 || dummyItem.accessory
            )
          ) {
            int index = i;
            if (index < 0)
              index += TerrariaUtils.ItemType_Max;

            TerrariaItems.equipableItemTypes[index] = true;
          }
        }
      }

      {
        int index = (int)itemType;
        if (index < 0)
          index += TerrariaUtils.ItemType_Max;

        return TerrariaItems.equipableItemTypes[index];
      }
    }

    private static List<ItemType> ammoTypes;
    public bool IsAmmoType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.ammoTypes == null) {
        TerrariaItems.ammoTypes = new List<ItemType>(20);

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.ammo > 0)
            TerrariaItems.ammoTypes.Add((ItemType)i);
        }
      }

      return TerrariaItems.ammoTypes.Contains(itemType);
    }

    private static List<ItemType> weaponTypes;
    public bool IsWeaponType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.weaponTypes == null) {
        TerrariaItems.weaponTypes = new List<ItemType>(20);

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.damage > 0 && dummyItem.ammo == 0)
            TerrariaItems.weaponTypes.Add((ItemType)i);
        }
      }

      return TerrariaItems.weaponTypes.Contains(itemType);
    }

    private static List<ItemType> accessoryTypes;
    public bool IsAccessoryType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.accessoryTypes == null) {
        TerrariaItems.accessoryTypes = new List<ItemType>(20);

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.accessory)
            TerrariaItems.accessoryTypes.Add((ItemType)i);
        }
      }

      return TerrariaItems.accessoryTypes.Contains(itemType);
    }

    private static List<ItemType> vanityTypes;
    public bool IsVanityType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.vanityTypes == null) {
        TerrariaItems.vanityTypes = new List<ItemType>(20);

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.vanity)
            TerrariaItems.vanityTypes.Add((ItemType)i);
        }
      }

      return TerrariaItems.vanityTypes.Contains(itemType);
    }

    private static List<ItemType> rareTypes;
    public bool IsRareType(ItemType itemType) {
      if ((int)itemType < TerrariaUtils.ItemType_Min || (int)itemType > TerrariaUtils.ItemType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", itemType), "itemType");
      
      if (TerrariaItems.rareTypes == null) {
        TerrariaItems.rareTypes = new List<ItemType>(20);

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.rare > 0)
            TerrariaItems.rareTypes.Add((ItemType)i);
        }
      }

      return TerrariaItems.rareTypes.Contains(itemType);
    }

    private static ItemType[][] blockTypesItemTypes;
    public ItemType GetItemTypeFromBlockType(BlockType blockType, int objectStyle = 0) {
      if ((int)blockType < TerrariaUtils.BlockType_Min || (int)blockType > TerrariaUtils.BlockType_Max)
        throw new ArgumentException(string.Format("The given block type {0} is invalid.", blockType), "blockType");

      if (TerrariaItems.blockTypesItemTypes == null) {
        TerrariaItems.blockTypesItemTypes = new ItemType[TerrariaUtils.ItemType_Max + 1][];

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.createTile != -1) {
            ItemType[] styleArray = TerrariaItems.blockTypesItemTypes[dummyItem.createTile];
            ItemType[] newStyleArray;

            if (styleArray != null) {
              newStyleArray = new ItemType[Math.Max(dummyItem.placeStyle + 1, styleArray.Length)];
              styleArray.CopyTo(newStyleArray, 0);
            } else {
              newStyleArray = new ItemType[dummyItem.placeStyle + 1];
            }

            newStyleArray[dummyItem.placeStyle] = (ItemType)i;
            TerrariaItems.blockTypesItemTypes[dummyItem.createTile] = newStyleArray;
          }
        }
      }

      {
        switch (blockType) {
          case BlockType.Mannequin:
            return ItemType.Mannequin;
          case BlockType.DartTrap:
            return ItemType.DartTrap;
          case BlockType.IceBlock:
            return ItemType.None;
          default: {
            ItemType[] styleArray = TerrariaItems.blockTypesItemTypes[(int)blockType];
            Contract.Assert(styleArray != null, "BlockType: " + blockType.ToString());

            if (objectStyle >= styleArray.Length)
              throw new ArgumentException(string.Format("There is no item type for block \"{0}\" with object style {1}", blockType, objectStyle));

            return styleArray[objectStyle];
          }
        }
      }
    }

    private static ItemType[] wallTypesItemTypes;
    public ItemType GetItemTypeFromWallType(WallType wallType) {
      if ((int)wallType < TerrariaUtils.WallType_Min || (int)wallType > TerrariaUtils.WallType_Max)
        throw new ArgumentException(string.Format("The given item type {0} is invalid.", wallType), "wallType");

      if (TerrariaItems.wallTypesItemTypes == null) {
        TerrariaItems.wallTypesItemTypes = new ItemType[TerrariaUtils.WallType_Max + 1];

        for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.WallType_Max + 1; i++) {
          Item dummyItem = new Item();
          dummyItem.netDefaults(i);
          if (!string.IsNullOrEmpty(dummyItem.name) && dummyItem.createWall != -1)
            TerrariaItems.wallTypesItemTypes[dummyItem.createWall] = (ItemType)i;
        }
      }

      return TerrariaItems.wallTypesItemTypes[(int)wallType];
    }

    public bool IsCoinType(ItemType itemType) {
      switch (itemType) {
        case ItemType.CopperCoin:
        case ItemType.SilverCoin:
        case ItemType.GoldCoin:
        case ItemType.PlatinumCoin:
          return true;
      }

      return false;
    }

    public int GetHigherTierCoinType(int itemType) {
      if (itemType == ItemID.CopperCoin)
        return ItemID.SilverCoin;
      else if (itemType == ItemID.SilverCoin)
        return ItemID.GoldCoin;
      else if (itemType == ItemID.GoldCoin)
        return ItemID.PlatinumCoin;

      return -1;
    }

    public string GetItemName(ItemData itemData, bool includePrefix = false) {
      string name = "";
      if ((itemData.Prefix != ItemPrefix.None && includePrefix) || itemData.Type < 0)
        name += "<" + this.GetItemPrefixName(itemData.Prefix) + "> ";

      name += Main.itemName[(int)itemData.Type];
      return name;
    }

    public string GetItemName(ItemType itemType) {
      return Main.itemName[(int)itemType];
    }

    public string GetItemRepresentativeString(ItemData itemData) {
      string format = "{0}";
      if (itemData.StackSize > 1)
        format = "{0} ({1})";

      return string.Format(format, this.GetItemName(itemData, true), itemData.StackSize);
    }

    public string GetItemPrefixName(ItemPrefix prefix) {
      return Lang.prefix[(int)prefix];
    }

    public int CreateNew(TSPlayer forPlayer, DPoint location, ItemData itemData, bool sendPacket = true) {
      int itemIndex = Item.NewItem(
        location.X, location.Y, 0, 0, (int)itemData.Type, itemData.StackSize, true, (int)itemData.Prefix
      );

      if (sendPacket)
        forPlayer.SendData(PacketTypes.ItemDrop, string.Empty, itemIndex);

      return itemIndex;
    }

    public IEnumerable<Item> EnumerateItemsInRect(Rectangle rect) {
      int areaL = rect.Left - (rect.Width / 2);
      int areaT = rect.Top - (rect.Height / 2);
      int areaR = rect.Left + (rect.Width / 2);
      int areaB = rect.Top + (rect.Height / 2);

      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (
          item.active &&
          item.position.X > areaL && item.position.X < areaR &&
          item.position.Y > areaT && item.position.Y < areaB
        ) {
          yield return item;
        }
      }
    }

    public IEnumerable<Item> EnumerateItemsAroundPoint(DPoint location, int radius) {
      for (int i = 0; i < 200; i++) {
        Item item = Main.item[i];

        if (
          item.active && 
          Math.Sqrt(Math.Pow(item.position.X - location.X, 2) + Math.Pow(item.position.Y - location.Y, 2)) <= radius
        )
          yield return item;
      }
    }

    public bool IsUniversalPrefix(ItemPrefix prefix) {
      switch (prefix) {
        case ItemPrefix.Keen:
        case ItemPrefix.Superior:
        case ItemPrefix.Forceful:
        case ItemPrefix.Broken:
        case ItemPrefix.Damaged:
        case ItemPrefix.Shoddy:
        case ItemPrefix.Hurtful:
        case ItemPrefix.Strong:
        case ItemPrefix.Unpleasant:
        case ItemPrefix.Weak:
        case ItemPrefix.Ruthless:
        case ItemPrefix.Godly:
        case ItemPrefix.Demonic:
        case ItemPrefix.Zealous:
          return true;
      }

      return false;
    }

    public bool IsCommonPrefix(ItemPrefix prefix) {
      switch (prefix) {
        case ItemPrefix.Quick:
        case ItemPrefix.Quick2:
        case ItemPrefix.Deadly:
        case ItemPrefix.Deadly2:
        case ItemPrefix.Agile:
        case ItemPrefix.Nimble:
        case ItemPrefix.Murderous:
        case ItemPrefix.Slow:
        case ItemPrefix.Sluggish:
        case ItemPrefix.Lazy:
        case ItemPrefix.Annoying:
        case ItemPrefix.Nasty:
          return true;
      }

      return false;
    }

    [Conditional("DEBUG")]
    public void DebugPrintItemIdList() {
      for (int i = TerrariaUtils.ItemType_Min; i < TerrariaUtils.ItemType_Max + 1; i++) {
        Item dummyItem = new Item();
        dummyItem.netDefaults(i);

        string itemName;
        if (string.IsNullOrEmpty(dummyItem.name)) {
          itemName = "NullOrEmpty";
        } else {
          itemName = dummyItem.name;
          itemName = itemName.Replace(" ", "");
          itemName = itemName.Replace("'", "");
          itemName = itemName.Replace("´", "");
          itemName = itemName.Replace("`", "");
        }

        Debug.Print(itemName + " = " + i + ',');
      }
    }
  }
}
