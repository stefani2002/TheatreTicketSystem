using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheatreTicketSystem.Service.ViewModels;

[Authorize]
public class TicketsController : Controller
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    // STEP 1: Confirm page
    public IActionResult Confirm(Guid showId)
    {
        var vm = _ticketService.GetConfirmModel(showId);
        if (vm == null) return NotFound();

        return View(vm);
    }

    // STEP 2: Confirm POST → Payment
    [HttpPost]
    public IActionResult Confirm(PurchaseConfirmVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        return RedirectToAction(nameof(Payment),
            new { showId = vm.ShowId, quantity = vm.Quantity });
    }

    // STEP 3: Payment page
    public IActionResult Payment(Guid showId, int quantity)
    {
        var vm = _ticketService.GetPaymentModel(showId, quantity);
        if (vm == null) return NotFound();

        return View(vm);
    }

    // STEP 4: BUY + PDF
    [HttpPost]
    public IActionResult PayAndBuy(PaymentVm vm)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var pdfBytes = _ticketService.ProcessPaymentAndBuy(vm, userId);

        if (pdfBytes == null)
            return View("Payment", vm);

        return File(pdfBytes, "application/pdf", "tickets.pdf");
    }
}
