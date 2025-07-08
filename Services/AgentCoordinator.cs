public class AgentCoordinator
{
    private readonly List<IAgent> _initialAgents;
    private readonly ILlmService _llm;

    public AgentCoordinator(ILlmService llm)
    {
        _llm = llm;
        var builder = new BuilderAgent();
        builder.InjectLlm(_llm);
        _initialAgents = new List<IAgent>
        {
            new PlannerAgent(),
            builder
        };
    }

    public async Task<AgentContext> ProcessAsync(string userRequest)
    {
        var context = new AgentContext { UserRequest = userRequest };

        foreach (var agent in _initialAgents)
            await agent.ExecuteAsync(context);

        if (context.Memory.TryGetValue("Workers", out var workerObj) && workerObj is List<IAgent> workers)
        {
            foreach (var worker in workers)
                await worker.ExecuteAsync(context);
        }

        return context;
    }
}