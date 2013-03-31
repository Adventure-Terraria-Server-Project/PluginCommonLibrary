using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class MissingPermissionException: Exception {
    #region [Property: Permission]
    private readonly string permission;

    public string Permission {
      get { return this.permission; }
    }
    #endregion


    #region [Method: Constructor]
    public MissingPermissionException(string message, string permission = null): base(message, null) {
      this.permission = permission;
    }

    public MissingPermissionException(string permission): base(
      string.Format("The user is missing the required permission \"{0}\" to process this action.", permission)
    ) {
      this.permission = permission;
    }

    public MissingPermissionException(string message, Exception inner = null): base(message, inner) {}

    public MissingPermissionException(): base("The user is missing a required permission to process this action.") {}
    #endregion

    #region [Serializable Implementation]
    protected MissingPermissionException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.permission = info.GetString("MissingPermissionException_Permission");
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("MissingPermissionException_Permission", this.permission);
    }
    #endregion
  }
}