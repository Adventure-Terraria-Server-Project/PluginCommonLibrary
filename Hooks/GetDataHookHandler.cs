using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Newtonsoft.Json.Serialization;
using Terraria.ID;
using TerrariaApi.Server;
using TShockAPI;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common.Hooks {
  public class GetDataHookHandler: IDisposable {
    public TerrariaPlugin Plugin { get; private set; }
    public bool InvokeTileEditOnChestKill { get; set; }
    public bool InvokeTileOnObjectPlacement { get; set; }

    #region [Event: TileEdit]
    public event EventHandler<TileEditEventArgs> TileEdit;

    protected virtual bool OnTileEdit(TileEditEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.TileEdit != null)
          this.TileEdit(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("TileEdit", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ObjectPlacement]
    public event EventHandler<ObjectPlacementEventArgs> ObjectPlacement;

    protected virtual bool OnObjectPlacement(ObjectPlacementEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ObjectPlacement != null)
          this.ObjectPlacement(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ObjectPlacement", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestPlace]
    public event EventHandler<ChestPlaceEventArgs> ChestPlace;

    protected virtual bool OnChestPlace(ChestPlaceEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestPlace != null)
          this.ChestPlace(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestPlace", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestOpen]
    public event EventHandler<ChestOpenEventArgs> ChestOpen;

    protected virtual bool OnChestOpen(ChestOpenEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestOpen != null)
          this.ChestOpen(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestOpen", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestRename]
    public event EventHandler<ChestRenameEventArgs> ChestRename;

    protected virtual bool OnChestRename(ChestRenameEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestRename != null)
          this.ChestRename(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestRename", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestKill]
    public event EventHandler<TileLocationEventArgs> ChestKill;

    protected virtual bool OnChestKill(TileLocationEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestKill != null)
          this.ChestKill(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestKill", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestGetContents]
    public event EventHandler<TileLocationEventArgs> ChestGetContents;

    protected virtual bool OnChestGetContents(TileLocationEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestGetContents != null)
          this.ChestGetContents(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestGetContents", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestModifySlot]
    public event EventHandler<ChestModifySlotEventArgs> ChestModifySlot;

    protected virtual bool OnChestModifySlot(ChestModifySlotEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.ChestModifySlot != null)
          this.ChestModifySlot(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestModifySlot", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: SignEdit]
    public event EventHandler<SignEditEventArgs> SignEdit;

    protected virtual bool OnSignEdit(SignEditEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.SignEdit != null)
          this.SignEdit(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("SignEdit", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: SignRead]
    public event EventHandler<TileLocationEventArgs> SignRead;

    protected virtual bool OnSignRead(TileLocationEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.SignRead != null)
          this.SignRead(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("SignRead", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: HitSwitch]
    public event EventHandler<TileLocationEventArgs> HitSwitch;

    protected virtual bool OnHitSwitch(TileLocationEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.HitSwitch != null)
          this.HitSwitch(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("HitSwitch", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: BossSpawn]
    public event EventHandler<BossSpawnEventArgs> BossSpawn;

    protected virtual bool OnBossSpawn(BossSpawnEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.BossSpawn != null)
          this.BossSpawn(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("BossSpawn", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ItemUpdate]
    public event EventHandler<ItemUpdateEventArgs> ItemUpdate;

    protected virtual bool OnItemUpdate(ItemUpdateEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.ItemUpdate != null)
          this.ItemUpdate(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ItemUpdate", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ItemOwner]
    public event EventHandler<ItemOwnerEventArgs> ItemOwner;

    protected virtual bool OnItemOwner(ItemOwnerEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.ItemOwner != null)
          this.ItemOwner(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ItemOwner", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: QuickStackNearby]
    public event EventHandler<PlayerSlotEventArgs> QuickStackNearby;

    protected virtual bool OnQuickStackNearby(PlayerSlotEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.QuickStackNearby != null)
          this.QuickStackNearby(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("QuickStackNearby", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: PlayerModifySlot]
    public event EventHandler<PlayerModifySlotEventArgs> PlayerModifySlot;

    protected virtual bool OnPlayerModifySlot(PlayerModifySlotEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.PlayerModifySlot != null)
          this.PlayerModifySlot(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("PlayerModifySlot", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: LiquidSet]
    public event EventHandler<LiquidSetEventArgs> LiquidSet;

    protected virtual bool OnLiquidSet(LiquidSetEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.LiquidSet != null)
          this.LiquidSet(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("LiquidSet", ex);
      }

      return e.Handled;
    }
    #endregion
    
    #region [Event: DoorUse]
    public event EventHandler<DoorUseEventArgs> DoorUse;

    protected virtual bool OnDoorUse(DoorUseEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.DoorUse != null)
          this.DoorUse(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("DoorUse", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: PlayerSpawn]
    public event EventHandler<PlayerSpawnEventArgs> PlayerSpawn;

    protected virtual bool OnPlayerSpawn(PlayerSpawnEventArgs e) {
      try {
        if (this.PlayerSpawn != null)
          this.PlayerSpawn(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("PlayerSpawn", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChestUnlock]
    public event EventHandler<TileLocationEventArgs> ChestUnlock;

    protected virtual bool OnChestUnlock(TileLocationEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChestUnlock != null)
          this.ChestUnlock(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChestUnlock", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: ChatText]
    public event EventHandler<ChatTextEventArgs> ChatText;

    protected virtual bool OnChatText(ChatTextEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.ChatText != null)
          this.ChatText(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ChatText", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: SendTileSquare]
    public event EventHandler<SendTileSquareEventArgs> SendTileSquare;

    protected virtual bool OnSendTileSquare(SendTileSquareEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.SendTileSquare != null)
          this.SendTileSquare(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("SendTileSquare", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: TilePaint]
    public event EventHandler<TilePaintEventArgs> TilePaint;

    protected virtual bool OnTilePaint(TilePaintEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.TilePaint != null)
          this.TilePaint(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("TilePaint", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: PlayerDeath]
    public event EventHandler<PlayerDeathEventArgs> PlayerDeath;

    protected virtual bool OnPlayerDeath(PlayerDeathEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.PlayerDeath != null)
          this.PlayerDeath(this, e);
      }
      catch (Exception ex) {
        this.ReportEventHandlerException("PlayerDeath", ex);
      }

      return e.Handled;
    }
    #endregion

    #region [Event: Teleport]
    public event EventHandler<TeleportEventArgs> Teleport;

    protected virtual bool OnTeleport(TeleportEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      try {
        if (this.Teleport != null)
          this.Teleport(this, e);
      }
      catch (Exception ex) {
        this.ReportEventHandlerException("Teleport", ex);
      }

      return e.Handled;
    }
    #endregion


    public GetDataHookHandler(TerrariaPlugin plugin, bool invokeTileEditOnChestKill = false, int hookPriority = 0) {
      Contract.Requires<ArgumentNullException>(plugin != null);

      this.Plugin = plugin;
      this.InvokeTileEditOnChestKill = invokeTileEditOnChestKill;

      ServerApi.Hooks.NetGetData.Register(plugin, this.NetHooks_GetData, hookPriority);
    }

    private void NetHooks_GetData(GetDataEventArgs e) {
      if (e == null || this.isDisposed || e.Handled)
        return;

      TSPlayer player = TShock.Players[e.Msg.whoAmI];
      if (player == null)
        return;

      try {
        switch (e.MsgID) {
          case PacketTypes.Tile: {
            if (this.TileEdit == null)
              break;

            int editType = e.Msg.readBuffer[e.Index];
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y) || editType > 14)
              return;

            int blockType = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 5);
            int objectStyle = e.Msg.readBuffer[e.Index + 6];

            e.Handled = this.OnTileEdit(
              new TileEditEventArgs(player, (TileEditType)editType, new DPoint(x, y), (BlockType)blockType, objectStyle
            ));
            break;
          }
          case PacketTypes.PlaceObject: {
            if (this.ObjectPlacement == null && this.TileEdit == null)
              break;

            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              return;

            int blockType = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 4);
            int objectStyle = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 6);
            int alternative = e.Msg.readBuffer[e.Index + 8];
            int random = ((sbyte) e.Msg.readBuffer[e.Index + 9]);
            bool direction = BitConverter.ToBoolean(e.Msg.readBuffer, e.Index + 10);

            if (this.InvokeTileOnObjectPlacement) {
              e.Handled = this.OnTileEdit(
                new TileEditEventArgs(player, TileEditType.PlaceTile, new DPoint(x, y), (BlockType)blockType, objectStyle
              ));
            }

            if (!e.Handled) { 
              e.Handled = this.OnObjectPlacement(
                new ObjectPlacementEventArgs(player, new DPoint(x, y), (BlockType)blockType, objectStyle, alternative, random, direction
              ));
            }

            break;
          }
          // Note: As for TileKill and TileKillNoItem, blockId will be of "1" if the player attempted to destroy
          // a tile but didn't succeed yet, and will be of "0" as the tile is actually destroyed.
          // However, there's one exception with Chests, they will never send their actual destroy packet, except a hack
          // tool is used, it seems.
          case PacketTypes.TileKill: {
            int type = e.Msg.readBuffer[e.Index];
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              break;

            int style = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 5);

            if (type == 0 || type == 2) { // Chest placement / Dresser Placement
              e.Handled = this.OnChestPlace(new ChestPlaceEventArgs(player, new DPoint(x, y), (ChestStyle)style));
            } else { // Chest kill
              int tileType = TerrariaUtils.Tiles[x, y].type;
              if (tileType != TileID.Containers && tileType != TileID.Dressers)
                break;

              if (this.InvokeTileEditOnChestKill)
                e.Handled = this.OnTileEdit(new TileEditEventArgs(player, TileEditType.TileKill, new DPoint(x, y), 0, 0));

              if (!e.Handled)
                e.Handled = this.OnChestKill(new TileLocationEventArgs(player, new DPoint(x, y)));
            }

            break;
          }
          case PacketTypes.ChestOpen: {
            if (this.ChestOpen == null && this.ChestRename == null)
              break;

            int chestIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 4);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              break;

            int nameLength = e.Msg.readBuffer[e.Index + 6];

            string newName = string.Empty;
            if ((nameLength > 0 && nameLength <= 20) || nameLength == 255) { // Name change requested?
              if (nameLength != 255)
                newName = Encoding.UTF8.GetString(e.Msg.readBuffer, e.Index + 7, nameLength + 1);

              e.Handled = this.OnChestRename(new ChestRenameEventArgs(player, chestIndex, newName));
              if (e.Handled) {
                int lastOpenedChestIndex = Main.player[player.Index].chest;

                // Close the chest, so that other players can view it.
                Main.player[player.Index].chest = -1;

                Chest lastOpenedChest = Main.chest[lastOpenedChestIndex];
                string oldName = lastOpenedChest.name;
                DPoint lastOpenedChestPos = new DPoint(lastOpenedChest.x, lastOpenedChest.y);
                player.SendData(PacketTypes.ChestName, oldName, lastOpenedChestIndex, lastOpenedChestPos.X, lastOpenedChestPos.Y);
              }
            }

            if (!e.Handled)
              e.Handled = this.OnChestOpen(new ChestOpenEventArgs(player, chestIndex, new DPoint(x, y)));

            break;
          }
          case PacketTypes.ChestGetContents: {
            if (this.ChestGetContents == null)
              break;
          
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active())
              return;
          
            e.Handled = this.OnChestGetContents(new TileLocationEventArgs(player, new DPoint(x, y)));
            break;
          }
          case PacketTypes.ChestItem: {
            if (this.ChestModifySlot == null)
              break;

            int chestIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int slotIndex = e.Msg.readBuffer[e.Index + 2];
            int itemStackSize = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);
            ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 5];
            ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 6);

            if (chestIndex >= Main.chest.Length || slotIndex > 39)
              break;
          
            e.Handled = this.OnChestModifySlot(new ChestModifySlotEventArgs(
              player, chestIndex, slotIndex, new ItemData(itemPrefix, itemType, itemStackSize)
            ));
            break;
          }
          case PacketTypes.SignNew: {
            if (this.SignEdit == null)
              break;

            int signIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 4);
            string newText;
            using (MemoryStream stream = new MemoryStream(e.Msg.readBuffer, e.Index + 6, e.Length - 7))
              newText = new BinaryReader(stream).ReadString();

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active())
              return;
              
            e.Handled = this.OnSignEdit(new SignEditEventArgs(player, signIndex, new DPoint(x, y), newText));
            break;
          }
          case PacketTypes.SignRead: {
            if (this.SignRead == null)
              break;

            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              break;

            e.Handled = this.OnSignRead(new TileLocationEventArgs(player, new DPoint(x, y)));
            break;
          }
          case PacketTypes.HitSwitch: {
            if (this.HitSwitch == null)
              break;

            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
      
            if (!TerrariaUtils.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active())
              return;

            // For some reason, TShock doesn't handle this packet so we just do our own checks.
            if (TShock.CheckIgnores(player))
              return;
            if (TShock.CheckRangePermission(player, x, y, 32))
              return;
      
            e.Handled = this.OnHitSwitch(new TileLocationEventArgs(player, new DPoint(x, y)));
            break;
          }
          case PacketTypes.SpawnBossorInvasion: {
            if (this.BossSpawn == null)
              break;

            //int playerIndex = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
            int bossType = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            e.Handled = this.OnBossSpawn(new BossSpawnEventArgs(player, (BossType)bossType));
            break;
          }
          case PacketTypes.ItemDrop:
          case PacketTypes.UpdateItemDrop: { // ItemDrop2
            if (this.ItemUpdate == null)
              break;

            int itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            float x = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 2);
            float y = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 6);
            float velocityX = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 10);
            float velocityY = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 14);
            int itemStackSize = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 18);
            ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 20];
            bool noDelay = (e.Msg.readBuffer[e.Index + 21] != 0);
            ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 22);

            // If it is actually an item pick up, then ensure a valid item index.
            if (itemType == 0 && (itemIndex < 0 || itemIndex >= Main.item.Length))
              break;

            e.Handled = this.OnItemUpdate(new ItemUpdateEventArgs(
              player, itemIndex, new Vector2(x, y), new Vector2(velocityX, velocityY), 
              noDelay, new ItemData(itemPrefix, itemType, itemStackSize)
            ));
            break;
          }
          case PacketTypes.ItemOwner: {
            if (this.ItemOwner == null)
              break;

            int itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int newOwnerPlayerIndex = e.Msg.readBuffer[e.Index + 2];
            TSPlayer newOwner;
            if (newOwnerPlayerIndex < 255)
              newOwner = TShock.Players[newOwnerPlayerIndex];
            else 
              break;

            e.Handled = this.OnItemOwner(new ItemOwnerEventArgs(player, itemIndex, newOwner));
            break;
          }
          case PacketTypes.ForceItemIntoNearestChest: { // QuickStackNearby
            if (this.QuickStackNearby == null)
              break;

            int slotIndex = e.Msg.readBuffer[e.Index];
            if (slotIndex >= TSPlayer.Server.TPlayer.inventory.Length)
              break;

            e.Handled = this.OnQuickStackNearby(new PlayerSlotEventArgs(player, slotIndex));
            if (e.Handled) {
              // NOTE: The client seems to require this packet in order to keep functioning properly!
              NetMessage.SendData(5, -1, -1, "", player.Index, slotIndex, Main.player[player.Index].inventory[slotIndex].prefix);
            }

            break;
          }
          case PacketTypes.PlayerSlot: {
            if (this.PlayerModifySlot == null)
              break;

            //byte playerIndex = e.Msg.readBuffer[e.Index];
            int slotIndex = e.Msg.readBuffer[e.Index + 1];
            int itemStackSize = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
            ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 4];
            ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 5);

            Player tServerPlayer = TSPlayer.Server.TPlayer;
            if (slotIndex >= tServerPlayer.inventory.Length + tServerPlayer.bank.item.Length + tServerPlayer.bank2.item.Length)
              break;
          
            e.Handled = this.OnPlayerModifySlot(new PlayerModifySlotEventArgs(
              player, slotIndex, new ItemData(itemPrefix, itemType, itemStackSize)
            ));
            break;
          }
          case PacketTypes.LiquidSet: {
            if (this.LiquidSet == null)
              break;

            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              break;

            int liquidAmount = e.Msg.readBuffer[e.Index + 4];
            LiquidKind liquidKind = (LiquidKind)e.Msg.readBuffer[e.Index + 5];

            e.Handled = this.OnLiquidSet(new LiquidSetEventArgs(player, new DPoint(x, y), liquidAmount, liquidKind));
            break;
          }
          case PacketTypes.DoorUse: {
            if (this.DoorUse == null)
              break;

            byte action = e.Msg.readBuffer[e.Index];
            int x = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            int y = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);

            if (!TerrariaUtils.Tiles.IsValidCoord(x, y))
              break;

            int direction = e.Msg.readBuffer[e.Index + 5];

            Direction actualDirection = Direction.Right;
            if (direction == 0)
              actualDirection = Direction.Left;

            e.Handled = this.OnDoorUse(new DoorUseEventArgs(player, new DPoint(x, y), (DoorAction)action, actualDirection));
            break;
          }
          case PacketTypes.PlayerSpawn: {
            if (this.PlayerSpawn == null)
              break;
            
            int playerIndex = e.Msg.readBuffer[e.Index];
            int spawnX = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            int spawnY = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);

            if (!TerrariaUtils.Tiles.IsValidCoord(spawnX, spawnY))
              break;

            e.Handled = this.OnPlayerSpawn(new PlayerSpawnEventArgs(player, new DPoint(spawnX, spawnY)));
            break;
          }
          // Note: Also door unlock
          case PacketTypes.ChestUnlock: {
            if (this.ChestUnlock == null)
              break;

            UnlockType unlockType = (UnlockType)e.Msg.readBuffer[e.Index];
            int chestX = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            int chestY = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 3);

            if (!TerrariaUtils.Tiles.IsValidCoord(chestX, chestY))
              break;

            e.Handled = this.OnChestUnlock(new UnlockEventArgs(player, new DPoint(chestX, chestY), unlockType));
            break;
          }
          case PacketTypes.ChatText: {
            if (this.ChatText == null)
              break;

            short playerIndex = e.Msg.readBuffer[e.Index];
            if (playerIndex != e.Msg.whoAmI)
              break;

            int colorR = e.Msg.readBuffer[e.Index + 1];
            int colorG = e.Msg.readBuffer[e.Index + 2];
            int colorB = e.Msg.readBuffer[e.Index + 3];
            string text = Encoding.UTF8.GetString(e.Msg.readBuffer, e.Index + 4, e.Length - 5);

            e.Handled = this.OnChatText(new ChatTextEventArgs(player, new Color(colorR, colorG, colorB), text));
            break;
          }
          case PacketTypes.TileSendSquare: {
            if (this.SendTileSquare == null)
              break;

            int size = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int tileX = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
            int tileY = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 4);

            if (!TerrariaUtils.Tiles.IsValidCoord(tileX, tileY))
              break;

            e.Handled = this.OnSendTileSquare(new SendTileSquareEventArgs(player, new DPoint(tileX, tileY), size));
            break;
          }
          case PacketTypes.PaintTile: {
            if (this.TilePaint == null)
              break;

            int tileX = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
            int tileY = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);

            if (!TerrariaUtils.Tiles.IsValidCoord(tileX, tileY))
              break;

            int color = e.Msg.readBuffer[e.Index + 8];

            e.Handled = this.OnTilePaint(new TilePaintEventArgs(player, new DPoint(tileX, tileY), (PaintColor)color));
            break;
          }
          case PacketTypes.PlayerKillMe: {
            if (this.PlayerDeath == null)
              break;

            int playerIndex = e.Msg.readBuffer[e.Index];
            int direction = e.Msg.readBuffer[e.Index + 1];
            int dmg = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 2);
            bool pvp = e.Msg.readBuffer[e.Index + 4] != 0;
            string deathText = Encoding.UTF8.GetString(e.Msg.readBuffer, e.Index + 6, e.Length - 7);

            e.Handled = this.OnPlayerDeath(new PlayerDeathEventArgs(player, direction, dmg, pvp, deathText));
            break;
          }
          case PacketTypes.Teleport: {
            if (this.Teleport == null)
              break;

            BitsByte flags = e.Msg.readBuffer[e.Index];
					  int playerIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 1);
            float x = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 3);
            float y = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 7);
					  Vector2 destLocation = new Vector2(x, y);

            TeleportType tpType = TeleportType.PlayerToPos;
					  if (flags[0])
						  tpType = TeleportType.NpcToPos;
					  if (flags[1]) {
						  if (flags[0])
                tpType = TeleportType.Unknown;
              else
                tpType = TeleportType.PlayerNearPlayerWormhole;
            }

            e.Handled = this.OnTeleport(new TeleportEventArgs(player, destLocation, tpType));
            break;
          }
        }
      } catch (Exception ex) {
        ServerApi.LogWriter.PluginWriteLine(
          this.Plugin, string.Format("Internal error on handling data packet {0}. Exception details: \n{1}", e.MsgID, ex), TraceLevel.Error
        );
      }
    }

    private void ReportEventHandlerException(string eventName, Exception exception) {
      ServerApi.LogWriter.PluginWriteLine(
        this.Plugin, string.Format("One {0} event handler has thrown an unexpected exception. Exception details:\n{1}", eventName, exception), TraceLevel.Error
      );
    }

    #region [IDisposable Implementation]
    private bool isDisposed;

    public bool IsDisposed {
      get { return this.isDisposed; } 
    }

    protected virtual void Dispose(bool isDisposing) {
      if (this.isDisposed)
        return;
    
      if (isDisposing)
        ServerApi.Hooks.NetGetData.Deregister(this.Plugin, this.NetHooks_GetData);
    
      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~GetDataHookHandler() {
      this.Dispose(false);
    }
    #endregion
  }
}
