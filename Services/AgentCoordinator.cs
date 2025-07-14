public class AgentCoordinator
{
    private readonly ILlmService _llm;

    public AgentCoordinator(ILlmService llm)
    {
        _llm = llm;
    }

    public async Task<AgentContext> ProcessAsync(string userRequest, string sessionId = "default")
    {
        var context = new AgentContext
        {
            UserRequest = userRequest,
            SessionId = sessionId,
            MessageHistory = new List<ChatMessage>
            {
                new ChatMessage { Role = "user", Content = userRequest }
            }
        };

        var planner = new PlannerAgent();
        planner.InjectLlm(_llm);
        await planner.ExecuteAsync(context);

        var builder = new BuilderAgent();
        builder.InjectLlm(_llm);
        await builder.ExecuteAsync(context);

        if (context.Memory.TryGetValue("Workers", out var list) && list is List<IWorkerAgent> workers)
        {
            foreach (var agent in workers)
            {
                await agent.ExecuteAsync(context);
            }
        }

        return context;
    }
}