
using Keycloak.AuthServices.Authentication;
using Shared.Messaging.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) => 
    config.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

// common services: carter, mediatR, fluentValidation
var catalogAssemply = typeof(CatalogModule).Assembly;
var basketAssemply = typeof(BasketModule).Assembly;

builder.Services
    .AddCarterWithAssemblies(
        typeof(CatalogModule).Assembly,
        typeof(BasketModule).Assembly);

builder.Services
    .AddMediatRWithAssemblies(catalogAssemply, basketAssemply);

builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services
    .AddMassTransitWithAssemblies(builder.Configuration,catalogAssemply, basketAssemply);

builder.Services.AddKeycloakWebApiAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// module services: catalog, basket, ordering
builder.Services
    .AddCatalogModule(builder.Configuration)
    .AddBasketModule(builder.Configuration)
    .AddOrderingModule(builder.Configuration);

builder.Services
    .AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();
app.UseSerilogRequestLogging();
// Exception Handler
app.UseExceptionHandler(options => { });
app.UseAuthentication();
app.UseAuthorization();

app
    .UseCatalogModule()
    .UseBasketModule()
    .UseOrderingModule();

app.Run();
