public class UserAgent : IWorkerAgent
{
    public string Name => $"UserAgent: {Step}";
    public string Step { get; }

    public UserAgent(string step) => Step = step;

    public async Task ExecuteAsync(AgentContext context)
    {
        context.OutboundMessages.Add(new AgentMessage
        {
            Sender = Name,
            Message = $"Нужно выполнить: {Step}. Как мне поступить?",
            RequiresUserResponse = true
        });

        context.Logs.Add($"{Name}: запрос к пользователю.");
        context.MessageHistory.Add(new ChatMessage { Role = "assistant", Content = $"Нужно выполнить: {Step}" });

        await Task.CompletedTask;
    }
}