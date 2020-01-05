using System;
using System.Runtime.Serialization;
using System.Security;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class ChestNotEmptyException: Exception {
    public DPoint ChestLocation { get; private set; }


    public ChestNotEmptyException(string message, DPoint chestLocation = default(DPoint)): base(message, null) {
      this.ChestLocation = chestLocation;
    }

    public ChestNotEmptyException(DPoint chestLocation): base("The chest is not empty.") {
      this.ChestLocation = chestLocation;
    }

    public ChestNotEmptyException(string message, Exception inner = null): base(message, inner) {}

    public ChestNotEmptyException(): base("The chest is not empty.") {}

    #region [Serializable Implementation]
    protected ChestNotEmptyException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.ChestLocation = (DPoint)info.GetValue("ChestNotEmptyException_ChestLocation", typeof(DPoint));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("ChestNotEmptyException_ChestLocation", this.ChestLocation, typeof(DPoint));
    }
    #endregion
  }
}