using System;

namespace Terraria.Plugins.Common {
  public struct CommandInteractionResult {
    public bool IsInteractionCompleted { get; set; }
    public bool IsHandled { get; set; }
  }
}
