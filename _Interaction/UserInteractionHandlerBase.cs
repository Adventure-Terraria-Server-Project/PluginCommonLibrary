// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

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

    private readonly object activeCommandInteractionsLock = new object();

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
      PlayerCommandInteraction newInteraction = new PlayerCommandInteraction(
        UserInteractionHandlerBase.CommandInteractionTimeout
      );

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction existingInteraction;
        if (this.ActiveCommandInteractions.TryGetValue(forPlayer.Name, out existingInteraction))
          this.ActiveCommandInteractions[forPlayer.Name] = newInteraction;
        else 
          this.ActiveCommandInteractions.Add(forPlayer.Name, newInteraction);
      }

      return newInteraction;
    }
    #endregion

    #region [Methods: HandleTileEdit, HandleChestGetContents, HandleSignEdit, HandleSignRead, HandleHitSwitch, HandleGameUpdate]
    public virtual bool HandleTileEdit(TSPlayer player, TileEditType editType, BlockType blockType, DPoint location, int objectStyle) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player.Name, out commandInteraction))
          return false;

        if (commandInteraction.TileEditCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.TileEditCallback(player, editType, blockType, location, objectStyle);
        if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player.Name);

        return result.IsHandled;
      }
    }

    public virtual bool HandleChestGetContents(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player.Name, out commandInteraction))
          return false;

        if (commandInteraction.ChestOpenCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.ChestOpenCallback(player, location);
        if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player.Name);

        return result.IsHandled;
      }
    }

    public virtual bool HandleSignEdit(TSPlayer player, short signIndex, DPoint location, string newText) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player.Name, out commandInteraction))
          return false;

        if (commandInteraction.SignEditCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.SignEditCallback(player, signIndex, location, newText);
        if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player.Name);

        return result.IsHandled;
      }
    }

    public virtual bool HandleSignRead(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      lock (this.activeCommandInteractionsLock) {
        PlayerCommandInteraction commandInteraction;
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player.Name, out commandInteraction))
          return false;

        if (commandInteraction.SignReadCallback == null)
          return false;
      
        CommandInteractionResult result = commandInteraction.SignReadCallback(player, location);
        if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player.Name);

        return result.IsHandled;
      }
    }

    public virtual bool HandleHitSwitch(TSPlayer player, DPoint location) {
      if (this.IsDisposed || this.ActiveCommandInteractions.Count == 0)
        return false;

      PlayerCommandInteraction commandInteraction;
      lock (this.activeCommandInteractionsLock) {
        // Is the player currently interacting with a command?
        if (!this.activeCommandInteractions.TryGetValue(player.Name, out commandInteraction))
          return false;

        if (commandInteraction.HitSwitchCallback == null)
          return false;

        CommandInteractionResult result = commandInteraction.HitSwitchCallback(player, location);
        if (result.IsInteractionCompleted)
          this.activeCommandInteractions.Remove(player.Name);

        return result.IsHandled;
      }
    }

    private int frameCounter;
    public virtual void HandleGameUpdate() {
      if (this.IsDisposed)
        return;

      this.frameCounter++;
      if (this.frameCounter >= UserInteractionHandlerBase.UpdateFrameRate) {
        this.frameCounter = 0;

        lock (this.activeCommandInteractionsLock) {
          if (this.ActiveCommandInteractions.Count == 0)
            return;

          List<string> playerInteractionsToRemove = null;
          foreach (KeyValuePair<string,PlayerCommandInteraction> interaction in this.ActiveCommandInteractions) {
            if (interaction.Value.FramesLeft <= 0) {
              if (playerInteractionsToRemove == null)
                playerInteractionsToRemove = new List<string>();

              playerInteractionsToRemove.Add(interaction.Key);
              continue;
            }

            interaction.Value.FramesLeft -= UserInteractionHandlerBase.UpdateFrameRate;
          }
        
          if (playerInteractionsToRemove != null) {
            foreach (string playerInteractionToRemove in playerInteractionsToRemove) {
              PlayerCommandInteraction commandInteraction = this.ActiveCommandInteractions[playerInteractionToRemove];
              TSPlayer player = TShockEx.GetPlayerByName(playerInteractionToRemove);
              if (player != null && player.ConnectionAlive && commandInteraction.TimeExpiredCallback != null)
                commandInteraction.TimeExpiredCallback(player);

              this.ActiveCommandInteractions.Remove(playerInteractionToRemove);
            }
          }
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
        lock (this.activeCommandInteractionsLock) {
          this.activeCommandInteractions.Clear();
        }
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
