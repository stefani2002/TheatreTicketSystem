using TheatreTicketSystem.Domain.Entities;
using TheatreTicketSystem.Repostory.Interfaces;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Service.Implementations
{
    public class PlayService : IPlayService
    {
        private readonly IPlayRepository _playRepository;

        public PlayService(IPlayRepository playRepository)
        {
            _playRepository = playRepository;
        }

        public List<Play> GetAllPlays()
        {
            return _playRepository.GetAll();
        }

        public Play GetPlayDetails(Guid id)
        {
            return _playRepository.GetById(id);
        }
    }
}
