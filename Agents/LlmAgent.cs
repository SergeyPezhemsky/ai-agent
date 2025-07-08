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
        var previous = context.Memory
            .Where(kv => kv.Key.StartsWith("Result:"))
            .Select(kv => $"{kv.Key.Replace("Result:", "")}: {kv.Value}")
            .ToList();

        var history = string.Join("\n", previous);
        var prompt = $"Контекст:\n{history}\n\nШаг: {Step}\nЧто нужно сделать?";

        var result = await _llm.AskAsync(prompt, context);

        context.Logs.Add($"{Name}: с учётом истории: {result}");
        context.Memory[$"Result:{Step}"] = result;
    }
}