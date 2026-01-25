using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatreTicketSystem.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string UserId { get; set; } // Identity User
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
