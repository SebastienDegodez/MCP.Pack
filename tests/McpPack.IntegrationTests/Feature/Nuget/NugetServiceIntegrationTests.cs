using McpPack.Api;
using McpPack.IntegrationTests.TestUtils;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using Xunit;

namespace McpPack.IntegrationTests.Feature.Nuget;

public sealed class NugetServiceIntegrationTests : IAsyncLifetime
{
    private readonly BaGetTestContainer _baGetContainer;
    private WebApplicationFactory<Program> _factory = null!; // Initialized in InitializeAsync

    private const string TestPackageName = "TestPackage";
    private const string TestPackageVersion1 = "1.0.0";
    private const string TestPackageVersion2 = "2.0.0";
    private string _nupkgPathV1 = string.Empty;
    private string _nupkgPathV2 = string.Empty;

    public NugetServiceIntegrationTests()
    {
        _baGetContainer = new BaGetTestContainer();
    }

    /// <summary>
    /// Tests the search functionality of the NuGet service with a known package in BaGet.
    /// This test verifies that the package can be retrieved successfully when it exists in the BaGet feed.
    /// It uses the ModelContextProtocol to call the tool for retrieving the package version.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SearchPackagesAsync_WithKnownPackageInBaGet_ShouldReturnPackage()
    {
        // Arrange
        var options = new SseClientTransportOptions
        {
            Endpoint = new Uri("http://localhost/sse"),
        };
        var transport = new SseClientTransport(options, _factory.CreateClient());

        var mcpClient = await McpClientFactory.CreateAsync(transport, cancellationToken: TestContext.Current.CancellationToken);

        // Act
        var response = await mcpClient.CallToolAsync(
            NugetPackageTools.RetrieveNugetPackageVersion,
            new Dictionary<string, object?>
            {
                ["packageName"] = TestPackageName
            },
            cancellationToken: TestContext.Current.CancellationToken
        );

        // Assert
        Assert.NotNull(response);
        Assert.Single(response.Content);
        Assert.Equal("text", response.Content[0].Type); // Assuming the type is a string, adjust as necessary
        Assert.NotNull(((TextContentBlock)response.Content[0]).Text);
    }

    /// <summary>
    /// Initializes the test environment by starting the BaGet container and uploading a test package.
    /// </summary>
    /// <returns></returns>
    public async ValueTask InitializeAsync()
    {
        await _baGetContainer.StartAsync();
        // Génère un .nupkg factice pour le test
        _nupkgPathV1 = TestPackageBuilder.CreateTestPackage(TestPackageName, TestPackageVersion1);
        await _baGetContainer.UploadPackageAsync(_nupkgPathV1);
        _nupkgPathV2 = TestPackageBuilder.CreateTestPackage(TestPackageName, TestPackageVersion2);
        await _baGetContainer.UploadPackageAsync(_nupkgPathV2);

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "NugetService:FeedUrls:0", _baGetContainer.FeedUrl }
                    });
                });
            });
    }

    public async ValueTask DisposeAsync()
    {
        if (!string.IsNullOrEmpty(_nupkgPathV1) && File.Exists(_nupkgPathV1))
        {
            File.Delete(_nupkgPathV1);
        }
        if (!string.IsNullOrEmpty(_nupkgPathV2) && File.Exists(_nupkgPathV2))
        {
            File.Delete(_nupkgPathV2);
        }
        await _baGetContainer.DisposeAsync();
    }
}
