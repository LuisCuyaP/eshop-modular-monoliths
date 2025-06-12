
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;

namespace Catalogo;
public static class CatalogoModule
{
    public static IServiceCollection AddCatalogoModule(this IServiceCollection services, IConfiguration configuration)
    {
        //1. Add services to the container.

        //2. Api Endpoint services

        //3. Application Use Case services
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        // Data - Infrastructure services
        var connectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<CatalogoDbContext>((sp, options) =>
        {
            options.AddInterceptors(new AuditableEntityInterceptor());
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IDataSeeder, CatalogoDataSeeder>();

        return services;
    }
    public static IApplicationBuilder UseCatalogoModule(this IApplicationBuilder app)
    {
        // Configure the HTTP request pipeline.

        // 1. Use Api Endpoint services

        // 2. Use Application Use Case services

        // 3. Use Data - Infrastructure services  
        
        app.UseMigration<CatalogoDbContext>();
        //InitialiseDatabaseAsync(app).GetAwaiter().GetResult();
        return app;
    }

    //private static async Task InitialiseDatabaseAsync(IApplicationBuilder app)
    //{
    //    using var scope = app.ApplicationServices.CreateScope();
    //    var context = scope.ServiceProvider.GetRequiredService<CatalogoDbContext>();
    //    await context.Database.MigrateAsync();
    //}
}
