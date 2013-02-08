// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics.Contracts;
using System.Text;

using Hooks;
using TShockAPI;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow.Hooks {
  public class NetHookHandler: IDisposable {
    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    public PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion

    #region [Property: InvokeTileEditOnChestKill]
    private bool invokeTileEditOnChestKill;

    public bool InvokeTileEditOnChestKill {
      get { return this.invokeTileEditOnChestKill; }
      set { this.invokeTileEditOnChestKill = value; }
    }
    #endregion

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

    #region [Event: ItemDropOrPickUp]
    public event EventHandler<ItemDropOrPickUpEventArgs> ItemDropOrPickUp;

    protected virtual bool OnItemDropOrPickUp(ItemDropOrPickUpEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);
      
      try {
        if (this.ItemDropOrPickUp != null)
          this.ItemDropOrPickUp(this, e);
      } catch (Exception ex) {
        this.ReportEventHandlerException("ItemDropOrPickUp", ex);
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


    #region [Method: Constructor]
    public NetHookHandler(PluginTrace pluginTrace, bool invokeTileEditOnChestKill = false) {
      Contract.Requires<ArgumentNullException>(pluginTrace != null);

      this.pluginTrace = pluginTrace;
      this.invokeTileEditOnChestKill = invokeTileEditOnChestKill;

      NetHooks.GetData += this.NetHooks_GetData;
    }
    #endregion

    #region [Method: NetHooks_GetData, GameHooks_PostInitialize]
    private void NetHooks_GetData(GetDataEventArgs e) {
      if (e == null || this.isDisposed || e.Handled)
        return;

      TSPlayer player = TShock.Players[e.Msg.whoAmI];
      if (player == null)
        return;
      
      switch (e.MsgID) {
        // Note: As for DestroyTile and DestroyTileNoItem, blockId will be of "1" if the player attempted to destroy
        // a tile but didn't succeed yet, and will be of "0" as the tile is actually destroyed.
        // However, there's one exception with Chests, they will never send their actual destroy packet, except a hack
        // tool is used, it seems.
        case PacketTypes.Tile: {
          if (this.TileEdit == null || e.Msg.readBuffer.Length - e.Index < 11)
            break;

          byte editType = e.Msg.readBuffer[e.Index];
          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 1);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 5);
          byte blockType = e.Msg.readBuffer[e.Index + 9];
          byte spriteStyle = e.Msg.readBuffer[e.Index + 10];

          if (!Terraria.Tiles.IsValidCoord(x, y) || editType > 6)
            return;

          e.Handled = this.OnTileEdit(
            new TileEditEventArgs(player, (TileEditType)editType, new DPoint(x, y), (BlockType)blockType, spriteStyle
          ));
          break;
        }
        // Note: TShock ensures that this packet reaches us here only if the tile to be killed is of type 21 (Chest)
        // OR the maximum amount of Chests has been placed on the map and the tile to be killed is of type 0 (Dirt) - for
        // whatever reason.
        // The packet is also sent if there are still items in the chest.
        case PacketTypes.TileKill: {
          if ((this.TileEdit == null && this.ChestKill == null) || e.Msg.readBuffer.Length - e.Index < 8)
            break;

          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);
          
          if (!Terraria.Tiles.IsValidCoord(x, y) || Terraria.Tiles[x, y].type == 0)
            break;
          
          if (this.InvokeTileEditOnChestKill)
            e.Handled = this.OnTileEdit(new TileEditEventArgs(player, TileEditType.DestroyTile, new DPoint(x, y), 0, 0));

          if (!e.Handled)
            e.Handled = this.OnChestKill(new TileLocationEventArgs(player, new DPoint(x, y)));

          break;
        }
        case PacketTypes.ChestGetContents: {
          if (this.ChestGetContents == null || e.Msg.readBuffer.Length - e.Index < 8)
            break;
          
          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);

          if (!Terraria.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active)
            return;
          
          e.Handled = this.OnChestGetContents(new TileLocationEventArgs(player, new DPoint(x, y)));
          break;
        }
        case PacketTypes.ChestItem: {
          if (this.ChestModifySlot == null || e.Msg.readBuffer.Length - e.Index < 7)
            break;

          short chestIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
          byte slotIndex = e.Msg.readBuffer[e.Index + 2];
          byte itemStackSize = e.Msg.readBuffer[e.Index + 3];
          ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 4];
          ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 5);

          if (chestIndex >= Main.chest.Length || slotIndex > 19)
            break;
          
          e.Handled = this.OnChestModifySlot(new ChestModifySlotEventArgs(
            player, chestIndex, slotIndex, itemPrefix, itemType, itemStackSize
          ));
          break;
        }
        case PacketTypes.SignNew: {
          if (this.SignEdit == null || e.Msg.readBuffer.Length - e.Index < 10)
            break;

          short signIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 2);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 6);
          string newText = Encoding.UTF8.GetString(e.Msg.readBuffer, e.Index + 10, e.Length - e.Index - 11);

          if (!Terraria.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active)
            return;

          e.Handled = this.OnSignEdit(new SignEditEventArgs(player, signIndex, new DPoint(x, y), newText));
          break;
        }
        case PacketTypes.SignRead: {
          if (this.SignRead == null || e.Msg.readBuffer.Length - e.Index < 8)
            break;

          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);

          e.Handled = this.OnSignRead(new TileLocationEventArgs(player, new DPoint(x, y)));
          break;
        }
        case PacketTypes.HitSwitch: {
          if (this.HitSwitch == null || e.Msg.readBuffer.Length - e.Index < 5)
            break;

          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);
      
          if (!Terraria.Tiles.IsValidCoord(x, y) || !Main.tile[x, y].active)
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
          if (this.BossSpawn == null || e.Msg.readBuffer.Length - e.Index < 8)
            break;

          //int playerIndex = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int bossType = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);

          e.Handled = this.OnBossSpawn(new BossSpawnEventArgs(player, (BossType)bossType));
          break;
        }
        case PacketTypes.ItemDrop: {
          if (this.ItemDropOrPickUp == null || e.Msg.readBuffer.Length - e.Index < 22)
            break;

          short itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
          float x = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 2);
          float y = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 6);
          float velocityX = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 10);
          float velocityY = BitConverter.ToSingle(e.Msg.readBuffer, e.Index + 14);
          byte itemStackSize = e.Msg.readBuffer[e.Index + 18];
          ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 19];
          ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 20);

          // If it is actually an item pick up, then ensure a valid item index.
          if (itemType == 0 && (itemIndex < 0 || itemIndex >= Main.item.Length))
            break;

          e.Handled = this.OnItemDropOrPickUp(new ItemDropOrPickUpEventArgs(
            player, itemIndex, new Vector2(x, y), new Vector2(velocityX, velocityY), itemPrefix, itemType, itemStackSize
          ));
          break;
        }
        case PacketTypes.ItemOwner: {
          if (this.ItemOwner == null || e.Msg.readBuffer.Length - e.Index < 3)
            break;

          short itemIndex = BitConverter.ToInt16(e.Msg.readBuffer, e.Index);
          byte newOwnerPlayerIndex = e.Msg.readBuffer[e.Index + 2];
          TSPlayer newOwner;
          if (newOwnerPlayerIndex == -1)
            newOwner = TSPlayer.Server;
          else if (newOwnerPlayerIndex < 255)
            newOwner = TShock.Players[newOwnerPlayerIndex];
          else 
            break;

          e.Handled = this.OnItemOwner(new ItemOwnerEventArgs(player, itemIndex, newOwner));
          break;
        }
        case PacketTypes.PlayerSlot: {
          if (this.PlayerModifySlot == null || e.Msg.readBuffer.Length - e.Index < 6)
            break;

          //byte playerIndex = e.Msg.readBuffer[e.Index];
          byte slotIndex = e.Msg.readBuffer[e.Index + 1];
          byte itemStackSize = e.Msg.readBuffer[e.Index + 2];
          ItemPrefix itemPrefix = (ItemPrefix)e.Msg.readBuffer[e.Index + 3];
          ItemType itemType = (ItemType)BitConverter.ToInt16(e.Msg.readBuffer, e.Index + 4);

          if (slotIndex > 59)
            break;
          
          e.Handled = this.OnPlayerModifySlot(new PlayerModifySlotEventArgs(
            player, slotIndex, itemPrefix, itemType, itemStackSize
          ));
          break;
        }
        case PacketTypes.LiquidSet: {
          if (this.LiquidSet == null || e.Msg.readBuffer.Length - e.Index < 10)
            break;

          int x = BitConverter.ToInt32(e.Msg.readBuffer, e.Index);
          int y = BitConverter.ToInt32(e.Msg.readBuffer, e.Index + 4);
          byte liquidAmount = e.Msg.readBuffer[e.Index + 8];
          byte lava = e.Msg.readBuffer[e.Index + 9];

          e.Handled = this.OnLiquidSet(new LiquidSetEventArgs(player, new DPoint(x, y), liquidAmount, lava != 0));
          break;
        }
      }
    }
    #endregion

    #region [Method: ReportEventHandlerException]
    private void ReportEventHandlerException(string eventName, Exception exception) {
      this.PluginTrace.WriteLineError(string.Format(
        "One {0} event handler has thrown an unexpected exception. Exception details:\n{1}", eventName, exception
      ));
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
    
      if (isDisposing)
        NetHooks.GetData -= this.NetHooks_GetData;
    
      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~NetHookHandler() {
      this.Dispose(false);
    }
    #endregion
  }
}
