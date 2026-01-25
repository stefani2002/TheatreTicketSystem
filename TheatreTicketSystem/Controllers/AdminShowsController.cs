using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Domain.Entities;

namespace TheatreTicketSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminShowsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminShowsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // List shows for a play
        public IActionResult Index(Guid playId)
        {
            var play = _context.Plays.FirstOrDefault(p => p.Id == playId);
            if (play == null) return NotFound();

            ViewBag.PlayId = playId;
            ViewBag.PlayTitle = play.Title;

            var shows = _context.Shows
                .Where(s => s.PlayId == playId)
                .OrderBy(s => s.ShowTime)
                .ToList();

            return View(shows);
        }

        // Create show (GET)
        public IActionResult Create(Guid playId)
        {
            ViewBag.PlayId = playId;
            return View(new Show
            {
                PlayId = playId,
                ShowTime = DateTime.Now.AddDays(1),
                TotalSeats = 100,
                AvailableSeats = 100,
                Price = 400
            });
        }

        // Create show (POST)
        [HttpPost]
        public IActionResult Create(Show model)
        {
            // PlayId мора да постои
            var playExists = _context.Plays.Any(p => p.Id == model.PlayId);
            if (!playExists) return NotFound();

            if (model.TotalSeats < 1) ModelState.AddModelError("", "TotalSeats must be >= 1");
            if (model.AvailableSeats < 0 || model.AvailableSeats > model.TotalSeats)
                ModelState.AddModelError("", "AvailableSeats must be between 0 and TotalSeats");
            if (model.Price < 0) ModelState.AddModelError("", "Price must be >= 0");

            if (!ModelState.IsValid)
            {
                ViewBag.PlayId = model.PlayId;
                return View(model);
            }

            model.Id = Guid.NewGuid();
            _context.Shows.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { playId = model.PlayId });
        }

        // Edit show (GET)
        public IActionResult Edit(Guid id)
        {
            var show = _context.Shows.FirstOrDefault(s => s.Id == id);
            if (show == null) return NotFound();

            return View(show);
        }

        // Edit show (POST)
        [HttpPost]
        public IActionResult Edit(Guid id, Show model)
        {
            var show = _context.Shows.FirstOrDefault(s => s.Id == id);
            if (show == null) return NotFound();

            if (model.TotalSeats < 1) ModelState.AddModelError("", "TotalSeats must be >= 1");
            if (model.AvailableSeats < 0 || model.AvailableSeats > model.TotalSeats)
                ModelState.AddModelError("", "AvailableSeats must be between 0 and TotalSeats");
            if (model.Price < 0) ModelState.AddModelError("", "Price must be >= 0");

            if (!ModelState.IsValid) return View(model);

            show.ShowTime = model.ShowTime;
            show.Price = model.Price;
            show.TotalSeats = model.TotalSeats;

            // Ако total seats се намали, да не падне available над него
            show.AvailableSeats = Math.Min(model.AvailableSeats, model.TotalSeats);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { playId = show.PlayId });
        }

        // Delete show (GET)
        public IActionResult Delete(Guid id)
        {
            var show = _context.Shows
                .Include(s => s.Play)
                .FirstOrDefault(s => s.Id == id);

            if (show == null) return NotFound();
            return View(show);
        }

        // Delete show (POST)
        [HttpPost]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var show = _context.Shows.FirstOrDefault(s => s.Id == id);
            if (show == null) return NotFound();

            var playId = show.PlayId;

            _context.Shows.Remove(show);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { playId });
        }
    }
}
