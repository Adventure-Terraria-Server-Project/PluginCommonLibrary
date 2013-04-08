using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class TileProtectedException: Exception {
    #region [Property: TileLocation]
    private readonly DPoint tileLocation;

    public DPoint TileLocation {
      get { return this.tileLocation; }
    }
    #endregion


    #region [Method: Constructor]
    public TileProtectedException(string message, DPoint tileLocation = default(DPoint)): base(message, null) {
      this.tileLocation = tileLocation;
    }

    public TileProtectedException(DPoint tileLocation): base(string.Format("The tile at {0} is protected.", tileLocation)) {
      this.tileLocation = tileLocation;
    }

    public TileProtectedException(string message, Exception inner = null): base(message, inner) {}

    public TileProtectedException(): base("The tile is protected.") {}
    #endregion

    #region [Serializable Implementation]
    protected TileProtectedException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.tileLocation = (DPoint)info.GetValue("TileProtectedException_TileLocation", typeof(DPoint));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("TileProtectedException_TileLocation", this.tileLocation, typeof(DPoint));
    }
    #endregion
  }
}