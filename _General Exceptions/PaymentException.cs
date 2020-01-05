using System;
using System.Runtime.Serialization;
using System.Security;

namespace Terraria.Plugins.Common {
  [Serializable]
  public class PaymentException: Exception {
    public int PaymentAmount { get; }


    public PaymentException(string message, int paymentAmount = -1): base(message, null) {
      this.PaymentAmount = paymentAmount;
    }

    public PaymentException(int paymentAmount): base(
      $"The user is missing the required {paymentAmount} of SEconomy currency to perform this action."
    ) {
      if (!(paymentAmount > 0)) throw new ArgumentException();
      this.PaymentAmount = paymentAmount;
    }

    public PaymentException(string message, Exception inner = null): base(message, inner) {}

    public PaymentException(): base("The user is missing the required amount of SEconomy currency to perform this action.") {}

    #region [Serializable Implementation]
    protected PaymentException(SerializationInfo info, StreamingContext context) : base(info, context) {
      this.PaymentAmount = info.GetInt32("PaymentException_PaymentAmount");
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context) {
      base.GetObjectData(info, context);

      info.AddValue("PaymentException_PaymentAmount", this.PaymentAmount);
    }
    #endregion
  }
}