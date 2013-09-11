using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class InvalidBlockTypeException: Exception {
    public BlockType BlockType { get; private set; }


    public InvalidBlockTypeException(string message, BlockType blockType = BlockType.Invalid): base(message, null) {
      this.BlockType = blockType;
    }

    public InvalidBlockTypeException(BlockType blockType): base(string.Format(
      "The given block type \"{0}\" was unexpected in this context.", blockType
    )) {
      this.BlockType = blockType;
    }

    public InvalidBlockTypeException(string message, Exception inner = null): base(message, inner) {}

    public InvalidBlockTypeException(): base("The given block type was unexpected in this context.") {}

    #region [Serializable Implementation]
    protected InvalidBlockTypeException(SerializationInfo info, StreamingContext context): base(info, context) {
      this.BlockType = (BlockType)info.GetValue("InvalidBlockTypeException_BlockType", typeof(BlockType));
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidBlockTypeException_BlockType", this.BlockType);
    }
    #endregion
  }
}