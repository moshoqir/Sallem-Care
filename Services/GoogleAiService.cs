using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SaleemCare.Api.Services;

public class GoogleAiService
{
    private readonly HttpClient _http;
    private readonly string _apiKey;
    private readonly string _apiVersion;
    private readonly string _model;

    // simple process-wide cache for discovered model
    private static string? _cachedModel;

    public GoogleAiService(IConfiguration cfg, IHttpClientFactory factory)
    {
        _http = factory.CreateClient();
        _http.Timeout = TimeSpan.FromSeconds(12); // Big-3: fast timeout for mobile MVP

        _apiKey = cfg["GoogleAi:ApiKey"] ?? throw new Exception("Missing GoogleAi:ApiKey");
        _apiVersion = cfg["GoogleAi:ApiVersion"] ?? "v1beta";
        _model = cfg["GoogleAi:Model"] ?? "gemini-2.5-flash";
    }

    public async Task<string> AskAsync(string userMessage, string? systemPreface = null, CancellationToken ct = default)
    {
        var prompt = string.IsNullOrWhiteSpace(systemPreface)
            ? userMessage
            : $"{systemPreface}\n\nUser: {userMessage}\nAssistant:";

        var modelToUse = _cachedModel ?? _model;

        try
        {
            return await GenerateAsync(modelToUse, prompt, ct);
        }
        catch (HttpRequestException ex) when (ex.Data["statusCode"]?.ToString() == "404")
        {
            // Big-3: cache auto-discovered supported model
            _cachedModel = await PickModelAsync(ct) ?? modelToUse;
            return await GenerateAsync(_cachedModel, prompt, ct);
        }
    }

    // ------------ Core call with retry + robust parse ------------
    private async Task<string> GenerateAsync(string model, string prompt, CancellationToken ct)
    {
        var url = $"https://generativelanguage.googleapis.com/{_apiVersion}/models/{model}:generateContent?key={_apiKey}";

        var payload = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        using var req = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        // Big-3: tiny retry for 429/5xx
        using var res = await SendWithSimpleRetry(req, ct);
        var body = await res.Content.ReadAsStringAsync(ct);

        if (!res.IsSuccessStatusCode)
        {
            var ex = new HttpRequestException($"Google AI API error: {res.StatusCode} {Truncate(body, 500)}");
            ex.Data["statusCode"] = ((int)res.StatusCode).ToString();
            throw ex;
        }

        // Big-3: robust parsing (safety blocks / missing fields)
        return ExtractText(body.AsSpan());
    }

    // ------------ Model discovery (cached) ------------
    private async Task<string?> PickModelAsync(CancellationToken ct)
    {
        var listUrl = $"https://generativelanguage.googleapis.com/{_apiVersion}/models?key={_apiKey}";
        using var req = new HttpRequestMessage(HttpMethod.Get, listUrl);

        using var res = await SendWithSimpleRetry(req, ct);
        var body = await res.Content.ReadAsStringAsync(ct);
        if (!res.IsSuccessStatusCode) return null;

        using var doc = JsonDocument.Parse(body);
        if (!doc.RootElement.TryGetProperty("models", out var models)) return null;

        string? best = null;
        foreach (var m in models.EnumerateArray())
        {
            var name = m.TryGetProperty("name", out var n) ? n.GetString() : null;
            if (string.IsNullOrWhiteSpace(name)) continue;

            if (m.TryGetProperty("supportedGenerationMethods", out var methods) &&
                methods.EnumerateArray().Any(x => string.Equals(x.GetString(), "generateContent", StringComparison.OrdinalIgnoreCase)))
            {
                var shortName = name.Replace("models/", "", StringComparison.Ordinal);
                if (shortName.StartsWith("gemini-2.5-flash", StringComparison.OrdinalIgnoreCase))
                    return shortName;

                best ??= shortName;
            }
        }

        return best;
    }

    // ------------ Simple retry helper (429/5xx) ------------
    private async Task<HttpResponseMessage> SendWithSimpleRetry(HttpRequestMessage req, CancellationToken ct)
    {
        // cloneable content? We rebuild per attempt below
        for (var attempt = 0; attempt < 2; attempt++)
        {
            using var clone = await CloneAsync(req, ct);
            var res = await _http.SendAsync(clone, ct);
            var code = (int)res.StatusCode;

            if (code == 429 || code >= 500)
            {
                // jittered backoff
                await Task.Delay(300 + Random.Shared.Next(200), ct);
                continue;
            }

            return res;
        }

        using var last = await CloneAsync(req, ct);
        return await _http.SendAsync(last, ct);
    }

    // Rebuild a request (since HttpContent can't be reused after send)
    private static async Task<HttpRequestMessage> CloneAsync(HttpRequestMessage original, CancellationToken ct)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri);

        // headers
        foreach (var h in original.Headers)
            clone.Headers.TryAddWithoutValidation(h.Key, h.Value);

        if (original.Content != null)
        {
            var contentStr = await original.Content.ReadAsStringAsync(ct);
            clone.Content = new StringContent(contentStr, Encoding.UTF8, original.Content.Headers.ContentType?.MediaType ?? "application/json");

            // copy content headers
            foreach (var h in original.Content.Headers)
                clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
        }

        return clone;
    }

    // ------------ Robust parsing ------------
    private static string ExtractText(ReadOnlySpan<char> jsonUtf16)
    {
        using var doc = JsonDocument.Parse(jsonUtf16.ToString());
        var root = doc.RootElement;

        // If blocked by safety, bubble up a clear error
        if (root.TryGetProperty("promptFeedback", out var pf) &&
            pf.TryGetProperty("blockReason", out var br))
        {
            var reason = br.GetString() ?? "unknown";
            throw new InvalidOperationException($"AI response blocked by safety: {reason}");
        }

        // candidates[0].content.parts[0].text
        if (root.TryGetProperty("candidates", out var cands) && cands.GetArrayLength() > 0)
        {
            var c0 = cands[0];

            if (c0.TryGetProperty("content", out var content) &&
                content.TryGetProperty("parts", out var parts) &&
                parts.GetArrayLength() > 0 &&
                parts[0].TryGetProperty("text", out var t))
            {
                return t.GetString() ?? string.Empty;
            }
        }

        // Fallback: empty (don’t throw if we simply didn’t get text)
        return string.Empty;
    }

    private static string Truncate(string s, int max)
        => s.Length <= max ? s : s[..max];
}
