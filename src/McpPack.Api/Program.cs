using McpPack.Api;
using McpPack.Application.RetrievePackage;
using McpPack.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Logging.AddConsole(consoleLogOptions =>
{
    // Configure all logs to go to stderr
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

// Register application use cases
builder.Services.AddTransient<RetrievePackageMetadataUseCase>();

// Register all infrastructure services through Infrastructure layer
// Configure NugetServiceOptions from configuration (appsettings.json or environment variables)
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithTools<NugetPackageTools>(serializerOptions: McpPackJsonContext.Default.Options);

var application = builder.Build();

application.MapMcp();

await application.RunAsync();

public partial class Program
{
    // This partial class is used to allow the HostBuilder to be extended in other files if needed.
    // It can be useful for adding additional configuration or services in a modular way.
}