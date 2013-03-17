using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.CoderCow {
  [Serializable]
  public class ChestNotEmptyException: Exception {
    #region [Property: ChestLocation]
    private readonly DPoint chestLocation;

    public DPoint ChestLocation {
      get { return this.chestLocation; }
    }
    #endregion


    #region [Method: Constructor]
    public ChestNotEmptyException(string message, DPoint chestLocation = default(DPoint)): base(message, null) {
      this.chestLocation = chestLocation;
    }

    public ChestNotEmptyException(DPoint chestLocation): base("The chest is not empty.") {
      this.chestLocation = chestLocation;
    }

    public ChestNotEmptyException(string message, Exception inner = null): base(message, inner) {}

    public ChestNotEmptyException() : base("The chest is not empty.") {}
    #endregion

    #region [Serializable Implementation]
    protected ChestNotEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.chestLocation = (DPoint)info.GetValue("ChestNotEmptyException_ChestLocation", typeof(DPoint));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("ChestNotEmptyException_ChestLocation", this.chestLocation, typeof(DPoint));
    }
    #endregion
  }
}