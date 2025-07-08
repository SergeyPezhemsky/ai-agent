public class BuilderAgent : IAgent
{
    private ILlmService _llm;
    public string Name => "Builder";

    public void InjectLlm(ILlmService llm) => _llm = llm;

    public async Task ExecuteAsync(AgentContext context)
    {
        if (!context.Memory.TryGetValue("PlannerPlan", out var planObj) || planObj is not List<string> steps)
        {
            context.Logs.Add("Builder: нет плана для построения агентов.");
            return;
        }

        var agents = new List<IWorkerAgent>();

        foreach (var step in steps)
        {
            var prompt = $@"Категоризуй следующий шаг как один из трех типов: 'tool', 'llm', 'user'.
Шаг: '{step}'
Ответи только одним словом: tool, llm или user.";

            var type = await _llm.AskAsync(prompt, context);
            type = type.ToLowerInvariant().Trim();

            IWorkerAgent agent = type switch
            {
                "tool" => new ToolAgent(step),
                "user" => new UserAgent(step),
                _ => new LlmAgent(step, _llm)
            };

            agents.Add(agent);
            context.Logs.Add($"Builder: создан агент для шага [{step}] типа [{type}]");
        }

        context.Memory["Workers"] = agents;
    }
}