namespace AIArbiter;

public interface IClaudeService
{
    Task<string> GetResponseAsync(string systemPrompt, string prompt);
}