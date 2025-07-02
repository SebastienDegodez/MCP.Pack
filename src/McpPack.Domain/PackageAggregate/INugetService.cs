namespace McpPack.Domain.PackageAggregate;

public interface INugetService
{
    Task<IReadOnlyList<PackageMetadata>> SearchPackagesAsync(string query, CancellationToken cancellationToken = default);
}
