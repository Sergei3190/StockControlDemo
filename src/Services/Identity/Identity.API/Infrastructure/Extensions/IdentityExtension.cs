using Identity.API.Configuration;
using Identity.API.DAL.Context;
using Identity.API.Domain.Entities;
using Identity.API.Infrastructure.Options.TokenProvider;
using Identity.API.Infrastructure.Settings.Authentication;
using Identity.API.Infrastructure.TokenProviders;
using Identity.API.Infrastructure.Valdiators;
using Identity.API.Services;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.Infrastructure.Extensions;

public static class IdentityExtension
{
	public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(nameof(services));

		var section = configuration.GetRequiredSection("Authentication");
		var authOptions = section.Get<AuthenticationSettings>();

		services.Configure<AuthenticationSettings>(section);

		// подключаем базовую Identity для хранения данных в бд и возможности работать с куками
		services.AddIdentity<User, Role>(opt =>
		{
			opt.Password.RequireDigit = authOptions.Password.RequireDigit;
			opt.Password.RequireLowercase = authOptions.Password.RequireLowercase;
			opt.Password.RequireUppercase = authOptions.Password.RequireUppercase;
			opt.Password.RequireNonAlphanumeric = authOptions.Password.RequireNonAlphanumeric;
			opt.Password.RequiredLength = authOptions.Password.RequiredLength;
			opt.Password.RequiredUniqueChars = authOptions.Password.RequiredUniqueChars;

			// для подтверждения почты пользователя
			opt.SignIn.RequireConfirmedEmail = authOptions.SignIn.RequireConfirmedEmail.HasValue ? authOptions.SignIn.RequireConfirmedEmail.Value : true;
			opt.User.RequireUniqueEmail = authOptions.User is { RequireUniqueEmail: { } flag } ? flag : true;

			opt.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";

			opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			opt.User.AllowedUserNameCharacters += " ";
			opt.User.AllowedUserNameCharacters += "1234567890";

			opt.Lockout.AllowedForNewUsers = authOptions.AccountBlocking.AllowedForNewUsers.HasValue ? authOptions.AccountBlocking.AllowedForNewUsers.Value : true;
			opt.Lockout.MaxFailedAccessAttempts = authOptions.AccountBlocking.MaxLoginAttempts;
			opt.Lockout.DefaultLockoutTimeSpan = authOptions.AccountBlocking.LockoutMinutes.HasValue
				? TimeSpan.FromMinutes(authOptions.AccountBlocking.LockoutMinutes.Value)
				: TimeSpan.FromMinutes(15);
		})
			.AddEntityFrameworkStores<IdentityDB>()
			.AddPasswordValidator<CustomPasswordValidator<User>>() // пользовательская проверка пароля
			.AddDefaultTokenProviders()
			.AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation"); // используем свой созданный токен пррвайдер для подтвержденяи электронной почты

		// устанавливаем время жизни токенов для сброса пароля
		services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(authOptions.TokenLifespan
			is { } tokenLifespan ? tokenLifespan : 1));

		// устанавливаем время жизни своего токена провайдера
		services.Configure<EmailConfirmationTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromDays(authOptions.EmailTokenLifespan
			is { } emailTokenLifespan ? emailTokenLifespan : 3));

		// регистрация системы identityServer4
		services.AddIdentityServer(options =>
		{
			options.IssuerUri = "null";
			options.Authentication.CookieLifetime = TimeSpan.FromHours(authOptions.CookieLifetime is { } cookieLifetime ? cookieLifetime : 2);

			// для вывода ошибок в Seq
			options.Events.RaiseErrorEvents = true;
			options.Events.RaiseInformationEvents = true;
			options.Events.RaiseFailureEvents = true;
			options.Events.RaiseSuccessEvents = true;
		})
		.AddInMemoryIdentityResources(Config.GetResources())
		.AddInMemoryApiScopes(Config.GetApiScopes())
		.AddInMemoryClients(Config.GetClients(configuration))
		.AddAspNetIdentity<User>()
		.AddDeveloperSigningCredential() // develop
		.AddProfileService<ProfileService>(); // для добавления новых claims 

		return services;
	}
}
