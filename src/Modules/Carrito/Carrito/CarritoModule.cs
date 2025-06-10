using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carrito;
public static class CarritoModule
{
    public static IServiceCollection AddCarritoModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register services, repositories, etc. here
        // Example: services.AddScoped<ICarritoService, CarritoService>();
        return services;
    }
    public static IApplicationBuilder UseCarritoModule(this IApplicationBuilder app)
    {
        // Configure middleware if needed
        return app;
    }
}
