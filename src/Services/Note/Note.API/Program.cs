using GrpcNote;

using Note.API.Infrastructure.Extensions;

using Service.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
	.AddNoteDbContext(configuration)
	.AddDefaultAuthentication(configuration)
	.AddDefaultOpenApi(configuration)
	.AddRabbitMqEventBus(configuration)
	.AddIntegrationServices()
	.AddNoteMediatR()
	.AddIntegrationEventHandlers()
	.AddHttpContextAccessor()
	.AddScopedServices()
	.AddControllers();

services.AddGrpc();
services.AddDefaultHealthChecks(configuration, "NoteDB");

var app = builder.Build();

await app.Services.InitialDbAsync(app.Configuration);
await app.Services.InitialRabbitMqSubscribeAsync();

app.UseDefaultOpenApi(configuration);

if (!app.Environment.IsDevelopment())
	app.UseHsts();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<NotesGrpcService>();

app.MapControllers();

app.MapDefaultHealthChecks();

await app.RunAsync();