// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace Terraria.Plugins.CoderCow.Test {
  [Serializable]
  public class AssertException: Exception {
    public AssertException(string message, Exception inner): base(message, inner) {}
    public AssertException(string message): base(message, null) {}
    public AssertException(): base("Assert failed.") {}
  }
}
