using Keycloak.AuthServices.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

//Add services to the container


//common services: carter, mediatr, fluent validation
var catalogAssembly = typeof(CatalogoModule).Assembly;
var carritoAssembly = typeof(CarritoModule).Assembly;
var pedidoAssembly = typeof(OrderingModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, carritoAssembly, pedidoAssembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, carritoAssembly, pedidoAssembly);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddMassTransitWithAssemblies(builder.Configuration, catalogAssembly, carritoAssembly, pedidoAssembly);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

//module services: catalogo, carrito, pedido
builder.Services
    .AddCatalogoModule(builder.Configuration)
    .AddCarritoModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

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
   .UseCarritoModule()
   .UseOrderingModule();

app.Run();
