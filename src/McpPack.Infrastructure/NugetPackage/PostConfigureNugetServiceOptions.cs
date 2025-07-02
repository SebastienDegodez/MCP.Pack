using Microsoft.Extensions.Options;

namespace McpPack.Infrastructure.NugetPackage;

/// <summary>
/// Ensures that NugetServiceOptions.FeedUrls contains at least the default NuGet feed if none are specified.
/// </summary>
public sealed class PostConfigureNugetServiceOptions : IPostConfigureOptions<NugetServiceOptions>
{
    private const string DefaultNugetFeed = "https://api.nuget.org/v3/index.json";

    public void PostConfigure(string? name, NugetServiceOptions options)
    {
        if (options.FeedUrls == null || options.FeedUrls.Count == 0)
        {
            options.FeedUrls = [DefaultNugetFeed];
        }
    }
}
