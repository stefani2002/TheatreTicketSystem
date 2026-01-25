using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TheatreTicketSystem.Domain.Entities;
using TheatreTicketSystem.Repository.Data;
using TheatreTicketSystem.Service.ViewModels;
using Document = QuestPDF.Fluent.Document;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }


    public PurchaseConfirmVm GetConfirmModel(Guid showId)
    {
        var show = _context.Shows
            .AsNoTracking()
            .Include(s => s.Play)
            .FirstOrDefault(s => s.Id == showId);

        if (show == null) return null;

        return new PurchaseConfirmVm
        {
            ShowId = show.Id,
            PlayTitle = show.Play.Title,
            ShowTime = show.ShowTime,
            Price = show.Price,
            AvailableSeats = show.AvailableSeats,
            Quantity = 1
        };
    }

  
    public PaymentVm GetPaymentModel(Guid showId, int quantity)
    {
        var show = _context.Shows
            .AsNoTracking()
            .Include(s => s.Play)
            .FirstOrDefault(s => s.Id == showId);

        if (show == null) return null;

        if (quantity < 1) quantity = 1;

        return new PaymentVm
        {
            ShowId = show.Id,
            Quantity = quantity,
            PlayTitle = show.Play.Title,
            ShowTime = show.ShowTime,
            UnitPrice = show.Price,
            TotalPrice = show.Price * quantity,

           
            CardHolderName = "Test User",
            CardNumber = "4242424242424242",
            ExpMonth = 12,
            ExpYear = 2030,
            Cvv = "123"
        };
    }


    public byte[] ProcessPaymentAndBuy(PaymentVm vm, string userId)
    {
        var show = _context.Shows
            .Include(s => s.Play)
            .FirstOrDefault(s => s.Id == vm.ShowId);

        if (show == null)
            throw new Exception("Show not found");

        if (vm.Quantity < 1)
            throw new Exception("Quantity must be at least 1");

        if (vm.Quantity > show.AvailableSeats)
            throw new Exception($"Only {show.AvailableSeats} seats available");

        // ✅ Normalize input (важно!)
        var cardNumber = (vm.CardNumber ?? "")
            .Replace(" ", "")
            .Replace("-", "")
            .Trim();

        var cvv = (vm.Cvv ?? "").Trim();

        // (Optional) quick sanity checks for nicer errors
        if (cardNumber.Length < 12)
            throw new Exception("Card number is too short");

        if (vm.ExpMonth < 1 || vm.ExpMonth > 12)
            throw new Exception("Invalid expiry month");

        if (vm.ExpYear < 2000)
            throw new Exception("Invalid expiry year");

        // Validate TEST card (from DB)
        var card = _context.PaymentCards.FirstOrDefault(c =>
            c.IsActive &&
            c.CardNumber == cardNumber &&
            c.Cvv == cvv &&
            c.ExpMonth == vm.ExpMonth &&
            c.ExpYear == vm.ExpYear);

        if (card == null)
        {
            
            var numberExists = _context.PaymentCards.Any(c => c.CardNumber == cardNumber);
            if (!numberExists)
                throw new Exception("Invalid test card (card number not found in database)");

            var existsButNotMatch = _context.PaymentCards.Any(c =>
                c.CardNumber == cardNumber && c.IsActive);

            if (existsButNotMatch)
                throw new Exception("Invalid test card (expiry/CVV mismatch)");

            throw new Exception("Invalid test card");
        }

        var total = show.Price * vm.Quantity;

        if (card.Balance < total)
            throw new Exception("Insufficient balance");

        using var tx = _context.Database.BeginTransaction();

       
        card.Balance -= total;

       
        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.Now,
            TotalPrice = total,
            UserId = userId
        };
        _context.Orders.Add(order);

       
        var sold = show.TotalSeats - show.AvailableSeats;
        var startSeat = sold + 1;

        var codes = new List<string>();

        for (int i = 0; i < vm.Quantity; i++)
        {
            var code = Guid.NewGuid().ToString("N").ToUpper();
            codes.Add(code);

            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                ShowId = show.Id,
                OrderId = order.Id,
                UserId = userId,
                SeatNumber = startSeat + i,
                Price = show.Price,
                Code = code
            };

            _context.Tickets.Add(ticket);
        }

        show.AvailableSeats -= vm.Quantity;

        _context.SaveChanges();
        tx.Commit();

        return GeneratePdf(order.Id, show, vm.Quantity, total, codes);
    }

    private byte[] LoadLogo()
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(),
                                 "wwwroot", "images", "logo.png");

        return File.Exists(path) ? File.ReadAllBytes(path) : null;
    }
    private byte[] GeneratePdf(Guid orderId, Show show, int quantity, decimal total, List<string> codes)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                var logo = LoadLogo();

                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Row(row =>
                {
                    row.Spacing(10);

                    if (logo != null)
                    {
                        row.ConstantItem(60)
                           .Height(60)
                           .Image(logo)
                           .FitArea();
                    }

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("Мој театар")
                            .FontSize(20)
                            .SemiBold();

                        col.Item().Text("Ви благодатиме на довербата! 🎭")
                            .FontSize(10)
                            .FontColor(Colors.Grey.Medium);
                    });
                });

                page.Content().Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text($"Order ID: {orderId}");
                    col.Item().Text($"Play: {show.Play.Title}");
                    col.Item().Text($"Show time: {show.ShowTime:dd.MM.yyyy HH:mm}");
                    col.Item().Text($"Quantity: {quantity}");
                    col.Item().Text($"Total price: {total} MKD").SemiBold();

                    col.Item().LineHorizontal(1);

                    col.Item().Text("Tickets:").SemiBold();
                    foreach (var c in codes)
                        col.Item().Text($"Code: {c}");
                });
            });
        });

        return doc.GeneratePdf();
    }
}
