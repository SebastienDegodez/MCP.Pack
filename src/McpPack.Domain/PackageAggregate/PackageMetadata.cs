namespace McpPack.Domain.PackageAggregate;

public sealed class PackageMetadata
{
    public string Title { get; }
    public Uri? ReadmeUri { get; }
    public AvailablePackageVersions Versions { get; }

    private PackageMetadata(string title, Uri? readmeUri, AvailablePackageVersions versions)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Package title cannot be null or empty", nameof(title));
        }

        Title = title;
        ReadmeUri = readmeUri;
        Versions = versions ?? throw new ArgumentNullException(nameof(versions));
    }

    public static PackageMetadata Create(string title, Uri? readmeUri, AvailablePackageVersions versions)
    {
        return new PackageMetadata(title, readmeUri, versions);
    }

    public bool HasVersions()
    {
        return Versions.Any();
    }

    public PackageVersion? GetLatestVersion()
    {
        return Versions.GetLatest();
    }
}
