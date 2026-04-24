using System;

namespace TheatreTicketSystem.Domain.Entities
{
    public class PaymentCard
    {
        public Guid Id { get; set; }
        public string CardHolderName { get; set; } = "";
        public string CardNumber { get; set; } = "";  
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string Cvv { get; set; } = "";         
        public decimal Balance { get; set; }          
        public bool IsActive { get; set; } = true;
    }
}
