using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

/// <summary>
/// Ресурсы IdentityServer и файл конфигурации клиентов
/// </summary>
public static class Config
{
	public static IEnumerable<ApiScope> GetApiScopes()
	{
		return new List<ApiScope>
			{
				new ApiScope(ApiScopeDefinitions.WebBffStockControl.name, ApiScopeDefinitions.WebBffStockControl.displayName),
				new ApiScope(ApiScopeDefinitions.StockControl.name, ApiScopeDefinitions.StockControl.displayName),
				new ApiScope(ApiScopeDefinitions.Note.name, ApiScopeDefinitions.Note.displayName),
				new ApiScope(ApiScopeDefinitions.NoteGrpc.name, ApiScopeDefinitions.NoteGrpc.displayName),
				new ApiScope(ApiScopeDefinitions.Notification.name, ApiScopeDefinitions.Notification.displayName),
				new ApiScope(ApiScopeDefinitions.PersonalCabinet.name, ApiScopeDefinitions.PersonalCabinet.displayName),
				new ApiScope(ApiScopeDefinitions.FileStorage.name, ApiScopeDefinitions.FileStorage.displayName)
			};
	}

	public static IEnumerable<IdentityResource> GetResources()
	{
		return new List<IdentityResource>
		{
			new IdentityResources.OpenId(), // информация о scope
			new IdentityResources.Profile(), // информация о пользователе 
			new IdentityResources.Email()
		};
	}

	// указываем перечень клиентов, которые будут взаимодействвовать с нашей системой identity,
	public static IEnumerable<Client> GetClients(IConfiguration configuration)
	{
		return new List<Client>
		{
			new Client
			{
				ClientId = "js",
				ClientName = "Stock Control SPA OpenId Client",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris =           { $"{configuration["SpaClient"]}/" },
				RequireConsent = false,
				PostLogoutRedirectUris = { $"{configuration["SpaClient"]}/" },
				AllowedCorsOrigins =     { $"{configuration["SpaClient"]}" },

				AllowedScopes =
				{
					IdentityServerConstants.StandardScopes.OpenId,
					IdentityServerConstants.StandardScopes.Profile,
					IdentityServerConstants.StandardScopes.Email,
					ApiScopeDefinitions.WebBffStockControl.name,
					ApiScopeDefinitions.StockControl.name,
					ApiScopeDefinitions.Note.name,
					ApiScopeDefinitions.NoteGrpc.name,
					ApiScopeDefinitions.Notification.name,
					ApiScopeDefinitions.PersonalCabinet.name,
					ApiScopeDefinitions.FileStorage.name,
				},
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.WebBffStockControl.name}.sw.ui",
				ClientName = "Web Stock Control Aggregator Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["WebStockControlAggregatorApiClient"]}/swagger/oauth2-redirect.html" }, 
				PostLogoutRedirectUris = { $"{configuration["WebStockControlAggregatorApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.WebBffStockControl.name,
				}
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.StockControl.name}.sw.ui",
				ClientName = "Stock Control Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["StockControlApiClient"]}/swagger/oauth2-redirect.html" }, 
				PostLogoutRedirectUris = { $"{configuration["StockControlApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.StockControl.name,
				}
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.Note.name}.sw.ui",
				ClientName = "Note Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["NoteApiClient"]}/swagger/oauth2-redirect.html" },
				PostLogoutRedirectUris = { $"{configuration["NoteApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.Note.name
				}
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.Notification.name}.sw.ui",
				ClientName = "Notification Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["NotificationApiClient"]}/swagger/oauth2-redirect.html" },
				PostLogoutRedirectUris = { $"{configuration["NotificationApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.Notification.name,
				}
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.PersonalCabinet.name}.sw.ui",
				ClientName = "Personal Cabinet Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["PersonalCabinetApiClient"]}/swagger/oauth2-redirect.html" },
				PostLogoutRedirectUris = { $"{configuration["PersonalCabinetApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.PersonalCabinet.name,
				}
			},
			new Client
			{
				ClientId = $"{ApiScopeDefinitions.FileStorage.name}.sw.ui",
				ClientName = "File Storage Swagger UI",
				AllowedGrantTypes = GrantTypes.Implicit,
				AllowAccessTokensViaBrowser = true,
				RedirectUris = { $"{configuration["FileStorageApiClient"]}/swagger/oauth2-redirect.html" },
				PostLogoutRedirectUris = { $"{configuration["FileStorageApiClient"]}/swagger/" },
				AllowedScopes =
				{
					ApiScopeDefinitions.FileStorage.name,
				}
			}
		};
	}
}