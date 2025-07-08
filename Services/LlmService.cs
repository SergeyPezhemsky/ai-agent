public interface ILlmService
{
    Task<string> AskAsync(string prompt, AgentContext context);
}

public class OpenAiLlmService : ILlmService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey = "sk-your-api-key";

    public OpenAiLlmService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> AskAsync(string prompt, AgentContext context)
    {
        var body = new
        {
            model = "gpt-4o",
            messages = new[] {
                new { role = "system", content = "Ты агент-помощник в системе управления задачами." },
                new { role = "user", content = prompt }
            },
            temperature = 0.7
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Headers = { { "Authorization", $"Bearer {_apiKey}" } },
            Content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(request);
        var result = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(result);

        return json.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }
}