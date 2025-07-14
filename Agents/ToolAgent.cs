public class ToolAgent : IWorkerAgent
{
    public string Name => $"ToolAgent: {Step}";
    public string Step { get; }

    public ToolAgent(string step) => Step = step;

    public async Task ExecuteAsync(AgentContext context)
    {
        var executor = new ToolExecutor();
        var result = await executor.ExecuteToolAsync("sort_tasks", context);
        context.Logs.Add($"{Name}: результат инструмента — {result}");
        context.Memory[$"Result:{Step}"] = result;
    }
}