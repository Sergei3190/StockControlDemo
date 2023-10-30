using Notification.API.Infrastructure.Extensions;
using Notification.API.SignalR.Hubs;

using Service.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddNotificationDbContext(configuration)
    .AddDefaultAuthentication(configuration)
    .AddDefaultOpenApi(configuration)
    .AddRabbitMqEventBus(configuration)
    .AddIntegrationServices()
    .AddNotificationMediatR() 
	.AddHttpContextAccessor() 
	.AddIntegrationEventHandlers() 
    .AddMemoryCache() 
    .AddNotificationSignalR(configuration) 
    .AddScopedServices()
    .AddControllers();

services.AddDefaultHealthChecks(configuration, "NotificationDB");

var app = builder.Build();

await app.Services.InitialDbAsync(app.Configuration);
await app.Services.InitialRabbitMqSubscribeAsync();

app.UseDefaultOpenApi(configuration);

if (!app.Environment.IsDevelopment())
	app.UseHsts();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationHub>("/hubs/notification-hub");

app.MapControllers();

app.MapDefaultHealthChecks();

await app.RunAsync();
