var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHttpClient<ILlmService, OpenAiLlmService>();
var app = builder.Build();
app.MapControllers();
app.Run();