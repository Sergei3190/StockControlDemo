using Identity.API.Infrastructure.Extensions;

using Service.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services
	.AddIdentityDbContext(configuration)
	.AddIdentityConfiguration(configuration)
	.AddDefaultEmailService()
	.AddIntegrationServices()
	.AddIdentityMediatR()
	.AddScopedServices()
	.AddControllersWithViews();

services.AddDefaultHealthChecks(configuration, "IdentityDB");

var app = builder.Build();

await app.Services.InitialDbAsync(app.Configuration);

if (app.Environment.IsEnvironment("Development"))
	app.UseDeveloperExceptionPage();
else
{
	app.UseHsts();
	app.UseExceptionHandler("/Home/Error");
}

app.UseCookiePolicy(new CookiePolicyOptions
{
	// Указывает, что клиент должен отправлять файл cookie с запросами «одного сайта» и с «межсайтовой» навигацией верхнего уровня.
	MinimumSameSitePolicy = SameSiteMode.Lax,
});

app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.MapDefaultHealthChecks();

await app.RunAsync();
