using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class LimitEnforcementException: Exception {
    public LimitEnforcementException(string message, Exception inner = null): base(message, inner) {}

    public LimitEnforcementException(): base("A limitation was reached.") {}

    protected LimitEnforcementException(SerializationInfo info, StreamingContext context): base(info, context) {}
  }
}