using System;

namespace TheatreTicketSystem.Service.ViewModels

{
    public class PaymentVm
    {
        public Guid ShowId { get; set; }
        public int Quantity { get; set; }

        public string PlayTitle { get; set; } = "";
        public DateTime ShowTime { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // card input
        public string CardHolderName { get; set; } = "";
        public string CardNumber { get; set; } = "";
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvv { get; set; } = "";
    }
}
