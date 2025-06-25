var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

//Add services to the container


//common services: carter, mediatr, fluent validation
var catalogAssembly = typeof(CatalogoModule).Assembly;
var carritoAssembly = typeof(CarritoModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(catalogAssembly, carritoAssembly);

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(catalogAssembly ,carritoAssembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssemblies([catalogAssembly, carritoAssembly]);

//module services: catalogo, carrito, pedido
builder.Services
    .AddCatalogoModule(builder.Configuration)
    .AddCarritoModule(builder.Configuration)
    .AddPedidoModule(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

//Configure the HTTP request pipeline
//add middleware here if needed
app.MapCarter();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(options => { });

app.UseCatalogoModule()
   .UseCarritoModule()
   .UsePedidoModule();

app.Run();
