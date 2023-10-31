using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddControllersWithViews();

services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy());

services
    .AddHealthChecksUI()
    .AddInMemoryStorage();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.MapHealthChecks("/liveness", new HealthCheckOptions
{
    Predicate = r => r.Name.Contains("self")
});

app.MapHealthChecksUI();

app.Run();
