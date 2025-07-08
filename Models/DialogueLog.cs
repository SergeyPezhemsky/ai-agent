public class DialogueLog
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<DialogueTurn> Turns { get; set; } = new();
}

public class DialogueTurn
{
    public string Sender { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}