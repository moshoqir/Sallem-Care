using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace SaleemCare.Api.Services;

public class GoogleAiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public GoogleAiService(IConfiguration cfg, IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
        _apiKey = cfg["GoogleAi:ApiKey"] ?? throw new Exception("Missing Google AI API Key");


    }

    public async Task<string> AskAsync(string prompt)
    {
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}";

        var payload = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt} } }
            }
        };

        var json = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };

        // send the request
        var response = await _http.SendAsync(request);

        // read response as string
        var body = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Google AI API error: {response.StatusCode} {body}");
        }

        // parse the response to extract the generated txt
        using var doc = JsonDocument.Parse(body);
        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "";



    }
}