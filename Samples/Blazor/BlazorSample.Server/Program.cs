using BlazorSample.Server;
using ConfigurationUi.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddConfigurationUi<Settings>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.Run();
