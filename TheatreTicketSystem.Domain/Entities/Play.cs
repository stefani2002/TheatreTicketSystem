using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheatreTicketSystem.Domain.Entities
{
    public class Play
    {
        public Guid Id { get; set; }
        public string? PosterImagePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }


        public ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
