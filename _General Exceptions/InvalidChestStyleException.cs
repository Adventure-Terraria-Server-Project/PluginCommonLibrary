using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class InvalidChestStyleException: Exception {
    public ChestStyle ActualStyle { get; private set; }


    public InvalidChestStyleException(string message, ChestStyle actualStyle = ChestStyle.None): base(message, null) {
      this.ActualStyle = actualStyle;
    }

    public InvalidChestStyleException(ChestStyle actualStyle): base("The chest type is invalid.") {
      this.ActualStyle = actualStyle;
    }

    public InvalidChestStyleException(string message, Exception inner = null): base(message, inner) {}

    public InvalidChestStyleException(): base("The chest type is invalid.") {}

    #region [Serializable Implementation]
    protected InvalidChestStyleException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.ActualStyle = (ChestStyle)info.GetValue("InvalidChestTypeException_ActualType", typeof(ChestStyle));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidChestTypeException_ActualType", this.ActualStyle, typeof(ChestStyle));
    }
    #endregion
  }
}