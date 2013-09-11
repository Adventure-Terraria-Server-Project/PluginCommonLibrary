using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public struct CommandInteractionResult {
    public bool IsInteractionCompleted { get; set; }
    public bool IsHandled { get; set; }
  }
}
