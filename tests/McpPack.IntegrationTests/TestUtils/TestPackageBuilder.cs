using NuGet.Packaging;
using NuGet.Versioning;

namespace McpPack.IntegrationTests.TestUtils;

public static class TestPackageBuilder
{
    public static string CreateTestPackage(string packageName, string version)
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        Directory.CreateDirectory(tempDir);
        var nuspecPath = Path.Combine(tempDir, $"{packageName}.nuspec");
        var nupkgPath = Path.Combine(tempDir, $"{packageName}.{version}.nupkg");

        var builder = new PackageBuilder
        {
            Id = packageName,
            Version = new NuGetVersion(version),
            Description = "Test package for integration"
        };
        builder.Authors.Add("Test");
        builder.Files.Add(new PhysicalPackageFile { SourcePath = Path.GetTempFileName(), TargetPath = "lib/netstandard2.0/_._" });

        using (var fs = File.Create(nupkgPath))
        {
            builder.Save(fs);
        }
        return nupkgPath;
    }
}
