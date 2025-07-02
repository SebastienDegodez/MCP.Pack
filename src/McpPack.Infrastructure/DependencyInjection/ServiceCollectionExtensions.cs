using McpPack.Domain.Packages;
using McpPack.Infrastructure.Packages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace McpPack.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods for registering services in the dependency injection container.
/// This follows Clean Architecture by keeping all dependency registrations in the Infrastructure layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds all infrastructure services to the dependency injection container.
    /// This method encapsulates all service registrations following Clean Architecture principles.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register domain services (interfaces implemented in Infrastructure)
        services.AddTransient<INugetService, NugetService>();

        // Bind options from configuration
        services.Configure<NugetServiceOptions>(configuration.GetSection("NugetService"));

        // Register PostConfigure for NugetServiceOptions
        services.AddTransient<IPostConfigureOptions<NugetServiceOptions>, PostConfigureNugetServiceOptions>();

        return services;
    }
}
