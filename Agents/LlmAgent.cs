public class LlmAgent : IWorkerAgent
{
    public string Name => $"LlmAgent: {Step}";
    public string Step { get; }
    private readonly ILlmService _llm;

    public LlmAgent(string step, ILlmService llm)
    {
        Step = step;
        _llm = llm;
    }

    public async Task ExecuteAsync(AgentContext context)
    {
        var result = await _llm.AskAsync($"Выполни шаг: {Step}", context);
        context.Logs.Add($"{Name}: результат LLM — {result}");
    }
}