// This file is provided unter the terms of the 
// Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported License.
// To view a copy of this license, visit http://creativecommons.org/licenses/by-nc-sa/3.0/.
// 
// Written by CoderCow

using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.CoderCow {
  [Serializable]
  public class InvalidChestStyleException: Exception {
    #region [Property: ActualStyle]
    private readonly ChestStyle actualStyle;

    public ChestStyle ActualStyle {
      get { return this.actualStyle; }
    }
    #endregion


    #region [Method: Constructor]
    public InvalidChestStyleException(string message, ChestStyle actualStyle = ChestStyle.None): base(message, null) {
      this.actualStyle = actualStyle;
    }

    public InvalidChestStyleException(ChestStyle actualStyle): base("The chest type is invalid.") {
      this.actualStyle = actualStyle;
    }

    public InvalidChestStyleException(string message, Exception inner = null): base(message, inner) {}

    public InvalidChestStyleException(): base("The chest type is invalid.") {}
    #endregion

    #region [Serializable Implementation]
    protected InvalidChestStyleException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.actualStyle = (ChestStyle)info.GetValue("InvalidChestTypeException_ActualType", typeof(ChestStyle));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidChestTypeException_ActualType", this.actualStyle, typeof(ChestStyle));
    }
    #endregion
  }
}