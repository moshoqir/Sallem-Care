
namespace SaleemCare.Api.Dtos.Voice;

public class VoiceRecognitionResponse
{
    public string Transcript { get; set; } = string.Empty;
    public string AiReply { get; set; } = string.Empty;
}