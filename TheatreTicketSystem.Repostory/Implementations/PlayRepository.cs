using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheatreTicketSystem.Domain.Entities;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Repostory.Interfaces;

namespace TheatreTicketSystem.Repostory.Implementations
{
    public class PlayRepository : IPlayRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Play> GetAll()
        {
            return _context.Plays
                .Include(p => p.Shows)
                .ToList();
        }

        public Play? GetById(Guid id)
        {
            return _context.Plays
        .Include(p => p.Shows)  
        .FirstOrDefault(p => p.Id == id);
        }
    }
}
