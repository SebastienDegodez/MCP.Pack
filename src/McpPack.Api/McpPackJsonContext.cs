using McpPack.Domain.PackageAggregate;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace McpPack.Api;

[ExcludeFromCodeCoverage(
    Justification = "This is a generated file for JSON serialization context, no manual changes are expected.")]
[JsonSerializable(typeof(IEnumerable<PackageMetadata>))]
[JsonSerializable(typeof(PackageMetadata))]
public partial class McpPackJsonContext : JsonSerializerContext
{
}
