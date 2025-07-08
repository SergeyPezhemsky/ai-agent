public class ToolExecutor
{
    private readonly Dictionary<string, Func<AgentContext, Task<string>>> _tools = new();

    public ToolExecutor()
    {
        _tools["sort_tasks"] = async context =>
        {
            if (context.Memory.TryGetValue("tasks", out var value) && value is List<string> tasks)
            {
                var sorted = tasks.OrderBy(t => t).ToList();
                context.Memory["sorted_tasks"] = sorted;
                return "Задачи отсортированы";
            }
            return "Нет задач для сортировки";
        };
    }

    public async Task<string> ExecuteToolAsync(string toolName, AgentContext context)
    {
        if (_tools.TryGetValue(toolName, out var tool))
            return await tool(context);
        return $"Инструмент '{toolName}' не найден";
    }
}