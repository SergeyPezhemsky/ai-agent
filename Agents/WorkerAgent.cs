public class WorkerAgent : IAgent
{
    public string Name => $"Worker-{_task}";
    private readonly string _task;
    private ILlmService _llm;

    public WorkerAgent(string task) => _task = task;

    public void InjectLlm(ILlmService llm) => _llm = llm;

    public async Task ExecuteAsync(AgentContext context)
    {
        context.Logs.Add($"{Name}: выполняет {_task}");

        if (_task.Contains("LLM"))
        {
            var prompt = $"Сформируй план для запроса: '{context.UserRequest}'";
            var plan = await _llm.AskAsync(prompt, context);
            context.Memory["LLMPlan"] = plan;
            context.Logs.Add($"{Name}: план от LLM:\n{plan}");
        }

        if (_task.Contains("сортировать"))
        {
            var executor = new ToolExecutor();
            var result = await executor.ExecuteToolAsync("sort_tasks", context);
            context.Logs.Add($"{Name}: {result}");
        }

        context.Memory[$"Result:{_task}"] = $"Готово по {_task}";
    }
}