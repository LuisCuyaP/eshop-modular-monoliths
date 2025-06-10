using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo;
public static class CatalogoModule
{
    public static IServiceCollection AddCatalogoModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register services, repositories, etc. here
        // Example: services.AddScoped<ICatalogService, CatalogService>();
        return services;
    }
}
