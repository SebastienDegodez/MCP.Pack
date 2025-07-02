using NuGet.Protocol.Core.Types;
using NuGet.Configuration;
using Microsoft.Extensions.Options;
using McpPack.Domain.PackageAggregate;

namespace McpPack.Infrastructure.NugetPackage;

/// <summary>
/// Service for interacting with NuGet packages.
/// This service allows searching for packages in a specified NuGet feed.
/// It is initialized with a set of feed URLs, and it provides methods to search for packages
/// based on a query string.
/// </summary>
public sealed class NugetService : INugetService
{
    public NugetServiceOptions NugetServiceOptions { get; }
    public string FeedUrl { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="NugetService"/> class with the
    /// specified options.
    /// </summary>
    /// <param name="options">The options containing the NuGet feed URLs.</param>
    /// <exception cref="ArgumentException">Thrown when no feed URLs are provided.</exception>
    /// <remarks>
    /// This constructor expects the options to contain at least one feed URL.
    /// If no URLs are provided, an exception is thrown.
    /// </remarks>
    public NugetService(IOptions<NugetServiceOptions> options)
    {
        this.NugetServiceOptions = options.Value;
        FeedUrl = NugetServiceOptions.FeedUrls.FirstOrDefault()
            ?? throw new ArgumentException("At least one feed URL must be provided", nameof(NugetServiceOptions.FeedUrls));
    }

    public async Task<IReadOnlyList<PackageMetadata>> SearchPackagesAsync(string query, CancellationToken cancellationToken = default)
    {

        if (string.IsNullOrWhiteSpace(query))
        {
            throw new ArgumentException("Query cannot be null or empty", nameof(query));
        }

        var providers = Repository.Provider.GetCoreV3();
        var source = new PackageSource(FeedUrl);
        var repo = new SourceRepository(source, providers);

        var searchResource = await repo.GetResourceAsync<PackageSearchResource>();
        var searchFilter = new SearchFilter(includePrerelease: true);
        var packageSearchMetadatas = await searchResource.SearchAsync(
            query,
            searchFilter,
            skip: 0,
            take: 10,
            log: NuGet.Common.NullLogger.Instance,
            cancellationToken: cancellationToken);

        var packages = new List<PackageMetadata>();
        foreach (var packageSearchMetadata in packageSearchMetadatas)
        {
            var versions = packageSearchMetadata.GetVersionsAsync() != null
                ? (await packageSearchMetadata.GetVersionsAsync())
                    .Select(v => PackageVersion.Create(v.Version.ToNormalizedString()))
                    .ToList()
                : [];

            var availableVersions = AvailablePackageVersions.Create(versions);

            var packageMetadata = PackageMetadata.Create(
                packageSearchMetadata.Title ?? packageSearchMetadata.Identity.Id,
                packageSearchMetadata.ReadmeUrl,
                availableVersions);

            packages.Add(packageMetadata);
        }
        return packages;
    }
}
