using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using Hooks;

using TShockAPI;

namespace Terraria.Plugins.Common.Collections {
  public class PlayerDataDictionary<DataType>: Dictionary<Player,DataType>, IDisposable {
    #region [Property: PlayerDataFactoryFunction]
    private Func<Player,DataType> playerDataFactoryFunction;

    protected Func<Player,DataType> PlayerDataFactoryFunction {
      get { return this.playerDataFactoryFunction; }
    }
    #endregion

    #region [Properties: Indexers]
    public DataType this[TSPlayer tsPlayer] {
      get {
        if (this.addAfterLogin && !tsPlayer.IsLoggedIn)
          throw new ArgumentException("The player to retrieve data for is not logged in.");
        
        return base[tsPlayer.TPlayer];
      }
      set { base[tsPlayer.TPlayer] = value; }
    }

    public new DataType this[Player player] {
      get { return this[TShockEx.GetPlayerByTPlayer(player)]; }
      set { base[player] = value; }
    }
    #endregion

    #region [Property: SyncRoot]
    private readonly object syncRoot;

    public object SyncRoot {
      get { return this.syncRoot; }
    }
    #endregion

    private readonly bool addAfterLogin;


    #region [Method: Constructor]
    public PlayerDataDictionary(
      Func<Player,DataType> playerDataFactoryFunction, bool addPlayersAfterLoginOnly = true, bool registerHooks = false
    ): base(20) {
      Contract.Requires<ArgumentNullException>(playerDataFactoryFunction != null);

      this.playerDataFactoryFunction = playerDataFactoryFunction;
      this.addAfterLogin = addPlayersAfterLoginOnly;
      this.syncRoot = new object();

      if (registerHooks) {
        if (addPlayersAfterLoginOnly)
          TShockAPI.Hooks.PlayerHooks.PlayerPostLogin += this.TShock_PlayerPostLogin;
        else
          NetHooks.GreetPlayer += this.HandleGreetPlayer;

        ServerHooks.Leave += this.HandleLeave;
      }
    }
    #endregion

    #region [Methods: Hooks Handles, AddPlayerData]
    public virtual void HandlePlayerPostLogin(TSPlayer player) {
      if (this.IsDisposed || !this.addAfterLogin)
        return;

      this.AddPlayerData(player.TPlayer);
    }

    public virtual void HandleGreetPlayer(int playerIndex, HandledEventArgs e) {
      if (this.IsDisposed || (this.addAfterLogin && TShock.Config.DisableLoginBeforeJoin))
        return;

      Player tPlayer = Main.player[playerIndex];
      TSPlayer tsPlayer = TShockEx.GetPlayerByTPlayer(tPlayer);
      if (this.addAfterLogin && !tsPlayer.IsLoggedIn)
        return;

      this.AddPlayerData(tPlayer);
    }

    public virtual void HandleLeave(int playerIndex) {
      if (this.IsDisposed)
        return;

      lock (this.SyncRoot) {
        this.Remove(Main.player[playerIndex]);
        Debug.WriteLine("PlayerDataDictionary: {0} players.", this.Count);
      }
    }

    private bool AddPlayerData(Player tPlayer) {
      lock (this.SyncRoot) {
        if (base.ContainsKey(tPlayer))
          return false;

        DataType data = default(DataType);
        try {
          data = this.PlayerDataFactoryFunction(tPlayer);
        } catch (AggregateException ex) {
          throw new PlayerDataCreationException("The player data factory function has thrown an exception:\n" + ex);
        }

        this.Add(tPlayer, data);
        Debug.WriteLine("PlayerDataDictionary: {0} players.", this.Count);

        return true;
      }
    }

    private void TShock_PlayerPostLogin(TShockAPI.Hooks.PlayerPostLoginEventArgs e) {
      this.HandlePlayerPostLogin(e.Player);
    }
    #endregion

    #region [Methods: TSPlayer Wrappers]
    public void Add(TSPlayer player, DataType data) {
      base.Add(player.TPlayer, data);
    }

    public void Remove(TSPlayer player) {
      base.Remove(player.TPlayer);
    }

    public bool ContainsKey(TSPlayer player) {
      return base.ContainsKey(player.TPlayer);
    }

    public bool TryGetValue(TSPlayer player, out DataType data) {
      return base.TryGetValue(player.TPlayer, out data);
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
    
      if (isDisposing) {
        NetHooks.GreetPlayer -= this.HandleGreetPlayer;
        ServerHooks.Leave -= this.HandleLeave;
        TShockAPI.Hooks.PlayerHooks.PlayerPostLogin -= this.TShock_PlayerPostLogin;

        if (this.addAfterLogin) {
          try {
          // May occur due to unsynchronous thread operations.
          } catch (InvalidOperationException) {}
        }
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
