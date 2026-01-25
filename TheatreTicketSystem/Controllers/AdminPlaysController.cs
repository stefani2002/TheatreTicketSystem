using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Domain.Entities;

[Authorize(Roles = "Admin")]
public class AdminPlaysController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AdminPlaysController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var plays = _context.Plays.ToList();
        return View(plays);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Play model, IFormFile? posterImage)
    {
        if (!ModelState.IsValid) return View(model);

        if (posterImage != null && posterImage.Length > 0)
        {
            model.PosterImagePath = SaveUpload(posterImage);
        }

        model.Id = Guid.NewGuid();
        _context.Plays.Add(model);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(Guid id)
    {
        var play = _context.Plays.FirstOrDefault(x => x.Id == id);
        if (play == null) return NotFound();
        return View(play);
    }

    [HttpPost]
    public IActionResult Edit(Guid id, Play model, IFormFile? posterImage)
    {
        var play = _context.Plays.FirstOrDefault(x => x.Id == id);
        if (play == null) return NotFound();

        play.Title = model.Title;
        play.Description = model.Description;
        play.Genre = model.Genre;
        play.DurationMinutes = model.DurationMinutes;

        if (posterImage != null && posterImage.Length > 0)
        {
            play.PosterImagePath = SaveUpload(posterImage);
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(Guid id)
    {
        var play = _context.Plays.FirstOrDefault(x => x.Id == id);
        if (play == null) return NotFound();
        return View(play);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(Guid id)
    {
        var play = _context.Plays.FirstOrDefault(x => x.Id == id);
        if (play == null) return NotFound();

        _context.Plays.Remove(play);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private string SaveUpload(IFormFile file)
    {
        var uploads = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploads);

        var ext = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{ext}";
        var fullPath = Path.Combine(uploads, fileName);

        using var stream = new FileStream(fullPath, FileMode.Create);
        file.CopyTo(stream);

        return $"/uploads/{fileName}";
    }
}
