using System;

namespace TheatreTicketSystem.Service.ViewModels
{
    public class PurchaseConfirmVm
    {
        public Guid ShowId { get; set; }
        public string PlayTitle { get; set; } = "";
        public DateTime ShowTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }

        public int Quantity { get; set; } = 1;
    }
}
