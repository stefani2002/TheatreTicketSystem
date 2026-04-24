using System.ComponentModel.DataAnnotations;

namespace TheatreTicketSystem.Service.ViewModels
{
    public class PaymentVm
    {
        public Guid ShowId { get; set; }

        [Range(1, 20)]
        public int Quantity { get; set; }

        public string PlayTitle { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Stripe
        public string StripePublishableKey { get; set; }
        public string PaymentIntentClientSecret { get; set; }

        // returned from client
        public string PaymentIntentId { get; set; }
    }
}
