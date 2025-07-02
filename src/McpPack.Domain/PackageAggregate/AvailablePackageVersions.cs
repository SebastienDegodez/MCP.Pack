using System.Collections;

namespace McpPack.Domain.PackageAggregate;

public sealed class AvailablePackageVersions
{
    private readonly IReadOnlyList<PackageVersion> _versions;

    private AvailablePackageVersions(IEnumerable<PackageVersion> versions)
    {
        ArgumentNullException.ThrowIfNull(versions);
        var versionList = versions.ToList();
        if (versionList.Count == 0)
        {
            throw new ArgumentException("At least one version must be provided", nameof(versions));
        }

        _versions = versionList;
    }

    public static AvailablePackageVersions Create(IEnumerable<PackageVersion> versions) => new(versions);

    public bool Any() => _versions.Count > 0;

    public PackageVersion GetLatest()
    {
        return _versions.First();
    }

}
