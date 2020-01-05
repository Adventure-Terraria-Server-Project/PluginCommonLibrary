using System;
using System.Runtime.Serialization;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class PlayerNotLoggedInException: Exception {
    public PlayerNotLoggedInException(string message, Exception inner = null): base(message, inner) {}

    public PlayerNotLoggedInException(): base("The player has to be logged into a valid account in order to perform this action.") {}

    protected PlayerNotLoggedInException(SerializationInfo info, StreamingContext context): base(info, context) {}
  }
}