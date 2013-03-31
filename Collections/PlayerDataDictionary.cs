using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using Hooks;
using TShockAPI;

namespace Terraria.Plugins.CoderCow.Collections {
  public class PlayerDataDictionary<DataType>: Dictionary<Player,DataType>, IDisposable {
    #region [Property: ConstructDataFunction]
    private Func<Player, DataType> constructDataFunction;

    protected Func<Player,DataType> ConstructDataFunction {
      get { return this.constructDataFunction; }
    }
    #endregion

    #region [Property: Indexer(TSPlayer)]
    public DataType this[TSPlayer player] {
      get { return base[player.TPlayer]; }
      set { base[player.TPlayer] = value; }
    }
    #endregion

    #region [Property: SyncRoot]
    private readonly object syncRoot;

    public object SyncRoot {
      get { return this.syncRoot; }
    }
    #endregion

    #region [Event: PlayerLogin]
    public event EventHandler<PlayerEventArgs> PlayerLogin;

    protected virtual void OnPlayerLogin(PlayerEventArgs e) {
      Contract.Requires<ArgumentNullException>(e != null);

      if (this.PlayerLogin != null)
        this.PlayerLogin(this, e);
    }
    #endregion

    private readonly bool addAfterLogin;
    private readonly Command loginDetectCommand;


    #region [Method: Constructor]
    public PlayerDataDictionary(
      Func<Player,DataType> constructDataFunction, bool addPlayersAfterLoginOnly = true, bool registerHooks = false
    ): base(20) {
      Contract.Requires<ArgumentNullException>(constructDataFunction != null);

      this.constructDataFunction = constructDataFunction;
      this.addAfterLogin = addPlayersAfterLoginOnly;
      this.syncRoot = new object();

      if (addPlayersAfterLoginOnly) {
        this.loginDetectCommand = new Command(this.LoginDetectCommand_Exec, "login") {
          AllowServer = false, DoLog = false
        };
        Commands.ChatCommands.Add(this.loginDetectCommand);
      }
      if (registerHooks) {
        NetHooks.GreetPlayer += this.HandleGreetPlayer;
        ServerHooks.Leave += this.HandleLeave;
      }
    }
    #endregion

    #region [Methods: LoginDetectCommand_Exec, HandleGreetPlayer, HandleLeave, AddPlayerData]
    private void LoginDetectCommand_Exec(CommandArgs args) {
      if (this.IsDisposed || !this.addAfterLogin)
        return;
      // Did the login fail?
      if (!args.Player.IsLoggedIn)
        return;

      if (this.AddPlayerData(args.TPlayer))
        this.OnPlayerLogin(new PlayerEventArgs(args.TPlayer));
    }

    public virtual void HandleGreetPlayer(int playerIndex, HandledEventArgs e) {
      if (this.IsDisposed || (this.addAfterLogin && TShock.Config.DisableLoginBeforeJoin))
        return;

      Player tPlayer = Main.player[playerIndex];
      this.AddPlayerData(tPlayer);
      if (!TShock.Config.DisableLoginBeforeJoin)
        this.OnPlayerLogin(new PlayerEventArgs(tPlayer));
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
          data = this.ConstructDataFunction(tPlayer);
        } catch (AggregateException ex) {
          throw new PlayerDataCreationException("The construct data function delegate has thrown an exception:\n" + ex);
        }

        this.Add(tPlayer, data);
        Debug.WriteLine("PlayerDataDictionary: {0} players.", this.Count);

        return true;
      }
    }
    #endregion

    #region [Methods: Dictionary Wrappers]
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

        if (this.addAfterLogin) {
          try {
            if (Commands.ChatCommands.Contains(this.loginDetectCommand))
              Commands.ChatCommands.Remove(this.loginDetectCommand);
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
