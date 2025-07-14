public class AgentContext
{
    public string UserRequest { get; set; }
    public string SessionId { get; set; }
    public Dictionary<string, object> Memory { get; set; } = new();
    public List<string> Logs { get; set; } = new();
    public List<AgentMessage> OutboundMessages { get; set; } = new();
    public List<ChatMessage> MessageHistory { get; set; } = new();
}