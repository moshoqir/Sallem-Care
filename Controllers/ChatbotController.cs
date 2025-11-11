using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaleemCare.Api.Services;
using System.Threading.Tasks;


namespace SaleemCare.Api.Controllers;

[ApiController]
[Route("v1/chatbot")]

public class ChatbotController : ControllerBase
{
    private readonly GoogleAiService _ai;

    public ChatbotController(GoogleAiService ai) => _ai = ai;

    public record ChatRequest(string Message);

    [HttpPost("ask")]
    public async Task<IActionResult> Message([FromBody] ChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(new { error = "Message cannot be empty." });
        }

        var systemPrompt =
             "You are SaleemCare, a helpful medical assistant. "
          + "Understand the patient's message in Arabic or English, "
          + "and reply with a concise, polite, medical-safe explanation. "
          + "If symptoms are mentioned, list them in plain text.";

        var fullPrompt = $"{systemPrompt}\n\nPatient: {request.Message}\nAssistant:";

        var reply = await _ai.AskAsync(fullPrompt);

        return Ok(new { reply });
    }
}