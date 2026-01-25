using System;

namespace TheatreTicketSystem.Domain.Entities
{
    public class PaymentCard
    {
        public Guid Id { get; set; }
        public string CardHolderName { get; set; } = "";
        public string CardNumber { get; set; } = "";  // test-only
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvv { get; set; } = "";         // test-only
        public decimal Balance { get; set; }          // test-only
        public bool IsActive { get; set; } = true;
    }
}
