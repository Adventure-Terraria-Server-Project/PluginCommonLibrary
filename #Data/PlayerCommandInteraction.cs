// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using TShockAPI;

namespace Terraria.Plugins.CoderCow {
  public class PlayerCommandInteraction {
    #region [Property: FramesLeft]
    private int framesLeft;

    public int FramesLeft {
      get { return this.framesLeft; }
      set { this.framesLeft = value; }
    }
    #endregion

    #region [Property: TileEditCallback]
    private Func<TSPlayer,TileEditType,int,int,int,CommandInteractionResult> tileEditCallback;

    public Func<TSPlayer,TileEditType,int,int,int,CommandInteractionResult> TileEditCallback {
      get { return this.tileEditCallback; }
      set { this.tileEditCallback = value; }
    }
    #endregion

    #region [Property: ChestOpenCallback]
    private Func<TSPlayer,int,int,CommandInteractionResult> chestOpenCallback;

    public Func<TSPlayer,int,int,CommandInteractionResult> ChestOpenCallback {
      get { return this.chestOpenCallback; }
      set { this.chestOpenCallback = value; }
    }
    #endregion

    #region [Property: SignEditCallback]
    private Func<TSPlayer,int,int,int,string,CommandInteractionResult> signEditCallback;

    public Func<TSPlayer,int,int,int,string,CommandInteractionResult> SignEditCallback {
      get { return this.signEditCallback; }
      set { this.signEditCallback = value; }
    }
    #endregion

    #region [Property: HitSwitchCallback]
    private Func<TSPlayer,int,int,CommandInteractionResult> hitSwitchCallback;

    public Func<TSPlayer,int,int,CommandInteractionResult> HitSwitchCallback {
      get { return this.hitSwitchCallback; }
      set { this.hitSwitchCallback = value; }
    }
    #endregion

    #region [Property: TimeExpiredCallback]
    private Action<TSPlayer> timeExpiredCallback;

    public Action<TSPlayer> TimeExpiredCallback {
      get { return this.timeExpiredCallback; }
      set { this.timeExpiredCallback = value; }
    }
    #endregion

    #region [Property: InteractionData]
    private object interactionData;

    public object InteractionData {
      get { return this.interactionData; }
      set { this.interactionData = value; }
    }
    #endregion


    #region [Method: Constructor]
    public PlayerCommandInteraction(int timeToInteract) {
      this.framesLeft = timeToInteract;
    }
    #endregion
  }
}
