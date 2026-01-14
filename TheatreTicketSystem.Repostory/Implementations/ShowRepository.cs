using TheatreTicketSystem.Domain.Entities;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Repostory.Interfaces;

namespace TheatreTicketSystem.Repository.Implementations
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Show GetById(Guid id)
        {
            return _context.Shows.FirstOrDefault(s => s.Id == id);
        }
    }
}
