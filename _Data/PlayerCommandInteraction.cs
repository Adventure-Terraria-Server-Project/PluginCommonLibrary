// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using DPoint = System.Drawing.Point;

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
    private Func<TSPlayer,TileEditType,BlockType,DPoint,CommandInteractionResult> tileEditCallback;

    public Func<TSPlayer,TileEditType,BlockType,DPoint,CommandInteractionResult> TileEditCallback {
      get { return this.tileEditCallback; }
      set { this.tileEditCallback = value; }
    }
    #endregion

    #region [Property: ChestOpenCallback]
    private Func<TSPlayer,DPoint,CommandInteractionResult> chestOpenCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> ChestOpenCallback {
      get { return this.chestOpenCallback; }
      set { this.chestOpenCallback = value; }
    }
    #endregion

    #region [Property: SignEditCallback]
    private Func<TSPlayer,int,DPoint,string,CommandInteractionResult> signEditCallback;

    public Func<TSPlayer,int,DPoint,string,CommandInteractionResult> SignEditCallback {
      get { return this.signEditCallback; }
      set { this.signEditCallback = value; }
    }
    #endregion

    #region [Property: SignReadCallback]
    private Func<TSPlayer, DPoint, CommandInteractionResult> signReadCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> SignReadCallback {
      get { return this.signReadCallback; }
      set { this.signReadCallback = value; }
    }
    #endregion

    #region [Property: HitSwitchCallback]
    private Func<TSPlayer,DPoint,CommandInteractionResult> hitSwitchCallback;

    public Func<TSPlayer,DPoint,CommandInteractionResult> HitSwitchCallback {
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
