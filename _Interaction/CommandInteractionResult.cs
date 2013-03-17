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
