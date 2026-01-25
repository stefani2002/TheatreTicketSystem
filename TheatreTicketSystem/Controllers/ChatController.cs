using Microsoft.AspNetCore.Mvc;
using TheatreTicketSystem.Service.Interfaces;

namespace TheatreTicketSystem.Controllers
{
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public record ChatRequest(string message);

        [HttpPost("Ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest req)
        {
            var reply = await _chatService.GetResponseAsync(req?.message ?? "");
            return Json(new { reply });
        }
    }
}
