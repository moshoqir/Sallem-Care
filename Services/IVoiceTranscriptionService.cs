using System.Threading;
using System.Threading.Tasks;

namespace SaleemCare.Api.Services;

public interface IVoiceTranscriptionService
{
    // Transcribes audio files into text.
    // Language will be en,ar etc.

    Task<string> TranscribeAsync(
        Stream audioStream,
        string? languageCode,
        CancellationToken ct = default
        );
}

