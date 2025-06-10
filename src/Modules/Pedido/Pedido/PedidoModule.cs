using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pedido;

public static class PedidoModule
{
    public static IServiceCollection AddPedidoModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register services, repositories, etc. here
        // Example: services.AddScoped<IPedidoService, PedidoService>();
        return services;
    }
}
