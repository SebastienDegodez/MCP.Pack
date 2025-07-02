using FakeItEasy;
using McpPack.Application.RetrievePackage;
using McpPack.Domain.PackageAggregate;
using Xunit;

namespace McpPack.UnitTests.Application.RetrievePackage;

public sealed class RetrievePackageMetadataUseCaseTests
{
    private readonly INugetService _nugetService;
    private readonly RetrievePackageMetadataUseCase _useCase;

    public RetrievePackageMetadataUseCaseTests()
    {
        _nugetService = A.Fake<INugetService>();
        _useCase = new RetrievePackageMetadataUseCase(_nugetService);
    }

    [Fact]
    public void Constructor_WithNullNugetService_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => 
            new RetrievePackageMetadataUseCase(null!));

        Assert.Equal("nugetService", exception.ParamName);
    }

    [Fact]
    public async Task ExecuteAsync_WithValidQuery_ShouldReturnPackageMetadata()
    {
        // Arrange
        var query = "Newtonsoft.Json";
        var availableVersions = AvailablePackageVersions.Create([PackageVersion.Create("13.0.3")]);
        var expectedPackages = new List<PackageMetadata>
        {
            PackageMetadata.Create("Newtonsoft.Json", new Uri("https://www.newtonsoft.com/json"), availableVersions)
        };

        A.CallTo(() => _nugetService.SearchPackagesAsync(query, A<CancellationToken>._))
            .Returns(expectedPackages);

        // Act
        var result = await _useCase.ExecuteAsync(query, TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(expectedPackages, result);
        A.CallTo(() => _nugetService.SearchPackagesAsync(query, A<CancellationToken>._))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExecuteAsync_WithInvalidQuery_ShouldThrowArgumentException(string invalidQuery)
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _useCase.ExecuteAsync(invalidQuery, TestContext.Current.CancellationToken));
        
        Assert.Contains("Query cannot be null or empty", exception.Message);
        Assert.Equal("query", exception.ParamName);

        A.CallTo(() => _nugetService.SearchPackagesAsync(A<string>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task ExecuteAsync_WithNullQuery_ShouldThrowArgumentException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _useCase.ExecuteAsync(null!, TestContext.Current.CancellationToken));
        
        Assert.Contains("Query cannot be null or empty", exception.Message);
        Assert.Equal("query", exception.ParamName);

        A.CallTo(() => _nugetService.SearchPackagesAsync(A<string>._, A<CancellationToken>._))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task ExecuteAsync_WithCancellationToken_ShouldPassTokenToService()
    {
        // Arrange
        var query = "TestPackage";
        var cancellationToken = new CancellationToken(true);
        var expectedPackages = new List<PackageMetadata>();

        A.CallTo(() => _nugetService.SearchPackagesAsync(query, cancellationToken))
            .Returns(expectedPackages);

        // Act
        var result = await _useCase.ExecuteAsync(query, cancellationToken);

        // Assert
        Assert.Equal(expectedPackages, result);
        A.CallTo(() => _nugetService.SearchPackagesAsync(query, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ExecuteAsync_WhenServiceThrows_ShouldPropagateException()
    {
        // Arrange
        var query = "TestPackage";
        var expectedException = new InvalidOperationException("Service error");

        A.CallTo(() => _nugetService.SearchPackagesAsync(query, A<CancellationToken>._))
            .Throws(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _useCase.ExecuteAsync(query, TestContext.Current.CancellationToken));
        
        Assert.Equal("Service error", exception.Message);
    }
}
