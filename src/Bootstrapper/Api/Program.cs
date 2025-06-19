var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services
    .AddCarterWithAssemblies(typeof(CatalogoModule).Assembly);

builder.Services
    .AddCatalogoModule(builder.Configuration)
    .AddCarritoModule(builder.Configuration)
    .AddPedidoModule(builder.Configuration);

var app = builder.Build();

//Configure the HTTP request pipeline
//add middleware here if needed
app.MapCarter();

app.UseCatalogoModule()
   .UseCarritoModule()
   .UsePedidoModule();

app.Run();
