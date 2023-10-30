using PersonalCabinet.API.Infrastructure.Extensions;

using Service.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddPersonalCabinetDbContext(configuration)
    .AddDefaultAuthentication(configuration)
    .AddDefaultOpenApi(configuration)
    .AddRabbitMqEventBus(configuration)
    .AddIntegrationServices()
    .AddIntegrationEventHandlers()
    .AddPersonalCabinetMediatR()
    .AddMemoryCache()
    .AddHttpContextAccessor()
	.AddRedis(configuration)
	.AddScopedServices()
    .AddControllers();

services.AddPersonalCabinetHealthChecks(configuration);

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
