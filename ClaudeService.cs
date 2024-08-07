using System.Text;
using System.Text.Json;

namespace AIArbiter;

public class ClaudeService : IClaudeService
{
    private readonly HttpClient _httpClient;

    public ClaudeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetResponseAsync(string systemPrompt, string prompt)
    {
        var requestContent = new
        {
            model = "claude-3-sonnet-20240229",
            max_tokens = GlobalConfig.MaxTokens,
            temperature = 0.7,
            system = systemPrompt,
            messages = new[]
            {
                new { role = "user", content = prompt }
            }
        };

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.anthropic.com/v1/messages")
        {
            Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json")
        };

        var response = await _httpClient.SendAsync(requestMessage);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var cresp = System.Text.Json.JsonSerializer.Deserialize<ClaudeResponse>(responseContent);

        return cresp.Content.First().Text;
    }
}