public class PlannerAgent : IAgent
{
    public string Name => "Planner";

    public async Task ExecuteAsync(AgentContext context)
    {
        context.Logs.Add("Planner: анализ запроса");

        context.Memory["Plan"] = new List<string>
        {
            "составить план с помощью LLM",
            "запросить критерии приоритета",
            "сортировать задачи"
        };

        await Task.CompletedTask;
    }
}