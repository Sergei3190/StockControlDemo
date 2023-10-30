using Service.Common.Extensions;

using WebStockControl.API.Infrastructure.Extensions;
using WebStockControl.API.Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.Configure<AppSettings>(configuration.GetRequiredSection("Urls"));

services
	.AddDefaultCors()
	.AddControllers();

services.AddSpaHealthChecks(configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
	app.UseHsts();

app.UseRouting();

app.MapControllers();

app.UseDefaultCors();

app.MapDefaultHealthChecks();

await app.RunAsync();
