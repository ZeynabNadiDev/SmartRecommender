using Microsoft.AspNetCore.Mvc;
using SmartRecommender.Application.Abstractions.Services;
using SmartRecommender.Models.Requests;

namespace SmartRecommender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }
        [HttpPost("recommend")]
        public async Task <IActionResult> GetRecommendation([FromBody] ChatRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message cannot be empty.");

            var result = await _chatService.GetRecommendationAsync(request.Message, cancellationToken);
            return Ok(result);
        }
    }
}
