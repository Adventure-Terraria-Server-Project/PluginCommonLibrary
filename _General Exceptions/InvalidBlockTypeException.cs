using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class InvalidBlockTypeException: Exception {
    public int BlockType { get; private set; }


    public InvalidBlockTypeException(string message, int blockType = 0): base(message, null) {
      this.BlockType = blockType;
    }

    public InvalidBlockTypeException(int blockType): base($"The given block type \"{blockType}\" was unexpected in this context.") {
      this.BlockType = blockType;
    }

    public InvalidBlockTypeException(string message, Exception inner = null): base(message, inner) {}

    public InvalidBlockTypeException(): base("The given block type was unexpected in this context.") {}

    #region [Serializable Implementation]
    protected InvalidBlockTypeException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.BlockType = (int)info.GetValue("InvalidBlockTypeException_BlockType", typeof(int));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidBlockTypeException_BlockType", this.BlockType);
    }
    #endregion
  }
}