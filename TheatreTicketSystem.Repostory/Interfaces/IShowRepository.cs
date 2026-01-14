using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheatreTicketSystem.Domain.Entities;

namespace TheatreTicketSystem.Repostory.Interfaces
{
    public interface IShowRepository
    {
        Show GetById(Guid id);
    }
}
