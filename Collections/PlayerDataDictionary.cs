using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using TerrariaApi.Server;

using TShockAPI;

namespace Terraria.Plugins.Common.Collections {
  /// <summary>
  ///   Stores and manages a plugin's per Player data.
  /// </summary>
  /// <typeparam name="DataType"></typeparam>
  public class PlayerDataDictionary<DataType>: IEnumerable<DataType>, IDisposable {
    private readonly List<DataType> dataList;
    private readonly TerrariaPlugin owningPlugin;
    private readonly bool addAfterLogin;
    protected Func<int,DataType> PlayerDataFactoryFunction { get; private set; }
    public object SyncRoot { get; private set; }

    public DataType this[int playerIndex] {
      get { return this.dataList[playerIndex]; }
      set {
        while (playerIndex >= this.dataList.Count)
          this.dataList.Add(default(DataType));

        this.dataList[playerIndex] = value;
      }
    }

    public DataType this[TSPlayer tsPlayer] {
      get { return this[tsPlayer.Index]; }
      set { this[tsPlayer.Index] = value; }
    }

    public int Count => this.dataList.Count;


    public PlayerDataDictionary(
      TerrariaPlugin plugin, Func<int,DataType> playerDataFactoryFunction, bool addPlayersAfterLoginOnly = true, bool registerHooks = false
    ) {
      Contract.Requires<ArgumentNullException>(plugin != null);
      Contract.Requires<ArgumentNullException>(playerDataFactoryFunction != null);

      this.dataList = new List<DataType>(new DataType[100]);
      this.owningPlugin = plugin;
      this.addAfterLogin = addPlayersAfterLoginOnly;
      this.PlayerDataFactoryFunction = playerDataFactoryFunction;
      this.SyncRoot = new object();

      if (registerHooks) {
        if (addPlayersAfterLoginOnly)
          TShockAPI.Hooks.PlayerHooks.PlayerPostLogin += this.TShock_PlayerPostLogin;
        else
          ServerApi.Hooks.ServerJoin.Register(plugin, this.Server_Join);

        ServerApi.Hooks.ServerLeave.Register(plugin, this.Server_Leave);
      }
    }

    public virtual void HandlePlayerJoin(int playerIndex) {
      if (this.IsDisposed || this.addAfterLogin)
        return;

      this.AddPlayerData(playerIndex);
    }

    public virtual void HandlePlayerPostLogin(TSPlayer player) {
      if (this.IsDisposed || !this.addAfterLogin)
        return;

      this.AddPlayerData(player.Index);
    }

    public virtual void HandleLeave(int playerIndex) {
      if (this.IsDisposed)
        return;

      lock (this.SyncRoot)
        this[playerIndex] = default(DataType);
    }

    private bool AddPlayerData(int playerIndex) {
      lock (this.SyncRoot) {
        DataType data;
        try {
          data = this.PlayerDataFactoryFunction(playerIndex);
        } catch (AggregateException ex) {
          throw new PlayerDataCreationException("The player data factory function has thrown an exception:\n" + ex);
        }

        this[playerIndex] = data;
        return true;
      }
    }

    private void Server_Join(JoinEventArgs e) {
      this.HandlePlayerJoin(e.Who);
    }

    private void Server_Leave(LeaveEventArgs e) {
      this.HandleLeave(e.Who);
    }

    private void TShock_PlayerPostLogin(TShockAPI.Hooks.PlayerPostLoginEventArgs e) {
      this.HandlePlayerPostLogin(e.Player);
    }

    public int IndexOf(DataType playerData) {
      return this.dataList.IndexOf(playerData);
    }

    public bool Contains(DataType playerData) {
      return this.dataList.Contains(playerData);
    }

    #region [IEnumerable Implementation]
    public IEnumerator<DataType> GetEnumerator() {
      return this.dataList.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.dataList.GetEnumerator();
    }
    #endregion

    #region [IDisposable Implementation]
    private bool isDisposed;
    public bool IsDisposed => this.isDisposed;

    protected virtual void Dispose(bool isDisposing) {
      if (this.isDisposed)
        return;
    
      if (isDisposing) {
        ServerApi.Hooks.ServerJoin.Deregister(this.owningPlugin, this.Server_Join);
        ServerApi.Hooks.ServerLeave.Deregister(this.owningPlugin, this.Server_Leave);
        TShockAPI.Hooks.PlayerHooks.PlayerPostLogin -= this.TShock_PlayerPostLogin;
      }
    
      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~PlayerDataDictionary() {
      this.Dispose(false);
    }
    #endregion
  }
}
