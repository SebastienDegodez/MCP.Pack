using System.ComponentModel;
using McpPack.Application.RetrievePackage;
using McpPack.Domain.PackageAggregate;
using ModelContextProtocol.Server;

namespace McpPack.Api;

/// <summary>
/// Provides tools for managing NuGet packages.
/// This class is registered as a server tool in the ModelContextProtocol.
/// It allows clients to retrieve package metadata, such as the latest version of a NuGet package.
/// </summary>
/// <remarks>
/// This class is part of the McpPack.Api assembly, which serves as the API layer
/// in the Clean Architecture structure of the McpPack application.
/// It interacts with the Application layer to perform operations related to NuGet packages.
/// The RetrieveNugetPackageVersionAsync method is exposed as a server tool that can be called
/// by clients to retrieve the latest version of a specified NuGet package.
/// </remarks>
/// <example>
/// Example usage:
/// ```csharp
/// var packageTools = new PackageTools(RetrievePackageMetadataUseCase);
/// var latestVersion = await packageTools.RetrieveNugetPackageVersionAsync("Newtonsoft.Json");
/// foreach (var version in latestVersion)
/// {
///     Console.WriteLine($"Package: {version.Id}, Version: {version.Version}");
/// }
/// ```
/// </example>
/// <seealso cref="RetrievePackageMetadataUseCase"/>
[McpServerToolType]
public sealed class NugetPackageTools
{
    public const string RetrieveNugetPackageVersion = "RetrieveNugetPackageVersion";

    private readonly RetrievePackageMetadataUseCase _retrievePackageMetadataUseCase;

    /// <summary>
    /// Initializes a new instance of the <see cref="NugetPackageTools"/> class.
    /// </summary>
    /// <param name="retrievePackageMetadataUseCase">The use case for retrieving package metadata.</param>
    public NugetPackageTools(RetrievePackageMetadataUseCase retrievePackageMetadataUseCase)
    {
        _retrievePackageMetadataUseCase = retrievePackageMetadataUseCase;
    }

    /// <summary>
    /// Gets the latest version of a NuGet package from nuget server.
    /// </summary>
    [McpServerTool(Name = RetrieveNugetPackageVersion), Description("Retrieve the latest version of a NuGet package from nuget server.")]
    public async Task<IEnumerable<PackageMetadata>> RetrieveNugetPackageVersionAsync(
        [Description("The NuGet package name")] string packageName)
    {
        var metadata = await _retrievePackageMetadataUseCase.ExecuteAsync(packageName);
        return metadata;
    }
}
