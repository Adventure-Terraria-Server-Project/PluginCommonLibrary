using System;
using System.Runtime.Serialization;
using System.Security;
using DPoint = System.Drawing.Point;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class TileProtectedException: Exception {
    public DPoint TileLocation { get; private set; }


    public TileProtectedException(string message, DPoint tileLocation = default(DPoint)): base(message, null) {
      this.TileLocation = tileLocation;
    }

    public TileProtectedException(DPoint tileLocation): base(string.Format("The tile at {0} is protected.", tileLocation)) {
      this.TileLocation = tileLocation;
    }

    public TileProtectedException(string message, Exception inner = null): base(message, inner) {}

    public TileProtectedException(): base("The tile is protected.") {}

    #region [Serializable Implementation]
    protected TileProtectedException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.TileLocation = (DPoint)info.GetValue("TileProtectedException_TileLocation", typeof(DPoint));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("TileProtectedException_TileLocation", this.TileLocation, typeof(DPoint));
    }
    #endregion
  }
}