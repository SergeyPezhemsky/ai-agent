public class BuilderAgent : IAgent
{
    private ILlmService _llm;
    public string Name => "Builder";

    public void InjectLlm(ILlmService llm) => _llm = llm;

    public async Task ExecuteAsync(AgentContext context)
    {
        var plan = context.Memory["Plan"] as List<string>;
        var workers = plan?.Select(task =>
        {
            var worker = new WorkerAgent(task);
            worker.InjectLlm(_llm);
            return worker;
        }).ToList();

        context.Memory["Workers"] = workers;
        await Task.CompletedTask;
    }
}