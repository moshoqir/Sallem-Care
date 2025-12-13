using System.Threading;
using System.Threading.Tasks;

namespace SaleemCare.Api.Services;

public class FakeVoiceTranscriptionService : IVoiceTranscriptionService
{
    public Task<string> TranscribeAsync(
        Stream audioStream,
        string? languageCode,
        CancellationToken ct = default
        )

    {
        // TODO: We'll replce this with a real transcription service later.

        var lang = string.IsNullOrWhiteSpace(languageCode) ? "ar" : languageCode;
        var text = $"[FAKE TRANSCRIPTION ({lang}) ]";

        return Task.FromResult(text);
    }
}