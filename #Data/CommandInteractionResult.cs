// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow {
  public struct CommandInteractionResult {
    #region [Property: IsInteractionCompleted]
    private bool isInteractionCompleted;

    public bool IsInteractionCompleted {
      get { return this.isInteractionCompleted; }
      set { this.isInteractionCompleted = value; }
    }
    #endregion

    #region [Property: IsHandled]
    private bool isHandled;

    public bool IsHandled {
      get { return this.isHandled; }
      set { this.isHandled = value; }
    }
    #endregion
  }
}
