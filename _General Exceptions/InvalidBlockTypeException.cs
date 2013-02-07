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
  public class InvalidBlockTypeException: Exception {
    #region [Property: BlockType]
    private readonly int blockType;

    public int BlockType {
      get { return this.blockType; }
    }
    #endregion


    #region [Method: Constructor]
    public InvalidBlockTypeException(string message, int blockType = -1): base(message, null) {
      this.blockType = blockType;
    }

    public InvalidBlockTypeException(int blockType): base(string.Format("The given block type \"{0}\" is invalid.", blockType)) {
      this.blockType = blockType;
    }

    public InvalidBlockTypeException(string message, Exception inner = null): base(message, inner) {}

    public InvalidBlockTypeException(): base("The given block type is invalid.") {}
    #endregion

    #region [Serializable Implementation]
    protected InvalidBlockTypeException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.blockType = info.GetInt32("InvalidBlockTypeException_BlockType");
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("InvalidBlockTypeException_BlockType", this.blockType);
    }
    #endregion
  }
}