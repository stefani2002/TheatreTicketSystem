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

    public IActionResult Confirm(Guid showId)
    {
        var vm = _ticketService.GetConfirmModel(showId);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    public IActionResult Confirm(PurchaseConfirmVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        return RedirectToAction(nameof(Payment),
            new { showId = vm.ShowId, quantity = vm.Quantity });
    }

    public async Task<IActionResult> Payment(Guid showId, int quantity)
    {
        var vm = await _ticketService.GetPaymentModelAsync(showId, quantity);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> PayAndBuy([FromBody] PaymentVm vm)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            var pdfBytes = await _ticketService.ConfirmStripeAndBuyAsync(vm, userId);
            return File(pdfBytes, "application/pdf", "tickets.pdf");
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
