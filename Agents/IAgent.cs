public interface IAgent
{
    string Name { get; }
    Task ExecuteAsync(AgentContext context);
}