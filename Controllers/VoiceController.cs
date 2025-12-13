using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SaleemCare.Api.Dtos.Voice;
using SaleemCare.Api.Services;

namespace SaleemCare.Api.Controllers;


[ApiController]
[Route("v1/voice")]
[Authorize]

public class VoiceController : ControllerBase
{
    private readonly IVoiceTranscriptionService _voice;
    private readonly GoogleAiService _ai;


    public VoiceController(IVoiceTranscriptionService voice, GoogleAiService ai)
    {
        _voice = voice;
        _ai = ai;
    }


    /// <summary>
    /// Recognizes speech from an audio file and retrns the transcript AI Reply
    /// </summary>
    /// 
    [HttpPost("recognize")]
    [EnableRateLimiting("VoicePolicy")]
    [RequestSizeLimit(10_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Recognize(
        [FromForm] IFormFile audio,
        [FromForm] string? languageCode,
        CancellationToken ct)
    {
        if (audio is null || audio.Length == 0)
        {
            return BadRequest(new { error = "No audio file uploaded." });
        }

        // validate audio file type
        var allowedTypes = new[] { "audio/wav", "audio/mpeg", "audio/mp3", "audio/webm", "audio/ogg", "audio/m4a" };

        if (!string.IsNullOrEmpty(audio.ContentType) && !allowedTypes.Contains(audio.ContentType))
        {
            return BadRequest(new { error = "Unsupported audio format." });
        }


        // transcirbe audio
        string transcript;
       
        await using (var stream = audio.OpenReadStream())
        {
            
            transcript = await _voice.TranscribeAsync(stream, languageCode, ct);
        }

       if (string.IsNullOrWhiteSpace(transcript))
        {
            return BadRequest(new { error = "Could not transcribe the audio." });
        }

        // send transcript to AI for reply
        var aiReply = await _ai.AskAsync(transcript);


        var response = new VoiceRecognitionResponse
        {
            Transcript = transcript,
            AiReply = aiReply
        };

        return Ok(response);
    }
}


