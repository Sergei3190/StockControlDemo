using Service.Common.Extensions;

using Web.StockControl.HttpAggregator.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
	.AddDefaultCors()
	.AddDefaultAuthentication(configuration)
    .AddDefaultOpenApi(configuration)
	.AddTransientServices()
	.AddBffReverseProxy(configuration)
	.AddHttpContextAccessor()
	.AddGrpcServices(configuration)
	.AddControllers();

services.AddBffHealthChecks(configuration);

var app = builder.Build();

app.UseDefaultOpenApi(configuration);

if (!app.Environment.IsDevelopment())
	app.UseHsts();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapReverseProxy();

app.UseDefaultCors();

app.MapDefaultHealthChecks();

await app.RunAsync();