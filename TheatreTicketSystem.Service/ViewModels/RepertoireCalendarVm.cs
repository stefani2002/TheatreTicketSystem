namespace TheatreTicketSystem.Service.ViewModels
{
    public class RepertoireCalendarVm
    {
        public int Year { get; set; }
        public int Month { get; set; } // 1-12

        // Calendar grid (weeks x days)
        public List<List<CalendarDayVm>> Weeks { get; set; } = new();
    }

    public class CalendarDayVm
    {
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }

        public List<CalendarShowVm> Shows { get; set; } = new();
    }

    public class CalendarShowVm
    {
        public Guid ShowId { get; set; }
        public string PlayTitle { get; set; } = "";
        public string? PosterImagePath { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
    }
}
