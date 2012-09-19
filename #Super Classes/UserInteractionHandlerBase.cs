using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public abstract class UserInteractionHandlerBase: IDisposable {
    #region [Constants]
    public const int CommandInteractionTimeout = 600; // In frames
    private const int UpdateFrameRate = 60;
    #endregion

    #region [Property: PluginTrace]
    private readonly PluginTrace pluginTrace;

    protected PluginTrace PluginTrace {
      get { return this.pluginTrace; }
    }
    #endregion

    #region [Property: ActiveCommandInteractions]
    private readonly Dictionary<string,PlayerCommandInteraction> activeCommandInteractions = 
      new Dictionary<string,PlayerCommandInteraction>();

    protected Dictionary<string,PlayerCommandInteraction> ActiveCommandInteractions {
      get { return this.activeCommandInteractions; }
    }
    #endregion


    #region [Method: Constructor]
    protected UserInteractionHandlerBase(PluginTrace pluginTrace) {
      this.pluginTrace = pluginTrace;
      this.activeCommandInteractions = new Dictionary<string,PlayerCommandInteraction>();
    }
    #endregion

    #region [Method: StartOrResetCommandInteraction]
    protected PlayerCommandInteraction StartOrResetCommandInteraction(TSPlayer forPlayer) {
      PlayerCommandInteraction interaction;
      if (this.ActiveCommandInteractions.TryGetValue(forPlayer.UserAccountName, out interaction))
        interaction.FramesLeft = UserInteractionHandlerBase.CommandInteractionTimeout;
      else
        interaction = new PlayerCommandInteraction(UserInteractionHandlerBase.CommandInteractionTimeout);

      return interaction;
    }
    #endregion

    #region [Methods: HandleTileEdit, HandleChestOpen, HandleSignEdit, HandleHitSwitch, HandleGameUpdate]
    public virtual bool HandleTileEdit(TSPlayer player, TileEditType editType, int tileId, int x, int y) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      // Is the player currently interacting with a command?
      if (!this.activeCommandInteractions.TryGetValue(player.UserAccountName, out commandInteraction))
        return false;

      if (commandInteraction.TileEditCallback != null)
        return false;

      CommandInteractionResult result = commandInteraction.TileEditCallback(player, editType, tileId, x, y);
      if (result.IsInteractionCompleted)
        this.activeCommandInteractions.Remove(player.UserAccountName);

      return result.IsHandled;
    }

    public virtual bool HandleChestOpen(TSPlayer player, int x, int y) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      // Is the player currently interacting with a command?
      if (!this.activeCommandInteractions.TryGetValue(player.UserAccountName, out commandInteraction))
        return false;

      if (commandInteraction.ChestOpenCallback != null)
        return false;

      CommandInteractionResult result = commandInteraction.ChestOpenCallback(player, x, y);
      if (result.IsInteractionCompleted)
        this.activeCommandInteractions.Remove(player.UserAccountName);

      return result.IsHandled;
    }

    public virtual bool HandleSignEdit(TSPlayer player, int signId, int x, int y, string newText) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      // Is the player currently interacting with a command?
      if (!this.activeCommandInteractions.TryGetValue(player.UserAccountName, out commandInteraction))
        return false;

      if (commandInteraction.SignEditCallback != null)
        return false;

      CommandInteractionResult result = commandInteraction.SignEditCallback(player, signId, x, y, newText);
      if (result.IsInteractionCompleted)
        this.activeCommandInteractions.Remove(player.UserAccountName);

      return result.IsHandled;
    }

    public virtual bool HandleHitSwitch(TSPlayer player, int x, int y) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      // Is the player currently interacting with a command?
      if (!this.activeCommandInteractions.TryGetValue(player.UserAccountName, out commandInteraction))
        return false;

      if (commandInteraction.HitSwitchCallback != null)
        return false;

      CommandInteractionResult result = commandInteraction.HitSwitchCallback(player, x, y);
      if (result.IsInteractionCompleted)
        this.activeCommandInteractions.Remove(player.UserAccountName);

      return result.IsHandled;
    }

    private int frameCounter;
    public virtual void HandleGameUpdate() {
      if (this.IsDisposed)
        return;

      this.frameCounter++;

      if (this.frameCounter >= UserInteractionHandlerBase.UpdateFrameRate) {
        this.frameCounter = 0;

        if (this.ActiveCommandInteractions.Count == 0)
          return;

        try {
          // Deleting multiple items from a dictionary in one loop requires quite a bit of performance, so we try to do 
          // it only when necessary.
          bool safeLoopRequired = false;
          foreach (KeyValuePair<string,PlayerCommandInteraction> interaction in this.ActiveCommandInteractions) {
            if (interaction.Value.FramesLeft <= 0) {
              safeLoopRequired = true;
              continue;
            }

            interaction.Value.FramesLeft -= UserInteractionHandlerBase.UpdateFrameRate;
          }
        
          if (safeLoopRequired) {
            List<string> interactingPlayers = new List<string>(this.ActiveCommandInteractions.Keys);
            foreach (string interactingPlayer in interactingPlayers) {
              PlayerCommandInteraction commandInteraction;
              if (!this.ActiveCommandInteractions.TryGetValue(interactingPlayer, out commandInteraction))
                continue;

              if (commandInteraction.FramesLeft <= 0) {
                TSPlayer player = TShockEx.GetPlayerByName(interactingPlayer);
                if (
                  player != null && player.ConnectionAlive && player.IsLoggedIn && 
                  commandInteraction.TimeExpiredCallback != null
                ) {
                  commandInteraction.TimeExpiredCallback(player);
                }

                this.ActiveCommandInteractions.Remove(interactingPlayer);
              }
            }
          }
        } catch (Exception ex) {
          this.PluginTrace.WriteLineError("Failed handling interactions. Exception details: {0}", ex);
        }
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
        this.activeCommandInteractions.Clear();
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
