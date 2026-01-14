using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatreTicketSystem.Domain.Entities
{
    public class Show
    {
        public Guid Id { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal Price { get; set; }
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public Guid PlayId { get; set; }
        public Play Play { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
