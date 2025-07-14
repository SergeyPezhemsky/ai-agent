public class LlmAgent : IWorkerAgent
{
    public string Name => $"LlmAgent: {Step}";
    public string Step { get; }
    private readonly ILlmService _llm;

    public LlmAgent(string step, ILlmService llm)
    {
        Step = step;
        _llm = llm;
    }

    public async Task ExecuteAsync(AgentContext context)
    {
        var messages = context.MessageHistory.ToList();
        messages.Add(new ChatMessage { Role = "user", Content = $"Теперь шаг: {Step}" });

        var result = await _llm.AskAsync(messages, context);

        context.Logs.Add($"{Name}: результат LLM — {result}");
        context.Memory[$"Result:{Step}"] = result;
        context.MessageHistory.Add(new ChatMessage { Role = "assistant", Content = result });
    }
}