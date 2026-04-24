using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    private readonly IStripePaymentService _stripe;
    private readonly IConfiguration _config;

    public TicketService(ApplicationDbContext context, IStripePaymentService stripe, IConfiguration config)
    {
        _context = context;
        _stripe = stripe;
        _config = config;
    }

    public PurchaseConfirmVm GetConfirmModel(Guid showId)
    {
        var show = _context.Shows
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

    public async Task<PaymentVm> GetPaymentModelAsync(Guid showId, int quantity)
    {
        var show = _context.Shows
            .Include(s => s.Play)
            .FirstOrDefault(s => s.Id == showId);

        if (show == null) return null;
        if (quantity < 1) quantity = 1;

        var total = show.Price * quantity;


        var publishableKey = _config["Stripe:PublishableKey"] ?? "";
        var description = $"Tickets for {show.Play.Title} ({show.ShowTime:yyyy-MM-dd HH:mm}) x{quantity}";

        var intent = await _stripe.CreatePaymentIntentAsync(total, "mkd", description);

        return new PaymentVm
        {
            ShowId = show.Id,
            Quantity = quantity,
            PlayTitle = show.Play.Title,
            ShowTime = show.ShowTime,
            UnitPrice = show.Price,
            TotalPrice = total,

            StripePublishableKey = publishableKey,
            PaymentIntentClientSecret = intent.ClientSecret
        };
    }


    public async Task<byte[]> ConfirmStripeAndBuyAsync(PaymentVm vm, string userId)
    {
        var show = _context.Shows
            .Include(s => s.Play)
            .FirstOrDefault(s => s.Id == vm.ShowId);

        if (show == null)
            throw new Exception("Show not found");

        if (vm.Quantity < 1 || vm.Quantity > show.AvailableSeats)
            throw new Exception("Invalid quantity");

        if (string.IsNullOrWhiteSpace(vm.PaymentIntentId))
            throw new Exception("Missing PaymentIntentId");

       
        var intent = await _stripe.GetPaymentIntentAsync(vm.PaymentIntentId);

        if (intent == null || intent.Status != "succeeded")
            throw new Exception("Payment not confirmed");


        var total = show.Price * vm.Quantity;

        using var tx = _context.Database.BeginTransaction();

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

    private byte[] GeneratePdf(Guid orderId, Show show, int quantity, decimal total, List<string> codes)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        var doc = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Text("Theatre Ticket Receipt").FontSize(20).SemiBold();

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
