using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class PlayerDataCreationException: Exception {
    public PlayerDataCreationException(string message, Exception inner = null): base(message, inner) {}

    public PlayerDataCreationException(): base("Error while creating the player data.") {}

    protected PlayerDataCreationException(SerializationInfo info, StreamingContext context): base(info, context) {}
  }
}