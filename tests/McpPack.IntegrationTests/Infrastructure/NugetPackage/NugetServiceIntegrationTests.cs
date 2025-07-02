using McpPack.Infrastructure.NugetPackage;
using McpPack.IntegrationTests.TestUtils;
using Microsoft.Extensions.Options;
using Xunit;

namespace McpPack.IntegrationTests.Infrastructure.NugetPackage;

public sealed class NugetServiceIntegrationTests : IAsyncLifetime
{
    private NugetService? _nugetService;
    private string? _nupkgPath;
    private BaGetTestContainer? _baGetContainer;
    private List<string> _nupkgPathPopular = [];

    [Fact]
    public async Task SearchPackagesAsync_WithValidQuery_ShouldReturnPackages()
    {
        // Arrange
        var query = "Fake.Newtonsoft.Json";

        // Act
        var result = await _nugetService!.SearchPackagesAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains(result, p => p.Title.Contains("Fake.Newtonsoft.Json", StringComparison.OrdinalIgnoreCase));
        Assert.All(result, p => Assert.True(p.HasVersions()));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SearchPackagesAsync_WithInvalidQuery_ShouldThrowArgumentException(string invalidQuery)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _nugetService!.SearchPackagesAsync(invalidQuery, TestContext.Current.CancellationToken));
        
        Assert.Contains("Query cannot be null or empty", exception.Message);
        Assert.Equal("query", exception.ParamName);
    }

    [Fact]
    public async Task SearchPackagesAsync_WithNullQuery_ShouldThrowArgumentException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(
            () => _nugetService!.SearchPackagesAsync(null!, TestContext.Current.CancellationToken));
        
        Assert.Contains("Query cannot be null or empty", exception.Message);
        Assert.Equal("query", exception.ParamName);
    }

    [Fact]
    public async Task SearchPackagesAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        // Arrange
        var query = "TestPackage";
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<TaskCanceledException>(
            () => _nugetService!.SearchPackagesAsync(query, cancellationTokenSource.Token));
    }

    [Fact]
    public async Task SearchPackagesAsync_WithUnknownPackage_ShouldReturnEmptyResult()
    {
        // Arrange
        var query = "ThisPackageDoesNotExistAnywhere12345";

        // Act
        var result = await _nugetService!.SearchPackagesAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task SearchPackagesAsync_WithPopularPackage_ShouldReturnMetadataWithVersions()
    {
        // Arrange
        var query = "Fake.Popular.Package";

        // Act
        var result = await _nugetService!.SearchPackagesAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.NotEmpty(result);
        var popularPackage = result.FirstOrDefault(p =>
            p.Title.Equals("Fake.Popular.Package", StringComparison.OrdinalIgnoreCase));

        Assert.NotNull(popularPackage);
        Assert.True(popularPackage.HasVersions());
        Assert.NotNull(popularPackage.GetLatestVersion());
    }

    public async ValueTask InitializeAsync()
    {
        _baGetContainer = new BaGetTestContainer();
        await _baGetContainer.StartAsync().ConfigureAwait(false);

        // Génère un .nupkg factice pour le test
        _nupkgPath = TestPackageBuilder.CreateTestPackage("Fake.Newtonsoft.Json", "1.0.0");
        await _baGetContainer.UploadPackageAsync(_nupkgPath);

        for(var version = 0; version <= 5; version++)
        {
            var popularPackage = TestPackageBuilder.CreateTestPackage($"Fake.Popular.Package", $"1.{version}.0");
            _nupkgPathPopular.Add(popularPackage);
            await _baGetContainer.UploadPackageAsync(popularPackage);
        }

        var options = Options.Create(new NugetServiceOptions
        {
            FeedUrls = [_baGetContainer.FeedUrl ]
        });
        _nugetService = new NugetService(options);

    }

    public async ValueTask DisposeAsync()
    {
        await _baGetContainer!.DisposeAsync();
        if (!string.IsNullOrEmpty(_nupkgPath) && File.Exists(_nupkgPath))
        {
            File.Delete(_nupkgPath);
        }
        foreach (var nupkg in _nupkgPathPopular)
        {
            if (File.Exists(nupkg))
            {
                File.Delete(nupkg);
            }
        }
    }
}
