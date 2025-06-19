var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

//Add services to the container
builder.Services
    .AddCarterWithAssemblies(typeof(CatalogoModule).Assembly);

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
