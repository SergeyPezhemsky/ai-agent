public class PlannerAgent : IAgent
{
    public string Name => "Planner";
    private ILlmService _llm;

    public void InjectLlm(ILlmService llm) => _llm = llm;

    public async Task ExecuteAsync(AgentContext context)
    {
        var messages = context.MessageHistory.ToList();
        messages.Add(new ChatMessage
        {
            Role = "user",
            Content = $"Составь пошаговый план для запроса: '{context.UserRequest}' в формате списка."
        });

        var planText = await _llm.AskAsync(messages, context);
        context.MessageHistory.Add(new ChatMessage { Role = "assistant", Content = planText });

        var steps = planText
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.TrimStart('-', '*', '•', ' ', '\t'))
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .ToList();

        context.Memory["PlannerPlan"] = steps;
        context.Logs.Add($"Planner: создан план из {steps.Count} шагов.");
    }
}