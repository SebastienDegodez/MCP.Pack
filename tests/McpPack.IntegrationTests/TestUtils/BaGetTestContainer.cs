using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace McpPack.IntegrationTests.TestUtils;

public sealed class BaGetTestContainer : IAsyncDisposable
{
    private const int NugetPort = 80;
    private readonly IContainer _container;
    public string FeedUrl { get; private set; } = string.Empty;

    public BaGetTestContainer()
    {
        _container = new ContainerBuilder()
            .WithImage("loicsharma/baget:latest")
            .WithName($"baget-test-{Guid.NewGuid():N}")
            .WithPortBinding(NugetPort, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(NugetPort))
            .Build();
    }

    public async Task StartAsync()
    {
        await _container.StartAsync().ConfigureAwait(false);
        FeedUrl = $"http://localhost:{_container.GetMappedPublicPort(NugetPort)}/v3/index.json";
    }

    public async Task UploadPackageAsync(string nupkgPath)
    {
        using var client = new HttpClient();
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(File.OpenRead(nupkgPath)), "package", Path.GetFileName(nupkgPath) }
        };
        var pushUrl = $"http://localhost:{_container.GetMappedPublicPort(NugetPort)}/api/v2/package";
        var response = await client.PutAsync(pushUrl, content);
        response.EnsureSuccessStatusCode();
    }

    public async ValueTask DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}
