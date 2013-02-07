// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Terraria.Plugins.CoderCow {
  [Serializable]
  public class PlayerNotLoggedInException: Exception {
    public PlayerNotLoggedInException(string message, Exception inner = null): base(message, inner) {}

    public PlayerNotLoggedInException(): base("The player has to be logged into a valid account in order to perform this action.") {}

    protected PlayerNotLoggedInException(SerializationInfo info, StreamingContext context): base(info, context) {}
  }
}