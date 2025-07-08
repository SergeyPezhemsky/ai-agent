public class DialogueStore
{
    private static readonly Dictionary<string, DialogueLog> _dialogues = new();

    public static DialogueLog GetOrCreate(string sessionId)
    {
        if (!_dialogues.TryGetValue(sessionId, out var log))
        {
            log = new DialogueLog { Id = sessionId };
            _dialogues[sessionId] = log;
        }
        return log;
    }

    public static void AppendMessage(string sessionId, string sender, string message)
    {
        var log = GetOrCreate(sessionId);
        log.Turns.Add(new DialogueTurn { Sender = sender, Message = message });
    }

    public static DialogueLog? GetDialogue(string sessionId)
    {
        _dialogues.TryGetValue(sessionId, out var log);
        return log;
    }
}