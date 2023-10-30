using Service.Common.Extensions;

using StockControl.API.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddStockControlDbContext(configuration)
    .AddDefaultAuthentication(configuration)
    .AddDefaultOpenApi(configuration)
    .AddRabbitMqEventBus(configuration)
    .AddIntegrationServices()
    .AddIntegrationEventHandlers()
    .AddStockControlMediatR()
	.AddHttpContextAccessor()
	.AddMemoryCache()
	.AddScopedServices()
    .AddControllers();

services.AddDefaultHealthChecks(configuration, "StockControlDB");

var app = builder.Build();

await app.Services.InitialDbAsync(app.Configuration);
await app.Services.InitialRabbitMqSubscribeAsync();

app.UseDefaultOpenApi(configuration);

if (!app.Environment.IsDevelopment())
	app.UseHsts();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapDefaultHealthChecks();

await app.RunAsync();