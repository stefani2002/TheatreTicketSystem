using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Service.Interfaces;
using TheatreTicketSystem.Service.ViewModels;

namespace TheatreTicketSystem.Service.Implementations
{
    public class RepertoireService : IRepertoireService
    {
        private readonly ApplicationDbContext _context;

        public RepertoireService(ApplicationDbContext context)
        {
            _context = context;
        }

        public RepertoireCalendarVm GetCalendar(int year, int month)
        {
            if (month < 1) month = 1;
            if (month > 12) month = 12;

            var firstDay = new DateTime(year, month, 1);
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var lastDay = new DateTime(year, month, daysInMonth);

            
            int diffToMonday = ((int)firstDay.DayOfWeek + 6) % 7; 
            var calendarStart = firstDay.AddDays(-diffToMonday);

            
            int diffToSunday = 6 - (((int)lastDay.DayOfWeek + 6) % 7);
            var calendarEnd = lastDay.AddDays(diffToSunday);

            
            var shows = _context.Shows
                .AsNoTracking()
                .Include(s => s.Play)
                .Where(s => s.ShowTime.Date >= calendarStart.Date && s.ShowTime.Date <= calendarEnd.Date)
                .OrderBy(s => s.ShowTime)
                .Select(s => new CalendarShowVm
                {
                    ShowId = s.Id,
                    PlayTitle = s.Play.Title,
                    PosterImagePath = s.Play.PosterImagePath,
                    ShowTime = s.ShowTime,
                    Price = s.Price,
                    AvailableSeats = s.AvailableSeats
                })
                .ToList();

            
            var byDate = shows
                .GroupBy(s => s.ShowTime.Date)
                .ToDictionary(g => g.Key, g => g.ToList());

            var vm = new RepertoireCalendarVm
            {
                Year = year,
                Month = month
            };

            var cur = calendarStart.Date;
            while (cur <= calendarEnd.Date)
            {
                var week = new List<CalendarDayVm>();
                for (int i = 0; i < 7; i++)
                {
                    var day = new CalendarDayVm
                    {
                        Date = cur,
                        IsCurrentMonth = (cur.Month == month)
                    };

                    if (byDate.TryGetValue(cur, out var dayShows))
                        day.Shows = dayShows;

                    week.Add(day);
                    cur = cur.AddDays(1);
                }
                vm.Weeks.Add(week);
            }

            return vm;
        }
    }
}

