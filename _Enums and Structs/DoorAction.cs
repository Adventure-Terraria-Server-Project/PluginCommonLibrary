using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.Common {
  public enum DoorAction {
    Invalid = -1,
    OpenDoor = 0,
    CloseDoor = 1,
    OpenTrapdoor = 2,
    CloseTrapdoor = 3,
    OpenTallGate = 4,
    CloseTallGate = 5
  }
}
