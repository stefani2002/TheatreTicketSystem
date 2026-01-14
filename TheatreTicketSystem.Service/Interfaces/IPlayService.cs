using TheatreTicketSystem.Domain.Entities;

namespace TheatreTicketSystem.Service.Interfaces
{
    public interface IPlayService
    {
        List<Play> GetAllPlays();
        Play GetPlayDetails(Guid id);
    }
}
