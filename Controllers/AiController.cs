[ApiController]
[Route("api/ai")]
public class AiController : ControllerBase
{
    private readonly AgentCoordinator _coordinator;

    public AiController(ILlmService llm)
    {
        _coordinator = new AgentCoordinator(llm);
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] UserRequest request)
    {
        var result = await _coordinator.ProcessAsync(request.Input);
        return Ok(new
        {
            Logs = result.Logs,
            Results = result.Memory.Where(kv => kv.Key.StartsWith("Result:")),
            Messages = result.OutboundMessages
        });
    }
}