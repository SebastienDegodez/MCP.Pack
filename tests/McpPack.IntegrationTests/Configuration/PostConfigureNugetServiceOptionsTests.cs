using McpPack.Infrastructure.Packages;
using Xunit;

namespace McpPack.IntegrationTests.Infrastructure.Configuration;

public sealed class PostConfigureNugetServiceOptionsTests
{

    [Fact]
    public void PostConfigure_ShouldSetNullFeed_WhenFeedUrlsIsEmpty()
    {
        // Arrange
        var options = new NugetServiceOptions { FeedUrls = null! };
        var postConfigure = new PostConfigureNugetServiceOptions();

        // Act
        postConfigure.PostConfigure(null, options);

        // Assert
        Assert.NotNull(options.FeedUrls);
        Assert.Single(options.FeedUrls);
        Assert.Equal("https://api.nuget.org/v3/index.json", options.FeedUrls[0]);
    }

    [Fact]
    public void PostConfigure_ShouldNotOverrideFeedUrls_WhenFeedUrlsIsNotEmpty()
    {
        // Arrange
        var customFeed = "https://custom.feed/index.json";
        var options = new NugetServiceOptions { FeedUrls = [customFeed] };
        var postConfigure = new PostConfigureNugetServiceOptions();

        // Act
        postConfigure.PostConfigure(null, options);

        // Assert
        Assert.Single(options.FeedUrls);
        Assert.Equal(customFeed, options.FeedUrls[0]);
    }
}
