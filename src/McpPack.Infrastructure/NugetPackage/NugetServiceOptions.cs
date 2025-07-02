namespace McpPack.Infrastructure.NugetPackage;

/// <summary>
/// Options for configuring the NugetService.
/// </summary>
public sealed class NugetServiceOptions
{
    /// <summary>
    /// List of NuGet feed URLs.
    /// </summary>
    /// <remarks>
    /// This allows multiple feeds to be specified, which can be useful for aggregating packages from different sources.
    /// </remarks>
    public List<string> FeedUrls { get; set; } = [];
}
