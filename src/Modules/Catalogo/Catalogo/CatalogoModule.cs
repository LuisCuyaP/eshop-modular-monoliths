using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalogo;
public static class CatalogoModule
{
    public static IServiceCollection AddCatalogoModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.

        // Api Endpoint services

        // Application Use Case services
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<CatalogoDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString);
        });

        return services;
    }
    public static IApplicationBuilder UseCatalogoModule(this IApplicationBuilder app)
    {
        return app;
    }
        
}
