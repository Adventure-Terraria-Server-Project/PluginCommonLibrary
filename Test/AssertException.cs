using System;

namespace Terraria.Plugins.Common.Test {
  [Serializable]
  public class AssertException: Exception {
    public AssertException(string message, Exception inner): base(message, inner) {}
    public AssertException(string message): base(message, null) {}
    public AssertException(): base("Assert failed.") {}
  }
}
