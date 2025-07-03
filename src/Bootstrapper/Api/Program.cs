using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

//Add services to the container


//common services: carter, mediatr, fluent validation
var catalogAssembly = typeof(CatalogoModule).Assembly;
var carritoAssembly = typeof(CarritoModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, carritoAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, carritoAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, carritoAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//module services: catalogo, carrito, pedido
builder.Services
    .AddCatalogoModule(builder.Configuration)
    .AddCarritoModule(builder.Configuration);
    //.AddPedidoModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline
//add middleware here if needed
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();

app.UseCatalogoModule()
   .UseCarritoModule();
   //.UsePedidoModule();

app.Run();
