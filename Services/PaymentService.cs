public class PaymentService
{
    public bool SimulatePayment(CreditCardInfo creditCardInfo) {
        return creditCardInfo.Number.StartsWith("4");
    }
}