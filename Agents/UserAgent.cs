public class UserAgent : IWorkerAgent
{
    public string Name => $"UserAgent: {Step}";
    public string Step { get; }

    public UserAgent(string step) => Step = step;

    public async Task ExecuteAsync(AgentContext context)
    {
        var previous = context.Memory
            .Where(kv => kv.Key.StartsWith("Result:"))
            .Select(kv => $"{kv.Key.Replace("Result:", "")}: {kv.Value}")
            .ToList();

        var history = string.Join("\n", previous);

        context.OutboundMessages.Add(new AgentMessage
        {
            Sender = Name,
            Message = $"Учитывая:\n{history}\n\nНужно выполнить: {Step}. Как мне поступить?",
            RequiresUserResponse = true
        });

        context.Logs.Add($"{Name}: запрос к пользователю по шагу.");
        await Task.CompletedTask;
    }
}