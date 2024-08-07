using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace AIArbiter;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        // put each key in a text file in your output (or copy-if-newer,  or refactor this to appsettings.runtime.json and make sure you gitignore it
        //read all keys
        var oaikey = File.ReadAllText("oaikey.txt");
        var clkey = File.ReadAllText("claudekey.txt");


#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject 

        // Not sure why thee are evaluation only APIs but... that's what they are at the moment...
        var kernels = new Dictionary<string, Kernel>
        {
            ["llama3.1"] = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion("llama3.1", new Uri("http://localhost:11434"), apiKey: null)
                .Build(),

            ["mistral-nemo"] = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion("mistral-nemo", new Uri("http://localhost:11434"), apiKey: null)
                .Build(),

            ["gemma2"] = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion("gemma2", new Uri("http://localhost:11434"), apiKey: null)
                .Build(),

            ["gpt-4o"] = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion("gpt-4o", oaikey)
                .Build()
        };
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject 

        // Configure ClaudeService
        // Claude uses an entirely different approach to API -- requires custom headers...
        services.AddHttpClient<IClaudeService, ClaudeService>(client =>
        {
            client.DefaultRequestHeaders.Add("x-api-key", clkey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        });

        services.AddSingleton<IKernelFactory>(new KernelFactory(kernels));
        services.AddSingleton<ChatManager>();

        var serviceProvider = services.BuildServiceProvider();

        var chatManager = serviceProvider.GetRequiredService<ChatManager>();

        foreach (var model in kernels.Keys.Prepend("claude"))
        {
            Console.ForegroundColor = ColorCycler.GetNextColor();
            Console.WriteLine($"Asking {model}...");
            var prompt = "What kind of LLM or AI model are you? What is your name? Can you (briefly) tell me some of your features?";
            var response = await chatManager.GetResponse(prompt, model);
            Console.WriteLine($"{model} Response:");
            Console.WriteLine(response);
            Console.WriteLine("---------------------------");
        }
    }
}