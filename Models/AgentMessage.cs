public class AgentMessage
{
    public string Sender { get; set; }
    public string Message { get; set; }
    public bool RequiresUserResponse { get; set; } = false;
    public string MessageId { get; set; } = Guid.NewGuid().ToString();
    public string? ToolName { get; set; }
}