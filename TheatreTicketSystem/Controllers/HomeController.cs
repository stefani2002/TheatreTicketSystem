using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPlayService _playService;

        public HomeController(IPlayService playService)
        {
            _playService = playService;
        }

        public IActionResult Index()
        {
            var plays = _playService.GetAllPlays();
            return View(plays);
        }
    }
}
