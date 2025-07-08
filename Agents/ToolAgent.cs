public class ToolAgent : IWorkerAgent
{
    public string Name => $"ToolAgent: {Step}";
    public string Step { get; }

    public ToolAgent(string step) => Step = step;

    public async Task ExecuteAsync(AgentContext context)
    {
        var executor = new ToolExecutor();

        // Собираем входные данные из предыдущих результатов
        var previousResults = context.Memory
            .Where(kv => kv.Key.StartsWith("Result:"))
            .ToDictionary(kv => kv.Key, kv => kv.Value?.ToString());

        var input = string.Join("; ", previousResults.Select(p => $"{p.Key}={p.Value}"));

        var result = await executor.ExecuteToolAsync("sort_tasks", context);
        context.Logs.Add($"{Name}: использовал вход: {input}");
        context.Logs.Add($"{Name}: результат инструмента — {result}");

        context.Memory[$"Result:{Step}"] = result;
    }
}