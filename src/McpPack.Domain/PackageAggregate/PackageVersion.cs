namespace McpPack.Domain.PackageAggregate;

public sealed class PackageVersion
{
    public string Value { get; }

    private PackageVersion(string value)
    {
        Value = value;
    }

    public static PackageVersion Create(string value) => new PackageVersion(value);

    public override bool Equals(object? obj)
    {
        return obj is PackageVersion other && Value == other.Value;
    }
    
    override public int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
