using McpPack.Domain.PackageAggregate;

namespace McpPack.Application.RetrievePackage;

public sealed class RetrievePackageMetadataUseCase
{
    private readonly INugetService _nugetService;

    public RetrievePackageMetadataUseCase(INugetService nugetService)
    {
        _nugetService = nugetService ?? throw new ArgumentNullException(nameof(nugetService));
    }

    public async Task<IReadOnlyList<PackageMetadata>> ExecuteAsync(
        string query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be null or empty", nameof(query));

        return await _nugetService.SearchPackagesAsync(query, cancellationToken);
    }
}