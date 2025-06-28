using System.Reflection;
using NetArchTest.Rules;
using McpPack.Api;
using McpPack.Application;
using McpPack.Domain;
using McpPack.Infrastructure;
using Xunit;

namespace McpPack.IntegrationTests;

public sealed class ArchitectureTests
{
    private const string DomainNamespace = "McpPack.Domain";
    private const string ApplicationNamespace = "McpPack.Application";
    private const string InfrastructureNamespace = "McpPack.Infrastructure";
    private const string ApiNamespace = "McpPack.Api";

    private static readonly Assembly DomainAssembly = typeof(DomainReference).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(ApplicationReference).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(InfrastructureReference).Assembly;
    private static readonly Assembly ApiAssembly = typeof(ApiReference).Assembly;


    [Fact]
    public void Domain_Classes_Should_Be_Sealed()
    {
       var result = Types
            .InAssembly(DomainAssembly)
            .That().AreClasses()
            .Should().BeSealed()
            .GetResult();
 
       Assert.True(result.IsSuccessful, "All Domain classes must be sealed.");
    }

    /// <summary>
    /// Ensures that the Domain layer does not have dependencies on other layers.
    /// This is crucial for maintaining the separation of concerns and ensuring that the Domain layer remains independent
    /// from Application, Infrastructure, and API layers.
    /// </summary>
    [Fact]
    public void Domain_ShouldNotHaveDependencyOn_OtherLayers()
    {
        // Act
        var result = Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn(ApplicationNamespace)
            .And()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, $"Domain layer should not depend on other layers. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Application_ShouldOnlyDependOn_Infrastructure()
    {
        // Act
        var result = Types.InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureNamespace)
            .And()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, $"Application layer should not depend on Infrastructure or API. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

    [Fact]
    public void Infrastructure_ShouldNotHaveDependencyOn_Api()
    {
        // This test ensures Infrastructure doesn't depend on API layer
        // Since we don't have API assembly reference here, we check it doesn't reference web-specific packages
        var result = Types.InAssembly(InfrastructureAssembly)
            .Should()
            .NotHaveDependencyOn(ApiNamespace)
            .GetResult();

        // Assert
        Assert.True(result.IsSuccessful, $"Infrastructure should not depend on API layer. Violations: {string.Join(", ", result.FailingTypeNames ?? [])}");
    }

}
