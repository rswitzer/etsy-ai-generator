using EtsyAiGenerator.Application.ListingBuilder.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EtsyAiGenerator.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IListingBuilderService, StubListingBuilderService>();

        return services;
    }
}
