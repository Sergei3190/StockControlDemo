using Service.Common.Interfaces;
using Service.Common.Services;

using StockControl.API.DAL.Context;
using StockControl.API.Services;
using StockControl.API.Services.ClassifierItems;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ClassifierItems;
using StockControl.API.Services.Interfaces.ProductFlow;
using StockControl.API.Services.Interfaces.Select;
using StockControl.API.Services.ProductFlow;
using StockControl.API.Services.Select;

namespace StockControl.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services
			.AddScoped<IDbInitializerService, DbInitializerService>()
			.AddScoped<IIdentityService, IdentityService>()
			.AddScoped<ISaveService<StockControlDB>, SaveService>()
			.AddScoped<IClassifiersService, ClassifiersService>()
			.AddScoped<IProductFlowTypesService, ProductFlowTypesService>()
			.AddScoped<INomenclaturesService, NomenclaturesService>()
			.AddScoped<IOrganizationsService, OrganizationsService>()
			.AddScoped<IWarehousesService, WarehousesService>()
			.AddScoped<ISelectNomenclaturesService, SelectNomenclaturesService>()
			.AddScoped<ISelectOrganizationsService, SelectOrganizationsService>()
			.AddScoped<ISelectWarehousesService, SelectWarehousesService>()
			.AddScoped<IReceiptsService, ReceiptsService>()
			.AddScoped<IMovingsService, MovingsService>()
			.AddScoped<IWriteOffsService, WriteOffsService>()
			.AddScoped<IStockAvailabilitiesService, StockAvailabilitiesService>()
			.AddScoped<IPartiesService, PartiesService>()
			.AddScoped<ISelectPartiesService, SelectPartiesService>();

		return services;
	}
}
