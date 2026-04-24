using Stripe;

public class StripePaymentService : IStripePaymentService
{
    public async Task<PaymentIntent> CreatePaymentIntentAsync(decimal amountMkd, string currency, string description)
    {
        
        var multiplier = 100m;
        var amountMinor = (long)Math.Round(amountMkd * multiplier, 0);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amountMinor,
            Currency = currency.ToLower(), 
            Description = description,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }

    public async Task<PaymentIntent> GetPaymentIntentAsync(string paymentIntentId)
    {
        var service = new PaymentIntentService();
        return await service.GetAsync(paymentIntentId);
    }
}
