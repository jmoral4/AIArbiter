using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AIArbiter;

public class ChatManager
{
    private readonly IKernelFactory _kernelFactory;
    private readonly IClaudeService _claudeService;

    public ChatManager(IKernelFactory kernelFactory, IClaudeService claudeService)
    {
        _kernelFactory = kernelFactory;
        _claudeService = claudeService;
    }

    public async Task<string> GetResponse(string prompt, string modelType)
    {
        if (modelType.ToLower() == "claude")
        {
            return await _claudeService.GetResponseAsync($"You are an intelligent AI Assistant based on the model known as {modelType}", prompt);
        }
        else
        {
            var kernel = _kernelFactory.GetKernel(modelType);
            var chatService = kernel.GetRequiredService<IChatCompletionService>();
            var chat = new ChatHistory();
            chat.AddSystemMessage($"You are an intelligent AI Assistant based on the model known as {modelType}");

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                MaxTokens = GlobalConfig.MaxTokens,
                Temperature = 0.5,
                TopP = 1,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
                StopSequences = new[] { "Human:", "AI:" },
            };

            var response = await chatService.GetChatMessageContentAsync(prompt, executionSettings);
            return response.Content;
        }
    }
}