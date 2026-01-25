using TheatreTicketSystem.Service.ViewModels;


public interface ITicketService
{
    PurchaseConfirmVm GetConfirmModel(Guid showId);
    PaymentVm GetPaymentModel(Guid showId, int quantity);
    byte[] ProcessPaymentAndBuy(PaymentVm vm, string userId);
}
