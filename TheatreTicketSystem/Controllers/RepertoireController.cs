using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Web.Controllers
{
    public class RepertoireController : Controller
    {
        private readonly IRepertoireService _repertoireService;

        public RepertoireController(IRepertoireService repertoireService)
        {
            _repertoireService = repertoireService;
        }

        public IActionResult Index(int? year, int? month)
        {
            var now = DateTime.Now;
            int y = year ?? now.Year;
            int m = month ?? now.Month;

            var vm = _repertoireService.GetCalendar(y, m);
            return View(vm);
        }
    }
}
