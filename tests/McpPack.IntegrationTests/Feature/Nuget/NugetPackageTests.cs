
using McpPack.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using ModelContextProtocol.Client;
using Xunit;

namespace McpPack.IntegrationTests.Feature.Nuget;

public sealed class NugetPackageTests
{
    [Fact]
    public async Task GetToolsList_ShouldReturnListOfTools()
    {
        // Arrange
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => 
            {
                builder.UseUrls("http://localhost:5000");
            });

        var transport = new SseClientTransport(new SseClientTransportOptions
        {
            Endpoint = new Uri("http://localhost:5000/sse"),
        }, factory.CreateClient());
        
        var mcpClient = await McpClientFactory.CreateAsync(transport, cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var response = await mcpClient.ListToolsAsync(cancellationToken:TestContext.Current.CancellationToken);

        // Assert
        Assert.Single(response);
        Assert.Equal(NugetPackageTools.RetrieveNugetPackageVersion, response[0].Name);
    }
}