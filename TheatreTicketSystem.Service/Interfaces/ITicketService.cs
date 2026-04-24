using TheatreTicketSystem.Service.ViewModels;

public interface ITicketService
{
    PurchaseConfirmVm GetConfirmModel(Guid showId);
    Task<PaymentVm> GetPaymentModelAsync(Guid showId, int quantity);
    Task<byte[]> ConfirmStripeAndBuyAsync(PaymentVm vm, string userId);
}
