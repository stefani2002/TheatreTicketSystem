using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Web.Controllers
{
    public class PlaysController : Controller
    {
        private readonly IPlayService _playService;

        public PlaysController(IPlayService playService)
        {
            _playService = playService;
        }

        public IActionResult Index()
        {
            var plays = _playService.GetAllPlays(); 
            return View(plays);
        }

        public IActionResult Details(Guid id)
        {
            var play = _playService.GetPlayDetails(id);
            if (play == null) return NotFound();

            return View(play);
        }
    }
}
