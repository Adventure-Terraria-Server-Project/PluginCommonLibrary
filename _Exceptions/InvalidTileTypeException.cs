using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.CoderCow {
  [Serializable]
  public class InvalidTileTypeException: Exception {
    #region [Property: TileType]
    private readonly int tileType;

    public int TileType {
      get { return this.tileType; }
    }
    #endregion


    #region [Method: Constructor]
    public InvalidTileTypeException(string message, int tileType = -1): base(message, null) {
      this.tileType = tileType;
    }

    public InvalidTileTypeException(int tileType): base(string.Format("The given tile type \"{0}\" is invalid.", tileType)) {
      this.tileType = tileType;
    }

    public InvalidTileTypeException(string message, Exception inner = null): base(message, inner) {}

    public InvalidTileTypeException(): base("The given tile type is invalid.") {}
    #endregion

    #region [Serializable Implementation]
    protected InvalidTileTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.tileType = info.GetInt32("InvalidTileTypeException_TileType");
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidTileTypeException_TileType", this.tileType);
    }
    #endregion
  }
}