using FileStorage.API.Infrastructure.Extensions;

using Service.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.SetConfigureKestrel();
builder.Host.AddDefaultSeqLog();

var services = builder.Services;
var configuration = builder.Configuration;

services
	.AddDefaultAuthentication(configuration)
	.AddDefaultOpenApi(configuration)
	.AddFileStorageMediatR()
	.AddRedis(configuration)
	.AddScopedServices()
	.AddHttpClient()
	.AddHttpContextAccessor()
	.AddControllers();

services.AddDefaultHealthChecks(configuration);

var app = builder.Build();

app.UseDefaultOpenApi(configuration);

if (!app.Environment.IsDevelopment())
	app.UseHsts();

// ����� ������� � �������������� ������� � app.UseHsts(); ����� ��� ��������� ���������� ��� �� ������������ �� https, ��� ������ ��������� � ��� �����. 
//app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapDefaultHealthChecks();

await app.RunAsync();
