using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using DPoint = System.Drawing.Point;

using TShockAPI;

namespace Terraria.Plugins.Common {
  public abstract class UserInteractionHandlerBase: IDisposable {
    #region [Constants]
    public const int CommandInteractionTimeout = 1200; // In frames
    private const int UpdateFrameRate = 60;
    #endregion

    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    protected PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion

    #region [Property: RegisteredCommands]
    private readonly Collection<Command> registeredCommands;

    protected Collection<Command> RegisteredCommands {
      get { return this.registeredCommands; }
    }
    #endregion

    private readonly Dictionary<TSPlayer,PlayerCommandInteraction> activeCommandInteractions = 
      new Dictionary<TSPlayer,PlayerCommandInteraction>();
    private readonly object activeCommandInteractionsLock = new object();


    #region [Method: Constructor]
    protected UserInteractionHandlerBase(PluginTrace pluginTrace) {
      this.pluginTrace = pluginTrace;
      this.registeredCommands = new Collection<Command>();
      this.activeCommandInteractions = new Dictionary<TSPlayer,PlayerCommandInteraction>();
    }
    #endregion

    #region [Methods: RegisterCommand, DeregisterCommand, CustomHelpCommand_Exec]
    protected Command RegisterCommand(
      string[] names, CommandDelegate commandExec, CommandDelegate commandHelpExec = null, string requiredPermission = null,
      bool allowServer = true, bool doLog = true
    ) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);
      Contract.Requires<ArgumentNullException>(names != null);
      Contract.Requires<ArgumentNullException>(commandExec != null);
      
      CommandDelegate actualCommandExec;
      if (commandHelpExec != null) {
        actualCommandExec = (args) => {
          if (args.ContainsParameter("help", StringComparison.InvariantCultureIgnoreCase)) {
            commandHelpExec(args);
            return;
          }

          commandExec(args);
        };
      } else {
        actualCommandExec = commandExec;
      }

      Command command;
      if (requiredPermission != null)
        command = new Command(requiredPermission, actualCommandExec, names);
      else
        command = new Command(actualCommandExec, names);

      TShockAPI.Commands.ChatCommands.Add(command);
      command.AllowServer = allowServer;
      command.DoLog = doLog;

      return command;
    }

    protected void DeregisterCommand(Command tshockCommand) {
      Contract.Requires<ArgumentNullException>(tshockCommand != null);

      if (!TShockAPI.Commands.ChatCommands.Contains(tshockCommand))
        throw new InvalidOperationException("Command is not registered.");
    }
    #endregion

    #region [Method: StartOrResetCommandInteraction, StopInteraction]
    protected PlayerCommandInteraction StartOrResetCommandInteraction(TSPlayer forPlayer) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);
      Contract.Requires<ArgumentNullException>(forPlayer != null);

      PlayerCommandInteraction newInteraction = new PlayerCommandInteraction(forPlayer);
      newInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;

      lock (this.activeCommandInteractionsLock) {
        if (!this.activeCommandInteractions.ContainsKey(forPlayer))
          this.activeCommandInteractions.Add(forPlayer, newInteraction);
        else
          this.activeCommandInteractions[forPlayer] = newInteraction;
      }

      newInteraction.TimeoutTask = Task.Factory.StartNew(() => {
        if (this.IsDisposed)
          return;

        while (newInteraction.framesLeft > 0) {
          if (this.IsDisposed)
            return;

          newInteraction.framesLeft -= 10;
          Thread.Sleep(100);
        }

        lock (this.activeCommandInteractionsLock) {
          if (this.IsDisposed)
            return;

          if (!this.activeCommandInteractions.ContainsValue(newInteraction))
            return;

          if (forPlayer.ConnectionAlive && newInteraction.TimeExpiredCallback != null) {
            try {
              newInteraction.TimeExpiredCallback(forPlayer);
            } catch (Exception ex) {
              this.PluginTrace.WriteLineError("A command interaction's time expired callback has thrown an exception:\n" + ex);
            }
          }

          this.activeCommandInteractions.Remove(forPlayer);
        }
      });

      return newInteraction;
    }

    protected void StopInteraction(TSPlayer forPlayer) {
      Contract.Requires<ObjectDisposedException>(!this.IsDisposed);
      Contract.Requires<ArgumentNullException>(forPlayer != null);

      lock (this.activeCommandInteractionsLock) {
        if (this.activeCommandInteractions.ContainsKey(forPlayer))
          this.activeCommandInteractions.Remove(forPlayer);
      }
    }
    #endregion

    #region [Methods: HandleTileEdit, HandleChestGetContents, HandleSignEdit, HandleSignRead, HandleHitSwitch, HandleGameUpdate]
    public virtual bool HandleTileEdit(TSPlayer player, TileEditType editType, BlockType blockType, DPoint location, int objectStyle) {
      if (this.IsDisposed || this.activeCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player, out commandInteraction))
          return false;

        if (commandInteraction.TileEditCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.TileEditCallback(player, editType, blockType, location, objectStyle);
        if (commandInteraction.DoesNeverComplete)
          commandInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
        else if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player);

        return result.IsHandled;
      }
    }

    public virtual bool HandleChestGetContents(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.activeCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player, out commandInteraction))
          return false;

        if (commandInteraction.ChestOpenCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.ChestOpenCallback(player, location);
        if (commandInteraction.DoesNeverComplete)
          commandInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
        else if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player);

        return result.IsHandled;
      }
    }

    public virtual bool HandleSignEdit(TSPlayer player, short signIndex, DPoint location, string newText) {
      if (this.IsDisposed || this.activeCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player, out commandInteraction))
          return false;

        if (commandInteraction.SignEditCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.SignEditCallback(player, signIndex, location, newText);
        if (commandInteraction.DoesNeverComplete)
          commandInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
        else if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player);

        return result.IsHandled;
      }
    }

    public virtual bool HandleSignRead(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.activeCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player, out commandInteraction))
          return false;

        if (commandInteraction.SignReadCallback == null)
          return false;
      
        CommandInteractionResult result = commandInteraction.SignReadCallback(player, location);
        if (commandInteraction.DoesNeverComplete)
          commandInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
        else if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player);

        return result.IsHandled;
      }
    }

    public virtual bool HandleHitSwitch(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.activeCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      lock (this.activeCommandInteractionsLock) {
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player, out commandInteraction))
          return false;

        if (commandInteraction.HitSwitchCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.HitSwitchCallback(player, location);
        if (commandInteraction.DoesNeverComplete)
          commandInteraction.framesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
        else if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player);

        return result.IsHandled;
      }
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
        lock (this.activeCommandInteractionsLock) {
          this.activeCommandInteractions.Clear();
        }

        try {
          foreach (Command command in this.registeredCommands)
            if (TShockAPI.Commands.ChatCommands.Contains(command))
              TShockAPI.Commands.ChatCommands.Remove(command);
        // May occur due to unsynchronous thread operations.
        } catch (InvalidOperationException) {}
      }

      this.isDisposed = true;
    }

    public void Dispose() {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    ~UserInteractionHandlerBase() {
      this.Dispose(false);
    }
    #endregion
  }
}
