using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatreTicketSystem.Domain.Entities
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public Guid ShowId { get; set; }
        public Show? Show { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public string UserId { get; set; } //Identity User
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
