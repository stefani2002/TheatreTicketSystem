using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Domain.Entities;

[Authorize(Roles = "Admin")]
public class AdminSettingsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AdminSettingsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var settings = _context.SiteSettings.First();
        return View(settings);
    }

    [HttpPost]
    public IActionResult UpdateHero(IFormFile? heroImage)
    {
        var settings = _context.SiteSettings.First();

        if (heroImage != null && heroImage.Length > 0)
        {
            settings.HeroImagePath = SaveUpload(heroImage);
            _context.SaveChanges();
        }

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

