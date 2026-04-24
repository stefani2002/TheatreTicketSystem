using Stripe;

public interface IStripePaymentService
{
    Task<PaymentIntent> CreatePaymentIntentAsync(decimal amountMkd, string currency, string description);
    Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId);
}
