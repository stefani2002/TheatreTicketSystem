using TheatreTicketSystem.Service.ViewModels;

namespace TheatreTicketSystem.Service.Interfaces
{
    public interface IRepertoireService
    {
        RepertoireCalendarVm GetCalendar(int year, int month);
    }
}
